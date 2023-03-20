using Building.Plateforms;
using Data;
using Enums;
using Game;
using Items;
using Player;
using UI.InGame.QuickSlots;
using UnityEngine;

namespace Building {
    public class SlotSwitch : MonoSingleton<SlotSwitch> {
        [SerializeField] private GameObject plateformPrefab;
        [SerializeField] private GameObject plateformSpawnerPoint;

        private GameObject _activePlateform;
        
        void FixedUpdate() {
            if (BananaMan.Instance.activeItemThrowableType == ItemThrowableType.PLATEFORM && _activePlateform != null) {
                _activePlateform.transform.position = plateformSpawnerPoint.transform.position;
            }
        }

        public void SwitchSlot(UISlot slot) {
            BananaMan.Instance.activeItemThrowableType = slot.itemThrowableType;
            BananaMan.Instance.activeItemThrowableCategory = ItemsManager.Instance.itemsDataScriptableObject.itemsThrowableCategoriesByType[slot.itemThrowableType];
            
            // remove plateform ghost if it exist
            if (_activePlateform != null) Destroy(_activePlateform);

            switch (BananaMan.Instance.activeItemThrowableCategory) {
                case ItemThrowableCategory.BANANA:
                    BananaMan.Instance.activeItem = ScriptableObjectManager.Instance.GetBananaScriptableObject(UISlotsManager.Instance.Get_Selected_Slot_Type());
                    break;
                
                case ItemThrowableCategory.CRAFTABLE:
                    if (BananaMan.Instance.activeItemThrowableType == ItemThrowableType.PLATEFORM) {
                        if (global::Game.Inventory.Instance.bananaManInventory[BananaMan.Instance.activeItemThrowableType] > 0) {
                            _activePlateform = Instantiate(
                                original: plateformPrefab,
                                position: plateformSpawnerPoint.transform.position,
                                rotation: plateformSpawnerPoint.transform.localRotation);
                        }

                        _activePlateform.transform.parent = MapItems.Instance.plateformsContainer.transform;
                        BananaGun.Instance.CancelMover();
                    }
                    break;
                
                case ItemThrowableCategory.EMPTY:
                    BananaGun.Instance.CancelMover();
                    break;
            }
        }

        public void ValidatePlateform() {
            var plateformQuantityInInventory = global::Game.Inventory.Instance.GetQuantity(BananaMan.Instance.activeItemThrowableType);
            
            if (BananaMan.Instance.activeItemThrowableType == ItemThrowableType.PLATEFORM && _activePlateform != null) {
                if (_activePlateform.GetComponent<Plateform>().isValid && plateformQuantityInInventory > 0) {
                    plateformQuantityInInventory--;
                    
                    global::Game.Inventory.Instance.RemoveQuantity(BananaMan.Instance.activeItemThrowableType, 1);
                    UISlotsManager.Instance.Get_Selected_Slot().SetAmmoQuantity(plateformQuantityInInventory);
            
                    MapsManager.Instance.currentMap.RefreshPlateformsDataMap();

                    _activePlateform.GetComponent<Plateform>().SetNormal();
                    _activePlateform = null;

                }
            }
        }
    

    }
}