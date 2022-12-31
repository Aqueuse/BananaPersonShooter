using UnityEngine;

namespace Building {
    public class Plateform : MonoBehaviour {
        private BoxCollider _boxCollider;

        [SerializeField] private Material plateformMaterial;
        [SerializeField] private Material ghostValidMaterial;
        [SerializeField] private Material ghostUnvalidMaterial;

        [SerializeField] private MeshRenderer turbineMeshRenderer;

        private MeshRenderer _meshRenderer;

        public bool isValid;

        private void Start() {
            _boxCollider = GetComponent<BoxCollider>();
            _meshRenderer = GetComponent<MeshRenderer>();

            isValid = true;
            Mover.Instance.isPlateformPlaced = false;
        }
        
        public void SetValid() {
            _meshRenderer.material = ghostValidMaterial;
        }

        public void SetUnvalid() {
            _meshRenderer.material = ghostUnvalidMaterial;
        }

        public void SetNormal() {
            Mover.Instance.isPlateformPlaced = true;
            _boxCollider.isTrigger = false;
            _meshRenderer.material = plateformMaterial;
            turbineMeshRenderer.enabled = true;
        }
        
        private void OnTriggerStay(Collider other) {
            if (other.CompareTag("MoverUnvalid") && !Mover.Instance.isPlateformPlaced) {
                isValid = false;
                SetUnvalid();
            }
        }

        private void OnTriggerExit(Collider other) {
            if (other.CompareTag("MoverUnvalid") && !Mover.Instance.isPlateformPlaced) {
                isValid = true;
                SetValid();
            }
        }
    }
}
