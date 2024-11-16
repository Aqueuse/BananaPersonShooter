using System;
using InGame.Interactions;
using InGame.Items.ItemsBehaviours;
using InGame.Items.ItemsBehaviours.BuildablesBehaviours;
using InGame.Items.ItemsProperties.Buildables;
using InGame.Items.ItemsProperties.Dropped;
using Tags;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InGame.Gestion {
    public class GestionViewMode : MonoBehaviour {
        [SerializeField] private LayerMask buildingLayerMask;
        [SerializeField] private LayerMask GestionViewSelectableLayerMask;

        private Camera mainCamera;

        public ViewModeContextType viewModeContextType;
        
        public GameObject targetedGameObject;

        public GameObject _activeGhost;
        private Ghost _activeGhostClass;
        
        private GenericDictionary<DroppedType, int> rawMaterialsWithQuantity;

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
            }
            else {
                targetedGameObject = null;
            }
            
            if (_activeGhost == null) return;
            
            if(Physics.Raycast(ray, out raycastHit, 2000, layerMask:buildingLayerMask)) {
                _activeGhost.transform.position = raycastHit.point;
            }
        }

        public void StartHarvesting() {
            if (_activeGhost != null) CancelGhost();

            viewModeContextType = ViewModeContextType.HARVEST;
        }

        public void StartRepairing() {
            if (_activeGhost != null) CancelGhost();

            viewModeContextType = ViewModeContextType.REPAIR;
        }

        public void ActivateGhostByScriptableObject(BuildablePropertiesScriptableObject buildablePropertiesScriptableObject) {
            if (_activeGhost != null) CancelGhost();
            
            _activeGhost = ObjectsReference.Instance.ghostsReference.GetGhostByBuildableType(buildablePropertiesScriptableObject.buildableType);
            _activeGhostClass = _activeGhost.GetComponent<Ghost>();
            
            viewModeContextType = ViewModeContextType.BUILD;
            
            if (ObjectsReference.Instance.droppedInventory.HasCraftingIngredients(buildablePropertiesScriptableObject)) {
                _activeGhostClass.SetGhostState(GhostState.VALID);
                ObjectsReference.Instance.uInventoriesManager.GetCurrentUIHelper().ShowNormalPlaceHelper();
            }

            else {
                _activeGhostClass.SetGhostState(GhostState.NOT_ENOUGH_MATERIALS);
            }

            ghostRotationEuler = _activeGhost.transform.rotation.eulerAngles;
        }
        
        public void CancelGhost() {
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
                    ObjectsReference.Instance.droppedInventory.RemoveQuantity(craftingIngredient.Key,
                        craftingIngredient.Value);
                }
                
                _buildable.GetComponent<BuildableBehaviour>().buildableGuid = Guid.NewGuid().ToString();
                
                _activeGhost.transform.position = ObjectsReference.Instance.ghostsReference.transform.position;
                _activeGhost = null;
                
                viewModeContextType = ViewModeContextType.SCAN;
            }
        }

        public void CancelBuild() {
            CancelGhost();
        }
        
        public void harvest() {
            if (targetedGameObject == null) return;
            
            var gameObjectTagClass = targetedGameObject.GetComponent<Tag>();
            var gameObjectTag = gameObjectTagClass.gameObjectTag;
            
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
                        ObjectsReference.Instance.droppedInventory.AddQuantity(craftingMaterial.Key, craftingMaterial.Value);
                    }

                    if (buildableType == BuildableType.BANANA_DRYER) targetedGameObject.GetComponent<BananasDryerBehaviour>().RetrieveRawMaterials();
                    
                    DestroyImmediate(targetedGameObject);
                    
                    ObjectsReference.Instance.uInventoriesManager.GetCurrentUIHelper().HideRetrieveConfirmation();
                    ObjectsReference.Instance.audioManager.PlayEffect(SoundEffectType.TAKE_SOMETHING, 0);

                    targetedGameObject = null;

                    ObjectsReference.Instance.build.setGhostColor();

                    break;

                case GAME_OBJECT_TAG.DROPPED:
                    var _wastePropertiesScriptableObject = (DroppedPropertiesScriptableObject)gameObjectTagClass.itemScriptableObject;
                    
                    rawMaterialsWithQuantity = _wastePropertiesScriptableObject.GetDroppedMaterialsWithQuantity();

                    foreach (var droppedRawMaterialIngredient in rawMaterialsWithQuantity) {
                        ObjectsReference.Instance.droppedInventory.AddQuantity(droppedRawMaterialIngredient.Key, droppedRawMaterialIngredient.Value);
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
                if (!ObjectsReference.Instance.droppedInventory.HasCraftingIngredients(buildableBehaviour.buildableType))
                    return;
                
                var _craftingIngredients = ObjectsReference.Instance.meshReferenceScriptableObject.buildablePropertiesScriptableObjects[buildableBehaviour.buildableType].rawMaterialsWithQuantity;

                foreach (var craftingIngredient in _craftingIngredients) {
                    ObjectsReference.Instance.droppedInventory.RemoveQuantity(craftingIngredient.Key, craftingIngredient.Value);
                }
                
                ObjectsReference.Instance.audioManager.PlayEffect(SoundEffectType.TAKE_SOMETHING, 0);
                
                buildableBehaviour.RepairBuildable();
            }
        }
    }
}
