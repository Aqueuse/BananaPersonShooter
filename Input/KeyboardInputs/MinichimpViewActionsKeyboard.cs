using Cameras;
using UnityEngine;
using UnityEngine.InputSystem;

namespace KeyboardInputs {
    public class MinichimpViewActionsKeyboard : InputActions {
        [SerializeField] private InputActionReference dragMouseRotateInputActionReference;
        [SerializeField] private InputActionReference dragMouseMoveInputActionReference;
        
        [SerializeField] private InputActionReference deltaMouseActionReference;

        [SerializeField] private float mouseMoveSensibility = 0.5f; 

        private CameraGestion _gestionCamera;
        
        private bool isDragging;

        private void Start() {
            _gestionCamera = ObjectsReference.Instance.gestionCamera;
        }
        
        private void OnEnable() {
            dragMouseMoveInputActionReference.action.Enable();
            dragMouseMoveInputActionReference.action.performed += StartToDragMove;

            dragMouseRotateInputActionReference.action.Enable();
            dragMouseRotateInputActionReference.action.performed += StartToDragRotate;
        }

        private void OnDisable() {
            dragMouseMoveInputActionReference.action.Disable();
            dragMouseMoveInputActionReference.action.performed -= StartToDragMove;

            dragMouseRotateInputActionReference.action.Disable();
            dragMouseRotateInputActionReference.action.performed -= StartToDragRotate;
        }

        private void StartToDragMove(InputAction.CallbackContext callbackContext) {
            if (callbackContext.performed && !isDragging) {
                deltaMouseActionReference.action.Enable();
                deltaMouseActionReference.action.performed += Move;

                isDragging = true;
            
                return;
            }

            if (callbackContext.performed && isDragging) {
                deltaMouseActionReference.action.Disable();
                deltaMouseActionReference.action.performed -= Move;
                _gestionCamera.CancelMove();

                isDragging = false;
            }
        }
    
        private void Move(InputAction.CallbackContext callbackContext) {
            var movement2D = callbackContext.ReadValue<Vector2>();
            var movement = new Vector3(-movement2D.x, 0, -movement2D.y); 
        
            _gestionCamera.Move(movement.y * mouseMoveSensibility, movement.x * mouseMoveSensibility);
        }

        private void StartToDragRotate(InputAction.CallbackContext callbackContext) {
            if (callbackContext.performed && !isDragging) {
                deltaMouseActionReference.action.Enable();
                deltaMouseActionReference.action.performed += Rotate;

                isDragging = true;
            
                return;
            }

            if (callbackContext.performed && isDragging) {
                deltaMouseActionReference.action.Disable();
                deltaMouseActionReference.action.performed -= Rotate;

                isDragging = false;
            }
        }

        private void Rotate(InputAction.CallbackContext callbackContext) {
            _gestionCamera.Rotate(callbackContext.ReadValue<Vector2>());
        }
    }
}