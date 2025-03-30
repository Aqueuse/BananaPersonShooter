using InGame.Player;
using InGame.Player.BananaGunActions;
using Tags;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SharedInputs {
    public class BananaGunShootActions : InputActions {
        [SerializeField] private Shoot shoot;
        
        [SerializeField] private InputActionReference shootActionReference;
        [SerializeField] private InputActionReference aspireActionReference;

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
            
            aspireActionReference.action.Enable();
            aspireActionReference.action.performed += StartToAspire;
            aspireActionReference.action.canceled += StopToAspire;
        }

        private void OnDisable() {
            shootActionReference.action.Disable();
            shootActionReference.action.performed -= Shoot;
            shootActionReference.action.canceled -= CancelShoot;
            
            aspireActionReference.action.Disable();
            aspireActionReference.action.performed -= StartToAspire;
            aspireActionReference.action.canceled -= StopToAspire;
        }

        private void Shoot(InputAction.CallbackContext callbackContext) {
            if (bananaManData.inventoriesByDroppedType[bananaManData.activeDropped].GetQuantity(
                    bananaManData.activeDroppableItem) <= 0) 
                return;
            
            shoot.LoadingGun();
        }

        private void CancelShoot(InputAction.CallbackContext callbackContext) {
            shoot.CancelThrow();
        }

        private void StartToAspire(InputAction.CallbackContext callbackContext) {
            shoot.LoadAspire();
        }

        private void StopToAspire(InputAction.CallbackContext callbackContext) {
            shoot.CancelAspire();
        }
    }
}