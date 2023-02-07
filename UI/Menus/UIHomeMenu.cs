using Input;
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

        private void Start() {
            _selectedButton = 0;
             homeMenuButtons = GetComponentsInChildren<Image>();

             UIActions.Instance.selectedTrigger = homeMenuButtons[_selectedButton].GetComponent<EventTrigger>();
        }

        public void Play() {
            GameManager.Instance.Play();
        }

        public void Quit() {
            GameManager.Instance.Quit();
        }

        public void SwitchToDownButton() {
            if (_selectedButton < homeMenuButtons.Length-1) {
                _selectedButton++;
            }
            
            SetActivatedButton(homeMenuButtons[_selectedButton]);
        }

        public void SwitchToUpButton() {
            if (_selectedButton > 0) {
                _selectedButton--;
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
            UIActions.Instance.selectedTrigger = homeMenuButtons[_selectedButton].GetComponent<EventTrigger>();
        }
    }
}
