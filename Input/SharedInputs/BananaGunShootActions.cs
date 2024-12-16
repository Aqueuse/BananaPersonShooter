using InGame.Player.BananaGunActions;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SharedInputs {
    public class BananaGunShootActions : InputActions {
        [SerializeField] private Shoot shoot;
        [SerializeField] private InputActionReference shootBananaActionReference;

        private void OnEnable() {
            shootBananaActionReference.action.Enable();
            shootBananaActionReference.action.performed += Shoot;
            shootBananaActionReference.action.canceled += CancelShoot;
        }

        private void OnDisable() {
            shootBananaActionReference.action.Disable();
            shootBananaActionReference.action.performed -= Shoot;
            shootBananaActionReference.action.canceled -= CancelShoot;
        }

        private void Shoot(InputAction.CallbackContext callbackContext) {
            ObjectsReference.Instance.bananaGun.GrabBananaGun();
            
            if (ObjectsReference.Instance.inventoriesHelper.GetActiveDroppedQuantity() <= 0) return;
            
            shoot.LoadingGun();
            ObjectsReference.Instance.uiCrosshairs.SetCrosshair(false);
        }

        private void CancelShoot(InputAction.CallbackContext callbackContext) {
            ObjectsReference.Instance.bananaGun.UngrabBananaGun();
            shoot.CancelThrow();
            ObjectsReference.Instance.uiCrosshairs.SetCrosshair(false);
        }
    }
}