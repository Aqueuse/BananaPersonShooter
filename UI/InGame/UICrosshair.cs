using Enums;
using UnityEngine;
using UnityEngine.UI;

namespace UI.InGame {
    public class UICrosshair : MonoSingleton<UICrosshair> {
        [SerializeField] private GenericDictionary<BananaType, Image> crosshairs = new GenericDictionary<BananaType, Image>();
        private CanvasGroup _canvasGroup;

        private void Start() {
            _canvasGroup = GetComponent<CanvasGroup>();
        }
    
        public void SetCrosshair(BananaType bananaType) {
            foreach (var crosshair in crosshairs) {
                crosshair.Value.enabled = false;
            }

            crosshairs[bananaType].enabled = true;
        }

        public void ShowHideCrosshairs(bool isVisible) {
            _canvasGroup.alpha = isVisible ? 1 : 0;
        }
    }
}
