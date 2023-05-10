using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Tutorials {
    public class UiTutorialButton : MonoBehaviour {
        [SerializeField] private GameObject associatedTutorial;

        [SerializeField] private Color yellow;
        [SerializeField] private Color black;

        private Image _buttonBackgroundImage;
        private TextMeshProUGUI _buttonText;
        
        private void Start() {
            _buttonBackgroundImage = GetComponent<Image>();
            _buttonText = GetComponentInChildren<TextMeshProUGUI>();
        }

        public void ShowTutorial() {
            associatedTutorial.SetActive(true);

            _buttonBackgroundImage.color = yellow;
            _buttonText.color = black;
        }

        public void HideTutorial() {
            associatedTutorial.SetActive(false);

            _buttonBackgroundImage.color = black;
            _buttonText.color = yellow;
        }
    }
}
