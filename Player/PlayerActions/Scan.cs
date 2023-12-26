using Data;
using Gestion.BuildablesBehaviours;
using Interactions;
using ItemsProperties.Wastes;
using Tags;
using UnityEngine;

namespace Player.PlayerActions {
    public class Scan : MonoBehaviour {
        public GameObject targetedGameObject;

        [SerializeField] private LayerMask GestionViewSelectableLayerMask;
        
        private Mesh _targetedGameObjectMesh;
        private BananaType _targetType;

        private GenericDictionary<RawMaterialType, int> rawMaterialsWithQuantity;
        
        private Camera mainCamera;
        
        private Ray ray;
        private RaycastHit raycastHit;
        
        private void Start() {
            mainCamera = Camera.main;
        }

        private void Update() {
            if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out raycastHit, 2000, layerMask: GestionViewSelectableLayerMask)) {
                targetedGameObject = raycastHit.transform.gameObject;
                ObjectsReference.Instance.uInventoriesManager.GetCurrentUIHelper().ShowRetrieveConfirmation();
                
                ObjectsReference.Instance.descriptionsManager.SetDescription(raycastHit.transform.GetComponent<Tag>().itemScriptableObject, raycastHit.transform.gameObject);
            }
            else {
                targetedGameObject = null;
                ObjectsReference.Instance.uInventoriesManager.GetCurrentUIHelper().ShowDefaultHelper();
                ObjectsReference.Instance.descriptionsManager.HideAllPanels();
            }
        }

        public void harvest() {
            if (targetedGameObject == null) return;
            
            Tag gameObjectTagClass = targetedGameObject.GetComponent<Tag>();
            GAME_OBJECT_TAG gameObjectTag = gameObjectTagClass.gameObjectTag;
            
            switch (gameObjectTag) {
                case GAME_OBJECT_TAG.REGIME:
                    var regimeClass = targetedGameObject.GetComponent<Regime>();
                    if (regimeClass.regimeStade != RegimeStade.MATURE) return;

                    var quantity = regimeClass.regimeDataScriptableObject.regimeQuantity;

                    ObjectsReference.Instance.bananasInventory.AddQuantity(regimeClass.regimeDataScriptableObject.associatedBananasPropertiesScriptableObject, quantity);

                    regimeClass.GrabBananas();

                    foreach (var monkey in Map.Instance.monkeysInMap) {
                        monkey.SearchForBananaManBananas();
                    }
                    
                    ObjectsReference.Instance.uInventoriesManager.GetCurrentUIHelper().HideRetrieveConfirmation();
                    ObjectsReference.Instance.audioManager.PlayEffect(SoundEffectType.TAKE_SOMETHING, 0);
                    ObjectsReference.Instance.gameData.currentMapData.isDiscovered = true;

                    targetedGameObject = null;
                    break;

                case GAME_OBJECT_TAG.BUILDABLE:
                    var buildableType = gameObjectTagClass.itemScriptableObject.buildableType;

                    var craftingMaterials = ObjectsReference.Instance.meshReferenceScriptableObject.buildablePropertiesScriptableObjects[buildableType].rawMaterialsWithQuantity;

                    foreach (var craftingMaterial in craftingMaterials) {
                        ObjectsReference.Instance.rawMaterialsInventory.AddQuantity(craftingMaterial.Key, craftingMaterial.Value);
                    }

                    if (buildableType == BuildableType.BANANA_DRYER) targetedGameObject.GetComponent<BananasDryerBehaviour>().RetrieveRawMaterials();
                    
                    DestroyImmediate(targetedGameObject);
                    
                    ObjectsReference.Instance.uInventoriesManager.GetCurrentUIHelper().HideRetrieveConfirmation();
                    ObjectsReference.Instance.audioManager.PlayEffect(SoundEffectType.TAKE_SOMETHING, 0);
                    ObjectsReference.Instance.gameData.currentMapData.isDiscovered = true;

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
                    ObjectsReference.Instance.gameData.currentMapData.isDiscovered = true;

                    targetedGameObject = null;

                    ObjectsReference.Instance.build.setGhostColor();

                    break;
            }
        }
        
        public void RepairBuildable() {
            if (targetedGameObject == null) return;

            if (targetedGameObject.TryGetComponent(out BuildableBehaviour buildableBehaviour)) {
                if (!ObjectsReference.Instance.rawMaterialsInventory.HasCraftingIngredients(buildableBehaviour.buildableType))
                    return;
                
                var _craftingIngredients = ObjectsReference.Instance.meshReferenceScriptableObject.buildablePropertiesScriptableObjects[buildableBehaviour.buildableType].rawMaterialsWithQuantity;

                foreach (var craftingIngredient in _craftingIngredients) {
                    ObjectsReference.Instance.rawMaterialsInventory.RemoveQuantity(craftingIngredient.Key, craftingIngredient.Value);
                }
                
                ObjectsReference.Instance.audioManager.PlayEffect(SoundEffectType.TAKE_SOMETHING, 0);
                
                buildableBehaviour.RepairBuildable();
            }
        }
    }
}