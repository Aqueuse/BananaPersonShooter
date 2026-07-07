using InGame;
using UnityEngine;
using UnityEngine.InputSystem;

namespace KeyboardInputs {
    public class MapActionsKeyboard : InputActions {
        [SerializeField] private InputActionReference dragMoveActionReference;
        [SerializeField] private InputActionReference deltaMouseActionReference;

        [SerializeField] private InputActionReference zoomDezoomActionReference;

        private Map _map;

        private bool isDragging;

        private void Start() {
            _map = ObjectsReference.Instance.map;
        }

        private void OnEnable() {
            dragMoveActionReference.action.Enable();
            dragMoveActionReference.action.performed += StartToDrag;
            
            zoomDezoomActionReference.action.Enable();
            zoomDezoomActionReference.action.performed += ZoomDezoom;
        }

        private void OnDisable() {
            dragMoveActionReference.action.Disable();
            dragMoveActionReference.action.performed -= StartToDrag;
            
            zoomDezoomActionReference.action.Disable();
            zoomDezoomActionReference.action.performed -= ZoomDezoom;
        }

        private void StartToDrag(InputAction.CallbackContext callbackContext) {
            if (callbackContext.performed & !isDragging) {
                deltaMouseActionReference.action.Enable();
                deltaMouseActionReference.action.performed += Move;

                isDragging = true;
            
                return;
            }

            if (callbackContext.performed & isDragging) {
                deltaMouseActionReference.action.Disable();
                deltaMouseActionReference.action.performed -= Move;

                isDragging = false;
            }
        }
    
        private void Move(InputAction.CallbackContext callbackContext) {
            var movement2D = callbackContext.ReadValue<Vector2>();
            var movement = new Vector3(-movement2D.x, 0, -movement2D.y);
            _map.Drag(movement);
        }

        private void ZoomDezoom(InputAction.CallbackContext callbackContext) {
            if (callbackContext.ReadValue<float>() > 0) {
                _map.Zoom();
            }
            else {
                _map.Dezoom();   
            }
        }
    }
}
