using Enums;
using Game;
using Input;
using Settings;
using UI.InGame.Inventory;
using UI.Save;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI {
    public class UIManager : MonoSingleton<UIManager> {
        public CanvasGroup homeMenuCanvasGroup;
        public CanvasGroup gameMenuCanvasGroup;
        public CanvasGroup optionsMenuCanvasGroup;
        [SerializeField] private CanvasGroup loadMenuCanvasGroup;
        [SerializeField] private CanvasGroup bananapediaMenuCanvasGroup;
        [SerializeField] private CanvasGroup creditsMenuCanvasGroup;
        [SerializeField] private CanvasGroup deathMenuCanvasGroup;
        [SerializeField] private CanvasGroup hudCanvasGroup;
        
        [SerializeField] private Animator interfaceAnimator;
        
        private static readonly int ShowInventoryID = Animator.StringToHash("SHOW INVENTORY");

        public bool isOnMenu;
        
        private void Start() {
            InputManager.Instance.uiSchemaContext = UISchemaSwitchType.HOME_MENU;
            isOnMenu = false;
        }

        public void Hide_menus() {
            if (GameSettings.Instance.isKeyRebinding) return;
            
            if (GameManager.Instance.isInGame) {
                Set_active(loadMenuCanvasGroup, false);
                Set_active(optionsMenuCanvasGroup, false);
                Set_active(bananapediaMenuCanvasGroup, false);

                Set_active(gameMenuCanvasGroup, false);

                if (interfaceAnimator.GetBool(ShowInventoryID)) {
                    interfaceAnimator.SetBool(ShowInventoryID, false);
                }

                GameManager.Instance.PauseGame(false);
            }

            else {
                Set_active(loadMenuCanvasGroup, false);
                Set_active(optionsMenuCanvasGroup, false);
                Set_active(bananapediaMenuCanvasGroup, false);
                Set_active(creditsMenuCanvasGroup, false);
            }

            isOnMenu = false;
        }
        
        public void Show_home_menu() {
            Set_active(optionsMenuCanvasGroup, false);
            Set_active(bananapediaMenuCanvasGroup, false);
            Set_active(creditsMenuCanvasGroup, false);

            Set_active(homeMenuCanvasGroup, true);
            Set_active(gameMenuCanvasGroup, false);
            
            isOnMenu = false;
        }

        public void Hide_home_menu() {
            Set_active(loadMenuCanvasGroup, false);
            Set_active(optionsMenuCanvasGroup, false);
            Set_active(bananapediaMenuCanvasGroup, false);
            Set_active(creditsMenuCanvasGroup, false);

            Set_active(homeMenuCanvasGroup, false);
            isOnMenu = true;
        }

        public void Show_Load_menu() {
            Set_active(loadMenuCanvasGroup, true);
            Set_active(optionsMenuCanvasGroup, false);
            Set_active(bananapediaMenuCanvasGroup, false);
            Set_active(creditsMenuCanvasGroup, false);
            UISave.Instance.newSaveButton.SetActive(GameManager.Instance.isInGame);
            isOnMenu = true;
        }

        public void Show_options_menu() {
            Set_active(loadMenuCanvasGroup, false);
            Set_active(optionsMenuCanvasGroup, true);
            Set_active(bananapediaMenuCanvasGroup, false);
            Set_active(creditsMenuCanvasGroup, false);
            isOnMenu = true;
        }

        public void Hide_options_menu() {
            if (GameManager.Instance.isInGame) {
                Set_active(optionsMenuCanvasGroup, false);
                Set_active(gameMenuCanvasGroup, true);
            }
            isOnMenu = false;
        }

        public void Show_Bananapedia() {
            UIBananapedia.Instance.SelectFirstBananapediaEntry();

            Set_active(loadMenuCanvasGroup, false);
            Set_active(optionsMenuCanvasGroup, false);
            Set_active(bananapediaMenuCanvasGroup, true);
            Set_active(creditsMenuCanvasGroup, false);
            isOnMenu = true;
        }

        public void Hide_Bananapedia() {
            if (GameManager.Instance.isInGame) {
                Set_active(bananapediaMenuCanvasGroup, false);
                Set_active(gameMenuCanvasGroup, true);
            }
            isOnMenu = false;
        }

        public void Show_Credits() {
            creditsMenuCanvasGroup.GetComponent<InfinityScroll.InfinityScroll>().enabled = true;
            creditsMenuCanvasGroup.GetComponent<InfinityScroll.InfinityScroll>().value = 0.13f;
            
            Set_active(loadMenuCanvasGroup, false);
            Set_active(optionsMenuCanvasGroup, false);
            Set_active(bananapediaMenuCanvasGroup, false);
            Set_active(creditsMenuCanvasGroup, true);
            isOnMenu = true;
        }

        public void Hide_Credits() {
            creditsMenuCanvasGroup.GetComponent<InfinityScroll.InfinityScroll>().enabled = false;

            if (GameManager.Instance.isInGame) {
                Set_active(loadMenuCanvasGroup, false);
                Set_active(optionsMenuCanvasGroup, false);
                Set_active(bananapediaMenuCanvasGroup, false);
                Set_active(creditsMenuCanvasGroup, false);

                Set_active(gameMenuCanvasGroup, true);
            }
            isOnMenu = false;
        }

        public void Show_game_menu() {
            if (GameManager.Instance.isInGame) {
                if (!interfaceAnimator.GetBool(ShowInventoryID)) {
                    Set_active(gameMenuCanvasGroup, true);
                }
            }
        }

        public void Hide_Game_Menu() {
            Set_active(gameMenuCanvasGroup, false);
        }

        /// IN GAME ///
        
        public void Show_Hide_interface() {
            if (GameManager.Instance.isInGame && gameMenuCanvasGroup.alpha == 0f) {
                if (!Is_Interface_Visible()) {
                    interfaceAnimator.SetBool(ShowInventoryID, true);
                    
                    InputManager.Instance.uiSchemaContext = UISchemaSwitchType.INVENTAIRE;
                    InputManager.Instance.SwitchContext(GameContext.UI);
                    
                    UInventory.Instance.RefreshUInventory();
                    GameManager.Instance.PauseGame(true);
                    Focus_interface();
                }
                else {
                    InputManager.Instance.SwitchContext(GameContext.GAME);
                    interfaceAnimator.SetBool(ShowInventoryID, false);
                    GameManager.Instance.PauseGame(false);
                }
            }
        }

        private bool Is_Interface_Visible() {
            return interfaceAnimator.GetBool(ShowInventoryID);
        }

        public void Show_HUD() {
            Set_active(hudCanvasGroup, true);
        }

        public void Hide_HUD() {
            Set_active(hudCanvasGroup, false);
        }

        void Focus_interface() {
            EventSystem.current.SetSelectedGameObject(UInventory.Instance.lastselectedInventoryItem);
        }
        
        public void Show_death_Panel() {
            Set_active(deathMenuCanvasGroup, true);
        }

        public void Hide_death_Panel() {
            Set_active(deathMenuCanvasGroup, false);
        }

        public void Set_active(CanvasGroup canvasGroup, bool visible) {
            canvasGroup.alpha = visible ? 1 : 0;
            canvasGroup.interactable = visible;
            canvasGroup.blocksRaycasts = visible;
        }
    }
}
