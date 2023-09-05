using Building.Buildables;
using Data;
using Data.Waste;
using Enums;
using Interactions;
using Tags;
using UnityEngine;

namespace Building {
    public class BananaGunGet : MonoBehaviour {
        private Mesh _targetedGameObjectMesh;
        private BananaType _targetType;
        private BananaGun bananaGun;

        private GenericDictionary<RawMaterialType, int> rawMaterialsWithQuantity;
        private ScriptableObjectManager scriptableObjectManager;
        private WasteDataScriptableObject wasteDataScriptableObject;

        private void Start() {
            scriptableObjectManager = ObjectsReference.Instance.scriptableObjectManager;
            bananaGun = ObjectsReference.Instance.bananaGun;
        }

        public void Harvest() {
            if (bananaGun.targetedGameObject == null || bananaGun.targetedGameObject.layer != 11) return;

            ObjectsReference.Instance.uInventoriesManager.GetCurrentUIHelper().Hide_retrieve_confirmation();
            ObjectsReference.Instance.audioManager.PlayEffect(EffectType.TAKE_SOMETHING, 0);
            ObjectsReference.Instance.mapsManager.currentMap.isDiscovered = true;

            GAME_OBJECT_TAG gameObjectTag = bananaGun.targetedGameObject.GetComponent<Tag>().gameObjectTag;

            switch (gameObjectTag) {
                case GAME_OBJECT_TAG.REGIME:
                    var regimeClass = bananaGun.targetedGameObject.GetComponent<Regime>();
                    var quantity = regimeClass.regimeDataScriptableObject.regimeQuantity;

                    ObjectsReference.Instance.bananasInventory.AddQuantity(regimeClass.regimeDataScriptableObject.associatedBananasDataScriptableObject, quantity);

                    regimeClass.GrabBananas();
                    break;

                case GAME_OBJECT_TAG.BUILDABLE:
                    _targetedGameObjectMesh = bananaGun.targetedGameObject.GetComponent<MeshFilter>().sharedMesh;

                    var buildableType = scriptableObjectManager.GetBuildableTypeByMesh(_targetedGameObjectMesh);

                    var craftingMaterials = scriptableObjectManager.GetBuildableCraftingIngredients(buildableType);

                    foreach (var craftingMaterial in craftingMaterials) {
                        ObjectsReference.Instance.rawMaterialsInventory.AddQuantity(craftingMaterial.Key, craftingMaterial.Value);
                    }

                    if (buildableType == BuildableType.BANANA_DRYER) GetComponent<BananasDryer>().RetrieveRawMaterials();

                    DestroyImmediate(bananaGun.targetedGameObject);
                    break;
                
                case GAME_OBJECT_TAG.DEBRIS:
                    wasteDataScriptableObject = (WasteDataScriptableObject)scriptableObjectManager._meshReferenceScriptableObject.gameObjectDataScriptableObjectsByTag[GAME_OBJECT_TAG.DEBRIS];
                    rawMaterialsWithQuantity = wasteDataScriptableObject.GetRawMaterialsWithQuantity();

                    foreach (var debrisRawMaterialIngredient in rawMaterialsWithQuantity) {
                        ObjectsReference.Instance.rawMaterialsInventory.AddQuantity(debrisRawMaterialIngredient.Key, debrisRawMaterialIngredient.Value);
                    }

                    foreach (var monkey in MapItems.Instance.monkeys) {
                        ObjectsReference.Instance.mapsManager.currentMap.RecalculateHappiness(monkey);
                    }

                    Destroy(bananaGun.targetedGameObject);
                    break;
                case GAME_OBJECT_TAG.RUINE:
                    wasteDataScriptableObject = (WasteDataScriptableObject)scriptableObjectManager._meshReferenceScriptableObject.gameObjectDataScriptableObjectsByTag[gameObjectTag];
                    rawMaterialsWithQuantity = wasteDataScriptableObject.GetRawMaterialsWithQuantity();

                    foreach (var debrisRawMaterialIngredient in rawMaterialsWithQuantity) {
                        ObjectsReference.Instance.rawMaterialsInventory.AddQuantity(debrisRawMaterialIngredient.Key, debrisRawMaterialIngredient.Value);
                    }
                    
                    Destroy(bananaGun.targetedGameObject);
                    break;
            }
            
            bananaGun.targetedGameObject = null;
            ObjectsReference.Instance.mapsManager.currentMap.RefreshAspirablesItemsDataMap();
        }
    }
}