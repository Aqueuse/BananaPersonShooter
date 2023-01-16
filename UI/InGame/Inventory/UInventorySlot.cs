using Data;
using Enums;
using Player;
using Settings;
using TMPro;
using UnityEngine;

namespace UI.InGame.Inventory {
    public class UInventorySlot : MonoBehaviour {
        public ItemThrowableType itemThrowableType;
        public ItemThrowableCategory itemThrowableCategory;

        [SerializeField] private GameObject quantityText;
        
        public void AssignToSlot() {
            SetDescription();
            
            BananaMan.Instance.activeItemThrowableCategory = itemThrowableCategory;
            BananaMan.Instance.activeItemThrowableType = itemThrowableType;
            
            UInventory.Instance.lastselectedInventoryItem = gameObject;
            
            if (itemThrowableCategory == ItemThrowableCategory.BANANA) {
                BananaMan.Instance.activeItem = ScriptableObjectManager.Instance.GetBananaScriptableObject(itemThrowableType);
                UISlotsManager.Instance.AssignToSelectedSlot(itemThrowableType, itemThrowableCategory);
            }

            else {
                UISlotsManager.Instance.AssignToSelectedSlot(itemThrowableType, itemThrowableCategory);
            }
        }
        
        public void SetQuantity(int quantity) {
            quantityText.GetComponent<TextMeshProUGUI>().text = quantity > 999 ? "999+" : quantity.ToString();
        }

        public void SetDescription() {
            UInventory.Instance.itemDescription.text = ScriptableObjectManager.Instance.GetDescription(itemThrowableType, GameSettings.Instance.languageIndexSelected);
        }
    }
}
