using UnityEngine;
using UnityEngine.InputSystem;

namespace SharedInputs {
    public class UiActions : InputActions {
        [SerializeField] private InputActionReference cancelActionReference;
        [SerializeField] private InputActionReference pointerActionReference;

        private void OnEnable() {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            cancelActionReference.action.Enable();
            cancelActionReference.action.performed += Escape;
            
            pointerActionReference.action.Enable();
        }

        private void OnDisable() {
            cancelActionReference.action.Disable();
            cancelActionReference.action.performed -= Escape;
            
            pointerActionReference.action.Disable();
        }
        
        private void Escape(InputAction.CallbackContext context) {
            if (ObjectsReference.Instance.gameManager.gameContext == GameContext.IN_HOME) {
                ObjectsReference.Instance.uiManager.ShowHomeMenu();
            }

            if (ObjectsReference.Instance.gameManager.gameContext == GameContext.IN_MINICHIMP_VIEW) {
                if (ObjectsReference.Instance.inputManager.bananaGunActions == BananaGunMode.BUILD) {
                    ObjectsReference.Instance.gestionMode.CancelBuild();
                }

                else {
                    ObjectsReference.Instance.mainCamera.CloseGestionView();
                    ObjectsReference.Instance.gameManager.gameContext = GameContext.IN_GAME;
                }
            }

            if (ObjectsReference.Instance.gameManager.gameContext == GameContext.IN_BANANAGUN_UI) {
                ObjectsReference.Instance.uiManager.HideBananaGunUI();

                ObjectsReference.Instance.inputManager.SwitchContext(InputContext.GAME);
                ObjectsReference.Instance.gameManager.gameContext = GameContext.IN_GAME;
            }

            if (ObjectsReference.Instance.gameManager.gameContext == GameContext.IN_GAME_MENU) {
                if (ObjectsReference.Instance.uiManager.isOnSubMenus) {
                    ObjectsReference.Instance.uiManager.ShowGameMenu();
                }
                else {
                    ObjectsReference.Instance.uiManager.HideGameMenu();
                    ObjectsReference.Instance.gameManager.gameContext = GameContext.IN_GAME;
                    
                    ObjectsReference.Instance.inputManager.SwitchContext(InputContext.GAME);
                }
            }

            if (ObjectsReference.Instance.gameManager.gameContext == GameContext.IN_COMMAND_ROOM_PANEL) {
                ObjectsReference.Instance.commandRoomControlPanelsManager.UnfocusPanel();
            }
        }
    }
}