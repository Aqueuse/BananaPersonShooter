using Game;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using Image = UnityEngine.UI.Image;

namespace UI.Menus {
    public class UIHomeMenu : MonoBehaviour {
        [SerializeField] private Color activatedColor;
        [SerializeField] private Color unactivatedColor;
        [SerializeField] private Color activatedTextColor;

        private Image[] _homeMenuButtons;
        private int _selectedButton;

        public EventTrigger selectedTrigger;
        
        private void Start() {
            _selectedButton = 0;
             _homeMenuButtons = GetComponentsInChildren<Image>();
        }

        public void NewGame() {
            GameManager.Prepare_New_Game();
        }

        public void Quit() {
            ObjectsReference.Instance.gameManager.Quit();
        }

        public void SwitchToLeftHomeMenuButton() {
            if (_selectedButton > 0) {
                _selectedButton--;
            }
            
            SetActivatedButton(_homeMenuButtons[_selectedButton]);
        }

        public void SwitchToRightHomeMenuButton() {
            if (_selectedButton < _homeMenuButtons.Length-1) {
                _selectedButton++;
            }
            
            SetActivatedButton(_homeMenuButtons[_selectedButton]);
        }
        
        public void SetActivatedButton(Image buttonImage) {
            foreach (var image in _homeMenuButtons) {
                image.color = unactivatedColor;
                image.GetComponentInChildren<TextMeshProUGUI>().color = activatedColor;
            }

            buttonImage.color = activatedColor;
            buttonImage.GetComponentInChildren<TextMeshProUGUI>().color = activatedTextColor;

            selectedTrigger = buttonImage.GetComponent<EventTrigger>();
        }
    }
}
