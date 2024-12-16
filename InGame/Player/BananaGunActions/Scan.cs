using InGame.Interactions;
using InGame.Items.ItemsBehaviours.BuildablesBehaviours;
using InGame.Items.ItemsProperties;
using InGame.Items.ItemsProperties.Buildables;
using InGame.Items.ItemsProperties.Dropped;
using Tags;
using UnityEngine;

namespace InGame.Player.BananaGunActions {
    public class Scan : MonoBehaviour {
        public GameObject targetedGameObject;

        [SerializeField] private LayerMask GestionViewSelectableLayerMask;

        private Mesh _targetedGameObjectMesh;
        private BananaType _targetType;

        private GenericDictionary<RawMaterialType, int> rawMaterialsWithQuantity;

        private Camera mainCamera;

        private Ray ray;
        private RaycastHit raycastHit;

        private GenericDictionary<RawMaterialType, int> craftingMaterials;
        private Regime regimeClass;

        private Tag gameObjectTagClass;
        private GAME_OBJECT_TAG gameObjectTag;
        
        private void Start() {
            mainCamera = Camera.main;
        }
        
        private void Update() {
            if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out raycastHit, 2000, layerMask: GestionViewSelectableLayerMask)) {
                targetedGameObject = raycastHit.transform.gameObject;
                ObjectsReference.Instance.uInventoriesManager.GetCurrentUIHelper().ShowRetrieveConfirmation();
            }
            else {
                targetedGameObject = null;
                ObjectsReference.Instance.uInventoriesManager.GetCurrentUIHelper().ShowDefaultHelper();
                ObjectsReference.Instance.uiManager.SetActive(UICanvasGroupType.MAIN_PANEL, false);
            }
        }

        public void RepairOrHarvest() {
            if (targetedGameObject == null) return;

            gameObjectTagClass = targetedGameObject.GetComponent<Tag>();
            gameObjectTag = gameObjectTagClass.gameObjectTag;
            
            if (gameObjectTag == GAME_OBJECT_TAG.BUILDABLE) {
                if (targetedGameObject.TryGetComponent(out BuildableBehaviour buildableBehaviour)) {
                    if (buildableBehaviour.isBreaked) {
                        TryToRepairBuildable(buildableBehaviour);
                    }
                    
                    else {
                        harvest(gameObjectTagClass.itemScriptableObject);
                    }
                }
            }

            else {
                harvest(gameObjectTagClass.itemScriptableObject);
            }
        }

        public void harvest(ItemScriptableObject itemScriptableObject) {
            switch (gameObjectTag) {
                case GAME_OBJECT_TAG.REGIME:
                    regimeClass = targetedGameObject.GetComponent<Regime>();
                    if (regimeClass.regimeStade != RegimeStade.MATURE) return;
                    
                    ObjectsReference.Instance.BananaManBananasInventory.AddQuantity(
                        regimeClass.regimeDataScriptableObject.associatedBananasPropertiesScriptableObject.bananaType, 
                        regimeClass.regimeDataScriptableObject.regimeQuantity);

                    regimeClass.GrabBananas();

                    foreach (var monkey in ObjectsReference.Instance.worldData.monkeys) {
                        monkey.SearchForBananaManBananas();
                    }
                    
                    targetedGameObject = null;
                    break;

                case GAME_OBJECT_TAG.BUILDABLE:
                    var buildableType = gameObjectTagClass.itemScriptableObject.buildableType;

                    BuildablePropertiesScriptableObject buildableProperties =
                        (BuildablePropertiesScriptableObject)itemScriptableObject;

                    craftingMaterials = buildableProperties.rawMaterialsWithQuantity;

                    foreach (var craftingMaterial in craftingMaterials) {
                        ObjectsReference.Instance.bananaManRawMaterialInventory.AddQuantity(
                            craftingMaterial.Key, 
                            craftingMaterial.Value);
                    }

                    if (buildableType == BuildableType.BANANA_DRYER) 
                        targetedGameObject.GetComponent<BananasDryerBehaviour>().RetrieveRawMaterials();
                    
                    DestroyImmediate(targetedGameObject);
                    targetedGameObject = null;

                    break;

                case GAME_OBJECT_TAG.WASTE:
                    var _wastePropertiesScriptableObject = 
                        (WastePropertiesScriptableObject)gameObjectTagClass.itemScriptableObject;

                    rawMaterialsWithQuantity = _wastePropertiesScriptableObject.GetRawMaterialsWithQuantity();

                    foreach (var droppedRawMaterialIngredient in rawMaterialsWithQuantity) {
                        ObjectsReference.Instance.bananaManRawMaterialInventory.AddQuantity(
                            droppedRawMaterialIngredient.Key, 
                            droppedRawMaterialIngredient.Value);
                    }
                    
                    Destroy(targetedGameObject);
                    targetedGameObject = null;

                    break;
                    
                case GAME_OBJECT_TAG.INGREDIENT:
                    ObjectsReference.Instance.bananaManIngredientsInventory.AddQuantity(
                        gameObjectTagClass.itemScriptableObject.ingredientsType, 
                        1);
                    
                    Destroy(targetedGameObject);
                    targetedGameObject = null;

                    break;
                    
                case GAME_OBJECT_TAG.RAW_MATERIAL:
                    ObjectsReference.Instance.bananaManRawMaterialInventory.AddQuantity(
                        gameObjectTagClass.itemScriptableObject.rawMaterialType, 
                        1);
                    
                    Destroy(targetedGameObject);
                    targetedGameObject = null;

                    break;

                case GAME_OBJECT_TAG.BANANA:
                    ObjectsReference.Instance.BananaManBananasInventory.AddQuantity(
                        gameObjectTagClass.itemScriptableObject.bananaType, 
                        1);
                    
                    Destroy(targetedGameObject);
                    targetedGameObject = null;

                    break;
                    
                case GAME_OBJECT_TAG.MANUFACTURED_ITEM:
                    ObjectsReference.Instance.bananaManManufacturedItemsInventory.AddQuantity(
                        gameObjectTagClass.itemScriptableObject.manufacturedItemsType, 
                        1);
                    
                    Destroy(targetedGameObject);
                    targetedGameObject = null;

                    break;
                    
                case GAME_OBJECT_TAG.FOOD:
                    // TODO : add food
                    break;
            }
            
            ObjectsReference.Instance.uInventoriesManager.GetCurrentUIHelper().HideRetrieveConfirmation();
            ObjectsReference.Instance.audioManager.PlayEffect(SoundEffectType.TAKE_SOMETHING, 0);

            ObjectsReference.Instance.uiFlippers.RefreshDroppableQuantity();

            ObjectsReference.Instance.build.setGhostColor();
        }
        
        private void TryToRepairBuildable(BuildableBehaviour buildableBehaviour) {
            if (!ObjectsReference.Instance.bananaManRawMaterialInventory.HasCraftingIngredients(buildableBehaviour.buildableType)) return;
            
            var _craftingIngredients = ObjectsReference.Instance.meshReferenceScriptableObject.buildablePropertiesScriptableObjects[buildableBehaviour.buildableType].rawMaterialsWithQuantity;

            foreach (var craftingIngredient in _craftingIngredients) {
                ObjectsReference.Instance.bananaManRawMaterialInventory.RemoveQuantity(
                    craftingIngredient.Key, 
                    craftingIngredient.Value);
                
                ObjectsReference.Instance.uiQueuedMessages.RemoveFromInventory(
                    ObjectsReference.Instance.meshReferenceScriptableObject.rawMaterialPropertiesScriptableObjects[craftingIngredient.Key], 
                    craftingIngredient.Value
                );
                ObjectsReference.Instance.uiFlippers.RefreshActiveBuildableAvailability();
            }
            
            ObjectsReference.Instance.audioManager.PlayEffect(SoundEffectType.TAKE_SOMETHING, 0);
            
            buildableBehaviour.RepairBuildable();
        }
    }
}