using System.Collections.Generic;
using Enums;
using UnityEngine;

namespace UI.InGame.QuickSlots {
    public class UIQuickSlotsManager : MonoBehaviour {
        public List<UIQuickSlot> uiQuickSlotsScripts;
        public int selectedSlotIndex;

        private int itemQuantity;
        
        public void RefreshQuantityInQuickSlot() {
            foreach (var uiQuickSlotScript in uiQuickSlotsScripts) {
                if (uiQuickSlotScript.slotItemScriptableObject == null) return;

                switch (uiQuickSlotScript.slotItemScriptableObject.itemCategory) {
                    case ItemCategory.BUILDABLE or ItemCategory.EMPTY:
                        return;
                    case ItemCategory.BANANA:
                        itemQuantity =
                            ObjectsReference.Instance.bananasInventory.bananasInventory[
                                uiQuickSlotScript.slotItemScriptableObject.bananaType];
                        break;
                    case ItemCategory.RAW_MATERIAL:
                        itemQuantity = ObjectsReference.Instance.rawMaterialsInventory.rawMaterialsInventory[
                            uiQuickSlotScript.slotItemScriptableObject.rawMaterialType];
                        break;
                    case ItemCategory.INGREDIENT:
                        itemQuantity =
                            ObjectsReference.Instance.ingredientsInventory.ingredientsInventory[
                                uiQuickSlotScript.slotItemScriptableObject.ingredientsType];
                        break;
                }
                
                if (itemQuantity <= 0) {
                    uiQuickSlotScript.EmptySlot();
                }

                else {
                    uiQuickSlotScript.SetQuantity(itemQuantity);
                }
            }
        }

        public void Switch_to_Slot_Index(int index) {
            selectedSlotIndex = index;
            foreach (var uiSlotsScript in uiQuickSlotsScripts) {
                uiSlotsScript.SetUnselectedSlot();
            }

            uiQuickSlotsScripts[selectedSlotIndex].SetSelectedSlot();
            ObjectsReference.Instance.slotSwitch.SwitchSlot(Get_Selected_Slot());
        }

        private void Switch_To_SelectedSlot() {
            foreach (var uiSlotsScript in uiQuickSlotsScripts) {
                uiSlotsScript.SetUnselectedSlot();
            }

            uiQuickSlotsScripts[selectedSlotIndex].SetSelectedSlot();
            ObjectsReference.Instance.slotSwitch.SwitchSlot(Get_Selected_Slot());
        }

        public void Select_Lefter_Slot() {
            if (selectedSlotIndex > 0) {
                selectedSlotIndex--;
                Switch_To_SelectedSlot();
            }
            else {
                selectedSlotIndex = 0;
                Switch_To_SelectedSlot();
            }
        }

        public void Select_Righter_Slot() {
            if (selectedSlotIndex < uiQuickSlotsScripts.Count - 1) {
                selectedSlotIndex++;
                Switch_To_SelectedSlot();
            }
            else {
                selectedSlotIndex = 3;
                Switch_To_SelectedSlot();
            }
        }
        
        public UIQuickSlot Get_Selected_Slot() {
            return uiQuickSlotsScripts[selectedSlotIndex];
        }

        public BananaType Get_Selected_Slot_Type() {
            return uiQuickSlotsScripts[selectedSlotIndex].slotItemScriptableObject.bananaType;
        }
    }
}