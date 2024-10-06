using InGame.Gestion;
using InGame.Items.ItemsProperties;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SharedInputs {
    public class MinichimpViewActions : InputActions {
        [SerializeField] private InputActionReference leftClickInputActionReference;
        [SerializeField] private InputActionReference rotateGhostInputActionReference;
        
        [SerializeField] private InputActionReference switchToGameReference;

        private float _counter;
        
        private ItemScriptableObject selectedBuildableScriptableObject;

        private MiniChimpViewMode miniChimpViewMode;
        
        private void Start() {
            miniChimpViewMode = ObjectsReference.Instance.miniChimpViewMode;
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
            
            switchToGameReference.action.performed += SwitchBackToGame;
            switchToGameReference.action.Enable();
        }

        private void OnDisable() {
            leftClickInputActionReference.action.Disable();
            leftClickInputActionReference.action.performed -= ContextualLeftClick;

            rotateGhostInputActionReference.action.Disable();
            rotateGhostInputActionReference.action.performed -= RotateGhost;
            
            switchToGameReference.action.performed -= SwitchBackToGame;
            switchToGameReference.action.Disable();
        }
        
        private void RotateGhost(InputAction.CallbackContext context) {
            var contextValue = context.ReadValue<float>(); 
            
            if (contextValue < 0) {
                miniChimpViewMode.RotateGhost(Vector3.up);
            }

            if (contextValue > 0) {
                miniChimpViewMode.RotateGhost(Vector3.down);
            }
        }
        
        private void ContextualLeftClick(InputAction.CallbackContext context) {
            var viewContextType = miniChimpViewMode.viewModeContextType;

            if (viewContextType == ViewModeContextType.SCAN) {
                ObjectsReference.Instance.scanWithMouseForDescription.enabled = true;
            }

            if (viewContextType == ViewModeContextType.BUILD) {
                miniChimpViewMode.ValidateBuildable();
            }

            if (viewContextType == ViewModeContextType.HARVEST) {
                miniChimpViewMode.harvest();
            }
            
            if (viewContextType == ViewModeContextType.REPAIR) {
                miniChimpViewMode.RepairBuildable();
            }
        }
        
        private void SwitchBackToGame(InputAction.CallbackContext callbackContext) {
            ObjectsReference.Instance.uiManager.HideBananaGunUI();
            ObjectsReference.Instance.uiManager.SwitchToBananaManPerspective();
            
            ObjectsReference.Instance.inputManager.SwitchBackToGame();
            
            ObjectsReference.Instance.gameManager.gameContext = GameContext.IN_GAME;
            ObjectsReference.Instance.inputManager.SwitchContext(InputContext.GAME);
                
            ObjectsReference.Instance.bananaGunActionsSwitch.gameObject.SetActive(true);
        }
    }
}