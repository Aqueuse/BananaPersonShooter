using Bananas;
using Building.PlateformsEffects;
using Enums;
using UnityEngine;

namespace Building {
    public class Plateform : MonoBehaviour {
        private BoxCollider _boxCollider;

        [SerializeField] private Material plateformMaterial;
        [SerializeField] private Material ghostValidMaterial;
        [SerializeField] private Material ghostUnvalidMaterial;
        
        public ItemThrowableType platformType;
        
        private MeshRenderer _meshRenderer;

        public bool isValid;

        private void Start() {
            _boxCollider = GetComponent<BoxCollider>();
            _meshRenderer = GetComponent<MeshRenderer>();

            isValid = true;
        }
        
        public void SetValid() {
            _meshRenderer.material = ghostValidMaterial;
        }

        public void SetUnvalid() {
            _meshRenderer.material = ghostUnvalidMaterial;
        }

        public void SetNormal() {
            _boxCollider.isTrigger = false;
            ResetMaterial();
        }

        public void ResetMaterial() {
            _meshRenderer.material = plateformMaterial;
        }

        private void OnTriggerEnter(Collider other) {
            if (other.CompareTag("Banana")) {
                switch (other.GetComponent<Banana>().bananasDataScriptableObject.itemThrowableType) {
                    case ItemThrowableType.CAVENDISH:
                        GetComponent<MeshRenderer>().material = other.GetComponent<MeshRenderer>().sharedMaterials[0];
                        GetComponent<UpDownEffect>().Activate();
                        break;
                }
            }
        }


        private void OnTriggerStay(Collider other) {
            if (other.CompareTag("MoverUnvalid") && _boxCollider.isTrigger) {
                isValid = false;
                SetUnvalid();
            }
        }

        private void OnTriggerExit(Collider other) {
            if (other.CompareTag("MoverUnvalid") && _boxCollider.isTrigger) {
                isValid = true;
                SetValid();
            }
        }
    }
}
