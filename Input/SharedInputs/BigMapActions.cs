using UnityEngine;
using UnityEngine.InputSystem;

namespace SharedInputs {
    public class BigMapActions : InputActions {
        [SerializeField] private InputActionReference hideBigMapActionReference;
        
        private void OnEnable() {
            hideBigMapActionReference.action.Enable();
            hideBigMapActionReference.action.performed += HideBigMap;
        }

        private void OnDisable() {
            hideBigMapActionReference.action.Disable();
            hideBigMapActionReference.action.performed -= HideBigMap;
        }
        
        private void HideBigMap(InputAction.CallbackContext context) {
            ObjectsReference.Instance.uIMap.HideBigMap();
            ObjectsReference.Instance.inputManager.SwitchBackToGame();
        }
    }
}