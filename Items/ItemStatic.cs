using Enums;
using UI.InGame;
using UnityEngine;

namespace Items {
    public class ItemStatic : MonoBehaviour {
        public ItemStaticType itemStaticType;

        private UICanvasItemsStatic _canvasItemsStatic;

        public bool _isActive;

        private void Start() {
            _canvasItemsStatic = GetComponentInChildren<UICanvasItemsStatic>();
        }

        public void Activate() {
            if (_isActive) return;

            ItemsManager.Instance.HideAllItemsStatics();
            _canvasItemsStatic.ShowUI();
            _isActive = true;
        }

        public void Desactivate() {
            _isActive = false;
            
            _canvasItemsStatic.HideUI();
        }
    }
}
