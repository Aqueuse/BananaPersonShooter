﻿using System.Collections.Generic;
using UnityEngine;

namespace UI.InGame.QuickSlots {
    public class UISlotsManager : MonoBehaviour {
        public List<UISlot> uiSlotsScripts;
        public int selectedSlotIndex;
        
        public void RefreshQuantityInQuickSlot(ItemCategory itemCategory, ItemType itemType = ItemType.EMPTY) {
            foreach (var uiSlotsScript in uiSlotsScripts) {
                if (itemCategory != ItemCategory.BUILDABLE && uiSlotsScript.itemType == itemType) {
                    uiSlotsScript.SetAmmoQuantity(ObjectsReference.Instance.inventory.bananaManInventory[itemType]);

                    if (ObjectsReference.Instance.inventory.GetQuantity(uiSlotsScript.itemType) <= 0) {
                        uiSlotsScript.EmptySlot();
                    }
                }

                else {
                    if (uiSlotsScript.itemCategory == ItemCategory.EMPTY) {
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
            if (selectedSlotIndex < uiSlotsScripts.Count-1) {
                selectedSlotIndex++;
                Switch_To_SelectedSlot();
            }
            else {
                selectedSlotIndex = 3;
                Switch_To_SelectedSlot();
            }
        }
        
        public void AssignToSelectedSlot(ItemCategory itemCategory, ItemType itemType = ItemType.EMPTY, BuildableType buildableType = BuildableType.EMPTY) {
            foreach (var uiSlot in uiSlotsScripts) {
                if (itemCategory == ItemCategory.BUILDABLE && uiSlot.buildableType == buildableType) {
                    uiSlot.EmptySlot();
                }
                if (itemCategory != ItemCategory.BUILDABLE && uiSlot.itemType == itemType) {
                    uiSlot.EmptySlot();
                }
            }
            
            Get_Selected_Slot().SetSlot(itemCategory, slotItemType:itemType, slotBuildableType:buildableType);
        }

        public void TryToPutOnSlot(ItemCategory itemCategory, ItemType itemType = ItemType.EMPTY, BuildableType buildableType = BuildableType.EMPTY) {
            if (IsAlreadyInQuickSlot(itemCategory, itemType, buildableType)) {
                var uislot = GetSlotWithItemCategoryAndType(itemCategory, itemType, buildableType);
                
                uislot.SetSlot(itemCategory, slotItemType:itemType, slotBuildableType:buildableType);
            }

            else {
                foreach (var uiSlot in uiSlotsScripts) {
                    if (uiSlot.itemCategory == ItemCategory.EMPTY && uiSlot.itemType == ItemType.EMPTY) {
                        uiSlot.SetSlot(itemCategory, slotItemType:itemType, slotBuildableType:buildableType);
                        return;
                    }
                }
            }
        }

        public void SetActiveSlot() {
            var activeItemCategory = ObjectsReference.Instance.bananaMan.activeItemCategory;
            var activeItemType = ObjectsReference.Instance.bananaMan.activeItemType;
            var activeBuilableType = ObjectsReference.Instance.bananaMan.activeBuildableType;
            
            var uiSlot = GetSlotWithItemCategoryAndType(activeItemCategory, activeItemType, activeBuilableType);
            selectedSlotIndex = uiSlot != null ? uiSlotsScripts.IndexOf(uiSlot) : 0;
            Switch_to_Slot_Index(selectedSlotIndex);
        }

        private bool IsAlreadyInQuickSlot(ItemCategory itemCategory, ItemType itemType = ItemType.EMPTY, BuildableType buildableType = BuildableType.EMPTY) {
            foreach (var uiSlot in uiSlotsScripts) {
                if (itemCategory == ItemCategory.BUILDABLE && uiSlot.buildableType == buildableType) {
                    return true;
                }

                if (itemCategory != ItemCategory.BUILDABLE && uiSlot.itemType == itemType) {
                    return true;
                }
            }
            return false;
        }
        
        public UISlot Get_Selected_Slot() {
            return uiSlotsScripts[selectedSlotIndex];
        }

        private UISlot GetSlotWithItemCategoryAndType(ItemCategory itemCategory, ItemType itemType = ItemType.EMPTY, BuildableType buildableType = BuildableType.EMPTY) {
            foreach (var uiSlot in uiSlotsScripts) {
                if (itemCategory == ItemCategory.BUILDABLE && uiSlot.buildableType == buildableType) {
                    return uiSlot;
                }

                if (itemCategory != ItemCategory.BUILDABLE && uiSlot.itemType == itemType) {
                    return uiSlot;
                }
            }

            return null;
        }
        
        public ItemType Get_Selected_Slot_Type() {
            return uiSlotsScripts[selectedSlotIndex].itemType;
        }
    }
}