using Enums;
using UI.InGame.QuickSlots;
using UnityEngine;

namespace Building {
    public class SlotSwitch : MonoBehaviour {
        [SerializeField] private GhostsReference ghostsReference;
        [SerializeField] private LayerMask layerMask;

        private GameObject _activeGhost;
        private Ghost _activeGhostClass;
        private GameObject buildable;
        private GenericDictionary<ItemType, int> craftingIngredients;

        private Transform mainCameraTransform;
        private Quaternion normalRotation;
        private Vector3 buildableSpawnerPoint;

        private void Start() {
            mainCameraTransform = ObjectsReference.Instance.mainCamera.transform;
        }

        void FixedUpdate() {
            if (ObjectsReference.Instance.bananaMan.activeItemCategory == ItemCategory.BUILDABLE && _activeGhost != null) {
                if (Physics.Raycast(origin: mainCameraTransform.position, direction:mainCameraTransform.forward, maxDistance:60f, 
                        hitInfo: out RaycastHit raycastHit, layerMask:layerMask)) {
                    _activeGhost.transform.position = raycastHit.point;
                    
                    buildableSpawnerPoint = raycastHit.point;
                }

                craftingIngredients = _activeGhostClass.buildableDataScriptableObject.rawMaterialsWithQuantity;

                if (ObjectsReference.Instance.inventory.HasCraftingIngredients(craftingIngredients)) {
                    _activeGhostClass.SetGhostState(GhostState.VALID);
                }
                else {
                    _activeGhostClass.SetGhostState(GhostState.UNBUILDABLE);
                }
            }
        }

        public void SwitchSlot(UISlot slot) {
            ObjectsReference.Instance.bananaMan.SetActiveItemTypeAndCategory(slot.itemType, slot.itemCategory, slot.buildableType);
            
            if (_activeGhost != null) _activeGhost.transform.position = ghostsReference.transform.position;

            switch (ObjectsReference.Instance.bananaMan.activeItemCategory) {
                case ItemCategory.BANANA:
                    ObjectsReference.Instance.bananaMan.activeItem = ObjectsReference.Instance.scriptableObjectManager.GetBananaScriptableObject(ObjectsReference.Instance.uiSlotsManager.Get_Selected_Slot_Type());
                    
                    ObjectsReference.Instance.bananaGun.GrabBananaGun();
                    ObjectsReference.Instance.uiCrosshair.SetCrosshair(slot.itemCategory, slot.itemType);
                    break;

                case ItemCategory.BUILDABLE:
                    // take the corresponding ghost in the buildable ghost list
                    _activeGhost = ghostsReference.GetGhostByBuildableType(ObjectsReference.Instance.bananaMan.activeBuildableType);
                    _activeGhostClass = _activeGhost.GetComponent<Ghost>();
                    
                    ObjectsReference.Instance.bananaGun.GrabBananaGun();
                    ObjectsReference.Instance.uiCrosshair.SetCrosshair(slot.itemCategory, ItemType.EMPTY);
                    break;

                case ItemCategory.EMPTY or ItemCategory.RAW_MATERIAL:
                    ObjectsReference.Instance.bananaGun.CancelMover();
                    ObjectsReference.Instance.uiCrosshair.SetCrosshair(ItemCategory.EMPTY, ItemType.EMPTY);
                    break;
            }
        }

        public void ValidateBuildable() {
            if (ObjectsReference.Instance.bananaMan.activeItemCategory == ItemCategory.BUILDABLE) {
                
                if (_activeGhostClass.GetPlateformState() == GhostState.VALID) {
                    buildable = Instantiate(original:_activeGhostClass.buildableDataScriptableObject.buildablePrefab,
                        position: buildableSpawnerPoint, rotation: _activeGhost.transform.rotation);

                    buildable.transform.parent = MapItems.Instance.plateformsContainer.transform;
                    
                    foreach (var craftingIngredient in craftingIngredients) {
                        ObjectsReference.Instance.inventory.RemoveQuantity(ItemCategory.RAW_MATERIAL, craftingIngredient.Key, craftingIngredient.Value);
                        ObjectsReference.Instance.uiSlotsManager.RefreshQuantityInQuickSlot(ItemCategory.RAW_MATERIAL, craftingIngredient.Key);
                    }
                    
                    if (ObjectsReference.Instance.bananaMan.activeBuildableType == BuildableType.PLATEFORM) ObjectsReference.Instance.mapsManager.currentMap.RefreshPlateformsDataMap();
                }
            }
        }
    

    }
}