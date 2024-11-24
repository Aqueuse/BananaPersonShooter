using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace UI.InGame {
    public class UICrosshairs : MonoBehaviour {
        [SerializeField] private GenericDictionary<DroppedType, Image> crosshairsByDroppedType;
        
        private List<Image> _crosshairsImage;

        private void Start() {
            _crosshairsImage = GetComponentsInChildren<Image>().ToList();
        }
    
        public void SetCrosshair(DroppedType droppedType) {
            HideCrosshairs();

            crosshairsByDroppedType[droppedType].enabled = true;
        }

        private void HideCrosshairs() {
            foreach (var image in _crosshairsImage) {
                image.enabled = false;
            }
        }
    }
}
