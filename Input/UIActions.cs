using UI;
using UI.InGame;
using UI.Menus;
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
            Scroll_Left_Options_Tab();
            Scroll_Right_Options_Tab();
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

        private void Scroll_Left_Options_Tab() {
            if (UnityEngine.Input.GetKeyDown(KeyCode.JoystickButton4)) {
                if (UIManager.Instance.optionsMenuCanvasGroup.alpha > 0) {
                    UIOptionsMenu.Instance.Switch_to_Left_Tab();
                }
                else {
                    if (UIManager.Instance.Is_Inventory_Visible()) {
                        Uihud.Instance.Switch_To_Inventory();
                    }
                }
            }
        }
        
        private void Scroll_Right_Options_Tab() {
            if (UnityEngine.Input.GetKeyDown(KeyCode.JoystickButton5)) {
                if (UIManager.Instance.optionsMenuCanvasGroup.alpha > 0) {
                    UIOptionsMenu.Instance.Switch_to_Right_Tab();
                }
                else {
                    if (UIManager.Instance.Is_Inventory_Visible()) {
                        Uihud.Instance.Switch_To_Statistics();
                    }
                }
            }
        }

        private void SwitchToUpperSlot() {
            if (UnityEngine.Input.GetAxis("DpadVertical") > 0 && !_scrolledUp) {
                UISlotsManager.Instance.Select_Upper_Slot();
                _scrolledUp = true;
            }

            if (UnityEngine.Input.GetAxis("DpadVertical") == 0) {
                _scrolledUp = false;
                _scrolledDown = false;
            }
        }
    
        private void SwitchToLowerSlot() {
            if (UnityEngine.Input.GetAxis("DpadVertical") < 0 && !_scrolledDown) {
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
