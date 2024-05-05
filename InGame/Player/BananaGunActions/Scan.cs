using InGame.Interactions;
using InGame.Items.ItemsBehaviours.BuildablesBehaviours;
using InGame.Items.ItemsProperties.Wastes;
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

        public bool isScanning;
        
        private void Start() {
            mainCamera = Camera.main;
        }
        
        private void Update() {
            if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out raycastHit, 2000, layerMask: GestionViewSelectableLayerMask)) {

                targetedGameObject = raycastHit.transform.gameObject;
                ObjectsReference.Instance.uInventoriesManager.GetCurrentUIHelper().ShowRetrieveConfirmation();

                if (isScanning) {
                    isScanning = false;
                    ObjectsReference.Instance.uiDescriptionsManager.SetDescription(raycastHit.transform.GetComponent<Tag>().itemScriptableObject, raycastHit.transform.gameObject);
                }
            }
            else {
                targetedGameObject = null;
                ObjectsReference.Instance.uInventoriesManager.GetCurrentUIHelper().ShowDefaultHelper();
                ObjectsReference.Instance.uiDescriptionsManager.HideAllPanels();
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
                        harvest();
                    }
                }
            }

            if (gameObjectTag == GAME_OBJECT_TAG.DEBRIS || gameObjectTag == GAME_OBJECT_TAG.REGIME) {
                harvest();
            }
        }

        private void harvest() {
            switch (gameObjectTag) {
                case GAME_OBJECT_TAG.REGIME:
                    regimeClass = targetedGameObject.GetComponent<Regime>();
                    if (regimeClass.regimeStade != RegimeStade.MATURE) return;

                    var quantity = regimeClass.regimeDataScriptableObject.regimeQuantity;

                    ObjectsReference.Instance.bananasInventory.AddQuantity(regimeClass.regimeDataScriptableObject.associatedBananasPropertiesScriptableObject, quantity);

                    regimeClass.GrabBananas();

                    foreach (var monkey in ObjectsReference.Instance.worldData.monkeys) {
                        monkey.SearchForBananaManBananas();
                    }
                    
                    ObjectsReference.Instance.uInventoriesManager.GetCurrentUIHelper().HideRetrieveConfirmation();
                    ObjectsReference.Instance.audioManager.PlayEffect(SoundEffectType.TAKE_SOMETHING, 0);

                    targetedGameObject = null;
                    break;

                case GAME_OBJECT_TAG.BUILDABLE:
                    var buildableType = gameObjectTagClass.itemScriptableObject.buildableType;

                    craftingMaterials = ObjectsReference.Instance.meshReferenceScriptableObject.buildablePropertiesScriptableObjects[buildableType].rawMaterialsWithQuantity;

                    foreach (var craftingMaterial in craftingMaterials) {
                        ObjectsReference.Instance.rawMaterialsInventory.AddQuantity(craftingMaterial.Key, craftingMaterial.Value);
                    }

                    if (buildableType == BuildableType.BANANA_DRYER) targetedGameObject.GetComponent<BananasDryerBehaviour>().RetrieveRawMaterials();
                    
                    DestroyImmediate(targetedGameObject);
                    
                    ObjectsReference.Instance.uInventoriesManager.GetCurrentUIHelper().HideRetrieveConfirmation();
                    ObjectsReference.Instance.audioManager.PlayEffect(SoundEffectType.TAKE_SOMETHING, 0);

                    targetedGameObject = null;

                    ObjectsReference.Instance.build.setGhostColor();

                    break;

                case GAME_OBJECT_TAG.DEBRIS:
                    var _wastePropertiesScriptableObject = (WastePropertiesScriptableObject)gameObjectTagClass.itemScriptableObject;
                    
                    rawMaterialsWithQuantity = _wastePropertiesScriptableObject.GetRawMaterialsWithQuantity();

                    foreach (var debrisRawMaterialIngredient in rawMaterialsWithQuantity) {
                        ObjectsReference.Instance.rawMaterialsInventory.AddQuantity(debrisRawMaterialIngredient.Key, debrisRawMaterialIngredient.Value);
                    }
                    
                    Destroy(targetedGameObject);
                    
                    ObjectsReference.Instance.uInventoriesManager.GetCurrentUIHelper().HideRetrieveConfirmation();
                    ObjectsReference.Instance.audioManager.PlayEffect(SoundEffectType.TAKE_SOMETHING, 0);

                    targetedGameObject = null;

                    ObjectsReference.Instance.build.setGhostColor();

                    break;
            }
        }
        
        private void TryToRepairBuildable(BuildableBehaviour buildableBehaviour) {
            if (!ObjectsReference.Instance.rawMaterialsInventory.HasCraftingIngredients(buildableBehaviour.buildableType)) return;
            
            var _craftingIngredients = ObjectsReference.Instance.meshReferenceScriptableObject.buildablePropertiesScriptableObjects[buildableBehaviour.buildableType].rawMaterialsWithQuantity;

            foreach (var craftingIngredient in _craftingIngredients) {
                ObjectsReference.Instance.rawMaterialsInventory.RemoveQuantity(craftingIngredient.Key, craftingIngredient.Value);
            }
            
            ObjectsReference.Instance.audioManager.PlayEffect(SoundEffectType.TAKE_SOMETHING, 0);
            
            buildableBehaviour.RepairBuildable();
        }
    }
}