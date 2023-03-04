using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Tutorials {
    public class UiTutorialButton : MonoBehaviour {
        [SerializeField] private GameObject associatedTutorial;

        [SerializeField] private Color yellow;
        [SerializeField] private Color black;

        private Image buttonBackgroundImage;
        private TextMeshProUGUI buttonText;
        
        private void Start() {
            buttonBackgroundImage = GetComponent<Image>();
            buttonText = GetComponentInChildren<TextMeshProUGUI>();
        }

        public void ShowTutorial() {
            associatedTutorial.SetActive(true);

            buttonBackgroundImage.color = yellow;
            buttonText.color = black;
        }

        public void HideTutorial() {
            associatedTutorial.SetActive(false);

            buttonBackgroundImage.color = black;
            buttonText.color = yellow;
        }
    }
}
