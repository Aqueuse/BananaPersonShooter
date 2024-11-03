using InGame.Player.BananaGunActions;
using UnityEngine;
using UnityEngine.InputSystem;

namespace KeyboardInputs {
    public class GameActionsKeyboard : InputActions {
        [SerializeField] private InputActionReference switchToShootMode;
        [SerializeField] private InputActionReference switchToScanMode;
        [SerializeField] private InputActionReference switchToBuildMode;

        [SerializeField] private InputActionReference scrollModes;

        private BananaGunActionsSwitch bananaGunActionsSwitch;

        private void Start() {
            bananaGunActionsSwitch = ObjectsReference.Instance.bananaGunActionsSwitch;
        }

        private void OnEnable() {
            switchToShootMode.action.Enable();
            switchToShootMode.action.performed += SwitchToShootMode;

            switchToScanMode.action.Enable();
            switchToScanMode.action.performed += SwitchToScanMode;

            switchToBuildMode.action.Enable();
            switchToBuildMode.action.performed += SwitchToBuildMode;
            
            scrollModes.action.Enable();
            scrollModes.action.performed += ScrollModes;
        }

        private void OnDisable() {
            switchToShootMode.action.Disable();
            switchToShootMode.action.performed -= SwitchToShootMode;

            switchToScanMode.action.Disable();
            switchToScanMode.action.performed -= SwitchToScanMode;

            switchToBuildMode.action.Disable();
            switchToBuildMode.action.performed -= SwitchToBuildMode;
            
            scrollModes.action.Disable();
            scrollModes.action.performed -= ScrollModes;
        }

        private void SwitchToShootMode(InputAction.CallbackContext callbackContext) {
            if (!ObjectsReference.Instance.bananaMan.tutorialFinished) return;

            bananaGunActionsSwitch.SwitchToBananaGunMode(BananaGunMode.SHOOT);
        }

        private void SwitchToScanMode(InputAction.CallbackContext callbackContext) {
            if (!ObjectsReference.Instance.bananaMan.tutorialFinished) return;
            
            bananaGunActionsSwitch.SwitchToBananaGunMode(BananaGunMode.SCAN);
        }

        private void SwitchToBuildMode(InputAction.CallbackContext callbackContext) {
            if (!ObjectsReference.Instance.bananaMan.tutorialFinished) return;

            bananaGunActionsSwitch.SwitchToBananaGunMode(BananaGunMode.BUILD);
        }

        private void ScrollModes(InputAction.CallbackContext callbackContext) {
            if (!ObjectsReference.Instance.bananaMan.tutorialFinished) return;
            
            if (callbackContext.ReadValue<Vector2>().y > 1) {
                bananaGunActionsSwitch.SwitchToLeftMode();
            }
            if (callbackContext.ReadValue<Vector2>().y < 1) {
                bananaGunActionsSwitch.SwitchToRightMode();
            }
        }
    }
}
