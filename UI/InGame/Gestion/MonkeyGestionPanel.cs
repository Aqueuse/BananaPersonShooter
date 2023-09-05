using Data.Monkeys;
using UnityEngine;
using UnityEngine.UI;

namespace UI.InGame.Gestion {
    public class MonkeyGestionPanel : MonoBehaviour {
        [SerializeField] private Slider sasietySlider;
        [SerializeField] private Slider cleanlinessSlider;
        
        public void SetDescription(MonkeyDataScriptableObject monkeyDataScriptableObject) {
            sasietySlider.value = monkeyDataScriptableObject.sasiety;
            cleanlinessSlider.value = ObjectsReference.Instance.mapsManager.currentMap.cleanliness;
        }
    }
}
