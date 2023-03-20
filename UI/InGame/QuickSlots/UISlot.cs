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
        
        private Color _transparent = Color.white;
        private Color _visible = Color.white;
        
        public ItemThrowableType itemThrowableType;

        private void Start() {
            itemThrowableType = ItemThrowableType.EMPTY;

            _transparent.a = 0f;
            _visible.a = 64f;
            
            EmptySlot();
        }

        public void SetSlot(ItemThrowableType slotItemThrowableType) {
            itemThrowableType = slotItemThrowableType;

            if (itemThrowableType != ItemThrowableType.EMPTY) {
                iconImage.sprite = UInventory.Instance.GetItemSprite(itemThrowableType);
                iconImage.color = _visible;
                SetAmmoQuantity(Game.Inventory.Instance.GetQuantity(itemThrowableType));
            }
            else {
                EmptySlot();
            }
        }

        public void EmptySlot() {
            itemThrowableType = ItemThrowableType.EMPTY;
            
            iconImage.color = _transparent;
            quantityText.text = "";
        }

        public void SetSelectedWeaponSlot() {
            colorImage.color = active;
        }

        public void SetUnselectedWeaponSlot() {
            colorImage.color = unactive;
        }

        public void SetAmmoQuantity(int quantity) {
            if (itemThrowableType != ItemThrowableType.EMPTY) {
                if (quantity == 0) EmptySlot();
                else {
                    quantityText.text = quantity > 999 ? "999+" : quantity.ToString();
                }
            }
        }

        public void SetSprite(Sprite sprite) {
            iconImage.sprite = sprite;
        }
    }
}
