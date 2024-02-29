using TMPro;
using UnityEngine;

namespace UI.InGame.SpaceTrafficControlMiniGame {
    public class UIDistantDot : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI spaceshipNameText;
        [SerializeField] private TextMeshProUGUI distanceText;

        public void NameSpaceship(string spaceshipName) {
            spaceshipNameText.text = spaceshipName;
        }
    }
}
