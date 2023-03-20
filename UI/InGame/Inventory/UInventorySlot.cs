using Data;
using Enums;
using Player;
using Settings;
using TMPro;
using UI.InGame.QuickSlots;
using UnityEngine;

namespace UI.InGame.Inventory {
    public class UInventorySlot : MonoBehaviour {
        public ItemThrowableType itemThrowableType;
        public ItemThrowableCategory itemThrowableCategory;

        [SerializeField] private TextMeshProUGUI quantityText;
        
        public void AssignToSlot() {
            SetDescription();
            
            BananaMan.Instance.activeItemThrowableCategory = itemThrowableCategory;
            BananaMan.Instance.activeItemThrowableType = itemThrowableType;
            
            UInventory.Instance.lastselectedInventoryItem = gameObject;

            UISlotsManager.Instance.AssignToSelectedSlot(itemThrowableType);

            if (itemThrowableCategory == ItemThrowableCategory.BANANA) {
                BananaMan.Instance.activeItem = ScriptableObjectManager.Instance.GetBananaScriptableObject(itemThrowableType);
            }
        }
        
        public void SetQuantity(int quantity) {
            quantityText.text = quantity > 999 ? "999+" : quantity.ToString();
        }

        public void SetDescription() {
            UInventory.Instance.itemDescription.text = ScriptableObjectManager.Instance.GetDescription(itemThrowableCategory, itemThrowableType, GameSettings.Instance.languageIndexSelected);
        }
    }
}
