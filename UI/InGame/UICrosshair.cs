using Enums;
using UnityEngine;
using UnityEngine.UI;

namespace UI.InGame {
    public class UICrosshair : MonoBehaviour {
        [SerializeField] private GenericDictionary<ItemType, Image> crosshairsByItemType;
        
        private CanvasGroup _canvasGroup;

        private void Start() {
            _canvasGroup = GetComponent<CanvasGroup>();
        }
    
        public void SetCrosshair(ItemType itemType) {
            foreach (var crosshair in crosshairsByItemType) {
                crosshair.Value.enabled = false;
            }

            crosshairsByItemType[itemType].enabled = true;
        }

        public void ShowHideCrosshairs(bool isVisible) {
            _canvasGroup.alpha = isVisible ? 1 : 0;
        }
    }
}
