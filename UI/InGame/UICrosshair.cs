using Enums;
using UnityEngine;
using UnityEngine.UI;

namespace UI.InGame {
    public class UICrosshair : MonoSingleton<UICrosshair> {
        [SerializeField] private GenericDictionary<ItemThrowableType, Image> crosshairsByItemType;
        
        private CanvasGroup _canvasGroup;

        private void Start() {
            _canvasGroup = GetComponent<CanvasGroup>();
        }
    
        public void SetCrosshair(ItemThrowableType itemThrowableType) {
            foreach (var crosshair in crosshairsByItemType) {
                crosshair.Value.enabled = false;
            }

            crosshairsByItemType[itemThrowableType].enabled = true;
        }

        public void ShowHideCrosshairs(bool isVisible) {
            _canvasGroup.alpha = isVisible ? 1 : 0;
        }
    }
}
