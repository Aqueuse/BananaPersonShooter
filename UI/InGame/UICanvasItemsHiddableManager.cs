using System.Collections.Generic;
using System.Linq;
using Building;
using Game;
using UnityEngine;
using UnityEngine.UI;

namespace UI.InGame {
    public class UICanvasItemsHiddableManager : MonoBehaviour {
        [SerializeField] private RectTransform canvasBananaTree;
        [SerializeField] private RectTransform[] canvasMonkeys;
        public List<Canvas> _debrisCanvasList;
        
        private Transform _cameraTransform;
        private Vector3 _localScale;
        private float _cameraDistance;
        
        public bool isBananaTreeVisible;
        public bool areDebrisVisible;
        public bool areMonkeysVisible;
        
        private void Start() {
            _debrisCanvasList = MapItems.Instance.aspirablesContainer.GetComponentsInChildren<Canvas>().ToList();
            _cameraTransform = ObjectsReference.Instance.mainCamera.transform;

            areDebrisVisible = ObjectsReference.Instance.gameSettings.isShowingDebris;
            areMonkeysVisible = ObjectsReference.Instance.gameSettings.isShowingMonkeys;
            isBananaTreeVisible = ObjectsReference.Instance.gameSettings.isShowingBananaTrees;
        }

        private void Update() {
            if (isBananaTreeVisible) {
                _cameraDistance = Vector3.Distance(canvasBananaTree.transform.position, _cameraTransform.position);

                _localScale.x = _cameraDistance / 6000f; // ugly optimisation, yeah
                _localScale.y = _cameraDistance / 6000f;

                canvasBananaTree.LookAt(_cameraTransform);
                canvasBananaTree.localScale = _localScale;
            }

            if (areMonkeysVisible) {
                foreach (var canvasMonkey in canvasMonkeys) {
                    _cameraDistance = Vector3.Distance(canvasMonkey.position, _cameraTransform.position);

                    _localScale.x = _cameraDistance / 6000f; // ugly optimisation, yeah
                    _localScale.y = _cameraDistance / 6000f;
                    
                    canvasMonkey.LookAt(_cameraTransform);
                    canvasMonkey.localScale = _localScale;
                }
            }

            if (areDebrisVisible) {
                var cameraPosition = _cameraTransform.position;
                
                foreach (var canva in _debrisCanvasList) {
                    if (canva != null) {
                        var canvaTransform = canva.transform;
                
                        _cameraDistance = Vector3.Distance(canvaTransform.position, cameraPosition);
                        canvaTransform.LookAt(_cameraTransform);
                        
                        if (_cameraDistance < 30f) {
                            canva.GetComponentInChildren<Image>().GetComponent<RectTransform>().sizeDelta = new Vector2(_cameraDistance/5, _cameraDistance/5);
                        }
                    }
                }
            }
        }

        public void RemoveCanva(Canvas canvasToRemove) {
            _debrisCanvasList.Remove(canvasToRemove);
        }

        public void SetDebrisCanvasVisibility(bool isVisible) {
            if (Map.GetDebrisQuantity() > 0) {
                _debrisCanvasList = MapItems.Instance.aspirablesContainer.GetComponentsInChildren<Canvas>().ToList();
                
                foreach (var canva in _debrisCanvasList) {
                    canva.enabled = isVisible;
                    canva.GetComponent<CanvasGroup>().alpha = 1;
                }

                areDebrisVisible = isVisible;
            }
        }

        public void SetMonkeysVisibility(bool isVisible) {
            areMonkeysVisible = isVisible;

            foreach (var canvas in canvasMonkeys) {
                canvas.GetComponent<Canvas>().enabled = isVisible;
            }
        }
        
        public void SetBananaTreeVisibility(bool isVisible) {
            isBananaTreeVisible = isVisible;
            if (canvasBananaTree != null) canvasBananaTree.GetComponent<Canvas>().enabled = isVisible;
        }
    }
}
