using Data;
using Enums;
using UnityEngine;
using UnityEngine.UI;

namespace UI.InGame.Statistics {
    public class UIStatistics : MonoSingleton<UIStatistics> {
        [SerializeField] private Slider satietySlider;
        [SerializeField] private Slider cleanlinessSlider;
        [SerializeField] private Slider happinnessSlider;

        [SerializeField] private GameObject sasiety;
        [SerializeField] private GameObject cleanness;
        [SerializeField] private GameObject happiness;

        public void Refresh_Map_Statistics(MonkeyType monkeyType) {
            sasiety.SetActive(true);
            cleanness.SetActive(true);
            happiness.SetActive(true);
            
            var mapData = ScriptableObjectManager.Instance.GetMapData(monkeyType);
            satietySlider.value = mapData.monkeySasiety;
            cleanlinessSlider.value = mapData.cleanliness;
            
            happinnessSlider.value = mapData.monkeySasiety + mapData.cleanliness;
        }
    }
}
