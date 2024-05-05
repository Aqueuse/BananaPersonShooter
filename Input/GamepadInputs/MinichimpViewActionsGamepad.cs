using Cameras;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GamepadInputs {
    public class MinichimpViewActionsGamepad : InputActions {
        [SerializeField] private InputActionReference moveCameraInputActionReference;
        [SerializeField] private InputActionReference rotateCameraInputActionReference;
        
        [SerializeField] private float mouseMoveSensibility = 0.5f; 
        
        private CameraGestion _gestionCamera;
        
        private void Start() {
            _gestionCamera = ObjectsReference.Instance.gestionCamera;
        }
    
        private void OnEnable() {
            moveCameraInputActionReference.action.Enable();
            moveCameraInputActionReference.action.performed += MoveCamera;
            moveCameraInputActionReference.action.canceled += CancelMoveCamera;

            rotateCameraInputActionReference.action.Enable();
            rotateCameraInputActionReference.action.performed += RotateCamera;
        }

        private void OnDisable() {
            moveCameraInputActionReference.action.Disable();
            moveCameraInputActionReference.action.performed -= MoveCamera;
            moveCameraInputActionReference.action.canceled -= CancelMoveCamera;

            rotateCameraInputActionReference.action.Disable();
            rotateCameraInputActionReference.action.performed -= RotateCamera;
            
        }

        private void MoveCamera(InputAction.CallbackContext context) {
            _gestionCamera.Move(context.ReadValue<Vector2>().y * mouseMoveSensibility, -context.ReadValue<Vector2>().x * mouseMoveSensibility);
        }
        
        private void CancelMoveCamera(InputAction.CallbackContext context) {
            _gestionCamera.CancelMove();
        }

        private void RotateCamera(InputAction.CallbackContext callbackContext) {
            _gestionCamera.Rotate(callbackContext.ReadValue<Vector2>());
        }

    }
}