using Enums;
using TMPro;
using UI.InGame.Inventory;
using UnityEngine;
using UnityEngine.UI;

namespace UI.InGame {
    public class UISlot : MonoBehaviour {
        [SerializeField] private RectTransform favoriteBackground;
        [SerializeField] private Image iconImage;
        [SerializeField] private TextMeshProUGUI quantityText;
    
        private Color _white;
        private Color _transparent;
        
        public ItemThrowableType itemThrowableType;
        public ItemThrowableCategory itemThrowableCategory;

        private void Start() {
            _white = Color.white;
            _transparent = _white;
            _transparent.a = 0;
        }
        
        public void SetSlot(ItemThrowableType slotItemThrowableType, ItemThrowableCategory slotItemThrowableCategory) {
            itemThrowableType = slotItemThrowableType;
            itemThrowableCategory = slotItemThrowableCategory;
            
            if (itemThrowableCategory == ItemThrowableCategory.ROCKET) {
                iconImage.sprite = null;
                quantityText.text = "";
                iconImage.color = _transparent;
            }

            else {
                iconImage.sprite = UInventory.Instance.lastselectedInventoryItem.transform.GetChild(0).GetComponentInChildren<Image>().sprite;
                SetAmmoQuantity(global::Inventory.Instance.GetQuantity(itemThrowableType).ToString());
                iconImage.color = _white;
            }
        }
    
        public void SetSelectedWeaponSlot() {
            favoriteBackground.sizeDelta = new Vector2(125f, 125f);
        }

        public void SetUnselectedWeaponSlot() {
            favoriteBackground.sizeDelta = new Vector2(100f, 100f);
        }

        public void SetAmmoQuantity(string quantity) {
            quantityText.text = quantity;
        }

        public void SetSprite(Sprite sprite) {
            iconImage.sprite = sprite;
        }
    }
}
