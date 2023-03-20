using Enums;
using Input;
using UI.InGame;
using UnityEngine;

namespace Items {
    public class ItemStatic : MonoBehaviour {
        public ItemStaticType itemStaticType;

        private UICanvasItemsStatic _canvasItemsStatic;
        private bool _isActive;

        private void Start() {
            _canvasItemsStatic = GetComponentInChildren<UICanvasItemsStatic>();
        }
        
        public void Activate() {
            if (_isActive) return;
            
            UIinGameManager.Instance.HideAllUIsinGame();
            _canvasItemsStatic.ShowUI();
            if (itemStaticType == ItemStaticType.UIMAP) GetComponent<UIMapActions>().enabled = true;

            _isActive = true;
        }

        public void Desactivate() {
            UIinGameManager.Instance.HideAllUIsinGame();
            if (itemStaticType == ItemStaticType.UIMAP) GetComponent<UIMapActions>().enabled = false;

            _isActive = false;
        }
    }
}
