using Cameras;
using UnityEngine;

namespace UI.InGame {
    public class UICanvasItemBananaTree : MonoSingleton<UICanvasItemBananaTree> {
        private Transform _canvasTransform;
        
        private Transform _cameraTransform;
        private Vector3 _localScale;
        private float _cameraDistance;
        
        public bool isItemVisible;
        
        private void Start() {
            _canvasTransform = GetComponent<Transform>();
            _cameraTransform = MainCamera.Instance.transform;
        }

        private void Update() {
            if (!isItemVisible) return;
            
            var mainCameraPosition = _cameraTransform.position;

            _cameraDistance = Vector3.Distance(transform.position, mainCameraPosition);
            
            _localScale.x = _cameraDistance / 6000f; // ugly optimisation, yeah
            _localScale.y = _cameraDistance / 6000f; 
            
            _canvasTransform.LookAt(mainCameraPosition);
            _canvasTransform.localScale = _localScale;
        }
    }
}
