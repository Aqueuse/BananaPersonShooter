using UnityEngine;
using UnityEngine.InputSystem;

namespace SharedInputs {
    public class MerchantActions : InputActions {
        [SerializeField] private InputActionReference hideMerchantUIActionReference;
        
        private void OnEnable() {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            hideMerchantUIActionReference.action.Enable();
            hideMerchantUIActionReference.action.performed += HideMerchantUI;
        }

        private void OnDisable() {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            hideMerchantUIActionReference.action.Disable();
            hideMerchantUIActionReference.action.performed -= HideMerchantUI;
        }
        
        private void HideMerchantUI(InputAction.CallbackContext context) {
            ObjectsReference.Instance.uiMerchant.HideMerchant();
            ObjectsReference.Instance.inputManager.SwitchBackToGame();
        }
    }
}