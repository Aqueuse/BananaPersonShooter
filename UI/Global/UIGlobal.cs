using TMPro;
using Tweaks;
using UI.Save;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Global {
    public class UIGlobal : MonoBehaviour {
        [SerializeField] private Color activatedColor;
        [SerializeField] private Color unactivatedColor;
        [SerializeField] private Color activatedTextColor;

        [SerializeField] private Image[] _homeMenuButtons;
        
        public GameObject firstSelectedButton;
        
        private void Start() {
            ShowHomeMenu();
        }
        
        public void SetActivatedButton(Image buttonImage) {
            foreach (var image in _homeMenuButtons) {
                image.color = unactivatedColor;
                image.GetComponentInChildren<TextMeshProUGUI>().color = activatedColor;
            }

            buttonImage.color = activatedColor;
            buttonImage.GetComponentInChildren<TextMeshProUGUI>().color = activatedTextColor;
        }
        
        public void ShowHomeMenu() {
            HideSubmenus();
            ObjectsReference.Instance.uiManager.SetActive(UICanvasGroupType.HOME_MENU, true);
            
            EventSystem.current.SetSelectedGameObject(firstSelectedButton);
        }

        public void HideHomeMenu() {
            ObjectsReference.Instance.uiManager.SetActive(UICanvasGroupType.HOME_MENU, false);
        }
        
        public void ShowLoadMenu() {
            HideSubmenus();
            ObjectsReference.Instance.uiManager.SetActive(UICanvasGroupType.LOAD, true);
            
            if (ObjectsReference.Instance.gameManager.gameContext == GameContext.GAME_MENU) {
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
            
            HideSubmenus();
            ObjectsReference.Instance.uiManager.SetActive(UICanvasGroupType.OPTIONS, true);
            ObjectsReference.Instance.uiOptionsMenu.SwitchToAudioTab();
        }
        
        public void ShowCredits() {
            HideSubmenus();
            ObjectsReference.Instance.uiManager.SetActive(UICanvasGroupType.CREDITS, true);
            EventSystem.current.SetSelectedGameObject(ObjectsReference.Instance.uiCredits.firstSelectedGameObject);
        }
        
        public void ReturnToMainMenu() {
            ShowHomeMenu();
        }
        
        private void HideSubmenus() {
            ObjectsReference.Instance.uiManager.SetActive(UICanvasGroupType.LOAD, false);
            ObjectsReference.Instance.uiManager.SetActive(UICanvasGroupType.OPTIONS, false);
            ObjectsReference.Instance.uiManager.SetActive(UICanvasGroupType.CREDITS, false);
            ObjectsReference.Instance.uiManager.SetActive(UICanvasGroupType.HOME_MENU, false);
        }
        
        public void GoToDiscord() {
            Application.OpenURL("https://discord.gg/2SUscY39dw");
        }
    }
}