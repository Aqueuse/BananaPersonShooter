using TMPro;
using UnityEngine;

namespace UI.InGame.Inventory {
    public class UInfobulle : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI descriptionText;

        private Vector2 infobullePosition;
        
        public void SetDescriptionAndName(string name, string description, RectTransform triggerRectTransform) {
            GetComponent<CanvasGroup>().alpha = 1;

            infobullePosition.x = triggerRectTransform.position.x + triggerRectTransform.rect.width * 2; 
            infobullePosition.y = triggerRectTransform.position.y;
            
            GetComponent<RectTransform>().position = infobullePosition;
            
            nameText.text = name;
            descriptionText.text = description;
        }

        public void Hide() {
            GetComponent<CanvasGroup>().alpha = 0;
        }
    }
}
