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
            SetActive(UICanvasGroupType.MINI_CHIMP_BANANAPEDIA_BLOCK, false);
            SetActive(UICanvasGroupType.CREDITS, false);
            SetActive(UICanvasGroupType.HOME_MENU, true);
            SetActive(UICanvasGroupType.GAME_MENU, false);

            isOnSubMenus = false;
            EventSystem.current.SetSelectedGameObject(ObjectsReference.Instance.uiHomeMenu.firstSelectedButton);
        }
        
        public void HideHomeMenu() {
            SetActive(UICanvasGroupType.LOAD, false);
            SetActive(UICanvasGroupType.OPTIONS, false);
            SetActive(UICanvasGroupType.MINI_CHIMP_BANANAPEDIA_BLOCK, false);
            SetActive(UICanvasGroupType.CREDITS, false);

            SetActive(UICanvasGroupType.HOME_MENU, false);
            isOnSubMenus = false;
        }

        public void ShowLoadMenu() {
            SetActive(UICanvasGroupType.LOAD, true);
            SetActive(UICanvasGroupType.OPTIONS, false);
            SetActive(UICanvasGroupType.MINI_CHIMP_BANANAPEDIA_BLOCK, false);
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
            SetActive(UICanvasGroupType.MINI_CHIMP_BANANAPEDIA_BLOCK, false);
            SetActive(UICanvasGroupType.CREDITS, false);
            SetActive(UICanvasGroupType.HOME_MENU, false);
            
            isOnSubMenus = true;
            HideGameMenu();
            
            ObjectsReference.Instance.uiOptionsMenu.SwitchToAudioVideoTab();
        }

        public void ShowBananapedia() {
            SetActive(UICanvasGroupType.LOAD, false);
            SetActive(UICanvasGroupType.OPTIONS, false);
            SetActive(UICanvasGroupType.MINI_CHIMP_BANANAPEDIA_BLOCK, true);
            SetActive(UICanvasGroupType.CREDITS, false);
            SetActive(UICanvasGroupType.HOME_MENU, false);

            isOnSubMenus = true;
            HideGameMenu();

            EventSystem.current.SetSelectedGameObject(ObjectsReference.Instance.uiBananapedia.firstSelectedGameObject);
        }
        
        public void ShowCredits() {
            SetActive(UICanvasGroupType.LOAD, false);
            SetActive(UICanvasGroupType.OPTIONS, false);
            SetActive(UICanvasGroupType.MINI_CHIMP_BANANAPEDIA_BLOCK, false);
            SetActive(UICanvasGroupType.CREDITS, true);
            SetActive(UICanvasGroupType.HOME_MENU, false);
            
            EventSystem.current.SetSelectedGameObject(ObjectsReference.Instance.uiCredits.firstSelectedGameObject);
            
            isOnSubMenus = true;
            HideGameMenu();
        }
        
        public void ShowGameMenu() {
            SetActive(UICanvasGroupType.LOAD, false);
            SetActive(UICanvasGroupType.OPTIONS, false);
            SetActive(UICanvasGroupType.MINI_CHIMP_BANANAPEDIA_BLOCK, false);
            SetActive(UICanvasGroupType.CREDITS, false);
            SetActive(UICanvasGroupType.HOME_MENU, false);
            
            SetActive(UICanvasGroupType.GAME_MENU, true);
            isOnSubMenus = false;
            
            EventSystem.current.SetSelectedGameObject(ObjectsReference.Instance.uiGameMenu.firstSelectedGameObject);
        }

        public void HideGameMenu() {
            SetActive(UICanvasGroupType.GAME_MENU, false);
        }

        public void ShowBananaGunUI() {
            if (!ObjectsReference.Instance.bananaMan.tutorialFinished) return;
            ObjectsReference.Instance.bananaGunActionsSwitch.DesactiveBananaGun();

            ObjectsReference.Instance.inputManager.SwitchContext(InputContext.BANANAGUN_UI);
            ObjectsReference.Instance.gameManager.gameContext = GameContext.IN_BANANAGUN_UI;
            
            ObjectsReference.Instance.uiManager.SetActive(UICanvasGroupType.INVENTORIES, true);
            
            ObjectsReference.Instance.uInventoriesManager.OpenInventories();
            
            SetActive(UICanvasGroupType.MINI_CHIMP_BLOCK, true);
            ObjectsReference.Instance.miniChimpBlockManager.enabled = true;
        }

        public void HideBananaGunUI() {
            SetActive(UICanvasGroupType.MINI_CHIMP_BLOCK, false);
            ObjectsReference.Instance.miniChimpBlockManager.enabled = false;
            
            SetActive(UICanvasGroupType.INVENTORIES, false);
            
            ObjectsReference.Instance.bananaGun.UngrabBananaGun();

            ObjectsReference.Instance.inputManager.SwitchContext(InputContext.GAME);
            ObjectsReference.Instance.gameManager.gameContext = GameContext.IN_GAME;
            
            ObjectsReference.Instance.bananaGunActionsSwitch.SwitchToBananaGunMode(ObjectsReference.Instance.bananaMan.bananaGunMode);
        }

        public void SetActive(UICanvasGroupType uiCanvasGroupType, bool visible) {
            var canvasGroup = canvasGroupsByUICanvasType[uiCanvasGroupType];

            canvasGroup.alpha = visible ? 1 : 0;
            canvasGroup.interactable = visible;
            canvasGroup.blocksRaycasts = visible;
        }
    }
}
