using UnityEngine;
using UnityEngine.InputSystem;

namespace SharedInputs {
    public class UiActions : InputActions {
        [SerializeField] private InputActionReference cancelActionReference;
        [SerializeField] private InputActionReference pointerActionReference;
        [SerializeField] private InputActionReference hideMainPanelActionReference;
        [SerializeField] private InputActionReference hideInventoriesActionReference;
        
        private void OnEnable() {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            cancelActionReference.action.Enable();
            cancelActionReference.action.performed += Escape;
            
            pointerActionReference.action.Enable();
            
            hideMainPanelActionReference.action.Enable();
            hideMainPanelActionReference.action.performed += HideMainPanel;
            
            hideInventoriesActionReference.action.Enable();
            hideInventoriesActionReference.action.performed += HideInventories;
        }

        private void OnDisable() {
            cancelActionReference.action.Disable();
            cancelActionReference.action.performed -= Escape;

            pointerActionReference.action.Disable();
            
            hideMainPanelActionReference.action.Disable();
            hideMainPanelActionReference.action.performed -= HideMainPanel;
            
            hideInventoriesActionReference.action.Disable();
            hideInventoriesActionReference.action.performed -= HideInventories;
        }
        
        private void Escape(InputAction.CallbackContext context) {
            switch (ObjectsReference.Instance.gameManager.gameContext) {
                case GameContext.IN_HOME:
                    ObjectsReference.Instance.uiManager.ShowHomeMenu();
                    break;
                
                case GameContext.IN_GAME_MENU:
                    if (ObjectsReference.Instance.uiManager.isOnSubMenus) {
                        ObjectsReference.Instance.uiManager.ShowGameMenu();
                    }
                    else {
                        ObjectsReference.Instance.uiManager.HideGameMenu();
                        ObjectsReference.Instance.inputManager.SwitchBackToGame();
                    }
                    
                    break;
                
                case GameContext.IN_COMMAND_ROOM_PANEL:
                    ObjectsReference.Instance.commandRoomControlPanelsManager.UnfocusPanel(false);
                    break;
            }
        }

        private void HideMainPanel(InputAction.CallbackContext context) {
            if (ObjectsReference.Instance.gameManager.gameContext == GameContext.IN_MAIN_PANEL) {
                ObjectsReference.Instance.uiManager.HideMainPanel();
            }
        }
        
        private void HideInventories(InputAction.CallbackContext context) {
            if (ObjectsReference.Instance.gameManager.gameContext == GameContext.IN_MAIN_PANEL) {
                ObjectsReference.Instance.uiManager.HideMainPanel();
            }
        }
    }
}