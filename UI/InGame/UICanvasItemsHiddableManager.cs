using System.Collections.Generic;
using System.Linq;
using Cameras;
using Settings;
using UnityEngine;

namespace UI.InGame {
    public class UICanvasItemsHiddableManager : MonoSingleton<UICanvasItemsHiddableManager> {
        private Transform _cameraTransform;
        private Vector3 _localScale;
        private float _cameraDistance;

        private List<Canvas> _canvas;

        public bool areItemsVisible;
        
        private void Start() {
            _canvas = GetComponentsInChildren<Canvas>().ToList();
            _cameraTransform = MainCamera.Instance.transform;

            areItemsVisible = GameSettings.Instance.isShowingDebris;
        }

        private void Update() {
            if (!areItemsVisible) return;
            
            foreach (var canva in _canvas) {
                var canvaTransform = canva.transform;

                _cameraDistance = Vector3.Distance(canvaTransform.position, _cameraTransform.position);

                _localScale.x = _cameraDistance / 6000f; // ugly optimisation, yeah
                _localScale.y = _cameraDistance / 6000f; 

                canvaTransform.LookAt(_cameraTransform);
                canvaTransform.localScale = _localScale;
                
                if (_cameraDistance <= 15f || _cameraDistance > 400f) {
                    canva.enabled = false;
                }

                else {
                    canva.enabled = true;
                }
            }
        }

        public void RemoveCanva(Canvas canvasToRemove) {
            _canvas.Remove(canvasToRemove);
        }

        public void SetItemsCanvasVisibility(bool isVisible) {
            areItemsVisible = isVisible;

            foreach (var canva in _canvas) {
                canva.enabled = isVisible;
            }
        }
    }
}
