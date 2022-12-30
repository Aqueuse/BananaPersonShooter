using UnityEngine;

namespace Building {
    public class Plateform : MonoBehaviour {
        private BoxCollider _boxCollider;

        [SerializeField] private Material plateformMaterial;
        [SerializeField] private Material ghostValidMaterial;
        [SerializeField] private Material ghostUnvalidMaterial;

        private MeshRenderer _meshRenderer;

        public bool isValid;
        public bool isPlaced;

        private void Start() {
            _boxCollider = GetComponent<BoxCollider>();
            _meshRenderer = GetComponent<MeshRenderer>();

            isValid = true;
            isPlaced = false;
        }
        
        public void SetGhostState(bool isGhost) {
            if (isGhost) {
                _boxCollider.isTrigger = true;
                _meshRenderer.material = ghostUnvalidMaterial;
            }
        }

        public void SetValid() {
            _meshRenderer.material = ghostValidMaterial;
        }

        public void SetUnvalid() {
            _meshRenderer.material = ghostUnvalidMaterial;
        }

        public void SetNormal() {
            isPlaced = true;
            _boxCollider.isTrigger = false;
            _meshRenderer.material = plateformMaterial;
        }
        
        private void OnTriggerStay(Collider other) {
            if (other.CompareTag("MoverUnvalid") && !isPlaced) {
                isValid = false;
                SetUnvalid();
            }
        }

        private void OnTriggerExit(Collider other) {
            if (other.CompareTag("MoverUnvalid") && !isPlaced) {
                isValid = true;
                SetValid();
            }
        }
    }
}
