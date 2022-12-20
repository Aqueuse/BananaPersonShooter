using TMPro;
using UnityEngine;

namespace UI {
    public class UIBlinkText : MonoBehaviour {
        private TextMeshProUGUI _textMeshProUGUI;
        private bool isVisible = true;
        private void Start() {
            _textMeshProUGUI = GetComponent<TextMeshProUGUI>();
        }

        private void Awake() {
            InvokeRepeating(nameof(Blink), 0, 0.4f);
        }

        void Blink() {
            if (isVisible) {
                _textMeshProUGUI.alpha = 0;
                isVisible = false;
                return;
            }
        
            _textMeshProUGUI.alpha = 1;
            isVisible = true;
        }
    }
}