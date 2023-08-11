using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UI.InGame {
    public class UICanvasItemsHiddableManager : MonoBehaviour {
        [SerializeField] private Transform bananaTreeSpriteTransform;
        [SerializeField] private RectTransform[] canvasMonkeys;
        private List<GameObject> debrisGameObjects;
        public List<SpriteRenderer> _debrisSpriteRendererList;
        
        private Transform _cameraTransform;
        private Vector3 _localScale;
        private float _cameraDistance;
        
        public bool isBananaTreeVisible;
        public bool areDebrisVisible;
        public bool areMonkeysVisible;
        
        private void Start() {
            RefreshDebrisSpriteRendererList();
            _cameraTransform = ObjectsReference.Instance.mainCamera.transform;

            areDebrisVisible = ObjectsReference.Instance.gameSettings.isShowingDebris;
            areMonkeysVisible = ObjectsReference.Instance.gameSettings.isShowingMonkeys;
            isBananaTreeVisible = ObjectsReference.Instance.gameSettings.isShowingBananaTrees;
        }

        private void Update() {
            if (isBananaTreeVisible) {
                _cameraDistance = Vector3.Distance(bananaTreeSpriteTransform.transform.position, _cameraTransform.position);

                _localScale.x = _cameraDistance / 6000f; // ugly optimisation, yeah
                _localScale.y = _cameraDistance / 6000f;

                bananaTreeSpriteTransform.LookAt(_cameraTransform);
                bananaTreeSpriteTransform.localScale = _localScale;
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
                
                foreach (var spriteRenderer in _debrisSpriteRendererList) {
                    if (spriteRenderer != null) {
                        var canvaTransform = spriteRenderer.transform;
                
                        _cameraDistance = Vector3.Distance(canvaTransform.position, cameraPosition);
                        canvaTransform.LookAt(_cameraTransform);
                        
                        if (_cameraDistance < 30f) {
                            spriteRenderer.GetComponent<Transform>().localScale = new Vector2(_cameraDistance/40, _cameraDistance/40);
                        }
                    }
                }
            }
        }

        public void RemoveSpriteRenderer(SpriteRenderer spriteRenderer) {
            _debrisSpriteRendererList.Remove(spriteRenderer);
        }

        private void RefreshDebrisSpriteRendererList() {
            debrisGameObjects = GameObject.FindGameObjectsWithTag("Debris").ToList();
            _debrisSpriteRendererList = new List<SpriteRenderer>();
            foreach (var debrisGameObject in debrisGameObjects) {
                _debrisSpriteRendererList.Add(debrisGameObject.GetComponentInChildren<SpriteRenderer>());
            }
        }

        public void SetDebrisSpriteRendererVisibility(bool isVisible) {
            if (ObjectsReference.Instance.mapsManager.currentMap.GetDebrisQuantity() > 0) {
                RefreshDebrisSpriteRendererList();
                
                foreach (var spriteRenderer in _debrisSpriteRendererList) {
                    spriteRenderer.enabled = isVisible;
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
            if (bananaTreeSpriteTransform != null) bananaTreeSpriteTransform.GetComponent<SpriteRenderer>().enabled = isVisible;
        }
    }
}
