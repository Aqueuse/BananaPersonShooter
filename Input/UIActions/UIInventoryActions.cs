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
        PointerEventData _pointer;

        private float _counter;
        private float _slowDownValue;
        
        private void Start() {
            _pointer = new PointerEventData(EventSystem.current);
            _slowDownValue = 0.15f;
        }

        private void Update() {
            Activate();
            Hide_Interface();
            
            SwitchToUpperSlot();
            SwitchToLowerSlot();
            Scroll_Slots();
        }

        private void Activate() {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Return) || UnityEngine.Input.GetKeyDown(KeyCode.JoystickButton0)) {
                ExecuteEvents.Execute(selectedTrigger.gameObject, _pointer, ExecuteEvents.pointerDownHandler);
            }
        }
        
        private void Hide_Interface() {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Escape) || UnityEngine.Input.GetKeyDown(KeyCode.I) || UnityEngine.Input.GetAxis("DpadHorizontal") > 0) {
                ObjectsReference.Instance.uiManager.Show_Hide_interface();
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
