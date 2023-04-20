using UnityEngine;
using UnityEngine.UI;

namespace UI {
    public class UICinematique : MonoBehaviour {
        private Slider slider;
        
        private void Start() {
            slider = GetComponent<Slider>();
        }

        private void Update() {
            if (slider.value >= 1f) {
                slider.value = 0;
                ObjectsReference.Instance.cinematiques.Skip();
            }

            slider.value -= Time.deltaTime;
        }

        public void AddToSlider() {
            slider.value += Time.deltaTime*1.5f;
        }
    }
}
