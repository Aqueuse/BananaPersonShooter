using UI.InGame;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace UI {
    public class UIManager : MonoSingleton<UIManager> {
        public GameObject homeMenu;
        public GameObject gameMenu;
        public GameObject optionsMenu;
        public GameObject inventory;
        public GameObject slotsPanel;
        public GameObject HUDPanel;

        private CanvasGroup _homeMenuCanvasGroup;
        private CanvasGroup _gameMenuCanvasGroup;
        private CanvasGroup _optionsMenuCanvasGroup;
        private CanvasGroup _inventoryCanvasGroup;
        private CanvasGroup _hudCanvasGroup;
        
        private GameObject _firstGameMenuItem;
        private GameObject _firstHomeMenuItem;

        public BananaType selectedWeapon = BananaType.EMPTY_HAND;
        
        private void Start() { 
            _firstGameMenuItem = gameMenu.transform.GetChild(0).gameObject;
            _firstHomeMenuItem = homeMenu.transform.GetChild(0).gameObject;

            _homeMenuCanvasGroup = homeMenu.GetComponent<CanvasGroup>();
            _gameMenuCanvasGroup = gameMenu.GetComponent<CanvasGroup>();
            _optionsMenuCanvasGroup = optionsMenu.GetComponent<CanvasGroup>();
            _inventoryCanvasGroup = inventory.GetComponent<CanvasGroup>();
            _hudCanvasGroup = HUDPanel.GetComponent<CanvasGroup>();

            EventSystem.current.SetSelectedGameObject(_firstHomeMenuItem);
        }

        public void Show_game_menu(InputAction.CallbackContext context) {
            if (context.performed && GameManager.Instance.isPlaying) {
                if (_inventoryCanvasGroup.alpha < 1f) {
                    Set_active(_gameMenuCanvasGroup, true);
                    EventSystem.current.SetSelectedGameObject(_firstGameMenuItem);
                    GameManager.Instance.PauseGame(true);
                }
            }
        }

        public void Hide_game_menu(InputAction.CallbackContext context) {
            if (context.performed && GameManager.Instance.isPlaying) {
                Set_active(_inventoryCanvasGroup, false);
                Set_active(_gameMenuCanvasGroup, false);
                Set_active(_optionsMenuCanvasGroup, false);
                GameManager.Instance.PauseGame(false);
            }
        }
        
        public void Show_home_menu() {
            Set_active(_homeMenuCanvasGroup, true);
            Set_active(_gameMenuCanvasGroup, false);
            EventSystem.current.SetSelectedGameObject(_firstHomeMenuItem);
        }

        public void Hide_home_menu() {
            Set_active(_homeMenuCanvasGroup, false);
        }

        public void Show_options_menu() {
            if (GameManager.Instance.isPlaying) {
                Set_active(_optionsMenuCanvasGroup, true);
                Set_active(_gameMenuCanvasGroup, false);
            }
            else {
                Set_active(_optionsMenuCanvasGroup, true);
                Set_active(_homeMenuCanvasGroup, false);
            }
        }

        public void Hide_options_menu() {
            if (GameManager.Instance.isPlaying) {
                Set_active(_optionsMenuCanvasGroup, false);
                Set_active(_gameMenuCanvasGroup, true);
                EventSystem.current.SetSelectedGameObject(_firstGameMenuItem);
            }

            else {
                Set_active(_optionsMenuCanvasGroup, false);
                Set_active(_homeMenuCanvasGroup, true);
            }
        }

        public void Show_inventory(InputAction.CallbackContext context) {
            if (context.performed && GameManager.Instance.isPlaying && _gameMenuCanvasGroup.alpha == 0f) {
                if (GameManager.Instance.isPlaying) UInventory.Instance.RefreshUInventory();
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

        public void Focus_inventory() {
            EventSystem.current.SetSelectedGameObject(UInventory.Instance.lastselectedInventoryItem);
        }

        public void Focus_SlotsPanel() {
            EventSystem.current.SetSelectedGameObject(slotsPanel);
        }

        void Set_active(CanvasGroup canvasGroup, bool visible) {
            canvasGroup.alpha = visible ? 1 : 0;
            canvasGroup.interactable = visible;
            canvasGroup.blocksRaycasts = visible;
        }

    }
}
