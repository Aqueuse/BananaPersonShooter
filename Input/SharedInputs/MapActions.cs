using InGame;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SharedInputs {
    public class MapActions : InputActions {
        [SerializeField] private InputActionReference zoomDezoomReference;
    
        private Map map;

        private bool isDragging;
    
        private void Start() {
            map = ObjectsReference.Instance.map;
        }

        private void OnEnable() {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            ObjectsReference.Instance.cameraPlayer.Set0Sensibility();
        
            ObjectsReference.Instance.playerController.canMove = false;
            ObjectsReference.Instance.bananaMan.GetComponent<Rigidbody>().isKinematic = true;
            
            zoomDezoomReference.action.Enable();
            zoomDezoomReference.action.performed += ZoomDezoom;
        }

        private void OnDisable() {
            zoomDezoomReference.action.Disable();
            zoomDezoomReference.action.performed -= ZoomDezoom;
        }
        
        private void ZoomDezoom(InputAction.CallbackContext callbackContext) {
            // zoom dezoom the space cam
            // with a max / min value
            if (callbackContext.ReadValue<float>() < 0) {
                map.Zoom();
            }

            else {
                map.Dezoom();
            }
        }
    }
}
