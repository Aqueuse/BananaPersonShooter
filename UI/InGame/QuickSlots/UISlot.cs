using TMPro;
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
        
        public ItemCategory itemCategory;
        public ItemType itemType;
        public BuildableType buildableType;

        private void Start() {
            itemType = ItemType.EMPTY;

            _transparent.a = 0f;
            _visible.a = 64f;
            
            EmptySlot();
        }

        public void SetSlot(ItemCategory slotItemCategory, ItemType slotItemType = ItemType.EMPTY, BuildableType slotBuildableType = BuildableType.EMPTY) {
            itemCategory = slotItemCategory;
            itemType = slotItemType;
            buildableType = slotBuildableType;

            switch (slotItemCategory) {
                case ItemCategory.BUILDABLE:
                    iconImage.sprite = ObjectsReference.Instance.scriptableObjectManager.GetItemSprite(itemCategory, buildableType:buildableType);
                    iconImage.color = _visible;
                    quantityText.text = "";
                    break;
                case ItemCategory.EMPTY:
                    EmptySlot();
                    break;
                case ItemCategory.RAW_MATERIAL or ItemCategory.BANANA:
                    iconImage.sprite = ObjectsReference.Instance.scriptableObjectManager.GetItemSprite(itemCategory, itemType: itemType);
                    iconImage.color = _visible;
                    SetAmmoQuantity(ObjectsReference.Instance.inventory.GetQuantity(itemType));
                    break;
            }
        }
        
        public void EmptySlot() {
            itemType = ItemType.EMPTY;
            buildableType = BuildableType.EMPTY;
            itemCategory = ItemCategory.EMPTY;
            
            iconImage.color = _transparent;
            quantityText.text = "";
        }

        public void SetSelectedSlot() {
            colorImage.color = active;
        }

        public void SetUnselectedSlot() {
            colorImage.color = unactive;
        }

        public void SetAmmoQuantity(int quantity) {
            if (itemType != ItemType.EMPTY) {
                if (quantity == 0) EmptySlot();
                else {
                    quantityText.text = quantity > 999 ? "999+" : quantity.ToString();
                }
            }
        }
    }
}
