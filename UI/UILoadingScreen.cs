using UnityEngine;
using UnityEngine.UI;

namespace UI {
    public class UILoadingScreen : MonoBehaviour {
        private Image _loadingImage;
        [SerializeField] private Sprite[] sprites;

        private float _animationCounter;
        private int _currentFrame;
        private int _spritesNumber;
    
        void Start() {
            _loadingImage = GetComponent<Image>();
            _spritesNumber = sprites.Length;
        }

        void Update() {
            _animationCounter += Time.deltaTime;

            if (_animationCounter >= 0.5f) {
                _animationCounter = 0f;
                _loadingImage.sprite = sprites[_currentFrame];
                _currentFrame+=1;
            }

            if (_currentFrame >= _spritesNumber) _currentFrame = 0;
        }
    }
}
