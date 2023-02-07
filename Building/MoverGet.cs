using Audio;
using Enums;
using Player;
using UnityEngine;

namespace Building {
    public class MoverGet : MonoSingleton<MoverGet> {
        [SerializeField] private GameObject moverTarget;
        private GameObject targetedGameObject;
        
        private ItemThrowableType targetType;
        private const int Layermask = 1 << 9;

        private bool isAspiring;
        
        void Update() {
            if (isAspiring && BananaMan.Instance.isGrabingMover) {
                Mover.Instance.bananaGun.transform.LookAt(moverTarget.transform, Vector3.up);

                if (Physics.Raycast(Mover.Instance.bananaGun.transform.position, Mover.Instance.bananaGun.transform.forward, out RaycastHit raycastHit, 20, Layermask)) {
                    if (raycastHit.transform.GetComponent<Debris>() != null) {
                        targetedGameObject = raycastHit.transform.gameObject;
                        var debrisClass = targetedGameObject.GetComponent<Debris>(); 

                        debrisClass.DissolveMe();
                        AudioManager.Instance.PlayEffect(EffectType.DESINTEGRATION);
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
            
            AudioManager.Instance.StopAudioSource(AudioSourcesType.EFFECT);

            Mover.Instance.CancelMover();
        }
    }
}