using InGame.Player.BananaGunActions;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SharedInputs {
    public class BananaGunAspireActions : MonoBehaviour {
        [SerializeField] private InputActionReference aspireActionReference;
        [SerializeField] private AspireAction aspireAction;

        private void OnEnable() {
            aspireActionReference.action.Enable();
            aspireActionReference.action.performed += StartToAspire;
            aspireActionReference.action.canceled += StopToAspire;
        }

        private void OnDisable() {
            aspireActionReference.action.Disable();
            aspireActionReference.action.performed -= StartToAspire;
            aspireActionReference.action.canceled -= StopToAspire;
        }
        
        private void StartToAspire(InputAction.CallbackContext callbackContext) {
            aspireAction.enabled = true;
            aspireAction.LoadAspire();
        }

        private void StopToAspire(InputAction.CallbackContext callbackContext) {
            aspireAction.CancelAspire();
            aspireAction.enabled = false;
        }
    }
}