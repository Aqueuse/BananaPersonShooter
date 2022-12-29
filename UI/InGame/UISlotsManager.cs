using System.Collections.Generic;
using Enums;
using Player;
using UnityEngine;

namespace UI.InGame {
    public class UISlotsManager : MonoSingleton<UISlotsManager> {
        [SerializeField] private List<UISlot> uiSlotsScripts;
        
        private int _selectSlotIndex = 2;

        public List<UISlot> GetUISlotsScript() {
            return uiSlotsScripts;
        }
        
        public void Switch_To_Slot(int index) {
            _selectSlotIndex = index;
            foreach (var uiSlotsScript in uiSlotsScripts) {
                uiSlotsScript.SetUnselectedWeaponSlot();
            }
            uiSlotsScripts[_selectSlotIndex].SetSelectedWeaponSlot();
            
           UICrosshair.Instance.SetCrosshair(BananaMan.Instance.activeItemThrowableType);
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

        public void AssignToSelectedSlot(ItemThrowableType itemThrowableType) {
            Get_Selected_Slot().SetSlot(itemThrowableType);
            UICrosshair.Instance.SetCrosshair(itemThrowableType);
        }
    }
}