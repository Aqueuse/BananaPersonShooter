using UnityEngine;
using UnityEngine.InputSystem;

namespace SharedInputs {
    public class MerchantActions : InputActions {
        [SerializeField] private InputActionReference hideMerchantUIActionReference;
        
        private void OnEnable() {
            hideMerchantUIActionReference.action.Enable();
            hideMerchantUIActionReference.action.performed += HideMerchantUI;
        }

        private void OnDisable() {
            hideMerchantUIActionReference.action.Disable();
            hideMerchantUIActionReference.action.performed -= HideMerchantUI;
        }
        
        private void HideMerchantUI(InputAction.CallbackContext context) {
            ObjectsReference.Instance.uiMerchant.HideMerchant();
            ObjectsReference.Instance.inputManager.SwitchBackToGame();
        }
    }
}