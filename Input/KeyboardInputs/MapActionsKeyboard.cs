using InGame;
using UnityEngine;
using UnityEngine.InputSystem;

namespace KeyboardInputs {
    public class MapActionsKeyboard : InputActions {
        [SerializeField] private InputActionReference dragMoveActionReference;
        [SerializeField] private InputActionReference deltaMouseActionReference;

        private MiniMap _miniMap;

        private bool isDragging;

        private void Start() {
            _miniMap = ObjectsReference.Instance.miniMap;
        }

        private void OnEnable() {
            dragMoveActionReference.action.Enable();
            dragMoveActionReference.action.performed += StartToDrag;
        }

        private void OnDisable() {
            dragMoveActionReference.action.Disable();
            dragMoveActionReference.action.performed -= StartToDrag;
        }

        private void StartToDrag(InputAction.CallbackContext callbackContext) {
            if (callbackContext.performed && !isDragging) {
                deltaMouseActionReference.action.Enable();
                deltaMouseActionReference.action.performed += Move;

                isDragging = true;
            
                return;
            }

            if (callbackContext.performed && isDragging) {
                deltaMouseActionReference.action.Disable();
                deltaMouseActionReference.action.performed -= Move;

                isDragging = false;
            }
        }
    
        private void Move(InputAction.CallbackContext callbackContext) {
            var movement2D = callbackContext.ReadValue<Vector2>();
            var movement = new Vector3(-movement2D.x, 0, -movement2D.y); 
        
            _miniMap.Move(movement);
        }
    }
}
