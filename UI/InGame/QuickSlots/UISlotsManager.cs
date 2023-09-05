using System.Collections.Generic;
using Enums;
using UnityEngine;

namespace UI.InGame.QuickSlots {
    public class UISlotsManager : MonoBehaviour {
        public List<UIQuickSlot> uiSlotsScripts;
        public int selectedSlotIndex;
        
        public void RefreshQuantityInQuickSlot() {
            foreach (var uiSlotsScript in uiSlotsScripts) {
                if (uiSlotsScript.slotItemScriptableObject == null) return;
                
                if (uiSlotsScript.slotItemScriptableObject.itemCategory == ItemCategory.BUILDABLE || uiSlotsScript.slotItemScriptableObject.itemCategory == ItemCategory.EMPTY) return;
                
                if (uiSlotsScript.slotItemScriptableObject.itemCategory == ItemCategory.BANANA) {
                    uiSlotsScript.SetQuantity(ObjectsReference.Instance.bananasInventory.bananasInventory[uiSlotsScript.slotItemScriptableObject.bananaType]);

                    if (ObjectsReference.Instance.bananasInventory.GetQuantity(uiSlotsScript.slotItemScriptableObject.bananaType) <= 0) {
                        uiSlotsScript.EmptySlot();
                    }
                }
                
                if (uiSlotsScript.slotItemScriptableObject.itemCategory == ItemCategory.RAW_MATERIAL) {
                    uiSlotsScript.SetQuantity(ObjectsReference.Instance.rawMaterialsInventory.rawMaterialsInventory[uiSlotsScript.slotItemScriptableObject.rawMaterialType]);

                    if (ObjectsReference.Instance.rawMaterialsInventory.GetQuantity(uiSlotsScript.slotItemScriptableObject.rawMaterialType) <= 0) {
                        uiSlotsScript.EmptySlot();
                    }
                }

                if (uiSlotsScript.slotItemScriptableObject.itemCategory == ItemCategory.INGREDIENT) {
                    uiSlotsScript.SetQuantity(ObjectsReference.Instance.ingredientsInventory.ingredientsInventory[uiSlotsScript.slotItemScriptableObject.ingredientsType]);

                    if (ObjectsReference.Instance.ingredientsInventory.GetQuantity(uiSlotsScript.slotItemScriptableObject.ingredientsType) <= 0) {
                        uiSlotsScript.EmptySlot();
                    }
                }
            }
        }

        public void Switch_to_Slot_Index(int index) {
            selectedSlotIndex = index;
            foreach (var uiSlotsScript in uiSlotsScripts) {
                uiSlotsScript.SetUnselectedSlot();
            }

            uiSlotsScripts[selectedSlotIndex].SetSelectedSlot();
            ObjectsReference.Instance.slotSwitch.SwitchSlot(Get_Selected_Slot());
        }

        private void Switch_To_SelectedSlot() {
            foreach (var uiSlotsScript in uiSlotsScripts) {
                uiSlotsScript.SetUnselectedSlot();
            }

            uiSlotsScripts[selectedSlotIndex].SetSelectedSlot();
            ObjectsReference.Instance.slotSwitch.SwitchSlot(Get_Selected_Slot());
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
            if (selectedSlotIndex < uiSlotsScripts.Count - 1) {
                selectedSlotIndex++;
                Switch_To_SelectedSlot();
            }
            else {
                selectedSlotIndex = 3;
                Switch_To_SelectedSlot();
            }
        }
        
        public UIQuickSlot Get_Selected_Slot() {
            return uiSlotsScripts[selectedSlotIndex];
        }

        public BananaType Get_Selected_Slot_Type() {
            return uiSlotsScripts[selectedSlotIndex].slotItemScriptableObject.bananaType;
        }
    }
}