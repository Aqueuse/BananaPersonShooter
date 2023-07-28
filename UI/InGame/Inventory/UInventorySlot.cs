using Enums;
using TMPro;
using UnityEngine;

namespace UI.InGame.Inventory {
    public class UInventorySlot : MonoBehaviour {
        public ItemType itemType;
        public ItemCategory itemCategory;

        [SerializeField] private TextMeshProUGUI quantityText;
        
        public void AssignToSlot() {
            SetDescriptionAndName();
            
            ObjectsReference.Instance.bananaMan.SetActiveItemTypeAndCategory(itemType, itemCategory, BuildableType.EMPTY);
            ObjectsReference.Instance.uInventory.lastselectedInventoryItem = gameObject;

            ObjectsReference.Instance.uiSlotsManager.AssignToSelectedSlot(itemCategory, itemType);

            if (itemCategory == ItemCategory.BANANA) {
                ObjectsReference.Instance.bananaMan.activeItem = ObjectsReference.Instance.scriptableObjectManager.GetBananaScriptableObject(itemType);
            }
        }
        
        public void SetQuantity(int quantity) {
            quantityText.text = quantity > 999 ? "999+" : quantity.ToString();
        }

        public void SetDescriptionAndName() {
            ObjectsReference.Instance.uInventory.itemName.text = ObjectsReference.Instance.scriptableObjectManager.GetName(itemCategory, itemType);
            ObjectsReference.Instance.uInventory.itemDescription.text = ObjectsReference.Instance.scriptableObjectManager.GetDescription(itemCategory, itemType);
        }
    }
}
