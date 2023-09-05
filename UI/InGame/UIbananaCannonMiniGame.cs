using Enums;
using Game.BananaCannonMiniGame;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;

namespace UI.InGame {
    public class UIbananaCannonMiniGame : MonoBehaviour {
        [SerializeField] private GameObject pauseButton;
        public TextMeshProUGUI playButtonBananasQuantityText;

        public CanvasGroup pauseMenuCanvasGroup;
        public CanvasGroup startMenuCanvasGroup;

        private CanvasGroup _miniGameCanvasGroup;

        private void Start() {
            _miniGameCanvasGroup = GetComponent<CanvasGroup>();
        }

        public void ShowGameUI() {
            SetActive(_miniGameCanvasGroup, true);
        }

        public void HideGameUI() {
            SetActive(_miniGameCanvasGroup, false);
        }

        public void ShowStartMenu() {
            SetActive(startMenuCanvasGroup, true);
        }
        
        public void HideStartMenu() {
            SetActive(startMenuCanvasGroup, false);
        }

        public void ShowPauseMenu() {
            SetActive(pauseMenuCanvasGroup, true);
        }

        public void HidePauseMenu() {
            SetActive(pauseMenuCanvasGroup, false);
        }
        
        private void SetActive(CanvasGroup canvasGroup, bool isVisible) {
            canvasGroup.alpha = isVisible ? 1 : 0;
            canvasGroup.interactable = isVisible;
            canvasGroup.blocksRaycasts = isVisible;
            
            RefreshBananasQuantity();
        }

        public void PlayMiniGame() {
            pauseButton.SetActive(true);

            BananaCannonMiniGameManager.Instance.PlayMiniGame();
        }

        public void Teleport() {
            BananaCannonMiniGameManager.Instance.Teleport();
            HideGameUI();
        }

        public void QuitMiniGame() {
            pauseButton.SetActive(false);
            
            BananaCannonMiniGameManager.Instance.QuitMiniGame();
        }

        public void PauseMiniGame() {
            BananaCannonMiniGameManager.PauseMiniGame();
        }
        
        public void UnpauseMiniGame() {
            BananaCannonMiniGameManager.UnpauseMiniGame();
        }

        private void RefreshBananasQuantity() {
            var bananasQuantity = ObjectsReference.Instance.bananasInventory.GetQuantity(BananaType.CAVENDISH);
            
            if (bananasQuantity == 0) playButtonBananasQuantityText.text = LocalizationSettings.StringDatabase.GetLocalizedString("UI", "no_bananas");
            else {
                playButtonBananasQuantityText.text = "("+
                                                     ObjectsReference.Instance.bananasInventory.GetQuantity(BananaType.CAVENDISH)+
                                                     " "+
                                                     LocalizationSettings.StringDatabase.GetLocalizedString("UI", "bananas")+
                                                     ")";
            }
        }
    }
}
