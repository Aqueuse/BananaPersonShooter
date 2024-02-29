using InGame.CommandRoomPanelControls;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Input {
    public class GestionCommandRoomPanelActions : InputActions {
        [SerializeField] private InputActionReference quitActionReference;

        private void OnEnable() {
            ObjectsReference.Instance.uiActions.enabled = true;
            
            quitActionReference.action.Enable();
            quitActionReference.action.performed += Quit;
        }

        private void OnDisable() {
            quitActionReference.action.Disable();
            quitActionReference.action.performed -= Quit;
        }

        private void Quit(InputAction.CallbackContext callbackContext) {
            CommandRoomControlPanelsManager.Instance.gestionPanel.SwitchBackToGame();
        }
    }
}