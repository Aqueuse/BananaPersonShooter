using UI.InGame.QuickSlots;
using UnityEngine;

namespace Building {
    public class SlotSwitch : MonoBehaviour {
        [SerializeField] private GhostsReference ghostsReference;
        [SerializeField] private LayerMask buildingLayerMask;

        private GameObject _activeGhost;
        public Ghost _activeGhostClass;
        private GameObject _buildable;
        private Mesh targetMesh;
        private GenericDictionary<ItemType, int> _craftingIngredients;

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

        private const float _buildableUnit = 1.3f;

        private Vector3 decalageLeft;
        private Vector3 decalageRight;
        private Vector3 decalageUp;
        private Vector3 decalageDown;
        private Vector3 decalageDoubleUp;
        private Vector3 decalageUpLeft;
        private Vector3 decalageUpRight;
        private Vector3 decalageDoubleDown;

        private Vector3 offsettedPosition;
        private Vector3 raycastHitPoint;
        
        private RaycastHit raycastHit;
        
        private void Start() {
            _mainCameraTransform = ObjectsReference.Instance.mainCamera.transform;
            
            decalageLeft = new Vector3(0, 0, -_buildableUnit);
            decalageRight = new Vector3(0, 0, _buildableUnit);
            decalageUp = new Vector3(0, _buildableUnit, 0);
            decalageDown = new Vector3(0, -_buildableUnit, 0);
            
            decalageDoubleUp = new Vector3(0, _buildableUnit*2, 0);
            decalageUpLeft = new Vector3(0, _buildableUnit, -_buildableUnit);
            decalageUpRight = new Vector3(0, _buildableUnit, _buildableUnit);
            decalageDoubleDown = new Vector3(0, -_buildableUnit*2, 0);
        }

        private void FixedUpdate() {
            if (ObjectsReference.Instance.bananaMan.activeItemCategory != ItemCategory.BUILDABLE || _activeGhost == null) return;
            if (ObjectsReference.Instance.uiManager.Is_Interface_Visible()) return;
            if (!ObjectsReference.Instance.gameActions.isBuildModeActivated) return;

            if (Physics.Raycast(origin: _mainCameraTransform.position, direction: _mainCameraTransform.forward,
                    maxDistance: 60f, hitInfo: out raycastHit, layerMask:buildingLayerMask)) {

                raycastHitPoint = raycastHit.point;
                var targetGameObject = raycastHit.transform.gameObject; 

                if (targetGameObject.layer == 7 && _activeGhostClass.buildableDataScriptableObject.mustSnap) {
                    targetMesh = targetGameObject.GetComponent<MeshFilter>().sharedMesh;
                    var gridSize = ObjectsReference.Instance.scriptableObjectManager.GetBuildableGridSizeByMesh(targetMesh);

                    pivotTransform = targetGameObject.transform;
                    pivotTransformPosition = pivotTransform.position;
                    pivotTransformRotation = pivotTransform.rotation;
                    
                    pivotLocalePosition = pivotTransform.InverseTransformPoint(pivotTransformPosition);
                    pivotLocaleRotation = Quaternion.Inverse(pivotTransformRotation) * pivotTransformRotation;

                    raycastHitPointLocalePosition = targetGameObject.transform.InverseTransformPoint(raycastHit.point);
                    deltaZ = raycastHitPointLocalePosition.z;
                    deltaY = raycastHitPointLocalePosition.y;

                    switch (gridSize) {
                        case BuildableGridSize.BUILDING_BLOCK_1X1:
                            RefreshGhostPositionForBuildingBlock1x1();
                            break;
                        case BuildableGridSize.BUILDING_BLOCK_1X2:
                            RefreshGhostPositionForBuildingBlock1x2();
                            break;
                        case BuildableGridSize.BUILDING_BLOCK_2X1:
//                            RefreshGhostPositionForBuildingBlock2x1();
                            break;
                        case BuildableGridSize.BUILDING_BLOCK_2X2:
  //                          RefreshGhostPositionForBuildingBlock2x2();
                            break;
                    }
                    _ghostPosition = pivotTransform.TransformPoint(offsettedPosition);
                    
                    _activeGhost.transform.position = _ghostPosition;
                }

                else {
                    _activeGhost.transform.position = raycastHitPoint;
                }
            }

            _craftingIngredients = _activeGhostClass.buildableDataScriptableObject.rawMaterialsWithQuantity;

            _activeGhostClass.SetGhostState(ObjectsReference.Instance.inventory.HasCraftingIngredients(_craftingIngredients)
                ? GhostState.VALID
                : GhostState.UNBUILDABLE);
        }

        private void RefreshGhostPositionForBuildingBlock1x1() {
            // WEST
            if (deltaY > 0.1f && deltaZ < 0.1f) {
                offsettedPosition = pivotLocalePosition + pivotLocaleRotation * decalageLeft;
                return;
            }
            
            // EAST
            if (deltaY > 0.1f && deltaZ > _buildableUnit-0.2f) {
                offsettedPosition = pivotLocalePosition + pivotLocaleRotation * decalageRight;
                return;
            }
            
            // NORTH
            if (deltaY > _buildableUnit-0.2f && deltaZ > 0.1f) {
                offsettedPosition = pivotLocalePosition + pivotLocaleRotation * decalageUp;
                return;
            }

            // SOUTH
            if (deltaY < 0.1f) {
                offsettedPosition = pivotLocalePosition + pivotLocaleRotation * decalageDown;
            }
        }

        private void RefreshGhostPositionForBuildingBlock1x2() {
            // NORTH
            if (deltaY > _buildableUnit*2) {
                offsettedPosition = pivotLocalePosition + pivotLocaleRotation * decalageDoubleUp;
            }

            // SOUTH
            if (deltaY < 0.1f) {
                offsettedPosition = pivotLocalePosition + pivotLocaleRotation * decalageDoubleDown;
                return;
            }

            // SOUTH WEST
            if (deltaY is < _buildableUnit and < _buildableUnit*2 && deltaZ < 0.1f) {
                offsettedPosition = pivotLocalePosition + pivotLocaleRotation * decalageLeft;
                return;
            }

            // NORTH WEST
            if (deltaY is > _buildableUnit and < _buildableUnit*2 && deltaZ < 0.1f) {
                offsettedPosition = pivotLocalePosition + pivotLocaleRotation * decalageUpLeft;
                return;
            }
            
            // NORTH EAST
            if (deltaY is > _buildableUnit and < _buildableUnit*2 && deltaZ > _buildableUnit) {
                offsettedPosition = pivotLocalePosition + pivotLocaleRotation * decalageUpRight;
                return;
            }
            
            // SOUTH EAST
            if (deltaY < _buildableUnit && deltaZ > _buildableUnit) {
                offsettedPosition = pivotLocalePosition + pivotLocaleRotation * decalageRight;
            }
        }

        public void SwitchSlot(UISlot slot) {
            ObjectsReference.Instance.bananaMan.SetActiveItemTypeAndCategory(slot.itemType, slot.itemCategory, slot.buildableType);

            CancelGhost();

            switch (ObjectsReference.Instance.bananaMan.activeItemCategory) {
                case ItemCategory.BANANA:
                    ObjectsReference.Instance.bananaMan.activeItem =
                        ObjectsReference.Instance.scriptableObjectManager.GetBananaScriptableObject(ObjectsReference.Instance
                            .uiSlotsManager.Get_Selected_Slot_Type());

                    ObjectsReference.Instance.uiCrosshair.SetCrosshair(slot.itemCategory, slot.itemType);
                    ObjectsReference.Instance.uiManager.canvasGroupsByUICanvasType[UICanvasGroupType.BUILD_HELPER].alpha = 0f;
                    break;

                case ItemCategory.BUILDABLE:
                    ObjectsReference.Instance.uiCrosshair.SetCrosshair(slot.itemCategory, ItemType.EMPTY);
                    if (ObjectsReference.Instance.gameActions.isBuildModeActivated) ActivateGhost();
                    break;

                case ItemCategory.EMPTY or ItemCategory.RAW_MATERIAL:
                    ObjectsReference.Instance.uiCrosshair.SetCrosshair(ItemCategory.EMPTY, ItemType.EMPTY);
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
            
            _activeGhost.transform.rotation = Quaternion.identity;
            _activeGhost.transform.rotation = ghostRotation;
        }

        public void ValidateBuildable() {
            if (ObjectsReference.Instance.bananaMan.activeItemCategory != ItemCategory.BUILDABLE) return;

            if (_activeGhostClass.GetPlateformState() == GhostState.VALID) {
                _buildable = Instantiate(original: _activeGhostClass.buildableDataScriptableObject.buildablePrefab,
                    position: _activeGhost.transform.position, rotation: _activeGhost.transform.rotation);

                _buildable.transform.parent = MapItems.Instance.plateformsContainer.transform;

                foreach (var craftingIngredient in _craftingIngredients) {
                    ObjectsReference.Instance.inventory.RemoveQuantity(ItemCategory.RAW_MATERIAL, craftingIngredient.Key,
                        craftingIngredient.Value);
                    ObjectsReference.Instance.uiSlotsManager.RefreshQuantityInQuickSlot(ItemCategory.RAW_MATERIAL,
                        craftingIngredient.Key);
                }

                ObjectsReference.Instance.mapsManager.currentMap.RefreshAspirablesDataMap();
            }
        }
    }
}