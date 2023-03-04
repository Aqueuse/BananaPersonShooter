using Enums;
using TMPro;
using UI.InGame.Inventory;
using UnityEngine;
using UnityEngine.UI;

namespace UI.InGame.QuickSlots {
    public class UISlot : MonoBehaviour {
        [SerializeField] private Image iconImage;
        [SerializeField] private Image colorImage;
        [SerializeField] private TextMeshProUGUI quantityText;
    
        [SerializeField] private Color unactive;
        [SerializeField] private Color active;
        
        private Color transparent = Color.white;
        private Color visible = Color.white;
        
        public ItemThrowableType itemThrowableType;
        public ItemThrowableCategory itemThrowableCategory;

        private void Start() {
            itemThrowableType = ItemThrowableType.EMPTY;
            itemThrowableCategory = ItemThrowableCategory.EMPTY;

            transparent.a = 0f;
            visible.a = 64f;
            
            EmptySlot();
        }
        
        public void SetSlot(ItemThrowableType slotItemThrowableType, ItemThrowableCategory slotItemThrowableCategory) {
            itemThrowableType = slotItemThrowableType;
            itemThrowableCategory = slotItemThrowableCategory;
            
            if (itemThrowableType != ItemThrowableType.EMPTY) {
                iconImage.sprite = UInventory.Instance.GetItemSprite(itemThrowableType);
                iconImage.color = visible;
                SetAmmoQuantity(Game.Inventory.Instance.GetQuantity(itemThrowableType));
            }
            else {
                EmptySlot();
            }
        }

        public void EmptySlot() {
            itemThrowableType = ItemThrowableType.EMPTY;
            itemThrowableCategory = ItemThrowableCategory.EMPTY;
            
            iconImage.color = transparent;
            quantityText.text = "";
        }

        public void SetSelectedWeaponSlot() {
            colorImage.color = active;
        }

        public void SetUnselectedWeaponSlot() {
            colorImage.color = unactive;
        }

        public void SetAmmoQuantity(int quantity) {
            if (quantity == 0) EmptySlot();
            else {
                quantityText.text = quantity > 999 ? "999+" : quantity.ToString();
            }
        }

        public void SetSprite(Sprite sprite) {
            iconImage.sprite = sprite;
        }
    }
}
