using UnityEngine;
using UnityEngine.InputSystem;

namespace SharedInputs {
    public class CommandRoomActions : InputActions {
        [SerializeField] private InputActionReference hideCommandRoomActionReference;
        
        private void OnEnable() {
            hideCommandRoomActionReference.action.Enable();
            hideCommandRoomActionReference.action.performed += HideCommandRoom;
        }

        private void OnDisable() {
            hideCommandRoomActionReference.action.Disable();
            hideCommandRoomActionReference.action.performed -= HideCommandRoom;
        }
        
        private void HideCommandRoom(InputAction.CallbackContext context) {
            ObjectsReference.Instance.commandRoomControlPanelsManager.UnfocusPanel(false);
            ObjectsReference.Instance.inputManager.SwitchBackToGame();
        }
    }
}