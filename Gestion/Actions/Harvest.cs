using Data;
using Data.Wastes;
using Enums;
using Gestion.Buildables;
using Interactions;
using Tags;
using UnityEngine;

namespace Gestion.Actions {
    public class Harvest : MonoBehaviour {
        private Mesh _targetedGameObjectMesh;
        private Tag targetedGameObjectTag;
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
            
            if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out RaycastHit hit, 2000, layerMask: gestionMode.GestionModeSelectableLayerMask)) {
                gestionMode.targetedGameObject = hit.transform.gameObject;
                ObjectsReference.Instance.uInventoriesManager.GetCurrentUIHelper().ShowRetrieveConfirmation();

                targetedGameObjectTag = hit.transform.GetComponent<Tag>();
                
                if (targetedGameObjectTag.gameObjectTag == GAME_OBJECT_TAG.REGIME)
                    ObjectsReference.Instance.descriptionsManager.SetDescription(targetedGameObjectTag.itemScriptableObject, targetedGameObjectTag.gameObject);

                else {
                    ObjectsReference.Instance.descriptionsManager.SetDescription(targetedGameObjectTag.itemScriptableObject);
                }
            }
            else {
                gestionMode.targetedGameObject = null;
                ObjectsReference.Instance.uInventoriesManager.GetCurrentUIHelper().ShowDefaultHelper();
                ObjectsReference.Instance.descriptionsManager.HideAllPanels();
            }
        }

        public void harvest() {
            if (!gestionMode.isGestionModeActivated
                && !ObjectsReference.Instance.bananaMan.isGrabingBananaGun) return;
            
            if (gestionMode.targetedGameObject == null) return;
            Tag gameObjectTagClass = gestionMode.targetedGameObject.GetComponent<Tag>();
            GAME_OBJECT_TAG gameObjectTag = gameObjectTagClass.gameObjectTag;
            
            switch (gameObjectTag) {
                case GAME_OBJECT_TAG.REGIME:
                    var regimeClass = gestionMode.targetedGameObject.GetComponent<Regime>();
                    if (regimeClass.regimeStade != RegimeStade.MATURE) return;
                    
                    var quantity = regimeClass.regimeDataScriptableObject.regimeQuantity;

                    ObjectsReference.Instance.bananasInventory.AddQuantity(regimeClass.regimeDataScriptableObject.associatedBananasDataScriptableObject, quantity);
    
                    regimeClass.GrabBananas();
                    
                    foreach (var monkey in MapItems.Instance.monkeys) {
                        monkey.SearchForBananaManBananas();
                    }
                    break;

                case GAME_OBJECT_TAG.BUILDABLE:
                    var buildableType = gameObjectTagClass.itemScriptableObject.buildableType;

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

                    ObjectsReference.Instance.mapsManager.currentMap.RecalculateCleanliness();

                    Destroy(gestionMode.targetedGameObject);
                    break;
            }

            ObjectsReference.Instance.uInventoriesManager.GetCurrentUIHelper().HideRetrieveConfirmation();
            ObjectsReference.Instance.audioManager.PlayEffect(EffectType.TAKE_SOMETHING, 0);
            ObjectsReference.Instance.mapsManager.currentMap.isDiscovered = true;

            gestionMode.targetedGameObject = null;
            ObjectsReference.Instance.mapsManager.currentMap.RefreshItemsDataMap();
            ObjectsReference.Instance.quickSlotsManager.SetPlateformSlotAvailability();
            ObjectsReference.Instance.build.setGhostColor();
        }
    }
}