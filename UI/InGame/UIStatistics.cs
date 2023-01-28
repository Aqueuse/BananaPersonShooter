using UnityEngine;
using UnityEngine.UI;

namespace UI.InGame {
    public class UIStatistics : MonoSingleton<UIStatistics> {
        [SerializeField] private Slider satietySlider;
        [SerializeField] private Slider cleannessSlider;
        [SerializeField] private Image satietyImage;
        [SerializeField] private Image cleannessImage;
    
        public void Refresh_Map_Statistics(float sasiety, float cleanness) {
            satietySlider.value = sasiety;
            cleannessSlider.value = cleanness;

            satietyImage.rectTransform.sizeDelta = new Vector2(sasiety, 20.373f);
            cleannessImage.rectTransform.sizeDelta = new Vector2(cleanness, 20.373f);
        }
    }
}
