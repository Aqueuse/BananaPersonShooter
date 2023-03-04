using UI;
using UI.InGame.QuickSlots;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Input.UIActions {
    public class UIInventoryActions : MonoBehaviour {
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
            
            Hide_Interface();
            SwitchToUpperSlot();
            SwitchToLowerSlot();
            Scroll_Slots();
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
        
        private void Hide_Interface() {
            if (UnityEngine.Input.GetKeyDown(KeyCode.I) || UnityEngine.Input.GetAxis("DpadHorizontal") > 0) {
                UIManager.Instance.Show_Hide_interface();
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
