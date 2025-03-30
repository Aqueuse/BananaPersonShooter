using UnityEngine;
using UnityEngine.UI;

namespace UI.InGame {
    public class UICrosshairs : MonoBehaviour {
        [SerializeField] private GenericDictionary<CrosshairType, Image> crosshairsByType;
        
        public void SetCrosshair(CrosshairType crosshairType) {
            foreach (var image in crosshairsByType) {
                image.Value.enabled = false;
            }

            crosshairsByType[crosshairType].enabled = true;
        }
    }
}
