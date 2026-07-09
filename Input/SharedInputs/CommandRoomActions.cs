using UnityEngine;
using UnityEngine.InputSystem;

namespace SharedInputs {
    public class CommandRoomActions : InputActions {
        [SerializeField] private InputActionReference hideCommandRoomActionReference;
        
        private void OnEnable() {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            hideCommandRoomActionReference.action.Enable();
            hideCommandRoomActionReference.action.performed += HideCommandRoom;
        }

        private void OnDisable() {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            hideCommandRoomActionReference.action.Disable();
            hideCommandRoomActionReference.action.performed -= HideCommandRoom;
        }
        
        private void HideCommandRoom(InputAction.CallbackContext context) {
            ObjectsReference.Instance.commandRoomControlPanelsManager.UnfocusPanel(false);
            ObjectsReference.Instance.inputManager.SwitchBackToGame();
        }
    }
}