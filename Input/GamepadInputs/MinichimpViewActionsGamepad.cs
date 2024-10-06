using Cameras;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GamepadInputs {
    public class MinichimpViewActionsGamepad : InputActions {
        [SerializeField] private InputActionReference moveCameraInputActionReference;
        [SerializeField] private InputActionReference rotateCameraInputActionReference;
        
        [SerializeField] private InputActionReference switchToTabLeftInputActionReference;
        [SerializeField] private InputActionReference switchToTabRightInputActionReference;

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
            
            switchToTabLeftInputActionReference.action.performed += SwitchToTabLeft;
            switchToTabLeftInputActionReference.action.Enable();

            switchToTabRightInputActionReference.action.performed += SwitchToTabRight;
            switchToTabRightInputActionReference.action.Enable();
        }

        private void OnDisable() {
            moveCameraInputActionReference.action.Disable();
            moveCameraInputActionReference.action.performed -= MoveCamera;
            moveCameraInputActionReference.action.canceled -= CancelMoveCamera;

            rotateCameraInputActionReference.action.Disable();
            rotateCameraInputActionReference.action.performed -= RotateCamera;

            switchToTabLeftInputActionReference.action.performed -= SwitchToTabLeft;
            switchToTabLeftInputActionReference.action.Disable();

            switchToTabRightInputActionReference.action.performed -= SwitchToTabRight;
            switchToTabRightInputActionReference.action.Disable();
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
        
        private void SwitchToTabLeft(InputAction.CallbackContext callbackContext) {
            ObjectsReference.Instance.uiBananaGun.SwitchToLeftTab();
        }
        
        private void SwitchToTabRight(InputAction.CallbackContext callbackContext) {
            ObjectsReference.Instance.uiBananaGun.SwitchToRightTab();
        }
    }
}