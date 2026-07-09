using UnityEngine;
using UnityEngine.InputSystem;

namespace SharedInputs {
    public class BigMapActions : InputActions {
        [SerializeField] private InputActionReference hideBigMapActionReference;
        
        private void OnEnable() {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            hideBigMapActionReference.action.Enable();
            hideBigMapActionReference.action.performed += HideBigMap;
        }

        private void OnDisable() {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            hideBigMapActionReference.action.Disable();
            hideBigMapActionReference.action.performed -= HideBigMap;
        }
        
        private void HideBigMap(InputAction.CallbackContext context) {
            ObjectsReference.Instance.uIMap.HideBigMap();
            ObjectsReference.Instance.inputManager.SwitchBackToGame();
        }
    }
}