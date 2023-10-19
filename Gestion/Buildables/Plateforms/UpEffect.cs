using Tags;
using UnityEngine;

namespace Gestion.Buildables.Plateforms {
    public class UpEffect : MonoBehaviour {
        public float force;
        public bool isActive;
        public bool isPlayerOn;
        
        private void OnCollisionExit(Collision other) {
            if (!isActive) return;

            if (TagsManager.Instance.HasTag(other.gameObject, GAME_OBJECT_TAG.PLAYER)) {
                isPlayerOn = false;
            }
        }
        
        private void OnCollisionStay(Collision other) {
            if (!isActive) return;
            
            if (TagsManager.Instance.HasTag(other.gameObject, GAME_OBJECT_TAG.PLAYER)) {

                if (!isPlayerOn) {
                    isPlayerOn = true;
                    Activate();
                }
            }
        }

        private void Activate() {
            ObjectsReference.Instance.playerController._damageCount = 0;
            ObjectsReference.Instance.bananaMan.GetComponent<Rigidbody>().AddForce(transform.up * force, ForceMode.Acceleration);
        }
    }
}