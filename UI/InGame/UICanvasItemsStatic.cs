using UnityEngine;

namespace UI.InGame {
    public class UICanvasItemsStatic : MonoBehaviour {
        [SerializeField] private GameObject uIinGame;
        [SerializeField] private GameObject icon;
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
            _cameraTransform = ObjectsReference.Instance.mainCamera.transform;

            uiInGameCanvasGroup = uIinGame.GetComponent<CanvasGroup>();
        }

        private void Update() {
            if (!canRotate) return;
            
            _transform.LookAt(_cameraTransform);
            
            _cameraDistance = Vector3.Distance(_transform.position, _cameraTransform.position);

            if (_cameraDistance < 15f) {
                _canvas.enabled = true;
            }

            else {
                _canvas.enabled = false;
            }
        }

        public void ShowUI() {
            uiInGameCanvasGroup.alpha = 1;
            icon.SetActive(false);
        }

        public void HideUI() {
            uiInGameCanvasGroup.alpha = 0;
            icon.SetActive(true);
        }
    }
}
