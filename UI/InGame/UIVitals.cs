using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.InGame {
    public class UIVitals : MonoSingleton<UIVitals> {
        [SerializeField] private TextMeshProUGUI healthTextMeshProUGUI;
        [SerializeField] private TextMeshProUGUI resistanceTextMeshProUGUI;

        [SerializeField] private Image healthImageRadial;
        [SerializeField] private Image resistanceImageRadial;

        public void Set_Resistance(float resistance) {
            resistanceTextMeshProUGUI.text = resistance.ToString(CultureInfo.InvariantCulture);
            resistanceImageRadial.fillAmount = resistance/100;
        }

        public void Set_Health(float health) {
            healthTextMeshProUGUI.text = health.ToString(CultureInfo.InvariantCulture);
            healthImageRadial.fillAmount = health/100;
        }
    }
}
