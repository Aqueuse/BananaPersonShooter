using Enums;
using Player;
using UnityEngine;
using UnityEngine.UI;

namespace UI.InGame {
    public class UIMonkey : MonoBehaviour {
        [SerializeField] private Image fillImage;
        [SerializeField] private GenericDictionary<MonkeyState, Color> monkeyStatetoColorFill;

        private Vector3 bananaManXY;
        private Transform bananaManTransform;

        private void Start() {
            bananaManTransform = BananaMan.Instance.transform;
        }

        private void Update() {
            var bananaManPosition = bananaManTransform.position;
            transform.LookAt(new Vector3(bananaManPosition.x, transform.position.y, bananaManPosition.z));
        }

        public void SetSliderValue(float sliderValue, MonkeyState monkeyState) {
            GetComponentInChildren<Slider>().value = sliderValue;

            fillImage.color = monkeyStatetoColorFill[monkeyState];
        }
        
        public void SetMaxSatiety(float maxSatiety) {
            GetComponentInChildren<Slider>().maxValue = maxSatiety;
        }
    }
}
