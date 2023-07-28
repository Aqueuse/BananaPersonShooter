using Enums;
using UI.Bananapedia;
using UI.InGame;
using UI.Save;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI {
    public class UIManager : MonoBehaviour {
        [SerializeField] private Animator interfaceAnimator;
        [SerializeField] private UIBananapedia uiBananapedia;

        public GenericDictionary<UICanvasGroupType, CanvasGroup> canvasGroupsByUICanvasType;
        public bool isOnMenu;
        
        private static readonly int ShowInventoryID = Animator.StringToHash("SHOW INVENTORY");
        
        private void Start() {
            ObjectsReference.Instance.inputManager.uiSchemaContext = UISchemaSwitchType.HOME_MENU;
            isOnMenu = false;
        }

        public void Hide_menus() {
            if (ObjectsReference.Instance.gameSettings.isKeyRebinding) return;
            ObjectsReference.Instance.uiSave.UnselectAll();

            isOnMenu = false;

            switch (ObjectsReference.Instance.gameManager.gameContext) {
                case GameContext.IN_GAME:
                    Set_active(UICanvasGroupType.LOAD, false);
                    Set_active(UICanvasGroupType.OPTIONS, false);
                    Set_active(UICanvasGroupType.BANANAPEDIA, false);

                    Set_active(UICanvasGroupType.GAME_MENU, false);

                    if (interfaceAnimator.GetBool(ShowInventoryID)) {
                        interfaceAnimator.SetBool(ShowInventoryID, false);
                    }

                    ObjectsReference.Instance.gameManager.PauseGame(false);
                    // to save performance
                    // we effectively change the save delay when the player come back to the game
                    ObjectsReference.Instance.gameSave.ResetAutoSave();
                    break;
                
                case GameContext.IN_CINEMATIQUE:
                    ObjectsReference.Instance.cinematiques.Unpause();
                    Set_active(UICanvasGroupType.HOME_MENU, false);
                    break;
                
                case GameContext.IN_HOME:
                    Set_active(UICanvasGroupType.LOAD, false);
                    Set_active(UICanvasGroupType.OPTIONS, false);
                    Set_active(UICanvasGroupType.BANANAPEDIA, false);
                    Set_active(UICanvasGroupType.CREDITS, false);
                    Set_active(UICanvasGroupType.HOME_MENU, true);

                    ObjectsReference.Instance.inputManager.uiSchemaSwitcher.SwitchUISchema(UISchemaSwitchType.HOME_MENU);
                    break;
            }
        }

        public void Show_home_menu() {
            Set_active(UICanvasGroupType.OPTIONS, false);
            Set_active(UICanvasGroupType.BANANAPEDIA, false);
            Set_active(UICanvasGroupType.CREDITS, false);

            Set_active(UICanvasGroupType.HOME_MENU, true);
            Set_active(UICanvasGroupType.GAME_MENU, false);
            
            isOnMenu = false;
            ObjectsReference.Instance.inputManager.uiSchemaSwitcher.SwitchUISchema(UISchemaSwitchType.HOME_MENU);
        }

        public void Hide_home_menu() {
            Set_active(UICanvasGroupType.LOAD, false);
            Set_active(UICanvasGroupType.OPTIONS, false);
            Set_active(UICanvasGroupType.BANANAPEDIA, false);
            Set_active(UICanvasGroupType.CREDITS, false);

            Set_active(UICanvasGroupType.HOME_MENU, false);
            isOnMenu = false;
        }

        public void Show_Load_Menu() {
            Set_active(UICanvasGroupType.LOAD, true);
            Set_active(UICanvasGroupType.OPTIONS, false);
            Set_active(UICanvasGroupType.BANANAPEDIA, false);
            Set_active(UICanvasGroupType.CREDITS, false);
            Set_active(UICanvasGroupType.HOME_MENU, false);

            isOnMenu = true;
            Hide_Game_Menu();

            if (ObjectsReference.Instance.gameManager.gameContext == GameContext.IN_GAME) {
                ObjectsReference.Instance.uiSave.newSaveButton.SetActive(true);
                EventSystem.current.SetSelectedGameObject(ObjectsReference.Instance.uiSave.newSaveButton);
            }

            else {
                ObjectsReference.Instance.uiSave.newSaveButton.SetActive(false);
                if (ObjectsReference.Instance.uiSave.GetComponentInChildren<UISaveSlot>() != null)
                    EventSystem.current.SetSelectedGameObject(ObjectsReference.Instance.uiSave.GetComponentsInChildren<UISaveSlot>()[0].gameObject);
            }
            
            ObjectsReference.Instance.inputManager.uiSchemaSwitcher.SwitchUISchema(UISchemaSwitchType.LOAD);
        }

        public void Show_Options() {
            Set_active(UICanvasGroupType.LOAD, false);
            Set_active(UICanvasGroupType.OPTIONS, true);
            Set_active(UICanvasGroupType.BANANAPEDIA, false);
            Set_active(UICanvasGroupType.CREDITS, false);
            Set_active(UICanvasGroupType.HOME_MENU, false);
            
            isOnMenu = true;
            Hide_Game_Menu();
            
            ObjectsReference.Instance.inputManager.uiSchemaSwitcher.SwitchUISchema(UISchemaSwitchType.AUDIOVIDEO_TAB);
            ObjectsReference.Instance.uiOptionsMenu.Switch_To_Audio_Video_Tab();
            
        }

        public void Show_Bananapedia() {
            uiBananapedia.SelectFirstBananapediaEntry();

            Set_active(UICanvasGroupType.LOAD, false);
            Set_active(UICanvasGroupType.OPTIONS, false);
            Set_active(UICanvasGroupType.BANANAPEDIA, true);
            Set_active(UICanvasGroupType.CREDITS, false);
            Set_active(UICanvasGroupType.HOME_MENU, false);

            isOnMenu = true;
            Hide_Game_Menu();
            
            ObjectsReference.Instance.inputManager.uiSchemaSwitcher.SwitchUISchema(UISchemaSwitchType.BANANAPEDIA);
        }
        
        public void Show_Credits() {
            Set_active(UICanvasGroupType.LOAD, false);
            Set_active(UICanvasGroupType.OPTIONS, false);
            Set_active(UICanvasGroupType.BANANAPEDIA, false);
            Set_active(UICanvasGroupType.CREDITS, true);
            Set_active(UICanvasGroupType.HOME_MENU, false);
            
            EventSystem.current.SetSelectedGameObject(ObjectsReference.Instance.uiCredits.scrollbar.gameObject);
            
            isOnMenu = true;
            Hide_Game_Menu();
            
            ObjectsReference.Instance.inputManager.uiSchemaSwitcher.SwitchUISchema(UISchemaSwitchType.CREDITS);
        }
        
        public void Show_game_menu() {
            switch (ObjectsReference.Instance.gameManager.gameContext) {
                case GameContext.IN_DIALOGUE:
                    Set_active(UICanvasGroupType.GAME_MENU, true);
                    break;
                case GameContext.IN_GAME:
                    if (!interfaceAnimator.GetBool(ShowInventoryID)) {
                        Set_active(UICanvasGroupType.GAME_MENU, true);
                    }
                    break;
            }
        }

        public void Hide_Game_Menu() {
            Set_active(UICanvasGroupType.GAME_MENU, false);
        }

        /// IN GAME ///
        
        public void Show_Hide_interface() {
            if (ObjectsReference.Instance.gameManager.gameContext == GameContext.IN_GAME && canvasGroupsByUICanvasType[UICanvasGroupType.GAME_MENU].alpha == 0f) {
                if (!Is_Interface_Visible()) {
                    interfaceAnimator.SetBool(ShowInventoryID, true);
                    
                    ObjectsReference.Instance.inputManager.uiSchemaContext = UISchemaSwitchType.INVENTAIRE;
                    ObjectsReference.Instance.inputManager.SwitchContext(InputContext.UI);
                        
                    ObjectsReference.Instance.uInventory.RefreshUInventory();
                    ObjectsReference.Instance.gameManager.PauseGame(true);
                    Focus_interface();
                    
                    switch (ObjectsReference.Instance.uihud.interfaceContext) {
                        case InterfaceContext.INVENTORY:
                            ObjectsReference.Instance.uihud.Switch_To_Inventory();
                            break;
                        case InterfaceContext.BLUEPRINTS:
                            ObjectsReference.Instance.uihud.Switch_To_Blueprints();
                            break;
                    }
                }
                else {
                    ObjectsReference.Instance.inputManager.SwitchContext(InputContext.GAME);
                    interfaceAnimator.SetBool(ShowInventoryID, false);
                    ObjectsReference.Instance.gameManager.PauseGame(false);
                }
            }
        }

        public void HideInterface() {
            interfaceAnimator.SetBool(ShowInventoryID, false);
        }

        public bool Is_Interface_Visible() {
            return interfaceAnimator.GetBool(ShowInventoryID);
        }

        private static void Focus_interface() {
            EventSystem.current.SetSelectedGameObject(ObjectsReference.Instance.uInventory.lastselectedInventoryItem);
        }

        public void Set_active(UICanvasGroupType uiCanvasGroupType, bool visible) {
            var canvasGroup = canvasGroupsByUICanvasType[uiCanvasGroupType];
            
            canvasGroup.alpha = visible ? 1 : 0;
            canvasGroup.interactable = visible;
            canvasGroup.blocksRaycasts = visible;
        }
    }
}
