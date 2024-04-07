using System;
using InGame.Interactions;
using InGame.Items.ItemsBehaviours.BuildablesBehaviours;
using InGame.Items.ItemsProperties.Buildables;
using InGame.Items.ItemsProperties.Wastes;
using Tags;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InGame.Items.ItemsBehaviours {
    public class GestionBuild : MonoBehaviour {
        [SerializeField] private LayerMask buildingLayerMask;
        [SerializeField] private LayerMask GestionViewSelectableLayerMask;

        private Camera mainCamera;
        
        public GameObject targetedGameObject;

        public GameObject _activeGhost;
        private Ghost _activeGhostClass;
        
        private GenericDictionary<RawMaterialType, int> rawMaterialsWithQuantity;

        private Ray ray;
        private RaycastHit raycastHit;
        
        private Quaternion _normalRotation;
        private Vector3 _ghostPosition;
        private Vector3 customRotation;
        private Vector3 ghostRotationEuler;
        private Quaternion ghostRotation;

        private Transform pivotTransform;
        private Vector3 pivotTransformPosition;
        private Quaternion pivotTransformRotation;
        
        private Vector3 pivotLocalePosition;
        private Quaternion pivotLocaleRotation;
        private Vector3 raycastHitPointLocalePosition;
        private float deltaZ;
        private float deltaY;

        private Vector3 offsettedPosition;
        private Vector3 raycastHitPoint;

        private GameObject _buildable;

        private void Start() {
            mainCamera = Camera.main;
        }

        private void Update() {
            ray = mainCamera.ScreenPointToRay(Mouse.current.position.value);
            
            if (Physics.Raycast(ray, out raycastHit, Single.PositiveInfinity, layerMask: GestionViewSelectableLayerMask)) {
                targetedGameObject = raycastHit.transform.gameObject;
                ObjectsReference.Instance.descriptionsManager.SetDescription(raycastHit.transform.GetComponent<Tag>().itemScriptableObject, raycastHit.transform.gameObject);
            }
            else {
                targetedGameObject = null;
            }
            
            if (_activeGhost == null) return;
            
            if(Physics.Raycast(ray, out raycastHit, 2000, layerMask:buildingLayerMask)) {
                _activeGhost.transform.position = raycastHit.point;
            }
        }

        public void ActivateGhostByScriptableObject(BuildablePropertiesScriptableObject buildablePropertiesScriptableObject) {
            _activeGhost = ObjectsReference.Instance.ghostsReference.GetGhostByBuildableType(buildablePropertiesScriptableObject.buildableType);
            _activeGhostClass = _activeGhost.GetComponent<Ghost>();
            
            if (ObjectsReference.Instance.rawMaterialsInventory.HasCraftingIngredients(buildablePropertiesScriptableObject)) {
                _activeGhostClass.SetGhostState(GhostState.VALID);
                ObjectsReference.Instance.uInventoriesManager.GetCurrentUIHelper().ShowNormalPlaceHelper();
            }

            ghostRotationEuler = _activeGhost.transform.rotation.eulerAngles;
        }
        
        private void CancelGhost() {
            if (_activeGhost != null) _activeGhost.transform.position = ObjectsReference.Instance.ghostsReference.transform.position;
            
            _activeGhost = null;
        }

        public void RotateGhost(Vector3 rotationVector) {
            if (_activeGhost == null) return;

            ghostRotationEuler += rotationVector * 15f;

            ghostRotationEuler.y %= 360;
            ghostRotationEuler.z %= 360;

            ghostRotation = Quaternion.Euler(ghostRotationEuler);
            _activeGhost.transform.rotation = ghostRotation;
        }

        public void ValidateBuildable() {
            if (_activeGhost == null) return;

            if (_activeGhostClass.GetGhostState() == GhostState.VALID) {
                _buildable = Instantiate(original: _activeGhostClass.buildablePropertiesScriptableObject.buildablePrefab,
                    position: _activeGhost.transform.position, rotation: _activeGhost.transform.rotation);

                _buildable.transform.parent = ObjectsReference.Instance.gameSave.buildablesSave.buildablesContainer.transform;

                var _craftingIngredients = _activeGhostClass.buildablePropertiesScriptableObject.rawMaterialsWithQuantity;

                foreach (var craftingIngredient in _craftingIngredients) {
                    ObjectsReference.Instance.rawMaterialsInventory.RemoveQuantity(craftingIngredient.Key,
                        craftingIngredient.Value);
                }
                
                _buildable.GetComponent<BuildableBehaviour>().buildableGuid = Guid.NewGuid().ToString();
                
                ObjectsReference.Instance.uiBlueprintsInventory.RefreshUInventory();

                _activeGhost.transform.position = ObjectsReference.Instance.ghostsReference.transform.position;
                _activeGhost = null;
            }
        }

        public void CancelBuild() {
            CancelGhost();
        }

        public void setGhostColor() {
            if (_activeGhost != null)
                if (ObjectsReference.Instance.rawMaterialsInventory.HasCraftingIngredientsForPlateform()) {
                    _activeGhostClass.SetGhostState(GhostState.VALID);
                }
                else {
                    if (_activeGhostClass.GetGhostState() != GhostState.UNBUILDABLE)
                        _activeGhostClass.SetGhostState(GhostState.NOT_ENOUGH_MATERIALS);
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

                    foreach (var monkey in ObjectsReference.Instance.worldData.monkeys) {
                        monkey.SearchForBananaManBananas();
                    }
                    
                    ObjectsReference.Instance.uInventoriesManager.GetCurrentUIHelper().HideRetrieveConfirmation();
                    ObjectsReference.Instance.audioManager.PlayEffect(SoundEffectType.TAKE_SOMETHING, 0);

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
