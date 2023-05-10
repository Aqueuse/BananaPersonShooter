using System.Collections.Generic;
using System.Linq;
using Enums;
using UnityEngine;
using UnityEngine.UI;

namespace UI.InGame {
    public class UICrosshair : MonoBehaviour {
        [SerializeField] private GenericDictionary<ItemType, Image> bananaCrosshairsByItemType;
        [SerializeField] private GenericDictionary<ItemCategory, Image> crosshairsByItemCategory;
        
        private CanvasGroup _canvasGroup;

        private List<Image> _crosshairsImage;

        private void Start() {
            _canvasGroup = GetComponent<CanvasGroup>();
            _crosshairsImage = GetComponentsInChildren<Image>().ToList();
        }
    
        public void SetCrosshair(ItemCategory itemTypeCategory, ItemType itemType) {
            HideCrosshairs();
            
            if (itemTypeCategory == ItemCategory.BANANA) {
                bananaCrosshairsByItemType[itemType].enabled = true;
            }

            else {
                crosshairsByItemCategory[itemTypeCategory].enabled = true;
            }
        }

        private void HideCrosshairs() {
            foreach (var image in _crosshairsImage) {
                image.enabled = false;
            }
        }

        public void ShowHideCrosshairs(bool isVisible) {
            _canvasGroup.alpha = isVisible ? 1 : 0;
        }
    }
}
