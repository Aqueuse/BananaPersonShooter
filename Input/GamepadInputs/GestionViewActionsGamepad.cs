using Cameras;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GamepadInputs {
    public class GestionViewActionsGamepad : InputActions {
        [SerializeField] private InputActionReference moveCameraInputActionReference;
        [SerializeField] private InputActionReference rotateCameraInputActionReference;
        
        [SerializeField] private float mouseMoveSensibility = 0.5f; 
        
        private CameraGestionDragRotate _gestionDragCamera;
        
        private void Start() {
            _gestionDragCamera = ObjectsReference.Instance.gestionDragCamera;
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
            _gestionDragCamera.Move(context.ReadValue<Vector2>().y * mouseMoveSensibility, -context.ReadValue<Vector2>().x * mouseMoveSensibility);
        }
        
        private void CancelMoveCamera(InputAction.CallbackContext context) {
            _gestionDragCamera.CancelMove();
        }

        private void RotateCamera(InputAction.CallbackContext callbackContext) {
            _gestionDragCamera.Rotate(callbackContext.ReadValue<Vector2>());
        }
    }
}