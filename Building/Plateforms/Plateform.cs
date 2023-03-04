using Audio;
using Bananas;
using Enums;
using Game;
using UI.InGame;
using UnityEngine;
using UnityEngine.Localization.Settings;

namespace Building.Plateforms {
    public class Plateform : MonoBehaviour {
        [SerializeField] private Material plateformMaterial;
        [SerializeField] private Material emissiveMaterial;
        [SerializeField] private Material ghostValidMaterial;
        [SerializeField] private Material ghostUnvalidMaterial;
        
        [SerializeField] private float dissolved;
        
        private float step;
        private Vector3 bananaManPosition;
        
        public bool isValid;

        private BoxCollider _boxCollider;
        private MeshRenderer _meshRenderer;

        private Material[] plateformMaterials;
        
        private static readonly int DissolveProperty = Shader.PropertyToID("Cutoff_Height");
        private float _dissolve;

        private bool isDissolving;

        private void Start() {
            _boxCollider = GetComponent<BoxCollider>();
            _meshRenderer = GetComponent<MeshRenderer>();

            plateformMaterials = new Material[2];
            
            isValid = true;
            _dissolve = 1.5f;
        }

        private void Update() {
            if (isDissolving) {
                _dissolve -= Time.deltaTime;
                plateformMaterials = _meshRenderer.materials;
                
                plateformMaterials[0].SetFloat(DissolveProperty, _dissolve);
                plateformMaterials[1].SetFloat(DissolveProperty, _dissolve);
                _meshRenderer.materials = plateformMaterials;

                if (_dissolve < dissolved) {
                    Inventory.Instance.AddQuantity(ItemThrowableType.PLATEFORM, ItemThrowableCategory.CRAFTABLE, 1);
                    AudioManager.Instance.StopAudioSource(AudioSourcesType.EFFECT);
                    //MapManager.Instance.Clean();
                    //MapManager.Instance.SaveDataOnScriptableObject();
                    UIQueuedMessages.Instance.AddMessage(
                        "+ 1 "+
                        LocalizationSettings.Instance.GetStringDatabase().GetLocalizedString("platform"));
                    Destroy(gameObject);
                }
            }
        }
        
        public void DissolveMe() {
            isDissolving = true;
        }
        
        private void SetValid() {
            plateformMaterials[0] = ghostValidMaterial;
            plateformMaterials[1] = ghostValidMaterial;
            _meshRenderer.materials = plateformMaterials;
        }

        private void SetUnvalid() {
            plateformMaterials[0] = ghostUnvalidMaterial;
            plateformMaterials[1] = ghostUnvalidMaterial;
            _meshRenderer.materials = plateformMaterials;
        }

        public void SetNormal() {
            _boxCollider.isTrigger = false;
            ResetMaterial();
        }

        private void ResetMaterial() {
            plateformMaterials[0] = plateformMaterial;
            plateformMaterials[1] = plateformMaterial;
            _meshRenderer.materials = plateformMaterials;
        }

        private void OnTriggerEnter(Collider other) {
            if (other.CompareTag("Banana")) {
                switch (other.GetComponent<Banana>().bananasDataScriptableObject.itemThrowableType) {
                    case ItemThrowableType.CAVENDISH:
                        plateformMaterials[0] = other.GetComponent<MeshRenderer>().sharedMaterials[0];
                        plateformMaterials[1] = emissiveMaterial;
                        _meshRenderer.materials = plateformMaterials;

                        GetComponent<UpDownEffect>().enabled = true;
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