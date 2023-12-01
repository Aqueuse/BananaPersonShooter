using TMPro;
using UnityEngine;

namespace UI.InGame.CommandRoomControlPanels {
    public class UIbananaCannonMiniGame : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI bottomBananasQuantityText;
        [SerializeField] private CanvasGroup _miniGameCanvasGroup;

        public void RefreshBananasQuantity(BananaType bananaType) {
            bottomBananasQuantityText.text = ObjectsReference.Instance.bananasInventory.GetQuantity(bananaType).ToString();
        }

        public void ShowGameUI() {
            SetActive(_miniGameCanvasGroup, true);
        }

        public void HideGameUI() {
            SetActive(_miniGameCanvasGroup, false);
        }
        
        private void SetActive(CanvasGroup canvasGroup, bool isVisible) {
            canvasGroup.alpha = isVisible ? 1 : 0;
            canvasGroup.interactable = isVisible;
            canvasGroup.blocksRaycasts = isVisible;
        }
    }
}
