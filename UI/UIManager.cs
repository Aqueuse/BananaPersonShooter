using Cameras;
using Enums;
using Game;
using Input;
using Settings;
using UI.InGame;
using UI.InGame.BuildStation;
using UI.InGame.Inventory;
using UI.Save;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI {
    public class UIManager : MonoSingleton<UIManager> {
        [SerializeField] private Animator interfaceAnimator;
        public UIBuildStation uiBuildStation;

        public GenericDictionary<UICanvasGroupType, CanvasGroup> canvasGroupsByUICanvasType;
        public bool isOnMenu;

        private static readonly int ShowInventoryID = Animator.StringToHash("SHOW INVENTORY");
        
        private void Start() {
            InputManager.Instance.uiSchemaContext = UISchemaSwitchType.HOME_MENU;
            isOnMenu = false;
        }

        public void Hide_menus() {
            if (GameSettings.Instance.isKeyRebinding) return;

            isOnMenu = false;

            switch (GameManager.Instance.gameContext) {
                case GameContext.IN_GAME:
                    Set_active(UICanvasGroupType.LOAD, false);
                    Set_active(UICanvasGroupType.OPTIONS, false);
                    Set_active(UICanvasGroupType.BANANAPEDIA, false);

                    Set_active(UICanvasGroupType.GAME_MENU, false);

                    if (interfaceAnimator.GetBool(ShowInventoryID)) {
                        interfaceAnimator.SetBool(ShowInventoryID, false);
                    }

                    GameManager.Instance.PauseGame(false);
                    break;
                
                case GameContext.IN_CINEMATIQUE:
                    Cinematiques.Instance.Unpause();
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
            
            UISave.Instance.newSaveButton.SetActive(GameManager.Instance.gameContext == GameContext.IN_GAME);
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
            UIBananapedia.Instance.SelectFirstBananapediaEntry();

            Set_active(UICanvasGroupType.LOAD, false);
            Set_active(UICanvasGroupType.OPTIONS, false);
            Set_active(UICanvasGroupType.BANANAPEDIA, true);
            Set_active(UICanvasGroupType.CREDITS, false);
            isOnMenu = true;
        }
        
        public void Show_Credits() {
            canvasGroupsByUICanvasType[UICanvasGroupType.CREDITS].GetComponent<InfinityScroll.InfinityScroll>().enabled = true;
            canvasGroupsByUICanvasType[UICanvasGroupType.CREDITS].GetComponent<InfinityScroll.InfinityScroll>().value = 0.13f;
            
            Set_active(UICanvasGroupType.LOAD, false);
            Set_active(UICanvasGroupType.OPTIONS, false);
            Set_active(UICanvasGroupType.BANANAPEDIA, false);
            Set_active(UICanvasGroupType.CREDITS, true);
            isOnMenu = true;
        }
        
        public void Show_game_menu() {
            switch (GameManager.Instance.gameContext) {
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
            if (GameManager.Instance.gameContext == GameContext.IN_GAME && canvasGroupsByUICanvasType[UICanvasGroupType.GAME_MENU].alpha == 0f) {
                if (!Is_Interface_Visible()) {
                    interfaceAnimator.SetBool(ShowInventoryID, true);
                    
                    InputManager.Instance.uiSchemaContext = UISchemaSwitchType.INVENTAIRE;
                    InputManager.Instance.SwitchContext(InputContext.UI);
                    
                    UInventory.Instance.RefreshUInventory();
                    GameManager.Instance.PauseGame(true);
                    Focus_interface();
                    
                    MainCamera.Instance.Switch_To_Shoot_Target();
                    
                    if (Uihud.Instance.interfaceContext == InterfaceContext.INVENTORY) Uihud.Instance.Switch_To_Inventory();
                    else {
                        Uihud.Instance.Switch_To_Statistics();
                    }
                }
                else {
                    InputManager.Instance.SwitchContext(InputContext.GAME);
                    interfaceAnimator.SetBool(ShowInventoryID, false);
                    GameManager.Instance.PauseGame(false);
                    
                    MainCamera.Instance.Switch_To_TPS_Target();
                }
            }
        }

        public void HideInterface() {
            interfaceAnimator.SetBool(ShowInventoryID, false);
        }

        private bool Is_Interface_Visible() {
            return interfaceAnimator.GetBool(ShowInventoryID);
        }
        
        void Focus_interface() {
            EventSystem.current.SetSelectedGameObject(UInventory.Instance.lastselectedInventoryItem);
        }
        
        public void Set_active(UICanvasGroupType uiCanvasGroupType, bool visible) {
            var canvasGroup = canvasGroupsByUICanvasType[uiCanvasGroupType];
            
            canvasGroup.alpha = visible ? 1 : 0;
            canvasGroup.interactable = visible;
            canvasGroup.blocksRaycasts = visible;
        }
    }
}
