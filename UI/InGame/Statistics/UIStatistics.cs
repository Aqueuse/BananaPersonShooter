using UnityEngine;
using UnityEngine.UI;

namespace UI.InGame.Statistics {
    public class UIStatistics : MonoBehaviour {
        [SerializeField] private Slider satietySlider;
        [SerializeField] private Slider cleanlinessSlider;
        [SerializeField] private Slider happinnessSlider;

        public void Refresh_Map_Statistics(string sceneName) {
            var mapData = ObjectsReference.Instance.mapsManager.mapBySceneName[sceneName];
            
            satietySlider.value = mapData.monkeySasiety;
            cleanlinessSlider.value = mapData.cleanliness;
            happinnessSlider.value = mapData.monkeySasiety + mapData.cleanliness;
        }
    }
}
