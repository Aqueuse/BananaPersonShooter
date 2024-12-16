using InGame.Items.ItemsProperties;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.InGame.MainPanel.Inventories {
    public class UIFlyingSlot : MonoBehaviour {
        [SerializeField] private Image slotImage;
        [SerializeField] private TextMeshProUGUI slotQuantityText;
        
        [SerializeField] private CanvasGroup canvasGroup;
        
        private Vector3 position;
        
        private Mesh targetMesh;
        
        public ItemScriptableObject itemScriptableObject;

        public bool isActivated;

        public void SetSlot(UInventorySlot uInventorySlot) {
            slotImage.sprite = uInventorySlot.itemScriptableObject.GetSprite();
            if (uInventorySlot.itemScriptableObject.itemCategory != ItemCategory.BUILDABLE) {
                slotQuantityText.text = uInventorySlot.GetQuantity();
            }

            itemScriptableObject = uInventorySlot.itemScriptableObject;
        }
        
        private void Update() {
            if (!isActivated) return;

            RectTransformUtility.ScreenPointToWorldPointInRectangle(
                GetComponent<RectTransform>(),
                Input.mousePosition,
                null,
                out position);

            GetComponent<RectTransform>().position = position;
        }

        public void Show() {
            canvasGroup.alpha = 1;
        }

        public void Hide() {
            canvasGroup.alpha = 0;            
        }
        
        public void Reset() {
            transform.position = new Vector3(-1000, -1000, 0);
            isActivated = false;
        }
    }
}