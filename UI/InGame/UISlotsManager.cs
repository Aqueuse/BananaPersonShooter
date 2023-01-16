using System.Collections.Generic;
using Data;
using Enums;
using Player;
using UI.InGame.Inventory;

namespace UI.InGame {
    public class UISlotsManager : MonoSingleton<UISlotsManager> {
        public List<UISlot> uiSlotsScripts;

        public GenericDictionary<int, int> slotsMappingToInventory;

        private int _selectedSlotIndex = 2;

        private void Start() {
            slotsMappingToInventory = new GenericDictionary<int, int> {
                {0, 0},
                {1, 0},
                {2, 0},
                {3, 0}
            };
        }

        public void Switch_To_SelectedSlot() {
            foreach (var uiSlotsScript in uiSlotsScripts) {
                uiSlotsScript.SetUnselectedWeaponSlot();
            }
            
            if (Get_Selected_Slot().itemThrowableCategory == ItemThrowableCategory.BANANA) {
                BananaMan.Instance.activeItem = ScriptableObjectManager.Instance.GetBananaScriptableObject(uiSlotsScripts[_selectedSlotIndex].itemThrowableType);
            }

            uiSlotsScripts[_selectedSlotIndex].SetSelectedWeaponSlot();
            BananaMan.Instance.activeItemThrowableType = uiSlotsScripts[_selectedSlotIndex].itemThrowableType;
            BananaMan.Instance.activeItemThrowableCategory = uiSlotsScripts[_selectedSlotIndex].itemThrowableCategory;

            UICrosshair.Instance.SetCrosshair(BananaMan.Instance.activeItemThrowableType, BananaMan.Instance.activeItemThrowableCategory);
        }

        public void RefreshQuantityInQuickSlot(ItemThrowableType itemThrowableType) {
            foreach (var uiSlotsScript in uiSlotsScripts) {
                if (uiSlotsScript.itemThrowableType == itemThrowableType) {
                    uiSlotsScript.SetAmmoQuantity(global::Inventory.Instance.bananaManInventory[itemThrowableType]);
                }

                if (uiSlotsScript.itemThrowableType == ItemThrowableType.EMPTY) {
                    uiSlotsScript.EmptySlot();
                }
            }
        }

        public void Select_Upper_Slot() {
            if (_selectedSlotIndex > 0) {
                _selectedSlotIndex--;
                Switch_To_SelectedSlot();
            }
            else {
                _selectedSlotIndex = 0;
                Switch_To_SelectedSlot();
            }
        }
        
        public void Select_Lower_Slot() {
            if (_selectedSlotIndex < uiSlotsScripts.Count-1) {
                _selectedSlotIndex++;
                Switch_To_SelectedSlot();
            }
            else {
                _selectedSlotIndex = 3;
                Switch_To_SelectedSlot();
            }
        }

        public UISlot Get_Selected_Slot() {
            return uiSlotsScripts[_selectedSlotIndex];
        }

        public void AssignToSelectedSlot(ItemThrowableType itemThrowableType, ItemThrowableCategory itemThrowableCategory) {
            foreach (var uiSlot in uiSlotsScripts) {
                if (uiSlot.itemThrowableType == itemThrowableType) {
                    uiSlot.EmptySlot();
                }
            }

            slotsMappingToInventory[_selectedSlotIndex] = UInventory.Instance.GetSlotIndex(itemThrowableType);
            
            Get_Selected_Slot().SetSlot(itemThrowableType, itemThrowableCategory);
            UICrosshair.Instance.SetCrosshair(itemThrowableType, itemThrowableCategory);
        }
    }
}