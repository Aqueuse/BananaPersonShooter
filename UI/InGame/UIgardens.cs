using UnityEngine;
using UnityEngine.UI;

namespace UI.InGame {
    public class UIgardens : MonoBehaviour {
        [SerializeField] private GenericDictionary<string, Slider[]> slidersByMapName;

        private void Start() {
            SetSlidersState();
        }

        private void SetSlidersState() {
            var maps = ObjectsReference.Instance.mapsManager.mapBySceneName;
            
            foreach (var sliders in slidersByMapName) {
                sliders.Value[0].value = maps[sliders.Key].mapDataScriptableObject.monkeyDataScriptableObject.sasiety;
                sliders.Value[1].value = maps[sliders.Key].mapDataScriptableObject.cleanliness;
            }
        }
    }
}
