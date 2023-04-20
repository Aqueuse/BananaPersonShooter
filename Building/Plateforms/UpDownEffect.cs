using UnityEngine;

namespace Building.Plateforms {
    public class UpDownEffect : MonoBehaviour {
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
            ObjectsReference.Instance.bananaMan.GetComponent<Rigidbody>().AddForce(Vector3.up * force, ForceMode.Acceleration);
        }
    }
}