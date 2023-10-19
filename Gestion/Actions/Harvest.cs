using System;
using Data;
using Data.Waste;
using Enums;
using Gestion.Buildables;
using Interactions;
using Tags;
using UnityEngine;

namespace Gestion.Actions {
    public class Harvest : MonoBehaviour {
        private Mesh _targetedGameObjectMesh;
        private BananaType _targetType;
        private GestionMode gestionMode;

        private GenericDictionary<RawMaterialType, int> rawMaterialsWithQuantity;
        private ScriptableObjectManager scriptableObjectManager;
        private WasteDataScriptableObject wasteDataScriptableObject;

        public bool isDirectHarvestActivated;
        
        private Camera mainCamera;
        
        private void Start() {
            mainCamera = Camera.main;
            scriptableObjectManager = ObjectsReference.Instance.scriptableObjectManager;
            gestionMode = ObjectsReference.Instance.gestionMode;
        }

        private void Update() {
            if (!isDirectHarvestActivated) return;
            
            if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out RaycastHit hit, Single.PositiveInfinity, layerMask: gestionMode.GestionModeSelectableLayerMask)) {
                gestionMode.targetedGameObject = hit.transform.gameObject;
                ObjectsReference.Instance.uInventoriesManager.GetCurrentUIHelper().Show_retrieve_confirmation();
            }
            else {
                gestionMode.targetedGameObject = null;
                ObjectsReference.Instance.uInventoriesManager.GetCurrentUIHelper().show_default_helper();
            }
        }

        public void harvest() {
            if (gestionMode.targetedGameObject == null) return;

            ObjectsReference.Instance.uInventoriesManager.GetCurrentUIHelper().Hide_retrieve_confirmation();
            ObjectsReference.Instance.audioManager.PlayEffect(EffectType.TAKE_SOMETHING, 0);
            ObjectsReference.Instance.mapsManager.currentMap.isDiscovered = true;

            GAME_OBJECT_TAG gameObjectTag = gestionMode.targetedGameObject.GetComponent<Tag>().gameObjectTag;

            switch (gameObjectTag) {
                case GAME_OBJECT_TAG.REGIME:
                    var regimeClass = gestionMode.targetedGameObject.GetComponent<Regime>();
                    var quantity = regimeClass.regimeDataScriptableObject.regimeQuantity;

                    ObjectsReference.Instance.bananasInventory.AddQuantity(regimeClass.regimeDataScriptableObject.associatedBananasDataScriptableObject, quantity);
    
                    regimeClass.GrabBananas();
                    break;

                case GAME_OBJECT_TAG.BUILDABLE:
                    _targetedGameObjectMesh = gestionMode.targetedGameObject.GetComponent<MeshFilter>().sharedMesh;

                    var buildableType = scriptableObjectManager.GetBuildableTypeByMesh(_targetedGameObjectMesh);

                    var craftingMaterials = scriptableObjectManager.GetBuildableCraftingIngredients(buildableType);

                    foreach (var craftingMaterial in craftingMaterials) {
                        ObjectsReference.Instance.rawMaterialsInventory.AddQuantity(craftingMaterial.Key, craftingMaterial.Value);
                    }

                    if (buildableType == BuildableType.BANANA_DRYER) GetComponent<BananasDryer>().RetrieveRawMaterials();

                    DestroyImmediate(gestionMode.targetedGameObject);
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

                    Destroy(gestionMode.targetedGameObject);
                    break;
                case GAME_OBJECT_TAG.RUINE:
                    wasteDataScriptableObject = (WasteDataScriptableObject)scriptableObjectManager._meshReferenceScriptableObject.gameObjectDataScriptableObjectsByTag[gameObjectTag];
                    rawMaterialsWithQuantity = wasteDataScriptableObject.GetRawMaterialsWithQuantity();

                    foreach (var debrisRawMaterialIngredient in rawMaterialsWithQuantity) {
                        ObjectsReference.Instance.rawMaterialsInventory.AddQuantity(debrisRawMaterialIngredient.Key, debrisRawMaterialIngredient.Value);
                    }
                    
                    Destroy(gestionMode.targetedGameObject);
                    break;
            }
            
            gestionMode.targetedGameObject = null;
            ObjectsReference.Instance.mapsManager.currentMap.RefreshItemsDataMap();
        }
    }
}