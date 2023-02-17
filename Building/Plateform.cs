using Audio;
using Bananas;
using Building.PlateformsEffects;
using Enums;
using Game;
using UI.InGame;
using UnityEngine;
using UnityEngine.Localization.Settings;

namespace Building {
    public class Plateform : MonoBehaviour {
        [SerializeField] private Material plateformMaterial;
        [SerializeField] private Material ghostValidMaterial;
        [SerializeField] private Material ghostUnvalidMaterial;
        
        [SerializeField] private float dissolved;

        public bool isValid;

        private BoxCollider _boxCollider;
        private MeshRenderer _meshRenderer;
        
        private static readonly int DissolveProperty = Shader.PropertyToID("Cutoff_Height");
        private float _dissolve;

        private bool isDissolving;
        
        private void Start() {
            _boxCollider = GetComponent<BoxCollider>();
            _meshRenderer = GetComponent<MeshRenderer>();

            isValid = true;
            _dissolve = 1.5f;
        }

        private void Update() {
            if (isDissolving) {
                _dissolve -= Time.deltaTime;
                GetComponent<MeshRenderer>().materials[0].SetFloat(DissolveProperty, _dissolve);
                
                if (_dissolve < dissolved) {
                    Inventory.Instance.AddQuantity(ItemThrowableType.PLATEFORM, ItemThrowableCategory.PLATEFORM, 1);
                    AudioManager.Instance.StopAudioSource(AudioSourcesType.EFFECT);
                    // MapManager.Instance.Clean();
                    // MapManager.Instance.SaveDataOnScriptableObject();
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
            _meshRenderer.material = ghostValidMaterial;
        }

        private void SetUnvalid() {
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
