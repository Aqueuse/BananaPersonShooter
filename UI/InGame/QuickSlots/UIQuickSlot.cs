using Data;
using Enums;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.InGame.QuickSlots {
    public class UIQuickSlot : MonoBehaviour {
        public Image iconImage;
        public TextMeshProUGUI quantityText;
        
        public Color _visible = Color.white;
        public Color _transparent = Color.white;

        public ItemScriptableObject slotItemScriptableObject;

        private RectTransform rectTransform;

        private void Start() {
            rectTransform = GetComponent<RectTransform>();
        }

        public void SetSelectedSlot() {
            rectTransform.sizeDelta = new Vector2(120f, 120f);
        }

        public void SetUnselectedSlot() {
            rectTransform.sizeDelta = new Vector2(100f, 100f);
        }
        
        public void SetSlot(ItemScriptableObject itemScriptableObject) {
            if (itemScriptableObject == null) return;
            
            slotItemScriptableObject = itemScriptableObject;

            iconImage.sprite = itemScriptableObject.GetSprite();
            iconImage.color = _visible;

            switch (itemScriptableObject.itemCategory) {
                case ItemCategory.BANANA:
                    SetQuantity(ObjectsReference.Instance.bananasInventory.GetQuantity(itemScriptableObject.bananaType));
                    break;
                case ItemCategory.RAW_MATERIAL:
                    SetQuantity(ObjectsReference.Instance.rawMaterialsInventory.GetQuantity(itemScriptableObject.rawMaterialType));
                    break;
                case ItemCategory.INGREDIENT:
                    SetQuantity(ObjectsReference.Instance.ingredientsInventory.GetQuantity(itemScriptableObject.ingredientsType));
                    break;
                case ItemCategory.BUILDABLE:
                    HideQuantity();
                    break;
            }
        }
        
        public void SetQuantity(int quantity) {
            if (quantity == 0) EmptySlot();
            else {
                quantityText.text = quantity > 999 ? "999+" : quantity.ToString();
            }
        }

        private void HideQuantity() {
            quantityText.text = "";
        }

        public void EmptySlot() {
            slotItemScriptableObject = null;
            
            iconImage.color = _transparent;
            quantityText.text = "";
        }
    }
}
