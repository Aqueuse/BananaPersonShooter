using UnityEngine;
using UnityEngine.InputSystem;

namespace SharedInputs {
    public class BananaGunUIActions : InputActions {
        [SerializeField] private InputActionReference switchToGameReference;
        
        private void OnEnable() {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            ObjectsReference.Instance.cameraPlayer.Set0Sensibility();
            
            ObjectsReference.Instance.playerController.canMove = false;
            ObjectsReference.Instance.bananaMan.GetComponent<Rigidbody>().isKinematic = true;

            switchToGameReference.action.performed += SwitchBackToGame;
            switchToGameReference.action.Enable();
        }

        private void OnDisable() {
            switchToGameReference.action.performed -= SwitchBackToGame;
            switchToGameReference.action.Disable();
        }
        
        private void SwitchBackToGame(InputAction.CallbackContext callbackContext) {
            ObjectsReference.Instance.inputManager.SwitchContext(InputContext.GAME);
            ObjectsReference.Instance.uiManager.HideBananaGunUI();
        }
    }
}