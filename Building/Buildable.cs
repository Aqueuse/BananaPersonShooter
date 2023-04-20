using Building.Plateforms;
using Enums;
using UnityEngine;
using UnityEngine.Localization.Settings;

namespace Building {
    public class Buildable : MonoBehaviour {
        [SerializeField] private Material ghostMaterial;
        [SerializeField] private Material normalMaterial;
        
        [SerializeField] private float dissolved;
        
        [SerializeField] private Color validColor;
        [SerializeField] private Color unvalidColor;
        [SerializeField] private Color unbuidableColor;

        [SerializeField] private BuildableType buildableType;

        private Material[] buildableMaterials;
        
        public bool isValid;
        public Vector3 initialPosition;

        private BoxCollider _boxCollider;
        private MeshRenderer _meshRenderer;
        private Material buildableInstanciatedMaterial;
        
        private static readonly int DissolveProperty = Shader.PropertyToID("Cutoff_Height");
        
        private float _step;
        private float _dissolve;
        private bool _isDissolving;

        private void Start() {
            _boxCollider = GetComponent<BoxCollider>();
            _meshRenderer = GetComponent<MeshRenderer>();

            buildableMaterials = new []{ghostMaterial};
            buildableInstanciatedMaterial = _meshRenderer.materials[0];

            isValid = true;
            _dissolve = 1.5f;
        }

        private void Update() {
            if (!_isDissolving) return;

            _dissolve -= Time.deltaTime;
            
            buildableInstanciatedMaterial.SetFloat(DissolveProperty, _dissolve);
            buildableMaterials[0] = buildableInstanciatedMaterial;
            _meshRenderer.materials = buildableMaterials;

            if (_dissolve < dissolved) {
                var craftingMaterials = ObjectsReference.Instance.scriptableObjectManager.GetBuildableCraftingIngredients(buildableType);

                foreach (var craftingMaterial in craftingMaterials) {
                    ObjectsReference.Instance.inventory.AddQuantity(ItemCategory.RAW_MATERIAL, craftingMaterial.Key, craftingMaterial.Value);
                }
                
                ObjectsReference.Instance.audioManager.StopAudioSource(AudioSourcesType.EFFECT);
                ObjectsReference.Instance.uiQueuedMessages.AddMessage(
                    "+ 1 "+
                    LocalizationSettings.Instance.GetStringDatabase().GetLocalizedString(buildableType.ToString().ToLower()));
                
                Destroy(gameObject);
            }
        }
        
        public void DissolveMe() {
            _isDissolving = true;
        }
        
        public void SetValid() {
            buildableMaterials = new []{ghostMaterial};
            _meshRenderer = GetComponent<MeshRenderer>();
            
            isValid = true;
            ghostMaterial.color = validColor;
            buildableMaterials[0] = ghostMaterial;
            _meshRenderer.materials = buildableMaterials;
        }

        private void SetUnvalid() {
            isValid = false;
            ghostMaterial.color = unvalidColor;
            buildableMaterials[0] = ghostMaterial;
            _meshRenderer.materials = buildableMaterials;
        }

        public void SetUnbuildable() {
            isValid = false;
            ghostMaterial.color = unbuidableColor;
            buildableMaterials[0] = ghostMaterial;
            _meshRenderer.materials = buildableMaterials;
        }
        
        public void SetNormal(BuildableType thisBuildableType) {
            isValid = true;
            _boxCollider.isTrigger = false;
            buildableMaterials[0] = normalMaterial;
            _meshRenderer.materials = buildableMaterials;

            buildableInstanciatedMaterial = _meshRenderer.materials[0];

            initialPosition = gameObject.transform.position;
            if (thisBuildableType == BuildableType.PLATEFORM) ObjectsReference.Instance.mapsManager.currentMap.RefreshPlateformsDataMap();
        }

        public void RespawnBuildable(ItemType respawnedPlateformType) {
            _boxCollider = GetComponent<BoxCollider>();
            _meshRenderer = GetComponent<MeshRenderer>();

            _boxCollider.isTrigger = false;

            buildableMaterials = new Material[1];

            if (buildableType == BuildableType.PLATEFORM) {
                GetComponent<Plateform>().plateformType = respawnedPlateformType;

                if (respawnedPlateformType == ItemType.EMPTY) GetComponent<Plateform>().SetUnactiveMaterial();
                else {
                    GetComponent<Plateform>().ActivePlateform(respawnedPlateformType);
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