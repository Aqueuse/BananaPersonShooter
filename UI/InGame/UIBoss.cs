using UnityEngine;
using UnityEngine.UI;

namespace UI.InGame {
    public class UIBoss : MonoBehaviour {

        public void Add_Satiety(float satietyValue) {
            GetComponent<Slider>().value = satietyValue;
        }

        public void Show_Boss_Life_Slider() {
            GetComponent<CanvasGroup>().alpha = 1f;
        }

        public void Hide_Boss_Life_Slider() {
            GetComponent<CanvasGroup>().alpha = 0f;
        }

        public void SetMaxSatiety(float maxSatiety) {
            GetComponent<Slider>().maxValue = maxSatiety;
        }
    }
}
