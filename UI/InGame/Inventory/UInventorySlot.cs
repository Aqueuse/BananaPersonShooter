using TMPro;
using UnityEngine;

namespace UI.InGame.Inventory {
    public class UInventorySlot : MonoBehaviour {
        public ItemType itemType;
        public ItemCategory itemCategory;

        [SerializeField] private TextMeshProUGUI quantityText;
        
        public void AssignToSlot() {
            SetDescription();
            
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

        public void SetDescription() {
            ObjectsReference.Instance.uInventory.itemDescription.text = ObjectsReference.Instance.scriptableObjectManager.GetDescription(itemCategory, ObjectsReference.Instance.gameSettings.languageIndexSelected, itemType);
        }
    }
}
