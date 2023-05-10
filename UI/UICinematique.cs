using UnityEngine;
using UnityEngine.UI;

namespace UI {
    public class UICinematique : MonoBehaviour {
        private Slider _slider;
        
        private void Start() {
            _slider = GetComponent<Slider>();
        }

        private void Update() {
            if (_slider.value >= 1f) {
                _slider.value = 0;
                ObjectsReference.Instance.cinematiques.Skip();
            }

            _slider.value -= Time.deltaTime;
        }

        public void AddToSlider() {
            _slider.value += Time.deltaTime*1.5f;
        }
    }
}
