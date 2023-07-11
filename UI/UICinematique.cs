using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI {
    public class UICinematique : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {
        private Slider _slider;
        private bool isAdding;

        private void Start() {
            _slider = GetComponent<Slider>();
        }

        private void Update() {
            if (isAdding) AddToSlider();
            
            if (_slider.value >= 1f) {
                _slider.value = 0;
                ObjectsReference.Instance.cinematiques.Skip();
            }

            _slider.value -= Time.deltaTime;
        }

        public void AddToSlider() {
            _slider.value += Time.deltaTime*1.5f;
        }

        public void OnPointerDown(PointerEventData eventData) {
            isAdding = true;
        }

        public void OnPointerUp(PointerEventData eventData) {
            isAdding = false;
        }
    }
}
