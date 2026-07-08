using TMPro;
using Tweaks;
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
            HideSubmenus();
            UITweaks.SetCanvasGroupActif(ObjectsReference.Instance.uiManager.canvasGroupsByUICanvasType[UICanvasGroupType.GAME_MENU], true);
            EventSystem.current.SetSelectedGameObject(firstSelectedGameObject);
        }

        public void HideGameMenu() {
            UITweaks.SetCanvasGroupActif(ObjectsReference.Instance.uiManager.canvasGroupsByUICanvasType[UICanvasGroupType.GAME_MENU], false);
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
            UITweaks.SetCanvasGroupActif(ObjectsReference.Instance.uiManager.canvasGroupsByUICanvasType[UICanvasGroupType.LOAD], false);
            UITweaks.SetCanvasGroupActif(ObjectsReference.Instance.uiManager.canvasGroupsByUICanvasType[UICanvasGroupType.OPTIONS], false);
            UITweaks.SetCanvasGroupActif(ObjectsReference.Instance.uiManager.canvasGroupsByUICanvasType[UICanvasGroupType.CREDITS], false);
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
