using UI.Menus;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
    public class UIActionButton : MonoBehaviour {
        private Image backgroundImage;
        
        private Color activatedColor;
        private Color unactivatedColor;

        private RectTransform rectTransform;

        private UIautoscrollList _uIautoscrollList;
            
        private void Start() {
            activatedColor = ObjectsReference.Instance.uiOptionsMenu.buttonActivatedColor;
            unactivatedColor = ObjectsReference.Instance.uiOptionsMenu.buttonUnactivatedColor;

            backgroundImage = GetComponent<Image>();
            rectTransform = GetComponent<RectTransform>();

            _uIautoscrollList = GetComponentInParent<UIautoscrollList>();
        }

        public void Select() {
            backgroundImage.color = activatedColor;
            
            if (UnityEngine.Input.GetMouseButton(0)) return;
            if (_uIautoscrollList != null) _uIautoscrollList.SynchronizeScrollbarAndSelectedButton(rectTransform);
        }

        public void Unselect() {
            backgroundImage.color = unactivatedColor;
        }
    }
}
