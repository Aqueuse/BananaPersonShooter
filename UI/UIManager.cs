using UI.Bananapedia;
using UI.InGame;
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
        }

        public void Hide_home_menu() {
            Set_active(UICanvasGroupType.LOAD, false);
            Set_active(UICanvasGroupType.OPTIONS, false);
            Set_active(UICanvasGroupType.BANANAPEDIA, false);
            Set_active(UICanvasGroupType.CREDITS, false);

            Set_active(UICanvasGroupType.HOME_MENU, false);
            isOnMenu = false;
        }

        public void Switch_To_Load_menu() {
            Set_active(UICanvasGroupType.LOAD, true);
            Set_active(UICanvasGroupType.OPTIONS, false);
            Set_active(UICanvasGroupType.BANANAPEDIA, false);
            Set_active(UICanvasGroupType.CREDITS, false);
            
            ObjectsReference.Instance.uiSave.newSaveButton.SetActive(ObjectsReference.Instance.gameManager.gameContext == GameContext.IN_GAME);
            isOnMenu = true;
        }

        public void Switch_To_options_menu() {
            Set_active(UICanvasGroupType.LOAD, false);
            Set_active(UICanvasGroupType.OPTIONS, true);
            Set_active(UICanvasGroupType.BANANAPEDIA, false);
            Set_active(UICanvasGroupType.CREDITS, false);
            isOnMenu = true;
        }

        public void Switch_To_Bananapedia() {
            uiBananapedia.SelectFirstBananapediaEntry();

            Set_active(UICanvasGroupType.LOAD, false);
            Set_active(UICanvasGroupType.OPTIONS, false);
            Set_active(UICanvasGroupType.BANANAPEDIA, true);
            Set_active(UICanvasGroupType.CREDITS, false);
            isOnMenu = true;
        }
        
        public void Show_Credits() {
            canvasGroupsByUICanvasType[UICanvasGroupType.CREDITS].GetComponent<InfinityScroll>().enabled = true;
            canvasGroupsByUICanvasType[UICanvasGroupType.CREDITS].GetComponent<InfinityScroll>().value = 0.13f;
            
            Set_active(UICanvasGroupType.LOAD, false);
            Set_active(UICanvasGroupType.OPTIONS, false);
            Set_active(UICanvasGroupType.BANANAPEDIA, false);
            Set_active(UICanvasGroupType.CREDITS, true);
            isOnMenu = true;
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
                        case InterfaceContext.STATISTICS:
                            ObjectsReference.Instance.uihud.Switch_To_Statistics();
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
        
        void Focus_interface() {
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
