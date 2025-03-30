using InGame.Items.ItemsProperties;
using InGame.Items.ItemsProperties.Buildables;
using TMPro;
using UnityEngine;

namespace UI.InGame.MainPanel.Inventories {
    public class UIToolTipOnMouseHover : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI toolTipText;
        
        private CanvasGroup _canvasGroup;
        private RectTransform _rectTransform;

        [SerializeField] private float worldPositionDeltaX;
        [SerializeField] private float worldPositionDeltaY;
        
        private Vector2 toolTipPosition;
        
        private void Start() {
            _canvasGroup = GetComponent<CanvasGroup>();
            _rectTransform = GetComponent<RectTransform>();
        }

        public void SetSlotInfo(ItemScriptableObject itemScriptableObject, RectTransform triggerRectTransform) {
            _canvasGroup.alpha = 1;

            toolTipPosition.x = triggerRectTransform.position.x + triggerRectTransform.rect.width * 2; 
            toolTipPosition.y = triggerRectTransform.position.y;
            
            _rectTransform.position = toolTipPosition;

            toolTipText.text = GenerateToolTipText(itemScriptableObject);
            }

        public void SetDescriptionAndNameInWorldPosition(ItemScriptableObject itemScriptableObject, Vector2 positionMouse) {
            _canvasGroup.alpha = 1;

            toolTipPosition.x = positionMouse.x + worldPositionDeltaX; 
            toolTipPosition.y = positionMouse.y + worldPositionDeltaY;
            
            _rectTransform.position = toolTipPosition;
            
            toolTipText.text = GenerateToolTipText(itemScriptableObject);
        }

        private string GenerateToolTipText(ItemScriptableObject itemScriptableObject) {
            var generatedText = "<line-height=100%>\n<color=#FDF200>"+itemScriptableObject.GetName()+"</color>\n\n" +
                               itemScriptableObject.GetDescription()+"\n\n";

            if (itemScriptableObject.itemCategory == ItemCategory.BUILDABLE) {
                var buildableData = (BuildablePropertiesScriptableObject)itemScriptableObject;

                foreach (var rawMaterial in buildableData.rawMaterialsWithQuantity) {
                    generatedText += "<voffset=-10>" + rawMaterial.Value + "</voffset> <size=150%> <sprite="+rawMaterial.Key.spriteAtlasIndex+"></size>\n";
                }
            }

            return generatedText;
        }

        public void Hide() {
            _canvasGroup.alpha = 0;
        }
    }
}
