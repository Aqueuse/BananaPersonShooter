using TMPro;
using UnityEngine;

namespace UI.InGame {
    public class UIBlinkText : MonoBehaviour {
        private TextMeshProUGUI _textMeshProUGUI;
        private bool _isVisible = true;
        private void Start() {
            _textMeshProUGUI = GetComponent<TextMeshProUGUI>();
        }

        private void Awake() {
            InvokeRepeating(nameof(Blink), 0, 0.4f);
        }

        void Blink() {
            if (_isVisible) {
                _textMeshProUGUI.alpha = 0;
                _isVisible = false;
                return;
            }
        
            _textMeshProUGUI.alpha = 1;
            _isVisible = true;
        }
    }
}