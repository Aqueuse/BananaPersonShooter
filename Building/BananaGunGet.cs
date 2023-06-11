using Building.Buildables;
using Data;
using Enums;
using Items;
using UI.InGame.Chimployee;
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
            
            switch (bananaGun.targetedGameObject.tag) {
                case "Regime":
                    ObjectsReference.Instance.audioManager.PlayEffect(EffectType.GRAB_BANANAS, 0);
                    
                    var regimeClass = bananaGun.targetedGameObject.GetComponent<Regime>();
                    var bananaType = regimeClass.bananasDataScriptableObject.itemType;
                    var quantity = regimeClass.bananasDataScriptableObject.regimeQuantity;
            
                    ObjectsReference.Instance.inventory.AddQuantity(ItemCategory.BANANA, bananaType, quantity);
            
                    regimeClass.GrabBananas();
                    break;
                
                case "Buildable":
                    _targetedGameObjectMesh = bananaGun.targetedGameObject.GetComponent<MeshFilter>().sharedMesh;

                    ObjectsReference.Instance.audioManager.PlayEffect(EffectType.GRAB_BANANAS, 0);
                    var buildableType = _scriptableObjectManager.GetBuildableTypeByMesh(_targetedGameObjectMesh);
                
                    var craftingMaterials =
                        ObjectsReference.Instance.scriptableObjectManager.GetBuildableCraftingIngredients(buildableType);
            
                    foreach (var craftingMaterial in craftingMaterials) {
                        ObjectsReference.Instance.inventory.AddQuantity(ItemCategory.RAW_MATERIAL, craftingMaterial.Key,
                            craftingMaterial.Value);
                    }
                    
                    if (buildableType == BuildableType.BANANA_DRYER) GetComponent<BananasDryer>().RetrieveRawMaterials();

                    ObjectsReference.Instance.mapsManager.currentMap.isDiscovered = true;

                    Destroy(bananaGun.targetedGameObject);
                    bananaGun.targetedGameObject = null;
                    break;
                
                case "Debris":
                    ObjectsReference.Instance.audioManager.PlayEffect(EffectType.GRAB_BANANAS, 0);
                    MapItems.Instance.uiCanvasItemsHiddableManager.RemoveCanva(bananaGun.targetedGameObject.GetComponentInChildren<Canvas>());
                    
                    ObjectsReference.Instance.inventory.AddQuantity(ItemCategory.RAW_MATERIAL, ItemType.METAL, 2);
                    ObjectsReference.Instance.inventory.AddQuantity(ItemCategory.RAW_MATERIAL, ItemType.ELECTRONIC, 1);
                    ObjectsReference.Instance.mapsManager.currentMap.RecalculateHappiness();
                    ObjectsReference.Instance.mapsManager.currentMap.isDiscovered = true;
                    
                    Destroy(bananaGun.targetedGameObject);
                    bananaGun.targetedGameObject = null;
                    break;
                case "Ruine":
                    ObjectsReference.Instance.audioManager.PlayEffect(EffectType.GRAB_BANANAS, 0);
                    ObjectsReference.Instance.inventory.AddQuantity(ItemCategory.RAW_MATERIAL, ItemType.METAL, 2);
                    
                    Destroy(bananaGun.targetedGameObject);
                    bananaGun.targetedGameObject = null;
                    break;
                case "Monkeyman" :
                    ObjectsReference.Instance.mapsManager.currentMap.isDiscovered = true;
                    ObjectsReference.Instance.gameData.bananaManSavedData.playerAdvancements.Add(AdvancementState.GET_MONKEYMAN_IA);
                    
                    ObjectsReference.Instance.inventory.AddQuantity(ItemCategory.RAW_MATERIAL, ItemType.METAL, 1);
                    ObjectsReference.Instance.inventory.AddQuantity(ItemCategory.RAW_MATERIAL, ItemType.ELECTRONIC, 2);

                    Destroy(bananaGun.targetedGameObject.transform.parent.gameObject);
                    bananaGun.targetedGameObject = null;

                    ObjectsReference.Instance.uiManager.canvasGroupsByUICanvasType[UICanvasGroupType.BUILD_HELPER].alpha = 0f;
                    
                    ObjectsReference.Instance.uihud.Activate_Chimployee_Tab();
                    
                    ObjectsReference.Instance.uiManager.Show_Hide_interface();
                    ObjectsReference.Instance.uihud.Switch_To_Chimployee();
                    
                    ObjectsReference.Instance.uiChimployee.InitDialogue(ChimployeeDialogue.chimployee_first_interaction);
                    ObjectsReference.Instance.bananaGun.UngrabBananaGun();
                    break;
            }
        }
    }
}