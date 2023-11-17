using Data;
using Data.Buildables;
using Enums;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Gestion.Actions {
    public class Build : MonoBehaviour {
        [SerializeField] private LayerMask buildingLayerMask;
        [SerializeField] private Camera mainCamera;
        
        [SerializeField] private GhostsReference ghostsReference;
        [SerializeField] private BuildableDataScriptableObject plateformBuildableDataScriptableObject;

        public ItemScriptableObject activatedItemScriptableObject;

        private GameObject _activeGhost;
        private Ghost _activeGhostClass;

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

        public bool isActivated;

        private void FixedUpdate() {
            if (!isActivated) return;
            
            ray = mainCamera.ScreenPointToRay(Mouse.current.position.value);

            if (activatedItemScriptableObject.buildableType == BuildableType.PLATEFORM) {
                _activeGhost.transform.position = ObjectsReference.Instance.uiHud.buildablePlacementTransform.position;
                if (Physics.Raycast(ray, out raycastHit, 40)) {
                    _activeGhost.transform.position = raycastHit.point;
                }
            }

            else {
                if(Physics.Raycast(ray, out raycastHit, 2000, layerMask:buildingLayerMask)) {
                    _activeGhost.transform.position = raycastHit.point;
                }
            }
        }

        public void ActivateGhostByScriptableObject(BuildableDataScriptableObject buildableDataScriptableObject) {
            _activeGhost = ghostsReference.GetGhostByBuildableType(buildableDataScriptableObject.buildableType);
            _activeGhostClass = _activeGhost.GetComponent<Ghost>();
            activatedItemScriptableObject = buildableDataScriptableObject;
            
            if (ObjectsReference.Instance.rawMaterialsInventory.HasCraftingIngredients(buildableDataScriptableObject)) {
                _activeGhostClass.SetGhostState(GhostState.VALID);
                ObjectsReference.Instance.uInventoriesManager.GetCurrentUIHelper().ShowNormalPlaceHelper();
            }

            else {
                _activeGhostClass.SetGhostState(GhostState.NOT_ENOUGH_MATERIALS);
                ObjectsReference.Instance.uInventoriesManager.GetCurrentUIHelper().ShowNotEnoughMaterialsHelper();
            }
            
            isActivated = true;
        }

        public void ActivatePlateformGhost() {
            ActivateGhostByScriptableObject(plateformBuildableDataScriptableObject);
        }
        
        public void CancelGhost() {
            isActivated = false;

            if (_activeGhost != null) _activeGhost.transform.position = ghostsReference.transform.position;
            
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
                ObjectsReference.Instance.mapsManager.currentMap.isDiscovered = true;
                
                _buildable = Instantiate(original: _activeGhostClass.buildableDataScriptableObject.buildablePrefab,
                    position: _activeGhost.transform.position, rotation: _activeGhost.transform.rotation);

                _buildable.transform.parent = MapItems.Instance.aspirablesContainer.transform;
                
                var _craftingIngredients = _activeGhostClass.buildableDataScriptableObject.rawMaterialsWithQuantity;
                
                foreach (var craftingIngredient in _craftingIngredients) {
                    ObjectsReference.Instance.rawMaterialsInventory.RemoveQuantity(craftingIngredient.Key,
                        craftingIngredient.Value);
                }

                ObjectsReference.Instance.mapsManager.currentMap.RefreshItemsDataMap();
                ObjectsReference.Instance.quickSlotsManager.SetPlateformSlotAvailability();
            }
        }

        public void CancelBuild() {
            CancelGhost();
            ObjectsReference.Instance.bananaGun.UngrabBananaGun();
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
    }
}
