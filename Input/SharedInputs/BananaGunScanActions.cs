using InGame.Player.BananaGunActions;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SharedInputs {
    public class BananaGunScanActions : InputActions {
        [SerializeField] private InputActionReference RepairOrHarvestInputActionReference;
        [SerializeField] private InputActionReference grabBananaGunInputActionReference;

        [SerializeField] private Scan scan;

        private void OnEnable() {
            RepairOrHarvestInputActionReference.action.performed += RepairOrHarvest;
            RepairOrHarvestInputActionReference.action.Enable();

            grabBananaGunInputActionReference.action.performed += GrabBananaGun;
            grabBananaGunInputActionReference.action.canceled += UngrabBananaGun;
            grabBananaGunInputActionReference.action.Enable();
        }

        private void OnDisable() {
            RepairOrHarvestInputActionReference.action.performed -= RepairOrHarvest;
            RepairOrHarvestInputActionReference.action.Disable();

            grabBananaGunInputActionReference.action.performed -= GrabBananaGun;
            grabBananaGunInputActionReference.action.canceled -= UngrabBananaGun;
            grabBananaGunInputActionReference.action.Disable();
        }

        private void RepairOrHarvest(InputAction.CallbackContext context) {
            if (scan.enabled)
                scan.RepairOrHarvest();
        }
        
        private void GrabBananaGun(InputAction.CallbackContext callbackContext) {
            ObjectsReference.Instance.bananaGun.GrabBananaGun();
            scan.enabled = true;
        }

        private void UngrabBananaGun(InputAction.CallbackContext callbackContext) {
            ObjectsReference.Instance.bananaGun.UngrabBananaGun();
            scan.enabled = false;
        }
    }
}