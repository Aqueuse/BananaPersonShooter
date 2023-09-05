using System.Collections.Generic;
using System.Linq;
using Enums;
using UnityEngine;
using UnityEngine.UI;

namespace UI.InGame {
    public class UICrosshairs : MonoBehaviour {
        [SerializeField] private GenericDictionary<BananaType, Image> bananaCrosshairsByItemType;
        [SerializeField] private GenericDictionary<ItemCategory, Image> crosshairsByItemCategory;
        
        private List<Image> _crosshairsImage;

        private void Start() {
            _crosshairsImage = GetComponentsInChildren<Image>().ToList();
        }
    
        public void SetCrosshair(ItemCategory itemTypeCategory, BananaType bananaType) {
            HideCrosshairs();
            
            if (itemTypeCategory == ItemCategory.BANANA) {
                bananaCrosshairsByItemType[bananaType].enabled = true;
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
    }
}
