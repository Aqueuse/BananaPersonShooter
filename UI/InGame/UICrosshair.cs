using Enums;
using UnityEngine;
using UnityEngine.UI;

namespace UI.InGame {
    public class UICrosshair : MonoSingleton<UICrosshair> {
        [SerializeField] private GenericDictionary<ItemThrowableType, Image> crosshairs;
        private CanvasGroup _canvasGroup;
        private Color crosshairColor;

        private void Start() {
            _canvasGroup = GetComponent<CanvasGroup>();
        }
    
        public void SetCrosshair(ItemThrowableType itemThrowableType) {
            foreach (var crosshair in crosshairs) {
                crosshairColor = crosshair.Value.color;
                crosshairColor.a = 0f;
                crosshair.Value.color = crosshairColor;
            }

            crosshairColor.a = 1f;
            crosshairs[itemThrowableType].color = crosshairColor;
        }

        public void ShowHideCrosshairs(bool isVisible) {
            _canvasGroup.alpha = isVisible ? 1 : 0;
        }
    }
}
