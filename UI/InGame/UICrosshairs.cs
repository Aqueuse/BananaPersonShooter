using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace UI.InGame {
    public class UICrosshairs : MonoBehaviour {
        [SerializeField] private GenericDictionary<ItemType, Image> bananaCrosshairsByItemType;
        [SerializeField] private GenericDictionary<ItemCategory, Image> crosshairsByItemCategory;
        
        private List<Image> _crosshairsImage;

        private void Start() {
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
    }
}
