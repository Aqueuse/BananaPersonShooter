using Cameras;
using UnityEngine;
using UnityEngine.InputSystem;

namespace KeyboardInputs {
    public class GestionViewActionsKeyboard : InputActions {
        [SerializeField] private InputActionReference dragMouseRotateInputActionReference;
        [SerializeField] private InputActionReference dragMouseMoveInputActionReference;

        [SerializeField] private InputActionReference deltaMouseActionReference;

        [SerializeField] private InputActionReference moveCameraWithKeyboardActionReference;
        [SerializeField] private InputActionReference boostCameraMoveActionReference;

        [SerializeField] private InputActionReference moveUpDownCameraInputActionReference;

        [SerializeField] private InputActionReference rotateGhostInputActionReference;
        
        [SerializeField] private InputActionReference showDescription;
        
        [SerializeField] private float mouseMoveSensibility = 0.2f;

        [SerializeField] private float scrollSpeed;

        private CameraGestionDragRotate _gestionDragCamera;
        private CameraGestionRelativeMove _gestionRelativeMove;

        private void Start() {
            _gestionDragCamera = ObjectsReference.Instance.gestionDragCamera;
            _gestionRelativeMove = ObjectsReference.Instance.gestionRelativeMoveCamera;
        }   
        
        private void OnEnable() {
            dragMouseMoveInputActionReference.action.Enable();
            dragMouseMoveInputActionReference.action.performed += StartToDragMoveCamera;
            dragMouseMoveInputActionReference.action.canceled += CancelDragMoveCamera;

            dragMouseRotateInputActionReference.action.Enable();
            dragMouseRotateInputActionReference.action.performed += StartToDragRotateCamera;
            dragMouseRotateInputActionReference.action.canceled += CancelDragRotate;
            
            moveCameraWithKeyboardActionReference.action.Enable();
            moveCameraWithKeyboardActionReference.action.performed += MoveCameraWithKeyboard;
            moveCameraWithKeyboardActionReference.action.canceled += CancelMoveCameraWithKeyboard;
            
            boostCameraMoveActionReference.action.Enable();
            boostCameraMoveActionReference.action.performed += BoostCameraSpeed;
            boostCameraMoveActionReference.action.canceled += CancelBoostCameraSpeed;
            
            moveUpDownCameraInputActionReference.action.Enable();
            moveUpDownCameraInputActionReference.action.performed += MoveUpDownCamera;
            moveUpDownCameraInputActionReference.action.canceled += CancelMoveUpDownCamera;
            
            rotateGhostInputActionReference.action.Enable();
            rotateGhostInputActionReference.action.performed += RotateGhost;
            
            showDescription.action.Enable();
            showDescription.action.performed += ShowDescription;
            showDescription.action.canceled += StopShowDescription;
        }
        
        private void OnDisable() {
            dragMouseMoveInputActionReference.action.Disable();
            dragMouseMoveInputActionReference.action.performed -= StartToDragMoveCamera;
            dragMouseMoveInputActionReference.action.canceled -= CancelDragMoveCamera;

            dragMouseRotateInputActionReference.action.Disable();
            dragMouseRotateInputActionReference.action.performed -= StartToDragRotateCamera;
            dragMouseRotateInputActionReference.action.canceled -= CancelDragRotate;
            
            moveCameraWithKeyboardActionReference.action.Disable();
            moveCameraWithKeyboardActionReference.action.performed -= MoveCameraWithKeyboard;
            moveCameraWithKeyboardActionReference.action.canceled -= CancelMoveCameraWithKeyboard;
            
            boostCameraMoveActionReference.action.Disable();
            boostCameraMoveActionReference.action.performed -= BoostCameraSpeed;
            boostCameraMoveActionReference.action.canceled -= CancelBoostCameraSpeed;
            
            moveUpDownCameraInputActionReference.action.Disable();
            moveUpDownCameraInputActionReference.action.performed -= MoveUpDownCamera;
            moveUpDownCameraInputActionReference.action.canceled -= CancelMoveUpDownCamera;
            
            showDescription.action.Disable();
            showDescription.action.performed -= ShowDescription;
            showDescription.action.canceled -= StopShowDescription;
        }

        private void StartToDragMoveCamera(InputAction.CallbackContext callbackContext) {
            deltaMouseActionReference.action.Enable();
            deltaMouseActionReference.action.performed += MoveCamera;
        }
    
        private void MoveCamera(InputAction.CallbackContext callbackContext) {
            var movement2D = callbackContext.ReadValue<Vector2>();
            
            var movement = new Vector3(movement2D.x, -movement2D.y, 0);
        
            _gestionDragCamera.Move(movement.y * mouseMoveSensibility, movement.x * mouseMoveSensibility);
        }

        private void StartToDragRotateCamera(InputAction.CallbackContext callbackContext) {
            deltaMouseActionReference.action.Enable();
            deltaMouseActionReference.action.performed += RotateCamera;
        }

        private void RotateCamera(InputAction.CallbackContext callbackContext) {
            _gestionDragCamera.Rotate(callbackContext.ReadValue<Vector2>());
        }

        private void CancelDragRotate(InputAction.CallbackContext callbackContext) {
            deltaMouseActionReference.action.Disable();
            deltaMouseActionReference.action.performed -= RotateCamera;
        }
        
        private void CancelDragMoveCamera(InputAction.CallbackContext callbackContext) {
            deltaMouseActionReference.action.Disable();
            deltaMouseActionReference.action.performed -= MoveCamera;
            _gestionDragCamera.CancelMove();
        }
        
        private void MoveCameraWithKeyboard(InputAction.CallbackContext context) {
            var movementInput = context.action.ReadValue<Vector2>(); // Read the input value
            
            _gestionRelativeMove.MoveWithKeyboard(movementInput);
        }
        
        private void CancelMoveCameraWithKeyboard(InputAction.CallbackContext context) {
            _gestionRelativeMove.CancelMove();
        }

        private void BoostCameraSpeed(InputAction.CallbackContext context) {
            _gestionRelativeMove.moveSpeed = 60;
        }

        private void CancelBoostCameraSpeed(InputAction.CallbackContext context) {
            _gestionRelativeMove.moveSpeed = 30;
        }
        
        private void MoveUpDownCamera(InputAction.CallbackContext context) {
            var movement = new Vector3(0, context.ReadValue<Vector2>().y * scrollSpeed, 0);
        
            _gestionRelativeMove.MoveWithKeyboard(movement);
        }
        
        private void CancelMoveUpDownCamera(InputAction.CallbackContext context) {
            _gestionRelativeMove.CancelMove();
        }
        
        private void RotateGhost(InputAction.CallbackContext context) {
            var contextValue = context.ReadValue<float>(); 
            
            if (contextValue < 0) {
                ObjectsReference.Instance.gestionViewMode.RotateGhost(Vector3.up); 
            }

            if (contextValue > 0) {
                ObjectsReference.Instance.gestionViewMode.RotateGhost(Vector3.down);
            }
        }
        
        private void ShowDescription(InputAction.CallbackContext callbackContext) {
            ObjectsReference.Instance.scanWithMouseForDescription.enabled = true;
        }

        private void StopShowDescription(InputAction.CallbackContext context) {
            ObjectsReference.Instance.scanWithMouseForDescription.enabled = false;
        }
    }
}