using UnityEngine;
using UnityEngine.UI;

namespace UI.InGame {
    public class UIMonkey : MonoBehaviour {
        [SerializeField] private Image fillImage;
        
        public void SetSliderValue(Color colorFill) {
            fillImage.color = colorFill;
        }
    }
}
