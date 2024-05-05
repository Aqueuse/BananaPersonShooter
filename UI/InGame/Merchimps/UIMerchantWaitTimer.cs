using TMPro;
using UnityEngine;

namespace UI.InGame.Merchimps {
    public class UIMerchantWaitTimer : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI waitTimerText;

        public void SetTimer(int time) {
            waitTimerText.text = time + "s";
        }
    }
}
