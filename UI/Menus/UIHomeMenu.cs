using TMPro;
using UnityEngine;
using Image = UnityEngine.UI.Image;

namespace UI.Menus {
    public class UIHomeMenu : MonoBehaviour {
        [SerializeField] private Color activatedColor;
        [SerializeField] private Color unactivatedColor;
        [SerializeField] private Color activatedTextColor;

        [SerializeField] private Image[] _homeMenuButtons;
        
        public GameObject firstSelectedButton;
        
        public void SetActivatedButton(Image buttonImage) {
            foreach (var image in _homeMenuButtons) {
                image.color = unactivatedColor;
                image.GetComponentInChildren<TextMeshProUGUI>().color = activatedColor;
            }

            buttonImage.color = activatedColor;
            buttonImage.GetComponentInChildren<TextMeshProUGUI>().color = activatedTextColor;
        }

        public void GoToDiscord() {
            Application.OpenURL("https://discord.gg/2SUscY39dw");
        }
    }
}
