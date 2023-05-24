using Game.BananaCannonMiniGame;
using UnityEngine;

namespace UI.InGame {
    public class UIbananaCannonMiniGame : MonoBehaviour {
        [SerializeField] private GameObject pauseButton;

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
            BananaCannonMiniGameManager.Instance.PauseMiniGame();
        }
        
        public void UnpauseMiniGame() {
            BananaCannonMiniGameManager.Instance.UnpauseMiniGame();
        }
    }
}
