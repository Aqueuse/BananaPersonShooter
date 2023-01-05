using Dialogues;
using UI.InGame.Inventory;
using UI.InGame.PlateformBuilder;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace UI {
    public class UIManager : MonoSingleton<UIManager> {
        [SerializeField] private CanvasGroup homeMenuCanvasGroup;
        [SerializeField] private CanvasGroup gameMenuCanvasGroup;
        [SerializeField] private CanvasGroup optionsMenuCanvasGroup;
        [SerializeField] private CanvasGroup bananapediaMenuCanvasGroup;
        [SerializeField] private CanvasGroup creditsMenuCanvasGroup;
        [SerializeField] private CanvasGroup deathMenuCanvasGroup;
        [SerializeField] private CanvasGroup dialoguesCanvasGroup;
        [SerializeField] private CanvasGroup moverUICanvasGroup;
        [SerializeField] private CanvasGroup miniChimpPlateformBuilderCanvasGroup;
        
        public GameObject inventory;
        public GameObject slotsPanel;
        public GameObject hudPanel;

        private CanvasGroup _inventoryCanvasGroup;
        private CanvasGroup _hudCanvasGroup;
        
        private GameObject _firstGameMenuItem;
        private GameObject _firstHomeMenuItem;
        
        private void Start() { 
            _firstGameMenuItem = gameMenuCanvasGroup.transform.GetChild(0).gameObject;
            _firstHomeMenuItem = homeMenuCanvasGroup.transform.GetChild(0).gameObject;

            _inventoryCanvasGroup = inventory.GetComponent<CanvasGroup>();
            _hudCanvasGroup = hudPanel.GetComponent<CanvasGroup>();
            
            EventSystem.current.SetSelectedGameObject(_firstHomeMenuItem);
        }


        public void Hide_menus(InputAction.CallbackContext context) {
            if (GameManager.Instance.isInGame) {
                Set_active(_inventoryCanvasGroup, false);
                Set_active(gameMenuCanvasGroup, false);
                Set_active(optionsMenuCanvasGroup, false);
                Set_active(miniChimpPlateformBuilderCanvasGroup, false);
                Set_active(bananapediaMenuCanvasGroup, false);

                GameManager.Instance.PauseGame(false);
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

        public void Show_game_menu(InputAction.CallbackContext context) {
            if (context.performed && GameManager.Instance.isInGame) {
                if (context.performed && GameManager.Instance.isInGame) {
                    if (dialoguesCanvasGroup.alpha > 0) {
                        DialogueSystem.Instance.Hide_Dialogue();
                        return;
                    }
                }

                if (_inventoryCanvasGroup.alpha < 1f) {
                    Set_active(gameMenuCanvasGroup, true);
                    EventSystem.current.SetSelectedGameObject(_firstGameMenuItem);
                    GameManager.Instance.PauseGame(true);
                }
            }
        }

        public void Hide_game_menu() {
            Set_active(gameMenuCanvasGroup, false);
        }

        /// IN GAME ///
        
        public void Show_inventory(InputAction.CallbackContext context) {
            if (context.performed && GameManager.Instance.isInGame && gameMenuCanvasGroup.alpha == 0f) {
                if (GameManager.Instance.isInGame) UInventory.Instance.RefreshUInventory();
                Set_active(_inventoryCanvasGroup, true);
                GameManager.Instance.PauseGame(true);
                Focus_inventory();
            }
        }

        public void Show_HUD() {
            Set_active(_hudCanvasGroup, true);
        }

        public void Hide_HUD() {
            Set_active(_hudCanvasGroup, false);
        }

        void Focus_inventory() {
            EventSystem.current.SetSelectedGameObject(UInventory.Instance.lastselectedInventoryItem);
        }

        public void Focus_SlotsPanel() {
            EventSystem.current.SetSelectedGameObject(slotsPanel);
        }

        public void Show_death_Panel() {
            Set_active(deathMenuCanvasGroup, true);
        }

        public void Hide_death_Panel() {
            Set_active(deathMenuCanvasGroup, false);
        }

        public void Show_Hide_Mover_UI(bool isVisible) {
            Set_active(moverUICanvasGroup, isVisible);
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
