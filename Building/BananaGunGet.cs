using Building.Buildables;
using Enums;
using Game.CommandRoomPanelControls;
using Items;
using UnityEngine;

namespace Building {
    public class BananaGunGet : MonoBehaviour {
        private BuildablesManager buildablesManager;
        private BananaGun bananaGun;

        private Mesh _targetedGameObjectMesh;
        private ItemType _targetType;
        
        private void Start() {
            buildablesManager = ObjectsReference.Instance.buildablesManager;
            bananaGun = ObjectsReference.Instance.bananaGun;
        }
        
        public void Harvest() {
            if (bananaGun.targetedGameObject == null || bananaGun.targetedGameObject.layer != 7) return;
            ObjectsReference.Instance.uihud.GetCurrentUIHelper().Hide_retrieve_confirmation();
            ObjectsReference.Instance.audioManager.PlayEffect(EffectType.TAKE_SOMETHING, 0);
            ObjectsReference.Instance.mapsManager.currentMap.isDiscovered = true;

            switch (bananaGun.targetedGameObject.tag) {
                case "Regime":
                    var regimeClass = bananaGun.targetedGameObject.GetComponent<Regime>();
                    var bananaType = regimeClass.bananasDataScriptableObject.itemType;
                    var quantity = regimeClass.bananasDataScriptableObject.regimeQuantity;
            
                    ObjectsReference.Instance.inventory.AddQuantity(ItemCategory.BANANA, bananaType, quantity);
            
                    regimeClass.GrabBananas();
                    break;

                case "Buildable":
                    _targetedGameObjectMesh = bananaGun.targetedGameObject.GetComponent<MeshFilter>().sharedMesh;

                    var buildableType = buildablesManager.GetBuildableTypeByMesh(_targetedGameObjectMesh);
                    var craftingMaterials = buildablesManager.GetBuildableCraftingIngredients(buildableType);

                    foreach (var craftingMaterial in craftingMaterials) {
                        ObjectsReference.Instance.inventory.AddQuantity(ItemCategory.RAW_MATERIAL, craftingMaterial.Key, craftingMaterial.Value);
                    }

                    if (buildableType == BuildableType.BANANA_DRYER) GetComponent<BananasDryer>().RetrieveRawMaterials();

                    DestroyImmediate(bananaGun.targetedGameObject);
                    bananaGun.targetedGameObject = null;
                    
                    ObjectsReference.Instance.mapsManager.currentMap.RefreshAspirablesItemsDataMap();
                    
                    break;
                
                case "Debris":
                    MapItems.Instance.uiCanvasItemsHiddableManager.RemoveSpriteRenderer(bananaGun.targetedGameObject.GetComponentInChildren<SpriteRenderer>());

                    foreach (var buildableCraftingIngredient in buildablesManager.GetBuildableCraftingIngredients(BuildableType.PLATEFORM)) {
                        ObjectsReference.Instance.inventory.AddQuantity(ItemCategory.RAW_MATERIAL, buildableCraftingIngredient.Key, buildableCraftingIngredient.Value);
                    }

                    if (Random.Range(0, 5) == 4) {
                        ObjectsReference.Instance.inventory.AddQuantity(ItemCategory.RAW_MATERIAL, ItemType.BATTERY, 1);                        
                    }

                    ObjectsReference.Instance.mapsManager.currentMap.RecalculateHappiness();

                    Destroy(bananaGun.targetedGameObject);
                    bananaGun.targetedGameObject = null;

                    ObjectsReference.Instance.mapsManager.currentMap.RefreshAspirablesItemsDataMap();

                    break;
                case "Ruine":
                    ObjectsReference.Instance.inventory.AddQuantity(ItemCategory.RAW_MATERIAL, ItemType.METAL, 10);

                    Destroy(bananaGun.targetedGameObject);
                    bananaGun.targetedGameObject = null;

                    ObjectsReference.Instance.mapsManager.currentMap.RefreshAspirablesItemsDataMap();

                    break;
            }
        }
    }
}