using System.Collections.Generic;
using Building;
using Enums;
using UI.InGame.Inventory;

namespace UI.InGame.QuickSlots {
    public class UISlotsManager : MonoSingleton<UISlotsManager> {
        public GenericDictionary<int, int> slotsMappingToInventory;
        public List<UISlot> uiSlotsScripts;
    
        public int selectedSlotIndex = 2;

        private void Start() {
            slotsMappingToInventory = new GenericDictionary<int, int> {
                { 0, 0 },
                { 1, 0 },
                { 2, 0 },
                { 3, 0 }
            };
        }

        public void RefreshQuantityInQuickSlot(ItemThrowableType itemThrowableType) {
            foreach (var uiSlotsScript in uiSlotsScripts) {
                if (uiSlotsScript.itemThrowableType == itemThrowableType) {
                    uiSlotsScript.SetAmmoQuantity(global::Game.Inventory.Instance.bananaManInventory[itemThrowableType]);
                }

                if (uiSlotsScript.itemThrowableType == ItemThrowableType.EMPTY) {
                    uiSlotsScript.EmptySlot();
                }

                if (global::Game.Inventory.Instance.GetQuantity(uiSlotsScript.itemThrowableType) <= 0) {
                    uiSlotsScript.EmptySlot();
                }
            }
            
        }

        public void Switch_to_Slot_Index(int index) {
            selectedSlotIndex = index;
            foreach (var uiSlotsScript in uiSlotsScripts) {
                uiSlotsScript.SetUnselectedWeaponSlot();
            }

            uiSlotsScripts[selectedSlotIndex].SetSelectedWeaponSlot();
            SlotSwitch.Instance.SwitchSlot(Get_Selected_Slot());
        }

        private void Switch_To_SelectedSlot() {
            foreach (var uiSlotsScript in uiSlotsScripts) {
                uiSlotsScript.SetUnselectedWeaponSlot();
            }

            uiSlotsScripts[selectedSlotIndex].SetSelectedWeaponSlot();
            SlotSwitch.Instance.SwitchSlot(Get_Selected_Slot());
        }

        public void Select_Upper_Slot() {
            if (selectedSlotIndex > 0) {
                selectedSlotIndex--;
                Switch_To_SelectedSlot();
            }
            else {
                selectedSlotIndex = 0;
                Switch_To_SelectedSlot();
            }
        }
        
        public void Select_Lower_Slot() {
            if (selectedSlotIndex < uiSlotsScripts.Count-1) {
                selectedSlotIndex++;
                Switch_To_SelectedSlot();
            }
            else {
                selectedSlotIndex = 3;
                Switch_To_SelectedSlot();
            }
        }
        
        public void AssignToSelectedSlot(ItemThrowableType itemThrowableType, ItemThrowableCategory itemThrowableCategory) {
            foreach (var uiSlot in uiSlotsScripts) {
                if (uiSlot.itemThrowableType == itemThrowableType) {
                    uiSlot.EmptySlot();
                }
            }

            slotsMappingToInventory[selectedSlotIndex] = UInventory.Instance.GetSlotIndex(itemThrowableType);
            
            Get_Selected_Slot().SetSlot(itemThrowableType, itemThrowableCategory);
        }

        public void TryToPutOnSlot(ItemThrowableType itemThrowableType, ItemThrowableCategory itemThrowableCategory) {
            if (IsAlreadyInQuickSlot(itemThrowableType)) {
                GetSlotWithItemType(itemThrowableType).SetSlot(itemThrowableType, itemThrowableCategory);
            }

            else {
                foreach (var uiSlot in uiSlotsScripts) {
                    if (uiSlot.itemThrowableType == ItemThrowableType.EMPTY) {
                        uiSlot.SetSlot(itemThrowableType, itemThrowableCategory);
                        return;
                    }
                }
            }
        }

        private bool IsAlreadyInQuickSlot(ItemThrowableType itemThrowableType) {
            foreach (var uiSlot in uiSlotsScripts) {
                if (uiSlot.itemThrowableType == itemThrowableType) {
                    return true;
                }
            }
            return false;
        }
        
        public UISlot Get_Selected_Slot() {
            return uiSlotsScripts[selectedSlotIndex];
        }

        private UISlot GetSlotWithItemType(ItemThrowableType itemThrowableType) {
            foreach (var uiSlot in uiSlotsScripts) {
                if (uiSlot.itemThrowableType == itemThrowableType) {
                    return uiSlot;
                }
            }

            return null;
        }

        public ItemThrowableType Get_Selected_Slot_Type() {
            return uiSlotsScripts[selectedSlotIndex].itemThrowableType;
        }

    }
}