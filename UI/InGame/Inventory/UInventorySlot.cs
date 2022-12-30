using Data;
using Enums;
using Player;
using Settings;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.InGame.Inventory {
    public class UInventorySlot : MonoBehaviour, ISelectHandler {
        public ItemThrowableType itemThrowableType;
        public ItemThrowableCategory itemThrowableCategory;

        [SerializeField] private GameObject quantityText;
        
        public void OnSelect(BaseEventData eventData) {
            SetDescription();

            switch (itemThrowableCategory) {
                case ItemThrowableCategory.BANANA:
                    BananaMan.Instance.activeItem = ScriptableObjectManager.Instance.GetBananaScriptableObject(itemThrowableType);
                    BananaMan.Instance.activeItemThrowableType = itemThrowableType;
                    BananaMan.Instance.activeItemThrowableCategory = itemThrowableCategory;
            
                    UInventory.Instance.lastselectedInventoryItem = gameObject;
            
                    UISlotsManager.Instance.AssignToSelectedSlot(itemThrowableType, itemThrowableCategory);
                    break;
                case ItemThrowableCategory.PLATEFORM:
                    BananaMan.Instance.activeItemThrowableCategory = itemThrowableCategory;
                    BananaMan.Instance.activeItemThrowableType = itemThrowableType;
            
                    UInventory.Instance.lastselectedInventoryItem = gameObject;
            
                    UISlotsManager.Instance.AssignToSelectedSlot(itemThrowableType, itemThrowableCategory);
                    break;
                case ItemThrowableCategory.ROCKET:
                    BananaMan.Instance.activeItemThrowableCategory = itemThrowableCategory;
                    BananaMan.Instance.activeItemThrowableType = itemThrowableType;

                    UInventory.Instance.lastselectedInventoryItem = gameObject;

                    UICrosshair.Instance.SetCrosshair(itemThrowableType, itemThrowableCategory);
                    UISlotsManager.Instance.AssignToSelectedSlot(itemThrowableType, itemThrowableCategory);
                    break;
            }
        }
        
        public void SetQuantity(int quantity) {
            quantityText.GetComponent<TextMeshProUGUI>().text = quantity.ToString();
        }

        public void SetDescription() {
            UInventory.Instance.itemDescription.text = ScriptableObjectManager.Instance.GetDescription(itemThrowableType, GameSettings.Instance.languageIndexSelected);
        }
    }
}
