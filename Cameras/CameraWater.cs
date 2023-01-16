using UnityEngine;

namespace Cameras {
    public class CameraWater : MonoBehaviour {
        [SerializeField] private MeshRenderer aquaticPlaneMeshRenderer;
        
        private void OnTriggerEnter(Collider other) {
            if (LayerMask.LayerToName(other.gameObject.layer).Equals("Water")) {
                aquaticPlaneMeshRenderer.GetComponent<MeshRenderer>().enabled = true;
            }
        }

        private void OnTriggerExit(Collider other) {
            if (LayerMask.LayerToName(other.gameObject.layer).Equals("Water")) {
                aquaticPlaneMeshRenderer.GetComponent<MeshRenderer>().enabled = false;
            }
        }
    }
}
