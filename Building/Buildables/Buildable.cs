using Building.Plateforms;
using Enums;
using UnityEngine;
using UnityEngine.Localization.Settings;

namespace Building.Buildables {
    public class Buildable : MonoBehaviour {
        [SerializeField] private float dissolved;
        [SerializeField] private BuildableType buildableType;
        private Material[] _buildableMaterials;
        
        private BoxCollider _boxCollider;
        private MeshRenderer _meshRenderer;
        private Material _buildableInstanciatedMaterial;

        private static readonly int DissolveProperty = Shader.PropertyToID("Cutoff_Height");

        private float _step;
        private float _dissolve;
        private bool _isDissolving;

        private void Start() {
            _boxCollider = GetComponent<BoxCollider>();
            _meshRenderer = GetComponent<MeshRenderer>();

            _buildableInstanciatedMaterial = _meshRenderer.materials[0];
            _buildableMaterials = new []{_buildableInstanciatedMaterial};

            _dissolve = 1.5f;
        }

        private void Update() {
            if (!_isDissolving) return;

            _dissolve -= Time.deltaTime;
            
            _buildableInstanciatedMaterial.SetFloat(DissolveProperty, _dissolve);
            _buildableMaterials[0] = _buildableInstanciatedMaterial;
            _meshRenderer.materials = _buildableMaterials;

            if (_dissolve < dissolved) {
                var craftingMaterials = ObjectsReference.Instance.scriptableObjectManager.GetBuildableCraftingIngredients(buildableType);

                foreach (var craftingMaterial in craftingMaterials) {
                    ObjectsReference.Instance.inventory.AddQuantity(ItemCategory.RAW_MATERIAL, craftingMaterial.Key, craftingMaterial.Value);
                }
                
                ObjectsReference.Instance.audioManager.StopAudioSource(AudioSourcesType.EFFECT);
                ObjectsReference.Instance.uiQueuedMessages.AddMessage("+ 1 "+ LocalizationSettings.Instance.GetStringDatabase().GetLocalizedString(buildableType.ToString().ToLower()));

                if (buildableType == BuildableType.BANANA_DRYER) GetComponent<BananasDryer>().RetrieveRawMaterials();
                
                Destroy(gameObject);
            }
        }
        
        public void DissolveMe() {
            _isDissolving = true;
        }
        
        public void RespawnBuildable(ItemType respawnedItemType) {
            _boxCollider = GetComponent<BoxCollider>();
            _meshRenderer = GetComponent<MeshRenderer>();

            _boxCollider.isTrigger = false;

            _buildableMaterials = new Material[1];

            if (buildableType == BuildableType.PLATEFORM) {
                GetComponent<Plateform>().plateformType = respawnedItemType;

                if (respawnedItemType == ItemType.EMPTY) GetComponent<Plateform>().SetUnactiveMaterial();
                else {
                    GetComponent<Plateform>().ActivePlateform(respawnedItemType);
                }
            }
        }
    }
}