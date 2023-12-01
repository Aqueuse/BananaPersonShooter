using UnityEngine;

namespace UI {
    public enum FadeState {
        FadeIn,
        FadeOut
    }
    
    public class UICanvasFadeInOut : MonoBehaviour {
        private CanvasGroup _canvasGroup;
        private FadeState _fadeState;

        private void Start() {
            _canvasGroup = GetComponent<CanvasGroup>();
        }
        
        private void Update() {
            if (_canvasGroup.alpha >= 1)
                _fadeState = FadeState.FadeOut;

            if (_canvasGroup.alpha < 0) {
                _fadeState = FadeState.FadeIn;
                _canvasGroup.alpha = 0;
                enabled = false;
            }
            
            if (_fadeState == FadeState.FadeIn) {
                _canvasGroup.alpha += 0.01f;
                return;
            }

            if (_fadeState == FadeState.FadeOut) {
                _canvasGroup.alpha -= 0.01f;
            }
        }
    }
}
