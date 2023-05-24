using Enums;
using UI.InGame.QuickSlots;
using UnityEngine;

namespace Building {
    public class SlotSwitch : MonoBehaviour {
        [SerializeField] private GhostsReference ghostsReference;
        [SerializeField] private LayerMask buildingLayerMask;
        private BananaGun bananaGun;

        private GameObject _activeGhost;
        private Ghost _activeGhostClass;
        private GameObject _buildable;
        private Mesh targetMesh;
        private GenericDictionary<ItemType, int> _craftingIngredients;

        private Transform _mainCameraTransform;
        private Quaternion _normalRotation;
        private Vector3 _ghostPosition;

        private float angleZ;
        private float angleY;

        private Transform pivotTransform;
        private Vector3 pivotTransformPosition;
        private Quaternion pivotTransformRotation;
        
        private Vector3 pivotLocalePosition;
        private Quaternion pivotLocaleRotation;
        private Vector3 raycastHitPointDirection;

        private readonly Vector3 decalageLeft = new(0, 0, -_buildableUnit);
        private readonly Vector3 decalageRight = new(0, 0, _buildableUnit);
        private readonly Vector3 decalageUp = new(0, _buildableUnit, 0);
        private readonly Vector3 decalageDown = new(0, -_buildableUnit, 0);

        private readonly Vector3 decalageDoubleUp = new(0, _buildableUnit*2, 0);
        private readonly Vector3 decalageUpLeft = new(0, _buildableUnit, -_buildableUnit);
        private readonly Vector3 decalageUpRight = new(0, _buildableUnit, _buildableUnit);
        private readonly Vector3 decalageDoubleDown = new(0, -_buildableUnit*2, 0);

        private Vector3 offsettedPosition;
        
        private RaycastHit raycastHit;
        private const float _buildableUnit = 1.3f;
        
        private void Start() {
            _mainCameraTransform = ObjectsReference.Instance.mainCamera.transform;
            bananaGun = ObjectsReference.Instance.bananaGun;
        }

        private void FixedUpdate() {
            if (ObjectsReference.Instance.bananaMan.activeItemCategory != ItemCategory.BUILDABLE || _activeGhost == null) return;
            if (ObjectsReference.Instance.uiManager.Is_Interface_Visible()) return;

            if (Physics.Raycast(origin: _mainCameraTransform.position, direction: _mainCameraTransform.forward,
                    maxDistance: 60f, hitInfo: out raycastHit, layerMask:buildingLayerMask)) {
                
                var raycastHitPoint = raycastHit.point;
                var targetGameObject = raycastHit.transform.gameObject; 

                if (targetGameObject.layer == 7 && _activeGhostClass.buildableDataScriptableObject.mustSnap) {
                    targetMesh = targetGameObject.GetComponent<MeshFilter>().sharedMesh;
                    var gridSize = ObjectsReference.Instance.scriptableObjectManager.GetBuildableGridSizeByMesh(targetMesh);

                    pivotTransform = targetGameObject.transform;
                    pivotTransformPosition = pivotTransform.position;
                    pivotTransformRotation = pivotTransform.rotation;

                    raycastHitPointDirection = (raycastHitPoint - pivotTransformPosition).normalized;
            
                    // get angles (Z et Y) beetween hitPoint et pivot.forward
                    angleZ = Vector3.Angle(pivotTransform.forward, raycastHitPointDirection);
                    angleY = Vector3.Angle(pivotTransform.up, raycastHitPointDirection);
            
                    pivotLocalePosition = pivotTransform.InverseTransformPoint(pivotTransformPosition);
                    pivotLocaleRotation = Quaternion.Inverse(pivotTransformRotation) * pivotTransformRotation;

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
                    _activeGhost.transform.rotation = targetGameObject.transform.rotation;
                }

                else {
                    raycastHitPoint.y += 0.65f;
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
            if (angleZ is > 165 and < 175) {
                offsettedPosition = pivotLocalePosition + pivotLocaleRotation * decalageLeft;
                return;
            }
            
            // EAST
            if (angleZ is > 30 and < 40) {
                offsettedPosition = pivotLocalePosition + pivotLocaleRotation * decalageRight;
                return;
            }
            
            // NORTH
            if (angleY is > 10 and < 20) {
                offsettedPosition = pivotLocalePosition + pivotLocaleRotation * decalageUp;
                return;
            }
            
            // SOUTH
            if (angleY is > 165 and < 175) {
                offsettedPosition = pivotLocalePosition + pivotLocaleRotation * decalageDown;
            }
        }

        private void RefreshGhostPositionForBuildingBlock1x2() {
            // // SOUTH
            if (angleY > 120) {
                offsettedPosition = pivotLocalePosition + pivotLocaleRotation * decalageDoubleDown;
                return;
            }

            // NORTH
            if (angleY < 20) {
                offsettedPosition = pivotLocalePosition + pivotLocaleRotation * decalageDoubleUp;
            }
            
            // SOUTH EAST
            if (angleZ < 40 && angleY is > 60 and < 120) {
                offsettedPosition = pivotLocalePosition + pivotLocaleRotation * decalageRight;
                return;
            }
            
            // SOUTH WEST
            if (angleZ > 130 && angleY is > 60 and < 120) {
                offsettedPosition = pivotLocalePosition + pivotLocaleRotation * decalageLeft;
                return;
            }

            // // NORTH EAST
            if (angleZ is > 50 and < 70 && angleY is < 60 and > 20) {
                offsettedPosition = pivotLocalePosition + pivotLocaleRotation * decalageUpRight;
                return;
            }
            
            // // NORTH WEST
            if (angleZ is > 110 and < 130 && angleY is < 60 and > 20) {
                offsettedPosition = pivotLocalePosition + pivotLocaleRotation * decalageUpLeft;
            }
        }

        private void RefreshGhostPositionForBuildingBlock2x1() {
            // TODO : implement when we will get a compatible buildable ¯\(°_o)/¯
        }

        private void RefreshGhostPositionForBuildingBlock2x2() {
            // TODO : implement when we will get a compatible buildable ¯\(°_o)/¯
        }

        public void SwitchSlot(UISlot slot) {
            ObjectsReference.Instance.bananaMan.SetActiveItemTypeAndCategory(slot.itemType, slot.itemCategory,
                slot.buildableType);

            if (_activeGhost != null) _activeGhost.transform.position = ghostsReference.transform.position;

            switch (ObjectsReference.Instance.bananaMan.activeItemCategory) {
                case ItemCategory.BANANA:
                    ObjectsReference.Instance.bananaMan.activeItem =
                        ObjectsReference.Instance.scriptableObjectManager.GetBananaScriptableObject(ObjectsReference.Instance
                            .uiSlotsManager.Get_Selected_Slot_Type());

                    bananaGun.GrabBananaGun();
                    ObjectsReference.Instance.uiCrosshair.SetCrosshair(slot.itemCategory, slot.itemType);
                    break;

                case ItemCategory.BUILDABLE:
                    // take the corresponding ghost in the buildable ghost list
                    _activeGhost =
                        ghostsReference.GetGhostByBuildableType(ObjectsReference.Instance.bananaMan.activeBuildableType);
                    _activeGhostClass = _activeGhost.GetComponent<Ghost>();

                    bananaGun.GrabBananaGun();
                    ObjectsReference.Instance.uiCrosshair.SetCrosshair(slot.itemCategory, ItemType.EMPTY);
                    break;

                case ItemCategory.EMPTY or ItemCategory.RAW_MATERIAL:
                    bananaGun.CancelMover();
                    ObjectsReference.Instance.uiCrosshair.SetCrosshair(ItemCategory.EMPTY, ItemType.EMPTY);
                    break;
            }
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

                if (ObjectsReference.Instance.bananaMan.activeBuildableType == BuildableType.PLATEFORM)
                    ObjectsReference.Instance.mapsManager.currentMap.RefreshPlateformsDataMap();
            }
        }
    }
}