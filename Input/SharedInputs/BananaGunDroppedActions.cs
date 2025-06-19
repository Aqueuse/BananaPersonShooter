using InGame.Player;
using InGame.Player.BananaGunActions;
using Tags;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SharedInputs {
    public class BananaGunDroppedActions : InputActions {
        [SerializeField] private ShootAction shootAction;
        
        [SerializeField] private InputActionReference shootActionReference;

        private Tag gameObjectTagClass;
        private GAME_OBJECT_TAG gameObjectTag;

        private BananaManData bananaManData;

        private void Start() {
            bananaManData = ObjectsReference.Instance.bananaMan.bananaManData;
        }

        private void OnEnable() {
            shootActionReference.action.Enable();
            shootActionReference.action.performed += Shoot;
            shootActionReference.action.canceled += CancelShoot;
        }

        private void OnDisable() {
            shootActionReference.action.Disable();
            shootActionReference.action.performed -= Shoot;
            shootActionReference.action.canceled -= CancelShoot;
        }

        private void Shoot(InputAction.CallbackContext callbackContext) {
            if (bananaManData.GetActiveSlotItemQuantity() <= 0) return;

            shootAction.LoadingGun();
        }

        private void CancelShoot(InputAction.CallbackContext callbackContext) {
            shootAction.CancelThrow();
        }
    }
}