using UnityEngine;
using UnityEngine.InputSystem;

public class DebugActions : InputActions {
    [SerializeField] private InputActionReference F1DebugActionReference;

    private void OnEnable() {
        F1DebugActionReference.action.Enable();
        F1DebugActionReference.action.performed += F1Debug;
    }

    private void OnDisable() {
        F1DebugActionReference.action.Disable();
        F1DebugActionReference.action.performed -= F1Debug;
    }

    private void F1Debug(InputAction.CallbackContext callbackContext) {
    }
}