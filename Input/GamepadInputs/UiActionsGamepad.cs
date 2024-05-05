using UnityEngine;
using UnityEngine.InputSystem;

namespace GamepadInputs {
    public class UiActionsGamepad : InputActions {
        [SerializeField] private InputActionReference SwitchToLeftOptionPanel;
        [SerializeField] private InputActionReference SwitchToRightOptionPanel;

        private void OnEnable() {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            SwitchToLeftOptionPanel.action.Enable();
            SwitchToLeftOptionPanel.action.performed += Scroll_Left_Options_Tab;

            SwitchToRightOptionPanel.action.Enable();
            SwitchToRightOptionPanel.action.performed += Scroll_Right_Options_Tab;
        }

        private void OnDisable() {
            SwitchToLeftOptionPanel.action.Disable();
            SwitchToLeftOptionPanel.action.performed -= Scroll_Left_Options_Tab;

            SwitchToRightOptionPanel.action.Disable();
            SwitchToRightOptionPanel.action.performed -= Scroll_Right_Options_Tab;
        }

        private void Scroll_Left_Options_Tab(InputAction.CallbackContext context) {
            ObjectsReference.Instance.uiOptionsMenu.Switch_to_Left_Tab();
        }

        private void Scroll_Right_Options_Tab(InputAction.CallbackContext context) {
            ObjectsReference.Instance.uiOptionsMenu.Switch_to_Right_Tab();
        }
    }
}