using Audio;
using Bananas;
using Data;
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

        public PlateformType plateformType;
        public bool isValid;
        public Vector3 initialPosition;

        private BoxCollider _boxCollider;
        private MeshRenderer _meshRenderer;
        
        private static readonly int DissolveProperty = Shader.PropertyToID("Cutoff_Height");
        private Material[] _plateformMaterials;
        
        private float _step;
        private float _dissolve;
        private bool _isDissolving;

        private void Start() {
            _boxCollider = GetComponent<BoxCollider>();
            _meshRenderer = GetComponent<MeshRenderer>();

            _plateformMaterials = new Material[2];

            plateformType = PlateformType.INACTIVE;
            isValid = true;
            _dissolve = 1.5f;
        }

        private void Update() {
            if (!_isDissolving) return;
            
            _dissolve -= Time.deltaTime;
            _plateformMaterials = _meshRenderer.materials;
                
            _plateformMaterials[0].SetFloat(DissolveProperty, _dissolve);
            _plateformMaterials[1].SetFloat(DissolveProperty, _dissolve);
            _meshRenderer.materials = _plateformMaterials;

            if (_dissolve < dissolved) {
                Inventory.Instance.AddQuantity(ItemThrowableType.PLATEFORM, 1);
                AudioManager.Instance.StopAudioSource(AudioSourcesType.EFFECT);
                UIQueuedMessages.Instance.AddMessage(
                    "+ 1 "+
                    LocalizationSettings.Instance.GetStringDatabase().GetLocalizedString("platform"));
                
                Destroy(gameObject);
            }
        }
        
        public void DissolveMe() {
            _isDissolving = true;
        }
        
        private void SetValid() {
            _plateformMaterials[0] = ghostValidMaterial;
            _plateformMaterials[1] = ghostValidMaterial;
            _meshRenderer.materials = _plateformMaterials;
        }

        private void SetUnvalid() {
            _plateformMaterials[0] = ghostUnvalidMaterial;
            _plateformMaterials[1] = ghostUnvalidMaterial;
            _meshRenderer.materials = _plateformMaterials;
        }

        public void SetNormal() {
            _boxCollider.isTrigger = false;
            ResetMaterial();
            
            initialPosition = gameObject.transform.position;
            MapsManager.Instance.currentMap.RefreshPlateformsDataMap();
        }

        private void ResetMaterial() {
            _plateformMaterials[0] = plateformMaterial;
            _plateformMaterials[1] = plateformMaterial;
            _meshRenderer.materials = _plateformMaterials;
        }

        private void OnTriggerEnter(Collider other) {
            if (!other.CompareTag("Banana")) return;
            
            ActivePlateform(other.GetComponent<Banana>().bananasDataScriptableObject.itemThrowableType);
        }

        public void RespawnPlateform(PlateformType respawnedPlateformType) {
            _boxCollider = GetComponent<BoxCollider>();
            _meshRenderer = GetComponent<MeshRenderer>();

            _plateformMaterials = new Material[2];

            initialPosition = gameObject.transform.position;
            plateformType = respawnedPlateformType;
            
            _boxCollider.isTrigger = false;

            switch (plateformType) {
                case PlateformType.CAVENDISH:
                    _plateformMaterials[0] = ScriptableObjectManager.Instance.GetBananaScriptableObject(ItemThrowableType.CAVENDISH).bananaMaterial;
                    _plateformMaterials[1] = emissiveMaterial;
                    _meshRenderer.materials = _plateformMaterials;

                    GetComponent<UpDownEffect>().enabled = true;
                    GetComponent<AudioSource>().enabled = true;
                
                    break;
                
                case PlateformType.INACTIVE:
                    _plateformMaterials[0] = plateformMaterial;
                    _plateformMaterials[1] = plateformMaterial;
                    _meshRenderer.materials = _plateformMaterials;
                    break;
            }
        }

        private void ActivePlateform(ItemThrowableType itemThrowableType) {
            switch (itemThrowableType) {
                case ItemThrowableType.CAVENDISH:
                    _plateformMaterials[0] = ScriptableObjectManager.Instance.GetBananaScriptableObject(itemThrowableType).bananaMaterial;
                    _plateformMaterials[1] = emissiveMaterial;
                    _meshRenderer.materials = _plateformMaterials;

                    GetComponent<UpDownEffect>().enabled = true;
                    GetComponent<AudioSource>().enabled = true;
                    
                    plateformType = PlateformType.CAVENDISH;
                    MapsManager.Instance.currentMap.RefreshPlateformsDataMap();
                    break;
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

        private void OnDestroy() {
            if (MapsManager.Instance == null) return;
            MapsManager.Instance.currentMap.RefreshPlateformsDataMap();
        }
    }
}