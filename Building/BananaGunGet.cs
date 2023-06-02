using System.Collections.Generic;
using Building.Buildables;
using Data;
using Items;
using UnityEngine;
using UnityEngine.Localization.Settings;

namespace Building {
    public class BananaGunGet : MonoBehaviour {
        private Mesh _targetedGameObjectMesh;
        
        private ItemType _targetType;
        
        private ScriptableObjectManager _scriptableObjectManager;
        
        private Dictionary<AdvancementState, BuildableType[]> _buildableUnlockedByAdvancementState;

        private BananaGun bananaGun;

        private void Start() {
            _scriptableObjectManager = ObjectsReference.Instance.scriptableObjectManager;
            bananaGun = ObjectsReference.Instance.bananaGun;
            
            _buildableUnlockedByAdvancementState = new Dictionary<AdvancementState, BuildableType[]> {
                { AdvancementState.GRAB_DEBRIS_ON_MAP, new [] { BuildableType.PLATEFORM , BuildableType.FIRST_DOOR_LEFT, BuildableType.FIRST_DOOR_RIGHT, BuildableType.FIRST_CLOISON } },
                { AdvancementState.GRAB_BANANAS, new [] { BuildableType.BANANA_DRYER } }
            };
        }
        
        public void Harvest() {
            if (bananaGun._targetedGameObject == null) return;
            
            _targetedGameObjectMesh = bananaGun._targetedGameObject.GetComponent<MeshFilter>().sharedMesh;

            // Regime
            if (_targetedGameObjectMesh == _scriptableObjectManager._meshReferenceScriptableObject.matureBananaTree) {
                ObjectsReference.Instance.audioManager.PlayEffect(EffectType.GRAB_BANANAS, 0);
            
                var regimeClass = bananaGun._targetedGameObject.GetComponent<Regime>();
            
                var bananaType = regimeClass.bananasDataScriptableObject.itemType;
                var quantity = regimeClass.bananasDataScriptableObject.regimeQuantity;
            
                ObjectsReference.Instance.inventory.AddQuantity(ItemCategory.BANANA, bananaType, quantity);
            
                regimeClass.GrabBananas();
            
                TryAddBlueprintByAdvancementState(AdvancementState.GRAB_BANANAS);
            }
            
            else {
                // buildable
            if (_scriptableObjectManager.IsBuildable(_targetedGameObjectMesh)) {
                var buildableType = _scriptableObjectManager.GetBuildableTypeByMesh(_targetedGameObjectMesh);
                
                var craftingMaterials =
                    ObjectsReference.Instance.scriptableObjectManager.GetBuildableCraftingIngredients(buildableType);
            
                foreach (var craftingMaterial in craftingMaterials) {
                    ObjectsReference.Instance.inventory.AddQuantity(ItemCategory.RAW_MATERIAL, craftingMaterial.Key,
                        craftingMaterial.Value);
                }
            
                ObjectsReference.Instance.audioManager.StopAudioSource(AudioSourcesType.EFFECT);
                ObjectsReference.Instance.uiQueuedMessages.AddMessage("+ 1 " + LocalizationSettings.Instance
                    .GetStringDatabase().GetLocalizedString(buildableType.ToString().ToLower()));
            
                if (buildableType == BuildableType.BANANA_DRYER) GetComponent<BananasDryer>().RetrieveRawMaterials();
                
                Destroy(bananaGun._targetedGameObject);
                bananaGun._targetedGameObject = null;
            }
                    
                else {
                    // debris
                    if (_scriptableObjectManager.IsDebris(_targetedGameObjectMesh)) {
                        MapItems.Instance.uiCanvasItemsHiddableManager.RemoveCanva(bananaGun._targetedGameObject.GetComponentInChildren<Canvas>());
            
                        TryAddBlueprintByAdvancementState(AdvancementState.GRAB_DEBRIS_ON_MAP);
            
                        ObjectsReference.Instance.audioManager.PlayEffect(EffectType.DESINTEGRATION, 0);
                        
                        ObjectsReference.Instance.inventory.AddQuantity(ItemCategory.RAW_MATERIAL, ItemType.METAL, 2);
                        ObjectsReference.Instance.inventory.AddQuantity(ItemCategory.RAW_MATERIAL, ItemType.ELECTRONIC, 1);
                        ObjectsReference.Instance.audioManager.StopAudioSource(AudioSourcesType.EFFECT);
                        ObjectsReference.Instance.mapsManager.currentMap.RecalculateHappiness();
                        ObjectsReference.Instance.mapsManager.currentMap.isDiscovered = true;
                        
                        Destroy(bananaGun._targetedGameObject);
                        bananaGun._targetedGameObject = null;
                    }
                }
            }
        }
        
        private void TryAddBlueprintByAdvancementState(AdvancementState advancementState) {
            if (!ObjectsReference.Instance.gameData.bananaManSavedData.playerAdvancements.Contains(advancementState)) {
                ObjectsReference.Instance.gameData.bananaManSavedData.playerAdvancements.Add(advancementState);
                
                foreach (var buildableType in _buildableUnlockedByAdvancementState[advancementState]) {
                    ObjectsReference.Instance.uiBlueprints.SetVisible(buildableType);
                }
            }
        }
    }
}