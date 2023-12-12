using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.InGame.Gestion.blocks {
    public class OneItemSliderBlock : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI sliderName;
        [SerializeField] private Slider _slider;

        public void SetBlock(string sliderName, float value, int maxValue) {
            this.sliderName.text = sliderName;
            _slider.maxValue = maxValue;
            _slider.value = value;
        }
    }
}
