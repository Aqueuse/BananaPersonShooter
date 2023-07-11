using UnityEngine;

namespace Input.UIActions {
    public class UIBlueprintsActions : MonoBehaviour {
        private bool _scrolledUp;
        private bool _scrolledDown;

        private bool _scrolledLeft;
        private bool _scrolledRight;

        public Vector2 scrollSlotsValue;

        private float _counter;
        private float _slowDownValue;
        
        private void Start() {
            _slowDownValue = 0.15f;
        }

        private void Update() {
            Hide_Interface();
            
            Switch_To_Left();
            Switch_To_Right();
            
            Switch_To_Inventory();
            Switch_To_Chimployee();
            
            SwitchToUpperSlot();
            SwitchToLowerSlot();
            Scroll_Slots();
        }
        
        private static void Hide_Interface() {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Escape) || UnityEngine.Input.GetKeyDown(KeyCode.I) || UnityEngine.Input.GetAxis("DpadHorizontal") > 0) {
                ObjectsReference.Instance.uiManager.Show_Hide_interface();
            }
        }
        
        private static void Switch_To_Left() {
            if (UnityEngine.Input.GetKeyDown(KeyCode.JoystickButton4)) {
                ObjectsReference.Instance.uihud.Switch_To_Left_Tab();
            }
        }

        private static void Switch_To_Right() {
            if (UnityEngine.Input.GetKeyDown(KeyCode.JoystickButton5)) {
                ObjectsReference.Instance.uihud.Switch_To_Right_Tab();
            }
        }
        
        private void Switch_To_Inventory() {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha1)) {
                ObjectsReference.Instance.uihud.Switch_To_Inventory();
            }
        }
        
        private void Switch_To_Chimployee() {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha3)) {
                ObjectsReference.Instance.uihud.Switch_To_Chimployee();
            }
        }

        private void SwitchToUpperSlot() {
            if (UnityEngine.Input.GetAxis("DpadVertical") > 0 && !_scrolledUp) {
                _counter+=Time.deltaTime;
                if (_counter > _slowDownValue) {
                    _counter = 0;
                    ObjectsReference.Instance.uiSlotsManager.Select_Upper_Slot();
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
                _counter+=Time.deltaTime;
                if (_counter > _slowDownValue) {
                    _counter = 0;
                    ObjectsReference.Instance.uiSlotsManager.Select_Lower_Slot();
                    _scrolledDown = true;
                }
            }
        }
    
        private void Scroll_Slots() {
            scrollSlotsValue = UnityEngine.Input.mouseScrollDelta;
            
            var scrollValue = scrollSlotsValue.y;
            if (scrollValue < 0) ObjectsReference.Instance.uiSlotsManager.Select_Upper_Slot();
            if (scrollValue > 0) ObjectsReference.Instance.uiSlotsManager.Select_Lower_Slot();
        }
    }
}
