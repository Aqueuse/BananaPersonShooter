using UnityEngine;
using UnityEngine.EventSystems;

namespace Input.UIActions {
    public class UIHomeMenuActions : MonoBehaviour {
        [SerializeField] private GameObject homeMenu;
        
        private bool _scrolledUp;
        private bool _scrolledDown;

        private bool _scrolledLeft;
        private bool _scrolledRight;
        
        PointerEventData pointer;

        private float counter;
        
        private void Start() {
            pointer = new PointerEventData(EventSystem.current);
            ObjectsReference.Instance.uiHomeMenu.selectedTrigger = homeMenu.GetComponentsInChildren<EventTrigger>()[0];
        }
        
        private void Update() {
            Activate();
            Escape();

            Scroll_Left_Home_Menu_Button();
            Scroll_Right_Home_Menu_Button();
        }

        private void Activate() {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Return) || UnityEngine.Input.GetKeyDown(KeyCode.JoystickButton0)) {
                ExecuteEvents.Execute(ObjectsReference.Instance.uiHomeMenu.selectedTrigger.gameObject, pointer, ExecuteEvents.pointerDownHandler);
            }
        }

        private void Escape() {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Escape) || UnityEngine.Input.GetKeyDown(KeyCode.JoystickButton1)) {
                ObjectsReference.Instance.uiManager.Hide_menus();
            }
        }
        
        private void Scroll_Left_Home_Menu_Button() {
            if (UnityEngine.Input.GetAxis("Horizontal") < 0 && !_scrolledLeft) {
                _scrolledLeft = true;

                ObjectsReference.Instance.uiHomeMenu.SwitchToLeftHomeMenuButton();
            }

            if (UnityEngine.Input.GetAxis("Horizontal") == 0) {
                _scrolledLeft = false;
                _scrolledRight = false;
            }
        }

        private void Scroll_Right_Home_Menu_Button() {
            if (UnityEngine.Input.GetAxis("Horizontal") > 0 && !_scrolledRight) {
                _scrolledRight = true;

                ObjectsReference.Instance.uiHomeMenu.SwitchToRightHomeMenuButton();
            }
            
            if (UnityEngine.Input.GetAxis("Horizontal") == 0) {
                _scrolledLeft = false;
                _scrolledRight = false;
            }
        }
    }
}
