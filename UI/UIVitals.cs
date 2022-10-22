using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
    public class UIVitals : MonoSingleton<UIVitals> {
        [SerializeField] private TextMeshProUGUI healthTextMeshProUGUI;
        [SerializeField] private TextMeshProUGUI resistanceTextMeshProUGUI;

        [SerializeField] private Image healthImageRadial;
        [SerializeField] private Image resistanceImageRadial;

        public void Set_Resistance(int resistance) {
            resistanceTextMeshProUGUI.text = resistance.ToString();
            resistanceImageRadial.fillAmount = resistance;
        }

        public void Set_Health(int health) {
            healthTextMeshProUGUI.text = health.ToString();
            healthImageRadial.fillAmount = health;
        }
    }
}
