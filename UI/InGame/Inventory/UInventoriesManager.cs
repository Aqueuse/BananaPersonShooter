using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace UI.InGame.Inventory {
    public enum InventoryUITabType {
        INVENTORY_DROPPED = 0,
        INVENTORY_INGREDIENTS = 1,
        INVENTORY_MANUFACTURED_ITEMS = 2,
        INVENTORY_BUILDABLES = 3
    }
    
    public enum MainBlockType {
        DIALOGUE,
        INVENTORIES,
        COMMAND_ROOM,
        HELP
    }

    public class UInventoriesManager : MonoBehaviour {
        [SerializeField] private GenericDictionary<ItemCategory, Image> buttonImageByInventoryCategory;
        [SerializeField] private GenericDictionary<ItemCategory, TextMeshProUGUI> buttonTextByInventoryCategory;

        [SerializeField] private GenericDictionary<ItemCategory, CanvasGroup> canvasGroupByInventoryCategory;

        public GenericDictionary<ItemCategory, GameObject> lastSelectedItemByInventoryCategory;

        [SerializeField] private TextMeshProUGUI bitkongQuantityText;
        
        public ItemCategory lastFocusedInventory;

        [SerializeField] private UIHelper gamepadUIHelper;
        [SerializeField] private UIHelper keyboardUIHelper;
        
        public void RefreshInventories() {
            ObjectsReference.Instance.bananaManUiDroppedInventory.RefreshUInventory();
            ObjectsReference.Instance.bananaManUiIngredientsInventory.RefreshUInventory();
            ObjectsReference.Instance.bananaManUiManufacturedItemsInventory.RefreshUInventory();
            ObjectsReference.Instance.bananaManUiBlueprintsInventory.RefreshUInventory();
        }
        
        public void SetLastSelectedItem(ItemCategory itemCategory, GameObject lastSelectedItem) {
            lastFocusedInventory = itemCategory;
            lastSelectedItemByInventoryCategory[itemCategory] = lastSelectedItem;
        }

        public void OpenInventories() {
            RefreshInventories();

            SwitchToInventoryTab(lastFocusedInventory);
        }

        public void FocusInventory() {
            if (lastSelectedItemByInventoryCategory[lastFocusedInventory] != null) {
                EventSystem.current.SetSelectedGameObject(lastSelectedItemByInventoryCategory[lastFocusedInventory]);
                FocusFirstSlotInInventory(lastFocusedInventory);
            }
        }
        
        private void FocusFirstSlotInInventory(ItemCategory inventoryCategory) {
            switch (inventoryCategory) {
                case ItemCategory.INGREDIENT:
                    ObjectsReference.Instance.bananaManUiIngredientsInventory.SelectFirstSlot();
                    break;
                case ItemCategory.DROPPED:
                    ObjectsReference.Instance.bananaManUiDroppedInventory.SelectFirstSlot();
                    break;
                case ItemCategory.MANUFACTURED_ITEM:
                    ObjectsReference.Instance.bananaManUiManufacturedItemsInventory.SelectFirstSlot();
                    break;
            }
        }

        public void SwitchToInventoryTab(ItemCategory itemCategory) {
            lastFocusedInventory = itemCategory;

            foreach (var (category, canvasGroup) in canvasGroupByInventoryCategory) {
                SetActive(canvasGroup, category == itemCategory);
            }

            FocusInventory();
            ObjectsReference.Instance.uInfobulle.Hide();
        }
        
        public UIHelper GetCurrentUIHelper() {
            if (Gamepad.current == null) return gamepadUIHelper;
            return keyboardUIHelper;
        }

        public void ShowCurrentUIHelper() {
            HideUIHelpers();
            GetCurrentUIHelper().ShowHelper();
        }

        public void HideUIHelpers() {
            keyboardUIHelper.HideHelper();
            gamepadUIHelper.HideHelper();
        }
        

        public void SetBitKongQuantity(int bitkongQuantity) {
            bitkongQuantityText.text = bitkongQuantity + " BTK";
        }
        
        public void SetActive(CanvasGroup canvasGroup, bool visible) {
            canvasGroup.alpha = visible ? 1 : 0;
            canvasGroup.interactable = visible;
            canvasGroup.blocksRaycasts = visible;
        }
    }
}