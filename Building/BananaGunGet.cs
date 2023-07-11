using Building.Buildables;
using Data;
using Enums;
using Game.CommandRoomPanelControls;
using Items;
using Monkeys.Chimployee;
using UnityEngine;

namespace Building {
    public class BananaGunGet : MonoBehaviour {
        private ScriptableObjectManager _scriptableObjectManager;
        private BananaGun bananaGun;

        private Mesh _targetedGameObjectMesh;
        private ItemType _targetType;
        
        private void Start() {
            _scriptableObjectManager = ObjectsReference.Instance.scriptableObjectManager;
            bananaGun = ObjectsReference.Instance.bananaGun;
        }
        
        public void Harvest() {
            if (bananaGun.targetedGameObject == null || bananaGun.targetedGameObject.layer != 7) return;
            ObjectsReference.Instance.uiHelper.Hide_retrieve_confirmation();

            switch (bananaGun.targetedGameObject.tag) {
                case "Regime":
                    ObjectsReference.Instance.audioManager.PlayEffect(EffectType.TAKE_SOMETHING, 0);
                    
                    var regimeClass = bananaGun.targetedGameObject.GetComponent<Regime>();
                    var bananaType = regimeClass.bananasDataScriptableObject.itemType;
                    var quantity = regimeClass.bananasDataScriptableObject.regimeQuantity;
            
                    ObjectsReference.Instance.inventory.AddQuantity(ItemCategory.BANANA, bananaType, quantity);
            
                    regimeClass.GrabBananas();
                    
                    if (!ObjectsReference.Instance.gameData.bananaManSavedData.playerAdvancements.Contains(AdvancementState.GRAB_BANANAS)) {
                        ObjectsReference.Instance.gameData.bananaManSavedData.playerAdvancements.Add(AdvancementState.GRAB_BANANAS);
                        ObjectsReference.Instance.uIadvancements.SetAdvancementBanana(AdvancementState.FEED_MONKEY);
                    }
                    
                    break;
                
                case "Buildable":
                    ObjectsReference.Instance.mapsManager.currentMap.isDiscovered = true;

                    _targetedGameObjectMesh = bananaGun.targetedGameObject.GetComponent<MeshFilter>().sharedMesh;

                    ObjectsReference.Instance.audioManager.PlayEffect(EffectType.TAKE_SOMETHING, 0);
                    var buildableType = _scriptableObjectManager.GetBuildableTypeByMesh(_targetedGameObjectMesh);
                
                    var craftingMaterials =
                        ObjectsReference.Instance.scriptableObjectManager.GetBuildableCraftingIngredients(buildableType);
            
                    foreach (var craftingMaterial in craftingMaterials) {
                        ObjectsReference.Instance.inventory.AddQuantity(ItemCategory.RAW_MATERIAL, craftingMaterial.Key,
                            craftingMaterial.Value);
                    }
                    
                    if (buildableType == BuildableType.BANANA_DRYER) GetComponent<BananasDryer>().RetrieveRawMaterials();
                    
                    DestroyImmediate(bananaGun.targetedGameObject);
                    bananaGun.targetedGameObject = null;
                    
                    ObjectsReference.Instance.mapsManager.currentMap.RefreshAspirablesItemsDataMap();
                    
                    break;
                
                case "Debris":
                    ObjectsReference.Instance.mapsManager.currentMap.isDiscovered = true;

                    ObjectsReference.Instance.audioManager.PlayEffect(EffectType.TAKE_SOMETHING, 0);
                    MapItems.Instance.uiCanvasItemsHiddableManager.RemoveCanva(bananaGun.targetedGameObject.GetComponentInChildren<Canvas>());
                    
                    ObjectsReference.Instance.inventory.AddQuantity(ItemCategory.RAW_MATERIAL, ItemType.METAL, 2);
                    ObjectsReference.Instance.inventory.AddQuantity(ItemCategory.RAW_MATERIAL, ItemType.ELECTRONIC, 1);
                    ObjectsReference.Instance.mapsManager.currentMap.RecalculateHappiness();
                    
                    Destroy(bananaGun.targetedGameObject);
                    bananaGun.targetedGameObject = null;
                    
                    ObjectsReference.Instance.mapsManager.currentMap.RefreshAspirablesItemsDataMap();

                    break;
                case "Ruine":
                    ObjectsReference.Instance.mapsManager.currentMap.isDiscovered = true;

                    ObjectsReference.Instance.audioManager.PlayEffect(EffectType.TAKE_SOMETHING, 0);
                    ObjectsReference.Instance.inventory.AddQuantity(ItemCategory.RAW_MATERIAL, ItemType.METAL, 10);
                    
                    Destroy(bananaGun.targetedGameObject);
                    bananaGun.targetedGameObject = null;
                    
                    ObjectsReference.Instance.mapsManager.currentMap.RefreshAspirablesItemsDataMap();

                    break;
                case "Monkeyman" :
                    ObjectsReference.Instance.mapsManager.currentMap.isDiscovered = true;

                    ObjectsReference.Instance.gameData.bananaManSavedData.playerAdvancements.Add(AdvancementState.ASPIRE_SOMETHING);
                    CommandRoomControlPanelsManager.Instance.SetMiniChimpDialogue(AdvancementState.ASPIRE_SOMETHING);
                    
                    ObjectsReference.Instance.inventory.AddQuantity(ItemCategory.RAW_MATERIAL, ItemType.METAL, 8);
                    ObjectsReference.Instance.inventory.AddQuantity(ItemCategory.RAW_MATERIAL, ItemType.ELECTRONIC, 2);
                    
                    ObjectsReference.Instance.uihud.Activate_Chimployee_Tab();

                    ObjectsReference.Instance.chimployee.InitDialogue(ChimployeeDialogue.chimployee_first_interaction);
                    ObjectsReference.Instance.bananaGun.UngrabBananaGun();
                    
                    DestroyImmediate(bananaGun.targetedGameObject.transform.parent.gameObject);
                    bananaGun.targetedGameObject = null;

                    ObjectsReference.Instance.mapsManager.currentMap.RefreshAspirablesItemsDataMap();

                    break;
            }
        }
    }
}