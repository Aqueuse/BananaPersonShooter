using Game;
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
                Cinematiques.Instance.Skip();
            }

            slider.value -= Time.deltaTime;
        }

        public void AddToSlider() {
            Debug.Log("pouet");
            
            slider.value += Time.deltaTime*1.5f;
        }
    }
}
