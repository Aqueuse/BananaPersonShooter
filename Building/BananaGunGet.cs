using Audio;
using Building.Plateforms;
using Enums;
using Game;
using Items;
using Player;
using UI.InGame;
using UnityEngine;

namespace Building {
    public class BananaGunGet : MonoSingleton<BananaGunGet> {
        [SerializeField] private GameObject moverTarget;
        private GameObject targetedGameObject;
        
        private ItemThrowableType targetType;
        private const int Layermask = 1 << 9;

        private bool isAspiring;
        
        void Update() {
            if (isAspiring && BananaMan.Instance.isGrabingBananaGun) {
                BananaGun.Instance.bananaGun.transform.LookAt(moverTarget.transform, Vector3.up);

                if (Physics.Raycast(GameManager.Instance.cameraMain.transform.position, GameManager.Instance.cameraMain.transform.forward, out RaycastHit raycastHit, 20, Layermask)) {
                    if (raycastHit.transform.GetComponent<ItemThrowable>() != null) {
                        ItemThrowable itemThrowable = raycastHit.transform.GetComponent<ItemThrowable>();

                        if (itemThrowable.ItemThrowableType == ItemThrowableType.PLATEFORM) {
                            targetedGameObject = raycastHit.transform.gameObject;

                            var platformClass = targetedGameObject.GetComponent<Plateform>(); 

                            platformClass.DissolveMe();
                            AudioManager.Instance.PlayEffect(EffectType.DESINTEGRATION);
                        }

                        if (itemThrowable.ItemThrowableCategory == ItemThrowableCategory.CRAFTABLE && itemThrowable.ItemThrowableType == ItemThrowableType.DEBRIS) {
                            targetedGameObject = raycastHit.transform.gameObject;
                            var debrisClass = targetedGameObject.GetComponent<Debris>(); 

                            debrisClass.DissolveMe();
                            AudioManager.Instance.PlayEffect(EffectType.DESINTEGRATION);
                        }
                    }
                }
            }
        }

        public void StartToGet() {
            isAspiring = true;
            BananaGun.Instance.GrabBananaGun();
            UICrosshair.Instance.SetCrosshair(ItemThrowableType.EMPTY);
            UICrosshair.Instance.ShowHideCrosshairs(true);
        }

        public void CancelGet() {
            isAspiring = false;
            
            AudioManager.Instance.StopAudioSource(AudioSourcesType.EFFECT);

            BananaGun.Instance.CancelMover();
            UICrosshair.Instance.ShowHideCrosshairs(false);
        }
    }
}