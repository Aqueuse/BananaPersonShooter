using UnityEngine;
using UnityEngine.InputSystem;

namespace SharedInputs {
    public class UiActions : InputActions {
        [SerializeField] private InputActionReference cancelActionReference;
        [SerializeField] private InputActionReference pointerActionReference;
        [SerializeField] private InputActionReference hideInGameUIPanelActionReference;
        [SerializeField] private InputActionReference hideInventoriesActionReference;
        
        private void OnEnable() {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            cancelActionReference.action.Enable();
            cancelActionReference.action.performed += Escape;
            
            pointerActionReference.action.Enable();
            
            hideInGameUIPanelActionReference.action.Enable();
            hideInGameUIPanelActionReference.action.performed += HideInGameUIPanel;
            
            hideInventoriesActionReference.action.Enable();
            hideInventoriesActionReference.action.performed += HideInventories;
        }

        private void OnDisable() {
            cancelActionReference.action.Disable();
            cancelActionReference.action.performed -= Escape;

            pointerActionReference.action.Disable();
            
            hideInGameUIPanelActionReference.action.Disable();
            hideInGameUIPanelActionReference.action.performed -= HideInGameUIPanel;
            
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
                
                case GameContext.IN_GAME_UI_PANEL:
                    ObjectsReference.Instance.uiGuichet.activatedGuichet.CloseGuichet();
                    break;
            }
        }

        private void HideInGameUIPanel(InputAction.CallbackContext context) {
            if (ObjectsReference.Instance.gameManager.gameContext == GameContext.IN_GAME_UI_PANEL) {
                ObjectsReference.Instance.uiManager.SetActive(UICanvasGroupType.MAIN_PANEL, false);
                ObjectsReference.Instance.uiManager.SetActive(UICanvasGroupType.MERCHANT_INTERFACE, false);
                ObjectsReference.Instance.inputManager.SwitchBackToGame();
            }
        }
        
        private void HideInventories(InputAction.CallbackContext context) {
            if (ObjectsReference.Instance.gameManager.gameContext == GameContext.IN_GAME_UI_PANEL) {
                ObjectsReference.Instance.uiManager.SetActive(UICanvasGroupType.MAIN_PANEL, false);
                ObjectsReference.Instance.inputManager.SwitchBackToGame();
            }
        }
    }
}