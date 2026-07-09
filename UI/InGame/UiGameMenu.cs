using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.InGame {
    public class UiGameMenu : MonoBehaviour {
        [SerializeField] private Color activatedColor;
        [SerializeField] private Color unactivatedColor;
        [SerializeField] private Color activatedTextColor;

        private Image[] _gameMenuButtons;
        private int _selectedButton;
        
        public GameObject firstSelectedGameObject;

        private void Start() {
            _selectedButton = 0;
            _gameMenuButtons = GetComponentsInChildren<Image>();
        }

        public void ShowGameMenu() {
            ObjectsReference.Instance.uiManager.SetActive(UICanvasGroupType.HUD_BANANAMAN, false);
            ObjectsReference.Instance.uiManager.SetActive(UICanvasGroupType.GAME_MENU, true);
            EventSystem.current.SetSelectedGameObject(firstSelectedGameObject);
        }

        public void HideGameMenu() {
            ObjectsReference.Instance.uiManager.SetActive(UICanvasGroupType.HUD_BANANAMAN, true);
            ObjectsReference.Instance.uiManager.SetActive(UICanvasGroupType.GAME_MENU, false);
        }
        
        public void ReturnToMainMenu() {
            ShowGameMenu();
        }
        
        public void ReturnToGame() {
            ObjectsReference.Instance.gameManager.UnpauseGame();
            HideGameMenu();

            ObjectsReference.Instance.inputManager.SwitchContext(InputContext.GAME);
        }
        
        public void Quit() {
            ObjectsReference.Instance.gameManager.ReturnHome();
        }

        public void SwitchToRightButton() {
            if (_selectedButton < _gameMenuButtons.Length-1) {
                _selectedButton++;
            }
            
            SetActivatedButton(_gameMenuButtons[_selectedButton]);
        }

        public void SwitchToLeftButton() {
            if (_selectedButton > 0) {
                _selectedButton--;
            }
            
            SetActivatedButton(_gameMenuButtons[_selectedButton]);
        }

        private static void HideSubmenus() {
            ObjectsReference.Instance.uiManager.SetActive(UICanvasGroupType.LOAD, false);
            ObjectsReference.Instance.uiManager.SetActive(UICanvasGroupType.OPTIONS, false);
            ObjectsReference.Instance.uiManager.SetActive(UICanvasGroupType.CREDITS, false);
        }

        public void SetActivatedButton(Image buttonImage) {
            foreach (var image in _gameMenuButtons) {
                image.color = unactivatedColor;
                image.GetComponentInChildren<TextMeshProUGUI>().color = activatedColor;
            }

            buttonImage.color = activatedColor;
            buttonImage.GetComponentInChildren<TextMeshProUGUI>().color = activatedTextColor;
        }
    }
}
