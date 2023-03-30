using Cameras;
using Enums;
using Game;
using Input;
using UnityEngine;

namespace UI.InGame {
    public class UICanvasItemsStatic : MonoBehaviour {
        [SerializeField] private GameObject uIinGame;
        [SerializeField] private bool canRotate;

        private Canvas _canvas;
        private Transform _transform;
        
        private Transform _cameraTransform;
        private Vector3 _localScale;
        private float _cameraDistance;

        private CanvasGroup uiInGameCanvasGroup;

        private void Start() {
            _canvas = GetComponent<Canvas>();
            _transform = GetComponent<Transform>();
            _cameraTransform = MainCamera.Instance.transform;

            uiInGameCanvasGroup = uIinGame.GetComponent<CanvasGroup>();
        }

        private void Update() {
            if (!canRotate) return;
            
            _transform.LookAt(_cameraTransform);
            
            _cameraDistance = Vector3.Distance(_transform.position, _cameraTransform.position);
            
            _canvas.enabled = !(_cameraDistance > 15f);
        }

        public void ShowUI() {
            uiInGameCanvasGroup.alpha = 1;
        }

        public void HideUI() {
            uiInGameCanvasGroup.alpha = 0;
        }
    }
}
