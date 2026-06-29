using UI.Save;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI {
    public class UIManager : MonoBehaviour {
        public GenericDictionary<UICanvasGroupType, CanvasGroup> canvasGroupsByUICanvasType;
        
        [SerializeField] private Material MiniMapSpritesMaterial;
        private static readonly int sizeMultiplier = Shader.PropertyToID("_SizeMultiplier");

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
            SetActive(UICanvasGroupType.CREDITS, false);
            SetActive(UICanvasGroupType.HOME_MENU, true);
            SetActive(UICanvasGroupType.GAME_MENU, false);

            isOnSubMenus = false;
            EventSystem.current.SetSelectedGameObject(ObjectsReference.Instance.uiHomeMenu.firstSelectedButton);
        }
        
        public void HideHomeMenu() {
            SetActive(UICanvasGroupType.LOAD, false);
            SetActive(UICanvasGroupType.OPTIONS, false);
            SetActive(UICanvasGroupType.CREDITS, false);

            SetActive(UICanvasGroupType.HOME_MENU, false);
            
            isOnSubMenus = false;
        }

        public void ShowLoadMenu() {
            SetActive(UICanvasGroupType.LOAD_BOTTOM_PANEL, false);
            SetActive(UICanvasGroupType.LOAD, true);
            
            SetActive(UICanvasGroupType.OPTIONS, false);
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
            SetActive(UICanvasGroupType.CREDITS, false);
            SetActive(UICanvasGroupType.HOME_MENU, false);
            
            isOnSubMenus = true;
            HideGameMenu();
            
            ObjectsReference.Instance.uiOptionsMenu.SwitchToAudioTab();
        }
        
        public void ShowCredits() {
            SetActive(UICanvasGroupType.LOAD, false);
            SetActive(UICanvasGroupType.OPTIONS, false);
            SetActive(UICanvasGroupType.CREDITS, true);
            SetActive(UICanvasGroupType.HOME_MENU, false);
            
            EventSystem.current.SetSelectedGameObject(ObjectsReference.Instance.uiCredits.firstSelectedGameObject);
            
            isOnSubMenus = true;
            HideGameMenu();
        }
        
        public void ShowGameMenu() {
            SetActive(UICanvasGroupType.LOAD, false);
            SetActive(UICanvasGroupType.OPTIONS, false);
            SetActive(UICanvasGroupType.CREDITS, false);
            SetActive(UICanvasGroupType.HOME_MENU, false);
            
            SetActive(UICanvasGroupType.GAME_MENU, true);
            isOnSubMenus = false;
            
            EventSystem.current.SetSelectedGameObject(ObjectsReference.Instance.uiGameMenu.firstSelectedGameObject);
        }

        public void HideGameMenu() {
            SetActive(UICanvasGroupType.GAME_MENU, false);
        }

        public void ShowBigMap() {
            canvasGroupsByUICanvasType[UICanvasGroupType.BIG_MAP].alpha = 0.99f;
            canvasGroupsByUICanvasType[UICanvasGroupType.MINI_MAP].alpha = 0f;

            MiniMapSpritesMaterial.SetFloat(sizeMultiplier, 1);
        }

        public void HideBigMap() {
            canvasGroupsByUICanvasType[UICanvasGroupType.BIG_MAP].alpha = 0;
            canvasGroupsByUICanvasType[UICanvasGroupType.MINI_MAP].alpha = 1f;
            
            MiniMapSpritesMaterial.SetFloat(sizeMultiplier, 10);
        }

        public void ShowMainPanel() {
            SetActive(UICanvasGroupType.MAIN_PANEL, true);
            ObjectsReference.Instance.uiMainPanel.SwitchToLastFocusedBlock();
            
            ObjectsReference.Instance.miniChimpDialoguesManager.ResetDialogue();
        }

        public void HideMainPanel() {
            SetActive(UICanvasGroupType.MAIN_PANEL, false);
            ObjectsReference.Instance.inputManager.SwitchBackToGame();
        }
        
        public void ShowInventory() {
            SetActive(UICanvasGroupType.MAIN_PANEL, true);
            ObjectsReference.Instance.uInventoriesManager.OpenInventories();
        }

        public void SwitchCameraGestionBananaMan() {
            if (ObjectsReference.Instance.gestionViewMode.isActiveAndEnabled) {
                SwitchToBananaManView();
            }

            else {
                SwitchToGestionView();
            }
        }

        private void SwitchToGestionView() {
            SetActive(UICanvasGroupType.HUD_BANANAMAN, false);
            SetActive(UICanvasGroupType.HUD_GESTION, true);
            
            ObjectsReference.Instance.bananaMan.SetToNotPlayable();
            
            ObjectsReference.Instance.gestionViewMode.enabled = true;
            ObjectsReference.Instance.inputManager.SwitchContext(InputContext.GESTION_VIEW);

            ObjectsReference.Instance.camerasManager.SwitchToGestionView();
        }

        private void SwitchToBananaManView() {
            ObjectsReference.Instance.gestionViewMode.CancelGhost();
            ObjectsReference.Instance.gestionViewMode.viewModeContextType = ViewModeContextType.SCAN;
            ObjectsReference.Instance.gestionViewMode.enabled = false;

            ObjectsReference.Instance.uInventoriesManager.GetCurrentUIHelper().ShowDefaultHelper();

            SetActive(UICanvasGroupType.HUD_GESTION, false);
            SetActive(UICanvasGroupType.HUD_BANANAMAN, true);

            ObjectsReference.Instance.bananaMan.SetToPlayable();

            ObjectsReference.Instance.camerasManager.SwitchToBananaManView();
        }
        
        public void ShowHideDebugPanel() {
            if (canvasGroupsByUICanvasType[UICanvasGroupType.DEBUG_PANEL].alpha == 0) {
                ObjectsReference.Instance.inputManager.SwitchContext(InputContext.UI);

                ObjectsReference.Instance.cameraPlayer.Set0Sensibility();
                ObjectsReference.Instance.playerController.canMove = false;
                
                SetActive(UICanvasGroupType.DEBUG_PANEL, true);
            }

            else {
                ObjectsReference.Instance.inputManager.SwitchContext(InputContext.GAME);

                ObjectsReference.Instance.cameraPlayer.SetNormalSensibility();
                ObjectsReference.Instance.playerController.canMove = true;

                SetActive(UICanvasGroupType.DEBUG_PANEL, false);
            }
        }

        public void ShowCrosshairs() {
            SetActive(UICanvasGroupType.CROSSHAIRS, true);
        }

        public void HideCrosshairs() {
            SetActive(UICanvasGroupType.CROSSHAIRS, false);
        }

        public void SetActive(UICanvasGroupType uiCanvasGroupType, bool visible) {
            var canvasGroup = canvasGroupsByUICanvasType[uiCanvasGroupType];

            canvasGroup.alpha = visible ? 1 : 0;
            canvasGroup.interactable = visible;
            canvasGroup.blocksRaycasts = visible;
        }
    }
}
