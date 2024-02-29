using UnityEngine;
using UnityEngine.InputSystem;

namespace Input {
    public class MerchantActions : InputActions {
        [SerializeField] private InputActionReference quitInputActionReference;
    
        private void OnEnable() {
            ObjectsReference.Instance.uiActions.enabled = true;
            
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            ObjectsReference.Instance.cameraPlayer.Set0Sensibility();

            ObjectsReference.Instance.playerController.canMove = false;
            ObjectsReference.Instance.bananaMan.GetComponent<Rigidbody>().isKinematic = true;

            quitInputActionReference.action.Enable();
            quitInputActionReference.action.performed += Quit;
        }

        private void OnDisable() {
            quitInputActionReference.action.Disable();
            quitInputActionReference.action.performed -= Quit;
        }

        private void Quit(InputAction.CallbackContext context) {
            if (ObjectsReference.Instance.uiMerchant.IsInOptionsMenu()) ObjectsReference.Instance.uiMerchant.HideOptions();

            else {
                ObjectsReference.Instance.uiManager.SetActive(UICanvasGroupType.MERCHANT_INTERFACE, false);
                ObjectsReference.Instance.descriptionsManager.HideAllPanels();
            
                ObjectsReference.Instance.inputManager.SwitchContext(InputContext.GAME);
                ObjectsReference.Instance.gameManager.gameContext = GameContext.IN_GAME;
            }
        }
    }
}
