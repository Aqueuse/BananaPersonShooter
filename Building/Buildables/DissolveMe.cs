using Enums;
using UnityEngine;
using UnityEngine.Localization.Settings;

namespace Building.Buildables {
    public class DissolveMe : MonoBehaviour {
        private float dissolved;
        
        private MeshRenderer[] _meshRenderers;
        private Material _buildableInstanciatedMaterial;

        private static readonly int AlphaProperty = Shader.PropertyToID("alpha");
        
        private float _step;
        private float _dissolve;
        private bool _isDissolving;
        
        private ItemCategory _itemCategory;
        private ItemType _itemType;
        private BuildableType _buildableType;
        
        private void Start() {
            _meshRenderers = GetComponents<MeshRenderer>();
            
            _buildableInstanciatedMaterial = _meshRenderers[0].materials[0];
            _dissolve = 1.5f;
        }

        private void Update() {
            if (!_isDissolving) return;

            _dissolve -= Time.deltaTime;

            _buildableInstanciatedMaterial.SetFloat(AlphaProperty, _dissolve);
            foreach (var meshRenderer in _meshRenderers) {
                meshRenderer.materials[0] = _buildableInstanciatedMaterial;
            }

            if (_dissolve < dissolved) {
                if (_itemCategory == ItemCategory.BUILDABLE) {
                    var craftingMaterials =
                        ObjectsReference.Instance.scriptableObjectManager.GetBuildableCraftingIngredients(_buildableType);

                    foreach (var craftingMaterial in craftingMaterials) {
                        ObjectsReference.Instance.inventory.AddQuantity(ItemCategory.RAW_MATERIAL, craftingMaterial.Key,
                            craftingMaterial.Value);
                    }

                    ObjectsReference.Instance.audioManager.StopAudioSource(AudioSourcesType.EFFECT);
                    ObjectsReference.Instance.uiQueuedMessages.AddMessage("+ 1 " + LocalizationSettings.Instance
                        .GetStringDatabase().GetLocalizedString(_buildableType.ToString().ToLower()));

                    if (_buildableType == BuildableType.BANANA_DRYER) GetComponent<BananasDryer>().RetrieveRawMaterials();
                }
                
                else {
                    Debug.Log(_itemCategory);
                    if (_itemCategory == ItemCategory.RAW_MATERIAL && _itemType == ItemType.DEBRIS) {
                        ObjectsReference.Instance.inventory.AddQuantity(ItemCategory.RAW_MATERIAL, ItemType.METAL, 2);
                        ObjectsReference.Instance.inventory.AddQuantity(ItemCategory.RAW_MATERIAL, ItemType.ELECTRONIC, 1);
                        ObjectsReference.Instance.audioManager.StopAudioSource(AudioSourcesType.EFFECT);
                        ObjectsReference.Instance.mapsManager.currentMap.RecalculateHappiness();
                        ObjectsReference.Instance.mapsManager.currentMap.isDiscovered = true;
                    }
                }

                Destroy(gameObject);
            }
        }

        public void Dissolve(ItemCategory itemCategory, BuildableType buildableType, ItemType itemType) {
            _itemCategory = itemCategory;
            _buildableType = buildableType;
            _itemType = itemType;
            _isDissolving = true;
        }
    }
}