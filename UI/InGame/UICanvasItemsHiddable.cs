using Cameras;
using Game;
using UnityEngine;
using UnityEngine.UI;

namespace UI.InGame {
    enum CanvasHiddableType {
        DEBRIS = 0,
        BANANA_TREE = 1
    }
    
    public class UICanvasItemsHiddable : MonoBehaviour {
        [SerializeField] private CanvasHiddableType canvasHiddableType;
        
        private Image _itemImage;
        private Transform _canvasTransform;
        
        private Transform _cameraTransform;
        private Vector3 _localScale;
        private float _cameraDistance;
        
        private void Start() {
            _itemImage = GetComponentInChildren<Image>();
            _canvasTransform = GetComponent<Transform>();
            _cameraTransform = MainCamera.Instance.transform;
        }

        public void SetVisibility() {
            if (canvasHiddableType == CanvasHiddableType.DEBRIS && !MapsManager.Instance.currentMap.isShowingDebris) {
                GetComponent<UICanvasItemsHiddable>().enabled = false;
                GetComponent<Canvas>().enabled = false;
            }
            
            if (canvasHiddableType == CanvasHiddableType.BANANA_TREE && !MapsManager.Instance.currentMap.isShowingBananaTrees) {
                GetComponent<UICanvasItemsHiddable>().enabled = false;
                GetComponent<Canvas>().enabled = false;
            }
        }

        private void Update() {
            var mainCameraPosition = _cameraTransform.position;
            _canvasTransform.LookAt(mainCameraPosition);
            
            _cameraDistance = Vector3.Distance(transform.position, mainCameraPosition);
            
            _localScale.x = _cameraDistance / 6000f; // ugly optimisation, yeah
            _localScale.y = _cameraDistance / 6000f; 
            _canvasTransform.localScale = _localScale;
            
            _itemImage.enabled = !(_cameraDistance < 30f);
        }
    }
}
