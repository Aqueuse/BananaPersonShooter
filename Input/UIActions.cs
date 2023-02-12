using UI;
using UI.InGame;
using UI.InGame.QuickSlots;
using UI.Menus;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Input {
    public class UIActions : MonoSingleton<UIActions> {
        private bool _scrolledUp;
        private bool _scrolledDown;

        private bool _scrolledLeft;
        private bool _scrolledRight;

        public Vector2 scrollSlotsValue;

        public EventTrigger selectedTrigger;
        PointerEventData pointer;

        private float counter;
        private float slowDownValue;
        
        private void Start() {
            pointer = new PointerEventData(EventSystem.current);
            slowDownValue = 0.15f;
        }

        private void Update() {
            Activate();
            Escape();
            Scroll_Left_Options_Tab();
            Scroll_Right_Options_Tab();

            Scroll_Right_Button();
            Scroll_Left_Button();

            if (GameManager.Instance.isInGame) {
                Hide_Inventory();
                SwitchToUpperSlot();
                SwitchToLowerSlot();
                Scroll_Slots();
            }
        }

        private void Activate() {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Return) || UnityEngine.Input.GetKeyDown(KeyCode.JoystickButton0)) {
                ExecuteEvents.Execute(selectedTrigger.gameObject, pointer, ExecuteEvents.pointerDownHandler);
            }
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
        
        private void Scroll_Left_Button() {
            if (UnityEngine.Input.GetAxis("Horizontal") < 0 && !_scrolledLeft) {
                _scrolledLeft = true;

                if (!GameManager.Instance.isInGame) {
                    UIHomeMenu.Instance.SwitchToLeftButton();
                }

                else {
                    counter+=Time.deltaTime;
                    if (counter > slowDownValue) {
                        counter = 0;
                        UiGameMenu.Instance.SwitchToLeftButton();
                    }
                }
            }

            if (UnityEngine.Input.GetAxis("Horizontal") == 0) {
                _scrolledLeft = false;
                _scrolledRight = false;
            }
        }

        private void Scroll_Right_Button() {
            if (UnityEngine.Input.GetAxis("Horizontal") > 0 && !_scrolledRight) {
                _scrolledRight = true;

                if (!GameManager.Instance.isInGame) {
                    UIHomeMenu.Instance.SwitchToRightButton();
                }

                else {
                    counter+=Time.deltaTime;
                    if (counter > slowDownValue) {
                        counter = 0;
                        UiGameMenu.Instance.SwitchToRightButton();
                    }
                }
            }
            
            if (UnityEngine.Input.GetAxis("Horizontal") == 0) {
                _scrolledLeft = false;
                _scrolledRight = false;
            }
        }
        
        private void SwitchToUpperSlot() {
            if (UnityEngine.Input.GetAxis("DpadVertical") > 0 && !_scrolledUp) {
                counter+=Time.deltaTime;
                if (counter > slowDownValue) {
                    counter = 0;
                    UISlotsManager.Instance.Select_Upper_Slot();
                    _scrolledUp = true;
                }
            }

            if (UnityEngine.Input.GetAxis("DpadVertical") == 0) {
                _scrolledUp = false;
                _scrolledDown = false;
            }
        }
    
        private void SwitchToLowerSlot() {
            if (UnityEngine.Input.GetAxis("DpadVertical") < 0 && !_scrolledDown) {
                counter+=Time.deltaTime;
                if (counter > slowDownValue) {
                    counter = 0;
                    UISlotsManager.Instance.Select_Lower_Slot();
                    _scrolledDown = true;
                }
            }
        }
    
        private void Scroll_Slots() {
            scrollSlotsValue = UnityEngine.Input.mouseScrollDelta;
            
            var scrollValue = scrollSlotsValue.y;
            if (scrollValue < 0) UISlotsManager.Instance.Select_Upper_Slot();
            if (scrollValue > 0) UISlotsManager.Instance.Select_Lower_Slot();
        }
    }
}
