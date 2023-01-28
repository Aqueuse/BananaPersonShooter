using Dialogues;
using Enums;
using Input;
using Settings;
using UI.InGame.Inventory;
using UI.InGame.PlateformBuilder;
using UI.Menus;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI {
    public class UIManager : MonoSingleton<UIManager> {
        [SerializeField] private CanvasGroup homeMenuCanvasGroup;
        [SerializeField] private CanvasGroup gameMenuCanvasGroup;
        public CanvasGroup optionsMenuCanvasGroup;
        [SerializeField] private CanvasGroup bananapediaMenuCanvasGroup;
        [SerializeField] private CanvasGroup creditsMenuCanvasGroup;
        [SerializeField] private CanvasGroup deathMenuCanvasGroup;
        [SerializeField] private CanvasGroup dialoguesCanvasGroup;
        [SerializeField] private CanvasGroup miniChimpPlateformBuilderCanvasGroup;
        [SerializeField] private CanvasGroup hudCanvasGroup;

        [SerializeField] private Animator interfaceAnimator;
        
        private GameObject _firstGameMenuItem;
        private GameObject _firstHomeMenuItem;
        private static readonly int ShowInventoryID = Animator.StringToHash("SHOW INVENTORY");

        private void Start() { 
            _firstGameMenuItem = gameMenuCanvasGroup.transform.GetChild(0).gameObject;
            _firstHomeMenuItem = homeMenuCanvasGroup.transform.GetChild(0).gameObject;
            
            EventSystem.current.SetSelectedGameObject(_firstHomeMenuItem);
        }


        public void Hide_menus() {
            if (GameSettings.Instance.isKeyRebinding) return;
            
            if (GameManager.Instance.isInGame) {
                Set_active(gameMenuCanvasGroup, false);
                Set_active(optionsMenuCanvasGroup, false);
                Set_active(miniChimpPlateformBuilderCanvasGroup, false);
                Set_active(bananapediaMenuCanvasGroup, false);

                if (interfaceAnimator.GetBool(ShowInventoryID)) {
                    interfaceAnimator.SetBool(ShowInventoryID, false);
                }

                GameManager.Instance.PauseGame(false);
                
                if (dialoguesCanvasGroup.alpha > 0) {
                    DialogueSystem.Instance.Hide_Dialogue();
                }
            }

            else {
                Set_active(optionsMenuCanvasGroup, false);
                Set_active(bananapediaMenuCanvasGroup, false);
                Set_active(creditsMenuCanvasGroup, false);
                
                Set_active(homeMenuCanvasGroup, true);
            }
        }
        
        public void Show_home_menu() {
            Set_active(homeMenuCanvasGroup, true);
            Set_active(gameMenuCanvasGroup, false);
            EventSystem.current.SetSelectedGameObject(_firstHomeMenuItem);
        }

        public void Hide_home_menu() {
            Set_active(homeMenuCanvasGroup, false);
        }

        public void Show_options_menu() {
            if (GameManager.Instance.isInGame) {
                Set_active(optionsMenuCanvasGroup, true);
                Set_active(gameMenuCanvasGroup, false);
            }
            else {
                Set_active(optionsMenuCanvasGroup, true);
                Set_active(homeMenuCanvasGroup, false);
            }
            
            EventSystem.current.SetSelectedGameObject(UIOptionsMenu.Instance.GetFirstTab(), null);
        }

        public void Hide_options_menu() {
            if (GameManager.Instance.isInGame) {
                Set_active(optionsMenuCanvasGroup, false);
                Set_active(gameMenuCanvasGroup, true);
                EventSystem.current.SetSelectedGameObject(_firstGameMenuItem);
            }

            else {
                Set_active(optionsMenuCanvasGroup, false);
                Set_active(homeMenuCanvasGroup, true);
            }
        }

        public void Show_Bananapedia() {
            UIBananapedia.Instance.SelectFirstBananapediaEntry();

            if (GameManager.Instance.isInGame) {
                Set_active(bananapediaMenuCanvasGroup, true);
                Set_active(gameMenuCanvasGroup, false);
            }
            else {
                Set_active(bananapediaMenuCanvasGroup, true);
                Set_active(homeMenuCanvasGroup, false);
            }
        }

        public void Hide_Bananapedia() {
            if (GameManager.Instance.isInGame) {
                Set_active(bananapediaMenuCanvasGroup, false);
                Set_active(gameMenuCanvasGroup, true);
                EventSystem.current.SetSelectedGameObject(_firstGameMenuItem);
            }

            else {
                Set_active(bananapediaMenuCanvasGroup, false);
                Set_active(homeMenuCanvasGroup, true);
            }
        }

        public void Show_Credits() {
            creditsMenuCanvasGroup.GetComponent<InfinityScroll.InfinityScroll>().enabled = true;
            creditsMenuCanvasGroup.GetComponent<InfinityScroll.InfinityScroll>().value = 0.13f;
            
            if (GameManager.Instance.isInGame) {
                Set_active(creditsMenuCanvasGroup, true);
                Set_active(gameMenuCanvasGroup, false);
            }
            else {
                Set_active(creditsMenuCanvasGroup, true);
                Set_active(homeMenuCanvasGroup, false);
            }
        }

        public void Hide_Credits() {
            creditsMenuCanvasGroup.GetComponent<InfinityScroll.InfinityScroll>().enabled = false;

            if (GameManager.Instance.isInGame) {
                Set_active(creditsMenuCanvasGroup, false);
                Set_active(gameMenuCanvasGroup, true);
                EventSystem.current.SetSelectedGameObject(_firstGameMenuItem);
            }

            else {
                Set_active(creditsMenuCanvasGroup, false);
                Set_active(homeMenuCanvasGroup, true);
            }
        }

        public void HideSubPanels() {
            Hide_options_menu();
            Hide_Bananapedia();
            Hide_Credits();
        }

        public void Show_game_menu() {
            if (GameManager.Instance.isInGame) {
                if (!interfaceAnimator.GetBool(ShowInventoryID)) {
                    Set_active(gameMenuCanvasGroup, true);
                    EventSystem.current.SetSelectedGameObject(_firstGameMenuItem);
                    GameManager.Instance.PauseGame(true);
                }
            }
        }
        
        /// IN GAME ///
        
        public void Show_Hide_inventory() {
            if (GameManager.Instance.isInGame && gameMenuCanvasGroup.alpha == 0f) {
                if (!Is_Inventory_Visible()) {
                    interfaceAnimator.SetBool(ShowInventoryID, true);

                    UInventory.Instance.RefreshUInventory();
                    Focus_inventory();
                    InputManager.Instance.SwitchContext(GameContext.UI);
                }
                else {
                    interfaceAnimator.SetBool(ShowInventoryID, false);
                    InputManager.Instance.SwitchContext(GameContext.GAME);
                    GameManager.Instance.PauseGame(false);
                }
            }
        }

        public bool Is_Inventory_Visible() {
            return interfaceAnimator.GetBool(ShowInventoryID);
        }

        public void Show_HUD() {
            Set_active(hudCanvasGroup, true);
        }

        public void Hide_HUD() {
            Set_active(hudCanvasGroup, false);
        }

        void Focus_inventory() {
            EventSystem.current.SetSelectedGameObject(UInventory.Instance.lastselectedInventoryItem);
        }
        
        public void Show_death_Panel() {
            Set_active(deathMenuCanvasGroup, true);
        }

        public void Hide_death_Panel() {
            Set_active(deathMenuCanvasGroup, false);
        }

        public void Show_Hide_minichimp_plateform_builder_interface(bool isVisible) {
            Set_active(miniChimpPlateformBuilderCanvasGroup, isVisible);

            if (isVisible) {
                UIBuildInventoryLeft.Instance.RefreshInventoySlots();
                GameManager.Instance.PauseGame(true);
                EventSystem.current.SetSelectedGameObject(miniChimpPlateformBuilderCanvasGroup.gameObject);
            }
        }

        public void Set_active(CanvasGroup canvasGroup, bool visible) {
            canvasGroup.alpha = visible ? 1 : 0;
            canvasGroup.interactable = visible;
            canvasGroup.blocksRaycasts = visible;
        }
    }
}
