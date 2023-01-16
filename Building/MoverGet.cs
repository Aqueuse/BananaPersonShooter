using Audio;
using Enums;
using Player;
using UI;
using UI.InGame;
using UnityEngine;

namespace Building {
    public class MoverGet : MonoSingleton<MoverGet> {
        [SerializeField] private GameObject deplaceur;
        [SerializeField] private GameObject moverTarget;
        
        private PlayerController _playerController;

        private GameObject targetedGameObject;
        private ItemThrowableType targetType;

        private static readonly int Activate = Shader.PropertyToID("_Activate");

        private float _timeCounter;

        private const int Layermask = 1 << 10;

        private void Start() {
            _playerController = BananaMan.Instance.GetComponent<PlayerController>();
            _timeCounter = 0;
        }

        void Update() {
            if (Mover.Instance.moverContext == MoverContext.GET && BananaMan.Instance.isGrabingMover) {
                deplaceur.transform.LookAt(moverTarget.transform, Vector3.up);

                if (Physics.Raycast(deplaceur.transform.position, deplaceur.transform.forward,
                        out RaycastHit raycastHit, Layermask)) {
                    if (raycastHit.transform.CompareTag("aspirable") && targetedGameObject == null) {
                        targetedGameObject = raycastHit.transform.gameObject;
                        targetedGameObject.GetComponent<MeshRenderer>().material.SetFloat(Activate, 1);

                        AudioManager.Instance.PlayLoopedEffect(EffectType.ROCK_SLIDE);
                        
                        _timeCounter += Time.deltaTime;

                        if (_timeCounter >= 1.5f && targetedGameObject != null) {
                            Destroy(targetedGameObject);
                            Inventory.Instance.AddQuantity(ItemThrowableType.ROCKET, 1);
                            AudioManager.Instance.StopAudioSource(AudioSourcesType.EFFECT);
                            AudioManager.Instance.PlayEffect(EffectType.FOMP);
                            targetedGameObject = null;
                            _timeCounter = 0;
                        }
                    }
                    else {
                        if (targetedGameObject != null) {
                            targetedGameObject.GetComponent<MeshRenderer>().material.SetFloat(Activate, 0);
                            targetedGameObject = null;
                        }
                    }
                }
            }
        }

        public void StartToGet() {
            deplaceur.SetActive(true);

            Mover.Instance.moverContext = MoverContext.GET;
            BananaMan.Instance.isGrabingMover = true;

            BananaMan.Instance.tpsPlayerAnimator.GrabMover();

            UIManager.Instance.Show_Hide_Mover_UI(true);
            Mover.Instance.SwitchMoverContextUI(MoverContext.GET);
            UICrosshair.Instance.SetCrosshair(ItemThrowableType.PLATEFORM_CAVENDISH, ItemThrowableCategory.PLATEFORM);
            
            Mover.Instance.wasFocus = _playerController.isFocusCamera;
        }

        public void CancelGet() {
            if (targetedGameObject != null) {
                targetedGameObject.GetComponent<MeshRenderer>().material.SetFloat(Activate, 0);
            }
            
            BananaMan.Instance.isGrabingMover = false;
            deplaceur.SetActive(false);
            _timeCounter = 0;

            targetedGameObject = null;
            AudioManager.Instance.StopAudioSource(AudioSourcesType.EFFECT);

            UICrosshair.Instance.SetCrosshair(BananaMan.Instance.activeItemThrowableType, BananaMan.Instance.activeItemThrowableCategory);
            UIManager.Instance.Show_Hide_Mover_UI(false);
        
            BananaMan.Instance.tpsPlayerAnimator.FocusCamera(Mover.Instance.wasFocus);
        }
    }
}