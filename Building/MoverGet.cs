using Audio;
using Enums;
using Player;
using UI.InGame;
using UnityEngine;
using UnityEngine.Localization.Settings;

namespace Building {
    public class MoverGet : MonoSingleton<MoverGet> {
        [SerializeField] private GameObject moverTarget;
        private GameObject targetedGameObject;
        
        private ItemThrowableType targetType;
        private float _timeCounter;
        private const int Layermask = 1 << 9;

        private bool isAspiring;
        
        private void Start() {
            _timeCounter = 0;
        }

        void Update() {
            if (isAspiring && BananaMan.Instance.isGrabingMover) {
                Mover.Instance.bananaGun.transform.LookAt(moverTarget.transform, Vector3.up);

                if (Physics.Raycast(Mover.Instance.bananaGun.transform.position, Mover.Instance.bananaGun.transform.forward, out RaycastHit raycastHit, 20, Layermask)) {
                    if (raycastHit.transform.GetComponent<Debris>() != null && targetedGameObject == null) {
                        targetedGameObject = raycastHit.transform.gameObject;
                        var shakingClass = targetedGameObject.GetComponent<Debris>(); 

                        shakingClass.ShakeMe(true);

                        AudioManager.Instance.PlayLoopedEffect(EffectType.ROCK_SLIDE);
                        
                        _timeCounter += Time.deltaTime;

                        if (_timeCounter >= 1.5f && targetedGameObject != null) {
                            Destroy(targetedGameObject);
                            Inventory.Instance.AddQuantity(shakingClass.itemThrowableType, shakingClass.itemThrowableCategory, 1);
                            AudioManager.Instance.StopAudioSource(AudioSourcesType.EFFECT);
                            AudioManager.Instance.PlayEffect(EffectType.FOMP);
                            MapManager.Instance.Clean();
                            UIQueuedMessages.Instance.AddMessage(
                                "+ 1 "+
                                LocalizationSettings.Instance.GetStringDatabase().GetLocalizedString("debris"));

                            targetedGameObject = null;
                            _timeCounter = 0;
                        }
                    }
                    else {
                        if (targetedGameObject != null) {
                            targetedGameObject.GetComponent<Debris>().ShakeMe(false);
                            targetedGameObject = null;
                        }
                    }
                }
            }
        }

        public void StartToGet() {
            isAspiring = true;
            Mover.Instance.GrabMover();
        }

        public void CancelGet() {
            isAspiring = false;

            if (targetedGameObject != null) {
                targetedGameObject.GetComponent<Debris>().ShakeMe(false);
                targetedGameObject = null;
            }
            
            _timeCounter = 0;

            AudioManager.Instance.StopAudioSource(AudioSourcesType.EFFECT);

            Mover.Instance.CancelMover();
        }
    }
}