using Game;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Menus {
    public class UiGameMenu : MonoSingleton<UiGameMenu> {
        [SerializeField] private Color activatedColor;
        [SerializeField] private Color unactivatedColor;
        [SerializeField] private Color activatedTextColor;

        private Image[] gameMenuButtons;
        private int _selectedButton;

        public EventTrigger selectedTrigger;

        private void Start() {
            _selectedButton = 0;
            gameMenuButtons = GetComponentsInChildren<Image>();
        }
        
        public void Quit() {
            ScenesSwitch.Instance.ReturnHome();
        }

        public void SwitchToRightButton() {
            if (_selectedButton < gameMenuButtons.Length-1) {
                _selectedButton++;
            }
            
            SetActivatedButton(gameMenuButtons[_selectedButton]);
        }

        public void SwitchToLeftButton() {
            if (_selectedButton > 0) {
                _selectedButton--;
            }
            
            SetActivatedButton(gameMenuButtons[_selectedButton]);
        }

        public void SetActivatedButton(Image buttonImage) {
            foreach (var image in gameMenuButtons) {
                image.color = unactivatedColor;
                image.GetComponentInChildren<TextMeshProUGUI>().color = activatedColor;
            }

            buttonImage.color = activatedColor;
            buttonImage.GetComponentInChildren<TextMeshProUGUI>().color = activatedTextColor;
            
            selectedTrigger = buttonImage.GetComponent<EventTrigger>();
        }
    }
}
