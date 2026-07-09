using UnityEngine;
using UnityEngine.InputSystem;

namespace SharedInputs {
    public class GuichetActions : InputActions {
        [SerializeField] private InputActionReference hideGuichetActionReference;
        
        private void OnEnable() {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            hideGuichetActionReference.action.Enable();
            hideGuichetActionReference.action.performed += HideGuichet;
        }

        private void OnDisable() {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            hideGuichetActionReference.action.Disable();
            hideGuichetActionReference.action.performed -= HideGuichet;
        }
        
        private void HideGuichet(InputAction.CallbackContext context) {
            if (ObjectsReference.Instance.uiGuichet.activatedGuichet != null)
                ObjectsReference.Instance.uiGuichet.activatedGuichet.CloseGuichet();
            
            ObjectsReference.Instance.inputManager.SwitchBackToGame();
        }
    }
}