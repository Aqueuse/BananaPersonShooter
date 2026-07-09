using UnityEngine;
using UnityEngine.InputSystem;

namespace SharedInputs {
    public class GameMenuActions : InputActions {
        [SerializeField] private InputActionReference hideGameMenuActionReference;
        
        private void OnEnable() {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            hideGameMenuActionReference.action.Enable();
            hideGameMenuActionReference.action.performed += HideGameMenu;
        }

        private void OnDisable() {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            hideGameMenuActionReference.action.Disable();
            hideGameMenuActionReference.action.performed -= HideGameMenu;
        }
        
        private void HideGameMenu(InputAction.CallbackContext context) {
            ObjectsReference.Instance.uIGameMenu.HideGameMenu();
            ObjectsReference.Instance.inputManager.SwitchBackToGame();
        }
    }
}