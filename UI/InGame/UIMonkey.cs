using UnityEngine;
using UnityEngine.UI;

namespace UI.InGame {
    public class UIMonkey : MonoBehaviour {
        [SerializeField] private Slider sasietySlider;
        [SerializeField] private Slider cleanlinessSlider;
        
        public void SetSasietySliderValue(float sasiety) {
            sasietySlider.value = sasiety;
        }

        public void SetCleanlinessSliderValue(float cleanliness) {
            cleanlinessSlider.value = cleanliness;
        }
    }
}
