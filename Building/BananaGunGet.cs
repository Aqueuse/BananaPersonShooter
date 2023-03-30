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
        private GameObject _targetedGameObject;
        
        private ItemThrowableType _targetType;
        private const int Layermask = 1 << 9;

        private bool _isAspiring;
        
        void Update() {
            if (!_isAspiring || !BananaMan.Instance.isGrabingBananaGun) return;
            
            BananaGun.Instance.bananaGun.transform.LookAt(moverTarget.transform, Vector3.up);

            if (Physics.Raycast(GameManager.Instance.cameraMain.transform.position, GameManager.Instance.cameraMain.transform.forward, out RaycastHit raycastHit, 100, Layermask)) {
                if (raycastHit.transform.GetComponent<ItemThrowable>() != null) {
                    ItemThrowable itemThrowable = raycastHit.transform.GetComponent<ItemThrowable>();

                    if (itemThrowable.ItemThrowableType == ItemThrowableType.PLATEFORM) {
                        _targetedGameObject = raycastHit.transform.gameObject;

                        var platformClass = _targetedGameObject.GetComponent<Plateform>(); 

                        AudioManager.Instance.PlayEffect(EffectType.DESINTEGRATION, 0);
                        platformClass.DissolveMe();
                    }

                    if (itemThrowable.ItemThrowableCategory == ItemThrowableCategory.CRAFTABLE && itemThrowable.ItemThrowableType == ItemThrowableType.DEBRIS) {
                        _targetedGameObject = raycastHit.transform.gameObject;
                        var debrisClass = _targetedGameObject.GetComponent<Debris>(); 
                        UICanvasItemsHiddableManager.Instance.RemoveCanva(_targetedGameObject.GetComponentInChildren<Canvas>());

                        AudioManager.Instance.PlayEffect(EffectType.DESINTEGRATION, 0);
                        debrisClass.DissolveMe();
                    }
                }
            }
        }

        public void StartToGet() {
            _isAspiring = true;
            BananaGun.Instance.GrabBananaGun();
            UICrosshair.Instance.SetCrosshair(ItemThrowableType.EMPTY);
            UICrosshair.Instance.ShowHideCrosshairs(true);
        }

        public void CancelGet() {
            _isAspiring = false;
            
            AudioManager.Instance.StopAudioSource(AudioSourcesType.EFFECT);

            BananaGun.Instance.CancelMover();
            UICrosshair.Instance.ShowHideCrosshairs(false);
        }
    }
}