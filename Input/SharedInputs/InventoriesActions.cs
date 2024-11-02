using UnityEngine;
using UnityEngine.InputSystem;

namespace SharedInputs {
    public class InventoriesActions : InputActions {
        [SerializeField] private InputActionReference closeInventoriesActionReference;

        private void OnEnable() {
            closeInventoriesActionReference.action.Enable();
            closeInventoriesActionReference.action.performed += CloseInventories;
        }

        private void OnDisable() {
            closeInventoriesActionReference.action.Disable();
            closeInventoriesActionReference.action.performed -= CloseInventories;
        }

        private void CloseInventories(InputAction.CallbackContext context) {
            ObjectsReference.Instance.uiManager.HideBananaGunUI();
            ObjectsReference.Instance.uiManager.HideInventories();
            ObjectsReference.Instance.inputManager.SwitchBackToGame();
        }
    }
}
