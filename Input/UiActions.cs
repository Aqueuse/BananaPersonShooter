using UnityEngine;
using UnityEngine.InputSystem;

public class UiActions : InputActions {
    [SerializeField] private InputActionReference cancelActionReference;
    [SerializeField] private InputActionReference SwitchToLeftOptionPanel;
    [SerializeField] private InputActionReference SwitchToRightOptionPanel;
    [SerializeField] private InputActionReference pointerActionReference;
        
    private void OnEnable() {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
            
        cancelActionReference.action.Enable();
        cancelActionReference.action.performed += Escape;
            
        SwitchToLeftOptionPanel.action.Enable();
        SwitchToLeftOptionPanel.action.performed += Scroll_Left_Options_Tab;
            
        SwitchToRightOptionPanel.action.Enable();
        SwitchToRightOptionPanel.action.performed += Scroll_Right_Options_Tab;

        pointerActionReference.action.Enable();
    }

    private void OnDisable() {
        cancelActionReference.action.Disable();
        cancelActionReference.action.performed -= Escape;
            
        SwitchToLeftOptionPanel.action.Disable();
        SwitchToLeftOptionPanel.action.performed -= Scroll_Left_Options_Tab;
            
        SwitchToRightOptionPanel.action.Disable();
        SwitchToRightOptionPanel.action.performed -= Scroll_Right_Options_Tab;
            
        pointerActionReference.action.Disable();
    }

    private void Scroll_Left_Options_Tab(InputAction.CallbackContext context) {
        ObjectsReference.Instance.uiOptionsMenu.Switch_to_Left_Tab();
    }
        
    private void Scroll_Right_Options_Tab(InputAction.CallbackContext context) {
        ObjectsReference.Instance.uiOptionsMenu.Switch_to_Right_Tab();
    }
        
    private void Escape(InputAction.CallbackContext context) {
        if (ObjectsReference.Instance.gameManager.gameContext == GameContext.IN_HOME) {
            ObjectsReference.Instance.uiManager.ShowHomeMenu();
        }

        if (ObjectsReference.Instance.gameManager.gameContext == GameContext.IN_GESTION_VIEW) {
            if (ObjectsReference.Instance.playerActionsSwitch.playerActions == PlayerActionsType.BUILD) {
                ObjectsReference.Instance.gestionBuild.CancelBuild();
            }

            else {
                ObjectsReference.Instance.mainCamera.CloseGestionView();
                ObjectsReference.Instance.gameManager.gameContext = GameContext.IN_GAME;
            }
        }

        if (ObjectsReference.Instance.gameManager.gameContext == GameContext.IN_INVENTORY) {
            ObjectsReference.Instance.uiManager.SetActive(UICanvasGroupType.INVENTORIES, false);
            ObjectsReference.Instance.descriptionsManager.HideAllPanels();
                
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