using Player;
using UnityEngine;

namespace Cameras {
    public class CameraWater : MonoBehaviour {
        private void OnTriggerEnter(Collider other) {
            if (LayerMask.LayerToName(other.gameObject.layer).Equals("Water")) {
                GetComponent<MeshRenderer>().enabled = true;
                // BananaMan.Instance.isInWater = true;
                // BananaMan.Instance.GetComponent<PlayerController>().baseMovementSpeed = 6f;
            }
        }

        private void OnTriggerExit(Collider other) {
            if (LayerMask.LayerToName(other.gameObject.layer).Equals("Water")) {
                GetComponent<MeshRenderer>().enabled = false;
                // BananaMan.Instance.isInWater = false;
            }
        }
    }
}
