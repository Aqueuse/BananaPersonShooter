using InGame.Gestion;
using InGame.Items.ItemsProperties;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SharedInputs {
    public class GestionViewActions : InputActions {
        [SerializeField] private InputActionReference leftClickInputActionReference;
        [SerializeField] private InputActionReference rotateGhostInputActionReference;
        
        private float _counter;
        
        private ItemScriptableObject selectedBuildableScriptableObject;

        private GestionViewMode _gestionViewMode;
        
        private void Start() {
            _gestionViewMode = ObjectsReference.Instance.gestionViewMode;
        }

        private void OnEnable() {
            ObjectsReference.Instance.cameraPlayer.Set0Sensibility();

            ObjectsReference.Instance.playerController.canMove = false;
            ObjectsReference.Instance.bananaMan.GetComponent<Rigidbody>().isKinematic = true;

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            ObjectsReference.Instance.playerController.canMove = false;
            ObjectsReference.Instance.bananaMan.GetComponent<Rigidbody>().isKinematic = true;
            
            leftClickInputActionReference.action.Enable();
            leftClickInputActionReference.action.performed += ContextualLeftClick;

            rotateGhostInputActionReference.action.Enable();
            rotateGhostInputActionReference.action.performed += RotateGhost;
        }

        private void OnDisable() {
            leftClickInputActionReference.action.Disable();
            leftClickInputActionReference.action.performed -= ContextualLeftClick;

            rotateGhostInputActionReference.action.Disable();
            rotateGhostInputActionReference.action.performed -= RotateGhost;
        }
        
        private void RotateGhost(InputAction.CallbackContext context) {
            var contextValue = context.ReadValue<float>(); 
            
            if (contextValue < 0) {
                _gestionViewMode.RotateGhost(Vector3.up);
            }

            if (contextValue > 0) {
                _gestionViewMode.RotateGhost(Vector3.down);
            }
        }
        
        private void ContextualLeftClick(InputAction.CallbackContext context) {
            var viewContextType = _gestionViewMode.viewModeContextType;

            if (viewContextType == ViewModeContextType.SCAN) {
                ObjectsReference.Instance.scanWithMouseForDescription.enabled = true;
            }

            if (viewContextType == ViewModeContextType.BUILD) {
                _gestionViewMode.ValidateBuildable();
            }

            if (viewContextType == ViewModeContextType.HARVEST) {
                _gestionViewMode.harvest();
            }
        }
    }
}