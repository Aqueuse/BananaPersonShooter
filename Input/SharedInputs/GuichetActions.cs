using UnityEngine;
using UnityEngine.InputSystem;

namespace SharedInputs {
    public class GuichetActions : InputActions {
        [SerializeField] private InputActionReference hideGuichetActionReference;
        
        private void OnEnable() {
            hideGuichetActionReference.action.Enable();
            hideGuichetActionReference.action.performed += HideGuichet;
        }

        private void OnDisable() {
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