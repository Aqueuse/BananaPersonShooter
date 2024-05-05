using UnityEngine;
using UnityEngine.InputSystem;

namespace GamepadInputs {
    public class BananaGunUIActionsGamepad : InputActions {
        [SerializeField] private InputActionReference switchToTabLeftInputActionReference;
        [SerializeField] private InputActionReference switchToTabRightInputActionReference;
        
        private void OnEnable() {
            switchToTabLeftInputActionReference.action.performed += SwitchToTabLeft;
            switchToTabLeftInputActionReference.action.Enable();

            switchToTabRightInputActionReference.action.performed += SwitchToTabRight;
            switchToTabRightInputActionReference.action.Enable();
        }

        private void OnDisable() {
            switchToTabLeftInputActionReference.action.performed -= SwitchToTabLeft;
            switchToTabLeftInputActionReference.action.Disable();

            switchToTabRightInputActionReference.action.performed -= SwitchToTabRight;
            switchToTabRightInputActionReference.action.Disable();
        }
        
        private void SwitchToTabLeft(InputAction.CallbackContext callbackContext) {
            ObjectsReference.Instance.uiBananaGun.SwitchToLeftTab();
        }
        
        private void SwitchToTabRight(InputAction.CallbackContext callbackContext) {
            ObjectsReference.Instance.uiBananaGun.SwitchToRightTab();
        }
    }
}