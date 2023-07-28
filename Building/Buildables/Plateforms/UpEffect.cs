using UnityEngine;

namespace Building.Buildables.Plateforms {
    public class UpEffect : MonoBehaviour {
        public float force;
        public bool isActive;
        public bool isPlayerOn;
        
        private void OnCollisionExit(Collision other) {
            if (!isActive) return;

            if (other.gameObject.CompareTag("Player")) {
                isPlayerOn = false;
            }
        }
        
        private void OnCollisionStay(Collision other) {
            if (!isActive) return;
            
            if (other.gameObject.CompareTag("Player")) {

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