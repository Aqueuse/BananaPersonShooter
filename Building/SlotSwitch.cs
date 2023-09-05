using Enums;
using UI.InGame.QuickSlots;
using UnityEngine;

namespace Building {
    public class SlotSwitch : MonoBehaviour {
        [SerializeField] private GhostsReference ghostsReference;
        [SerializeField] private LayerMask buildingLayerMask;
        public Transform plateformPlacementTransform;

        private GameObject _activeGhost;
        public Ghost _activeGhostClass;
        private GameObject _buildable;
        private Mesh targetMesh;
        private GenericDictionary<RawMaterialType, int> _craftingIngredients;

        private Transform _mainCameraTransform;
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

        private RaycastHit raycastHit;

        private void Start() {
            _mainCameraTransform = ObjectsReference.Instance.mainCamera.transform;
        }

        private void FixedUpdate() {
            if (ObjectsReference.Instance.bananaMan.activeItemCategory != ItemCategory.BUILDABLE || _activeGhost == null) return;
            if (ObjectsReference.Instance.uiManager.Is_Interface_Visible()) return;

            if (_activeGhostClass.buildableDataScriptableObject.buildableType == BuildableType.PLATEFORM) {
                _activeGhost.transform.position = plateformPlacementTransform.position;
            }

            if (Physics.Raycast(origin: _mainCameraTransform.position, direction: _mainCameraTransform.forward,
                    maxDistance: 15f, hitInfo: out raycastHit, layerMask:buildingLayerMask)) {
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
        
        public void SwitchSlot(UIQuickSlot slot) {
            CancelGhost();

            switch (ObjectsReference.Instance.bananaMan.activeItemCategory) {
                case ItemCategory.BANANA:
                    ObjectsReference.Instance.bananaMan.SetActiveItemTypeAndCategory(slot.slotItemScriptableObject.bananaType, slot.slotItemScriptableObject.itemCategory, slot.slotItemScriptableObject.buildableType);

                    ObjectsReference.Instance.bananaMan.activeItem =
                        ObjectsReference.Instance.scriptableObjectManager.GetBananaScriptableObject(ObjectsReference.Instance
                            .uiSlotsManager.Get_Selected_Slot_Type());

                    ObjectsReference.Instance.uiCrosshairs.SetCrosshair(slot.slotItemScriptableObject.itemCategory, slot.slotItemScriptableObject.bananaType);
                    ObjectsReference.Instance.uInventoriesManager.GetCurrentUIHelper().show_banana_helper();

                    break;

                case ItemCategory.BUILDABLE:
                    ObjectsReference.Instance.bananaMan.SetActiveItemTypeAndCategory(slot.slotItemScriptableObject.bananaType, slot.slotItemScriptableObject.itemCategory, slot.slotItemScriptableObject.buildableType);

                    ActivateGhost();

                    ObjectsReference.Instance.uiCrosshairs.SetCrosshair(slot.slotItemScriptableObject.itemCategory, BananaType.EMPTY);
                    ObjectsReference.Instance.uInventoriesManager.GetCurrentUIHelper().show_default_helper();
                    break;

                case ItemCategory.EMPTY or ItemCategory.RAW_MATERIAL:
                    if (ObjectsReference.Instance.gameManager.isGamePlaying) ObjectsReference.Instance.bananaGun.UngrabBananaGun();
                    
                    ObjectsReference.Instance.uiCrosshairs.SetCrosshair(ItemCategory.EMPTY, BananaType.EMPTY);
                    ObjectsReference.Instance.uInventoriesManager.GetCurrentUIHelper().show_default_helper();
                    break;
            }
        }

        public void ActivateGhost() {
            if (ObjectsReference.Instance.bananaMan.activeItemCategory == ItemCategory.BUILDABLE) {
                // take the corresponding ghost in the buildable ghost list
                _activeGhost = ghostsReference.GetGhostByBuildableType(ObjectsReference.Instance.bananaMan.activeBuildableType);
                _activeGhostClass = _activeGhost.GetComponent<Ghost>();
            }
        }


        public void CancelGhost() {
            if (_activeGhost != null) _activeGhost.transform.position = ghostsReference.transform.position;
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
            if (ObjectsReference.Instance.bananaMan.activeItemCategory != ItemCategory.BUILDABLE) return;

            if (_activeGhostClass.GetPlateformState() == GhostState.VALID) {
                ObjectsReference.Instance.mapsManager.currentMap.isDiscovered = true;

                _buildable = Instantiate(original: _activeGhostClass.buildableDataScriptableObject.buildablePrefab,
                    position: _activeGhost.transform.position, rotation: _activeGhost.transform.rotation);

                _buildable.transform.parent = MapItems.Instance.aspirablesContainer.transform;

                foreach (var craftingIngredient in _craftingIngredients) {
                    ObjectsReference.Instance.rawMaterialsInventory.RemoveQuantity(craftingIngredient.Key, craftingIngredient.Value);
                    ObjectsReference.Instance.uiSlotsManager.RefreshQuantityInQuickSlot();
                }
                
                ObjectsReference.Instance.mapsManager.currentMap.RefreshAspirablesItemsDataMap();
            }
        }
    }
}