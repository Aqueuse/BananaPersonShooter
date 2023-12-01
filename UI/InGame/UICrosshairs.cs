using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace UI.InGame {
    public class UICrosshairs : MonoBehaviour {
        [SerializeField] private GenericDictionary<BananaType, Image> bananaCrosshairsByItemType;
        
        private List<Image> _crosshairsImage;

        private void Start() {
            _crosshairsImage = GetComponentsInChildren<Image>().ToList();
        }
    
        public void SetCrosshair(BananaType bananaType) {
            HideCrosshairs();

            bananaCrosshairsByItemType[bananaType].enabled = true;
        }

        private void HideCrosshairs() {
            foreach (var image in _crosshairsImage) {
                image.enabled = false;
            }
        }
    }
}
