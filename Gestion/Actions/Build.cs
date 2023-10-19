using Data;
using Enums;
using UI.InGame.Inventory;
using UnityEngine;

namespace Gestion.Actions {
    public class Build : MonoBehaviour {
        [SerializeField] private LayerMask buildingLayerMask;
        [SerializeField] private Camera mainCamera;
        
        [SerializeField] private GhostsReference ghostsReference;

        public ItemScriptableObject activatedItemScriptableObject;

        private GameObject _activeGhost;
        private Ghost _activeGhostClass;

        private Ray ray;
        private RaycastHit raycastHit;
        
        private GenericDictionary<RawMaterialType, int> _craftingIngredients;

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
            
            if (activatedItemScriptableObject.buildableType == BuildableType.PLATEFORM) {
                _activeGhost.transform.position = ObjectsReference.Instance.uiHud.buildablePlacementTransform.position;
            }
            
            ray = mainCamera.ScreenPointToRay(UnityEngine.Input.mousePosition);

            if (Physics.Raycast(
                    ray:ray, 
                    out raycastHit,
                    15f,
                    layerMask:buildingLayerMask)) {
                raycastHitPoint = raycastHit.point;
                _activeGhost.transform.position = raycastHitPoint;
            }

            _craftingIngredients = _activeGhostClass.buildableDataScriptableObject.rawMaterialsWithQuantity;

            if (ObjectsReference.Instance.rawMaterialsInventory.HasCraftingIngredients(_craftingIngredients)) {
                _activeGhostClass.SetGhostState(GhostState.VALID);
                ObjectsReference.Instance.uInventoriesManager.GetCurrentUIHelper().ShowNormalPlaceHelper();
            }

            else {
                _activeGhostClass.SetGhostState(GhostState.UNBUILDABLE);
                ObjectsReference.Instance.uInventoriesManager.GetCurrentUIHelper().ShowNetEnoughMaterialsHelper();
            }
        }
    
        public void ActivateGhost(BuildableType ghostItemBuildableType) {
            // take the corresponding ghost in the buildable ghost list
            _activeGhost = ghostsReference.GetGhostByBuildableType(ghostItemBuildableType);
            _activeGhostClass = _activeGhost.GetComponent<Ghost>();
            activatedItemScriptableObject = _activeGhostClass.buildableDataScriptableObject;

            isActivated = true;
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

            if (_activeGhostClass.GetPlateformState() == GhostState.VALID) {
                ObjectsReference.Instance.mapsManager.currentMap.isDiscovered = true;

                _buildable = Instantiate(original: _activeGhostClass.buildableDataScriptableObject.buildablePrefab,
                    position: _activeGhost.transform.position, rotation: _activeGhost.transform.rotation);

                _buildable.transform.parent = MapItems.Instance.aspirablesContainer.transform;

                foreach (var craftingIngredient in _craftingIngredients) {
                    ObjectsReference.Instance.rawMaterialsInventory.RemoveQuantity(craftingIngredient.Key,
                        craftingIngredient.Value);
                    ObjectsReference.Instance.uiQuickSlotsManager.RefreshQuantityInQuickSlot();
                }

                ObjectsReference.Instance.mapsManager.currentMap.RefreshItemsDataMap();
            }
        }
        
        public void QuickBuild() {
            ObjectsReference.Instance.bananaGun.GrabBananaGun();
            
            // Hide interface
            ObjectsReference.Instance.uiManager.Hide_Interface();
            ObjectsReference.Instance.gestionMode.CloseGestionMode();

            ObjectsReference.Instance.uInventoriesManager.GetCurrentUIHelper().show_build_helper();
            
            // activate ghost
            ObjectsReference.Instance.build.ActivateGhost(ObjectsReference.Instance.uInventoriesManager.lastSelectedItemByInventoryCategory[ItemCategory.BUILDABLE].GetComponent<UInventorySlot>().itemScriptableObject.buildableType);
        }

        public void CancelBuild() {
            CancelGhost();
            ObjectsReference.Instance.bananaGun.UngrabBananaGun();
        }
    }
}
