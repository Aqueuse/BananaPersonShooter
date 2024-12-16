using TMPro;
using UnityEngine;

namespace UI.InGame.MainPanel.Inventories {
    public class UInfobulle : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI descriptionText;
        
        private CanvasGroup _canvasGroup;
        private RectTransform _rectTransform;

        [SerializeField] private float worldPositionDeltaX;
        [SerializeField] private float worldPositionDeltaY;
        
        private Vector2 infobullePosition;
        
        private void Start() {
            _canvasGroup = GetComponent<CanvasGroup>();
            _rectTransform = GetComponent<RectTransform>();
        }

        public void SetDescriptionAndName(string name, string description, RectTransform triggerRectTransform) {
            _canvasGroup.alpha = 1;

            infobullePosition.x = triggerRectTransform.position.x + triggerRectTransform.rect.width * 2; 
            infobullePosition.y = triggerRectTransform.position.y;
            
            _rectTransform.position = infobullePosition;
            
            nameText.text = name;
            descriptionText.text = description;
        }

        public void SetDescriptionAndNameInWorldPosition(string name, string description, Vector2 positionMouse) {
            _canvasGroup.alpha = 1;

            infobullePosition.x = positionMouse.x + worldPositionDeltaX; 
            infobullePosition.y = positionMouse.y + worldPositionDeltaY;
            
            _rectTransform.position = infobullePosition;
            
            nameText.text = name;
            descriptionText.text = description;
        }

        public void Hide() {
            _canvasGroup.alpha = 0;
        }
    }
}
