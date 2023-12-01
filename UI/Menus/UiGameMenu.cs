using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Menus {
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

        public void ReturnToGame() {
            ObjectsReference.Instance.gameManager.UnpauseGame();
            ObjectsReference.Instance.uiManager.HideGameMenu();

            ObjectsReference.Instance.inputManager.SwitchContext(InputContext.GAME);
        }
        
        public void Quit() {
            ObjectsReference.Instance.scenesSwitch.ReturnHome();
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
