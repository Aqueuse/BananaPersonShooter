using Game;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using Image = UnityEngine.UI.Image;

namespace UI.Menus {
    public class UIHomeMenu : MonoSingleton<UIHomeMenu> {
        [SerializeField] private Color activatedColor;
        [SerializeField] private Color unactivatedColor;
        [SerializeField] private Color activatedTextColor;

        private Image[] homeMenuButtons;
        private int _selectedButton;

        public EventTrigger selectedTrigger;
        
        private void Start() {
            _selectedButton = 0;
             homeMenuButtons = GetComponentsInChildren<Image>();
        }

        public void NewGame() {
            GameManager.Instance.New_Game();
        }

        public void Quit() {
            GameManager.Instance.Quit();
        }

        public void SwitchToLeftHomeMenuButton() {
            if (_selectedButton > 0) {
                _selectedButton--;
            }
            
            SetActivatedButton(homeMenuButtons[_selectedButton]);
        }

        public void SwitchToRightHomeMenuButton() {
            if (_selectedButton < homeMenuButtons.Length-1) {
                _selectedButton++;
            }
            
            SetActivatedButton(homeMenuButtons[_selectedButton]);
        }
        
        public void SetActivatedButton(Image buttonImage) {
            foreach (var image in homeMenuButtons) {
                image.color = unactivatedColor;
                image.GetComponentInChildren<TextMeshProUGUI>().color = activatedColor;
            }

            buttonImage.color = activatedColor;
            buttonImage.GetComponentInChildren<TextMeshProUGUI>().color = activatedTextColor;

            selectedTrigger = buttonImage.GetComponent<EventTrigger>();
        }
    }
}
