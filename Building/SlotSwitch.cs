using Enums;
using UI.InGame.QuickSlots;
using UnityEngine;

namespace Building {
    public class SlotSwitch : MonoBehaviour {
        [SerializeField] private GameObject plateformPrefab;
        [SerializeField] private GameObject buildableSpawnerPoint;

        private GameObject _activeBuildable;
        
        void FixedUpdate() {
            if (ObjectsReference.Instance.bananaMan.activeItemCategory == ItemCategory.BUILDABLE && _activeBuildable != null) {
                _activeBuildable.transform.position = buildableSpawnerPoint.transform.position;
            }
        }

        public void SwitchSlot(UISlot slot) {
            ObjectsReference.Instance.bananaMan.SetActiveItemTypeAndCategory(slot.itemType, slot.itemCategory, slot.buildableType);
            
            // remove plateform ghost if it exist
            if (_activeBuildable != null) Destroy(_activeBuildable);

            switch (ObjectsReference.Instance.bananaMan.activeItemCategory) {
                case ItemCategory.BANANA:
                    ObjectsReference.Instance.bananaMan.activeItem = ObjectsReference.Instance.scriptableObjectManager.GetBananaScriptableObject(ObjectsReference.Instance.uiSlotsManager.Get_Selected_Slot_Type());
                    break;

                case ItemCategory.BUILDABLE:
                    _activeBuildable = Instantiate(
                        original: plateformPrefab,
                        position: buildableSpawnerPoint.transform.position,
                        rotation: buildableSpawnerPoint.transform.localRotation);
                    
                    _activeBuildable.transform.parent = MapItems.Instance.plateformsContainer.transform;
                    
                    // set buildable color
                    var craftingIngredients =
                        ObjectsReference.Instance.scriptableObjectManager.GetBuildableCraftingIngredients(
                            ObjectsReference.Instance.bananaMan.activeBuildableType);

                    if (ObjectsReference.Instance.inventory.HasCraftingIngredients(craftingIngredients)) {
                        _activeBuildable.GetComponent<Buildable>().SetValid();
                    }
                    else {
                        _activeBuildable.GetComponent<Buildable>().SetUnbuildable();
                    }

                    ObjectsReference.Instance.bananaGun.CancelMover();
                    break;

                case ItemCategory.EMPTY:
                    ObjectsReference.Instance.bananaGun.CancelMover();
                    break;
            }
        }

        public void ValidateBuildable() {
            if (ObjectsReference.Instance.bananaMan.activeItemCategory == ItemCategory.BUILDABLE && _activeBuildable != null) {
                if (_activeBuildable.GetComponent<Buildable>().isValid) {
                    var craftingIngredients =
                        ObjectsReference.Instance.scriptableObjectManager.GetBuildableCraftingIngredients(
                            ObjectsReference.Instance.bananaMan.activeBuildableType);

                    foreach (var craftingIngredient in craftingIngredients) {
                        ObjectsReference.Instance.inventory.RemoveQuantity(ItemCategory.BUILDABLE, craftingIngredient.Key, craftingIngredient.Value);
                    }
                    
                    if (ObjectsReference.Instance.bananaMan.activeBuildableType == BuildableType.PLATEFORM) ObjectsReference.Instance.mapsManager.currentMap.RefreshPlateformsDataMap();

                    _activeBuildable.GetComponent<Buildable>().SetNormal(ObjectsReference.Instance.bananaMan.activeBuildableType);
                    _activeBuildable = null;
                }
            }
        }
    

    }
}