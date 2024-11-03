using UI.InGame.Inventory;
using UI.Save;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI {
    public class UIManager : MonoBehaviour {
        public GenericDictionary<UICanvasGroupType, CanvasGroup> canvasGroupsByUICanvasType;
        public bool isOnSubMenus;
        
        private void Start() {
            isOnSubMenus = false;
            
            ShowHomeMenu();
        }

        public void ReturnToMainMenu() {
            if (ObjectsReference.Instance.gameManager.gameContext == GameContext.IN_GAME_MENU) ShowGameMenu();
            else ShowHomeMenu();
        }

        public void ShowHomeMenu() {
            SetActive(UICanvasGroupType.LOAD, false);
            SetActive(UICanvasGroupType.OPTIONS, false);
            SetActive(UICanvasGroupType.BANANAPEDIA, false);
            SetActive(UICanvasGroupType.CREDITS, false);
            SetActive(UICanvasGroupType.HOME_MENU, true);
            SetActive(UICanvasGroupType.GAME_MENU, false);

            isOnSubMenus = false;
            EventSystem.current.SetSelectedGameObject(ObjectsReference.Instance.uiHomeMenu.firstSelectedButton);
        }
        
        public void HideHomeMenu() {
            SetActive(UICanvasGroupType.LOAD, false);
            SetActive(UICanvasGroupType.OPTIONS, false);
            SetActive(UICanvasGroupType.BANANAPEDIA, false);
            SetActive(UICanvasGroupType.CREDITS, false);

            SetActive(UICanvasGroupType.HOME_MENU, false);
            isOnSubMenus = false;
        }

        public void ShowLoadMenu() {
            SetActive(UICanvasGroupType.LOAD, true);
            SetActive(UICanvasGroupType.OPTIONS, false);
            SetActive(UICanvasGroupType.BANANAPEDIA, false);
            SetActive(UICanvasGroupType.CREDITS, false);
            SetActive(UICanvasGroupType.HOME_MENU, false);

            isOnSubMenus = true;
            HideGameMenu();

            if (ObjectsReference.Instance.gameManager.gameContext == GameContext.IN_GAME_MENU) {
                ObjectsReference.Instance.uiSave.newSaveButton.SetActive(true);
                EventSystem.current.SetSelectedGameObject(ObjectsReference.Instance.uiSave.newSaveButton);
            }

            else {
                ObjectsReference.Instance.uiSave.newSaveButton.SetActive(false);
                if (ObjectsReference.Instance.uiSave.GetComponentInChildren<UISaveSlot>() != null)
                    EventSystem.current.SetSelectedGameObject(ObjectsReference.Instance.uiSave.GetComponentsInChildren<UISaveSlot>()[0].gameObject);
            }
        }

        public void ShowOptions() {
            ObjectsReference.Instance.gameSettings.LoadSettings();
            
            SetActive(UICanvasGroupType.LOAD, false);
            SetActive(UICanvasGroupType.OPTIONS, true);
            SetActive(UICanvasGroupType.BANANAPEDIA, false);
            SetActive(UICanvasGroupType.CREDITS, false);
            SetActive(UICanvasGroupType.HOME_MENU, false);
            
            isOnSubMenus = true;
            HideGameMenu();
            
            ObjectsReference.Instance.uiOptionsMenu.SwitchToAudioVideoTab();
        }

        public void ShowBananapedia() {
            SetActive(UICanvasGroupType.LOAD, false);
            SetActive(UICanvasGroupType.OPTIONS, false);
            SetActive(UICanvasGroupType.BANANAPEDIA, true);
            SetActive(UICanvasGroupType.CREDITS, false);
            SetActive(UICanvasGroupType.HOME_MENU, false);

            isOnSubMenus = true;
            HideGameMenu();

            EventSystem.current.SetSelectedGameObject(ObjectsReference.Instance.uiBananapedia.firstSelectedGameObject);
        }
        
        public void ShowCredits() {
            SetActive(UICanvasGroupType.LOAD, false);
            SetActive(UICanvasGroupType.OPTIONS, false);
            SetActive(UICanvasGroupType.BANANAPEDIA, false);
            SetActive(UICanvasGroupType.CREDITS, true);
            SetActive(UICanvasGroupType.HOME_MENU, false);
            
            EventSystem.current.SetSelectedGameObject(ObjectsReference.Instance.uiCredits.firstSelectedGameObject);
            
            isOnSubMenus = true;
            HideGameMenu();
        }
        
        public void ShowGameMenu() {
            SetActive(UICanvasGroupType.LOAD, false);
            SetActive(UICanvasGroupType.OPTIONS, false);
            SetActive(UICanvasGroupType.BANANAPEDIA, false);
            SetActive(UICanvasGroupType.CREDITS, false);
            SetActive(UICanvasGroupType.HOME_MENU, false);
            
            SetActive(UICanvasGroupType.GAME_MENU, true);
            isOnSubMenus = false;
            
            EventSystem.current.SetSelectedGameObject(ObjectsReference.Instance.uiGameMenu.firstSelectedGameObject);
        }

        public void HideGameMenu() {
            SetActive(UICanvasGroupType.GAME_MENU, false);
        }

        public void ShowMiniChimpBlock() {
            if (!ObjectsReference.Instance.bananaMan.tutorialFinished) return;

            SetActive(UICanvasGroupType.MINI_CHIMP_BLOCK, true);
            ObjectsReference.Instance.miniChimpDialoguesManager.ResetDialogue();
        }

        public void ShowInventory() {
            if (!ObjectsReference.Instance.bananaMan.tutorialFinished) return;
            
            ObjectsReference.Instance.uInventoriesManager.OpenInventories();

            SetActive(UICanvasGroupType.INVENTORIES, true);
        }

        public void ShowHideMap() {
            if (canvasGroupsByUICanvasType[UICanvasGroupType.MAP].alpha == 0)
                canvasGroupsByUICanvasType[UICanvasGroupType.MAP].alpha = 0.6f;
            else {
                canvasGroupsByUICanvasType[UICanvasGroupType.MAP].alpha = 0f;
            }
        }

        public void ShowBananaGunUI() {
            if (!ObjectsReference.Instance.bananaMan.tutorialFinished) return;
            
            ObjectsReference.Instance.bananaGunActionsSwitch.DesactiveBananaGun();

            ObjectsReference.Instance.inputManager.SwitchContext(InputContext.MINICHIMP_VIEW);
            ObjectsReference.Instance.gameManager.gameContext = GameContext.IN_MINICHIMP_VIEW;
            
            ObjectsReference.Instance.uiManager.SetActive(UICanvasGroupType.INVENTORIES, true);
            
            ObjectsReference.Instance.uInventoriesManager.OpenInventories();
            
            SetActive(UICanvasGroupType.MINI_CHIMP_BLOCK, true);
            
            ObjectsReference.Instance.uiMiniChimpBlock.SwitchToBlock(MiniChimpBlockTabType.MINICHIMPBLOCK_DIALOGUE);
            ObjectsReference.Instance.miniChimpDialoguesManager.ResetDialogue();
        }

        public void HideBananaGunUI() {
            if (ObjectsReference.Instance.gameManager.gameContext == GameContext.IN_MINICHIMP_VIEW) {
                SetActive(UICanvasGroupType.MINI_CHIMP_BLOCK, false);
            }

            else {
                ObjectsReference.Instance.scanWithMouseForDescription.enabled = false;
            
                SetActive(UICanvasGroupType.MINI_CHIMP_BLOCK, false);
                SetActive(UICanvasGroupType.INVENTORIES, false);
            
                ObjectsReference.Instance.bananaGun.UngrabBananaGun();

                ObjectsReference.Instance.inputManager.SwitchContext(InputContext.GAME);
                ObjectsReference.Instance.gameManager.gameContext = GameContext.IN_GAME;
            
                ObjectsReference.Instance.bananaGunActionsSwitch.SwitchToBananaGunMode(ObjectsReference.Instance.bananaMan.bananaGunMode);
            }
        }

        public void HideInventories() {
            SetActive(UICanvasGroupType.INVENTORIES, false);
            ObjectsReference.Instance.uInfobulle.Hide();
        }
        
        public void SetActive(UICanvasGroupType uiCanvasGroupType, bool visible) {
            var canvasGroup = canvasGroupsByUICanvasType[uiCanvasGroupType];

            canvasGroup.alpha = visible ? 1 : 0;
            canvasGroup.interactable = visible;
            canvasGroup.blocksRaycasts = visible;
        }

        public void SwitchToMinichimpPerspective() {
            ObjectsReference.Instance.uiStats.RefreshStats();

            SetActive(UICanvasGroupType.HUD_BANANAMAN, false);
            SetActive(UICanvasGroupType.INVENTORIES, false);
            SetActive(UICanvasGroupType.HUD_MINICHIMP, true);
            SetActive(UICanvasGroupType.QUICK_ACCESS_BUTTONS, true);
            
            ObjectsReference.Instance.uiDescriptionsManager.HideAllPanels();

            ObjectsReference.Instance.uiManager.canvasGroupsByUICanvasType[UICanvasGroupType.CROSSHAIRS].alpha = 0;

            ObjectsReference.Instance.miniChimpViewMode.enabled = true;
            ObjectsReference.Instance.inputManager.SwitchContext(InputContext.MINICHIMP_VIEW);

            ObjectsReference.Instance.mainCamera.SwitchToGestionView();
        }

        public void SwitchToBananaManPerspective() {
            ObjectsReference.Instance.miniChimpViewMode.CancelGhost();
            ObjectsReference.Instance.miniChimpViewMode.viewModeContextType = ViewModeContextType.SCAN;
            ObjectsReference.Instance.miniChimpViewMode.enabled = false;

            ObjectsReference.Instance.uInventoriesManager.GetCurrentUIHelper().ShowDefaultHelper();

            SetActive(UICanvasGroupType.MINI_CHIMP_BLOCK, false);
            SetActive(UICanvasGroupType.QUICK_ACCESS_BUTTONS, false);
            SetActive(UICanvasGroupType.HUD_MINICHIMP, false);
            SetActive(UICanvasGroupType.HUD_BANANAMAN, true);

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Locked;
            ObjectsReference.Instance.uiManager.canvasGroupsByUICanvasType[UICanvasGroupType.CROSSHAIRS].alpha = 1;

            ObjectsReference.Instance.inputManager.SwitchContext(InputContext.GAME);

            ObjectsReference.Instance.mainCamera.SwitchToBananaManView();
        }
    }
}
