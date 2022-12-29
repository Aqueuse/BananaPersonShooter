using Data;
using Enums;
using Player;
using Settings;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.InGame.Inventory {
    public class UInventorySlot : MonoBehaviour, ISelectHandler {
        [SerializeField] private ItemThrowableType itemThrowableType;
        [SerializeField] private GameObject quantityText;

        [SerializeField] private ItemThrowableCategory itemThrowableCategory;

        public void OnSelect(BaseEventData eventData) {
            SetDescription();

            switch (itemThrowableCategory) {
                case ItemThrowableCategory.BANANA:
                    BananaMan.Instance.activeItem = ScriptableObjectManager.Instance.GetBananaScriptableObject(itemThrowableType);
                    BananaMan.Instance.activeItemThrowableType = itemThrowableType;
            
                    UInventory.Instance.lastselectedInventoryItem = gameObject;
            
                    UISlotsManager.Instance.AssignToSelectedSlot(itemThrowableType);
                    break;
                case ItemThrowableCategory.PLATEFORM:
                    Debug.Log("to be continued");
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
