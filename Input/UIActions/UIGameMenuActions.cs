using UnityEngine;
using UnityEngine.EventSystems;

namespace Input.UIActions {
    public class UIGameMenuActions : MonoBehaviour {
        private bool _scrolledUp;
        private bool _scrolledDown;

        private bool _scrolledLeft;
        private bool _scrolledRight;

        private PointerEventData _pointer;

        private void Start() {
            _pointer = new PointerEventData(EventSystem.current);
        }

        private void Update() {
            Activate();
            Escape();

            Scroll_Left_Game_Menu_Button();
            Scroll_Right_Game_Menu_Button();
        }

        private void Activate() {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Return) || UnityEngine.Input.GetKeyDown(KeyCode.JoystickButton0)) {
                ExecuteEvents.Execute(ObjectsReference.Instance.uiGameMenu.selectedTrigger.gameObject, _pointer, ExecuteEvents.pointerDownHandler);
            }
        }

        private static void Escape() {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Escape) || UnityEngine.Input.GetKeyDown(KeyCode.JoystickButton1)) {
                ObjectsReference.Instance.uiManager.Hide_Game_Menu();
                ObjectsReference.Instance.gameManager.UnpauseGame();
            }
        }

        private void Scroll_Left_Game_Menu_Button() {
            if (UnityEngine.Input.GetAxis("Horizontal") < 0 && !_scrolledLeft) {
                _scrolledLeft = true;

                ObjectsReference.Instance.uiGameMenu.SwitchToLeftButton();
            }

            if (UnityEngine.Input.GetAxis("Horizontal") == 0) {
                _scrolledLeft = false;
                _scrolledRight = false;
            }
        }

        private void Scroll_Right_Game_Menu_Button() {
            if (UnityEngine.Input.GetAxis("Horizontal") > 0 && !_scrolledRight) {
                _scrolledRight = true;
                
                ObjectsReference.Instance.uiGameMenu.SwitchToRightButton();
            }

            if (UnityEngine.Input.GetAxis("Horizontal") == 0) {
                _scrolledLeft = false;
                _scrolledRight = false;
            }
        }
    }
}