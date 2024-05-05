using UnityEngine;
using UnityEngine.InputSystem;

namespace KeyboardInputs {
    public class BananaGunActionsKeyboard : InputActions {
        [SerializeField] private InputActionReference switchToShootMode;
        [SerializeField] private InputActionReference switchToScanMode;
        [SerializeField] private InputActionReference switchToBuildMode;

        private void OnEnable() {
            switchToShootMode.action.Enable();
            switchToShootMode.action.performed += SwitchToShootMode;

            switchToScanMode.action.Enable();
            switchToScanMode.action.performed += SwitchToScanMode;

            switchToBuildMode.action.Enable();
            switchToBuildMode.action.performed += SwitchToBuildMode;
        }

        private void OnDisable() {
            switchToShootMode.action.Disable();
            switchToShootMode.action.performed -= SwitchToShootMode;

            switchToScanMode.action.Disable();
            switchToScanMode.action.performed -= SwitchToScanMode;

            switchToBuildMode.action.Disable();
            switchToBuildMode.action.performed -= SwitchToBuildMode;
        }

        private void SwitchToShootMode(InputAction.CallbackContext callbackContext) {
            ObjectsReference.Instance.bananaGunActionsSwitch.SwitchToBananaGunMode(BananaGunMode.SHOOT);
        }

        private void SwitchToScanMode(InputAction.CallbackContext callbackContext) {
            ObjectsReference.Instance.bananaGunActionsSwitch.SwitchToBananaGunMode(BananaGunMode.SCAN);
        }

        private void SwitchToBuildMode(InputAction.CallbackContext callbackContext) {
            ObjectsReference.Instance.bananaGunActionsSwitch.SwitchToBananaGunMode(BananaGunMode.BUILD);
        }
    }
}
