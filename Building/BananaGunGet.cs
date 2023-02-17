using Audio;
using Enums;
using Player;
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

                if (Physics.Raycast(BananaGun.Instance.bananaGun.transform.position, BananaGun.Instance.bananaGun.transform.forward, out RaycastHit raycastHit, 20, Layermask)) {
                    if (raycastHit.transform.GetComponent<Debris>() != null) {
                        targetedGameObject = raycastHit.transform.gameObject;
                        var debrisClass = targetedGameObject.GetComponent<Debris>(); 

                        debrisClass.DissolveMe();
                        AudioManager.Instance.PlayEffect(EffectType.DESINTEGRATION);
                    }
                    if (raycastHit.transform.GetComponent<Plateform>() != null) {
                        targetedGameObject = raycastHit.transform.gameObject;
                        var platformClass = targetedGameObject.GetComponent<Plateform>(); 

                        platformClass.DissolveMe();
                        AudioManager.Instance.PlayEffect(EffectType.DESINTEGRATION);
                    }
                }
            }
        }

        public void StartToGet() {
            isAspiring = true;
            BananaGun.Instance.GrabMover();
        }

        public void CancelGet() {
            isAspiring = false;
            
            AudioManager.Instance.StopAudioSource(AudioSourcesType.EFFECT);

            BananaGun.Instance.CancelMover();
        }
    }
}