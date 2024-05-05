using InGame.Player.BananaGunActions;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SharedInputs {
    public class BananaGunScanActions : InputActions {
        [SerializeField] private InputActionReference RepairOrHarvestInputActionReference;
        [SerializeField] private InputActionReference grabBananaGunInputActionReference;
        [SerializeField] private InputActionReference checkDescriptionInputActionReference;

        [SerializeField] private Scan scan;

        private void OnEnable() {
            RepairOrHarvestInputActionReference.action.performed += RepairOrHarvest;
            RepairOrHarvestInputActionReference.action.Enable();

            grabBananaGunInputActionReference.action.performed += GrabBananaGun;
            grabBananaGunInputActionReference.action.canceled += UngrabBananaGun;
            grabBananaGunInputActionReference.action.Enable();

            checkDescriptionInputActionReference.action.performed += CheckDescription;
            checkDescriptionInputActionReference.action.canceled += StopCheckingDescription;
            checkDescriptionInputActionReference.action.Enable();
        }

        private void OnDisable() {
            RepairOrHarvestInputActionReference.action.performed -= RepairOrHarvest;
            RepairOrHarvestInputActionReference.action.Disable();

            grabBananaGunInputActionReference.action.performed -= GrabBananaGun;
            grabBananaGunInputActionReference.action.canceled -= UngrabBananaGun;
            grabBananaGunInputActionReference.action.Disable();

            checkDescriptionInputActionReference.action.performed -= CheckDescription;
            checkDescriptionInputActionReference.action.canceled -= StopCheckingDescription;
            checkDescriptionInputActionReference.action.Disable();
        }

        private void RepairOrHarvest(InputAction.CallbackContext context) {
            if (scan.enabled)
                scan.RepairOrHarvest();
        }

        private void CheckDescription(InputAction.CallbackContext context) {
            scan.isScanning = true;
        }

        private void StopCheckingDescription(InputAction.CallbackContext context) {
            scan.isScanning = false;
        }

        private void GrabBananaGun(InputAction.CallbackContext callbackContext) {
            ObjectsReference.Instance.bananaGun.GrabBananaGun();
            scan.enabled = true;
        }

        private void UngrabBananaGun(InputAction.CallbackContext callbackContext) {
            ObjectsReference.Instance.bananaGun.UngrabBananaGun();
            scan.isScanning = false;
            scan.enabled = false;
        }
    }
}