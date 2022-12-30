using System.Collections.Generic;
using Data;
using Enums;
using Player;
using UI.InGame.Inventory;

namespace UI.InGame {
    public class UISlotsManager : MonoSingleton<UISlotsManager> {
        public List<UISlot> uiSlotsScripts;

        public Dictionary<int, int> slotsMappingToInventory;

        private int _selectSlotIndex = 2;

        private void Start() {
            slotsMappingToInventory = new Dictionary<int, int>(5) {
                {0, 0},
                {1, 0},
                {2, 0},
                {3, 0},
                {4, 0}
            };
        }

        public void Switch_To_Slot(int index) {
            _selectSlotIndex = index;
            foreach (var uiSlotsScript in uiSlotsScripts) {
                uiSlotsScript.SetUnselectedWeaponSlot();
            }
            
            if (Get_Selected_Slot().itemThrowableCategory == ItemThrowableCategory.BANANA) {
                BananaMan.Instance.activeItem = ScriptableObjectManager.Instance.GetBananaScriptableObject(uiSlotsScripts[_selectSlotIndex].itemThrowableType);
            }

            uiSlotsScripts[_selectSlotIndex].SetSelectedWeaponSlot();
            BananaMan.Instance.activeItemThrowableType = uiSlotsScripts[_selectSlotIndex].itemThrowableType;
            BananaMan.Instance.activeItemThrowableCategory = uiSlotsScripts[_selectSlotIndex].itemThrowableCategory;

            UICrosshair.Instance.SetCrosshair(BananaMan.Instance.activeItemThrowableType, BananaMan.Instance.activeItemThrowableCategory);
        }

        public void Select_Left_Slot() {
            if (_selectSlotIndex > 0) {
                _selectSlotIndex--;
                Switch_To_Slot(_selectSlotIndex);
            }
        }
        
        public void Select_Right_Slot() {
            if (_selectSlotIndex < 4) {
                _selectSlotIndex++;
                Switch_To_Slot(_selectSlotIndex);
            }
        }

        public UISlot Get_Selected_Slot() {
            return uiSlotsScripts[_selectSlotIndex];
        }

        public void AssignToSelectedSlot(ItemThrowableType itemThrowableType, ItemThrowableCategory itemThrowableCategory) {
            slotsMappingToInventory[_selectSlotIndex] = UInventory.Instance.GetSlotIndex(itemThrowableType);
            
            Get_Selected_Slot().SetSlot(itemThrowableType, itemThrowableCategory);
            UICrosshair.Instance.SetCrosshair(itemThrowableType, itemThrowableCategory);
        }
    }
}