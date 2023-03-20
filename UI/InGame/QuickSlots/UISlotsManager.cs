using System.Collections.Generic;
using Building;
using Enums;

namespace UI.InGame.QuickSlots {
    public class UISlotsManager : MonoSingleton<UISlotsManager> {
        public List<UISlot> uiSlotsScripts;
    
        public int selectedSlotIndex = 2;
        
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
        
        public void AssignToSelectedSlot(ItemThrowableType itemThrowableType) {
            // if the item is already present in the quickslots, empty the last slot where it was to prevent duplicates
            foreach (var uiSlot in uiSlotsScripts) {
                if (uiSlot.itemThrowableType == itemThrowableType) {
                    uiSlot.EmptySlot();
                }
            }
            
            Get_Selected_Slot().SetSlot(itemThrowableType);
        }

        public void TryToPutOnSlot(ItemThrowableType itemThrowableType) {
            if (IsAlreadyInQuickSlot(itemThrowableType)) {
                GetSlotWithItemType(itemThrowableType).SetSlot(itemThrowableType);
            }

            else {
                foreach (var uiSlot in uiSlotsScripts) {
                    if (uiSlot.itemThrowableType == ItemThrowableType.EMPTY) {
                        uiSlot.SetSlot(itemThrowableType);
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

        public int GetSlotIndexByItemType(ItemThrowableType itemThrowableType) {
            for (int i = 0; i < uiSlotsScripts.Count; i++) {
                if (uiSlotsScripts[i].itemThrowableType == itemThrowableType) {
                    return i;
                }
            }
            return 0;
        }

        public ItemThrowableType Get_Selected_Slot_Type() {
            return uiSlotsScripts[selectedSlotIndex].itemThrowableType;
        }
    }
}