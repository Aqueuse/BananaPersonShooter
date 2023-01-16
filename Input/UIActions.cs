using UI;
using UI.InGame;
using UnityEngine;

namespace Input {
    public class UIActions : MonoSingleton<UIActions> {
        private bool _scrolledUp;
        private bool _scrolledDown;
        
        public Vector2 scrollSlotsValue;
        
        private void Update() {
            Escape();
            Hide_Inventory();
            SwitchToUpperSlot();
            SwitchToLowerSlot();
            Scroll_Slots();
        }

        private void Escape() {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Escape) || UnityEngine.Input.GetKeyDown(KeyCode.JoystickButton1)) {
                UIManager.Instance.Hide_menus();
            }
        }
        
        private void Hide_Inventory() {
            if (UnityEngine.Input.GetKeyDown(KeyCode.I) || UnityEngine.Input.GetKeyDown(KeyCode.JoystickButton6)) {
                UIManager.Instance.Show_Hide_inventory();
            }
        }
        
        private void SwitchToUpperSlot() {
            if (UnityEngine.Input.GetAxis("DpadHorizontal") > 0 && !_scrolledUp) {
                UISlotsManager.Instance.Select_Upper_Slot();
                _scrolledUp = true;
            }

            if (UnityEngine.Input.GetAxis("DpadHorizontal") == 0) {
                _scrolledUp = false;
                _scrolledDown = false;
            }
        }
    
        private void SwitchToLowerSlot() {
            if (UnityEngine.Input.GetAxis("DpadHorizontal") < 0 && !_scrolledDown) {
                UISlotsManager.Instance.Select_Lower_Slot();
                _scrolledDown = true;
            }
        }
    
        public void Scroll_Slots() {
            scrollSlotsValue = UnityEngine.Input.mouseScrollDelta;
            
            float scrollValue = scrollSlotsValue.y;
            if (scrollValue < 0) UISlotsManager.Instance.Select_Upper_Slot();
            if (scrollValue > 0) UISlotsManager.Instance.Select_Lower_Slot();
        }

    }
}
