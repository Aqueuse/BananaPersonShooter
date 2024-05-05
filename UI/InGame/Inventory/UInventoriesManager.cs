using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace UI.InGame.Inventory {
    public enum BananaGunUITabType {
        INVENTORY_RAW_MATERIALS = 0,
        INVENTORY_INGREDIENTS = 1,
        INVENTORY_MANUFACTURED_ITEMS = 2,
        INVENTORY_BUILDABLES = 3,
        MINICHIMPBLOCK_DIALOGUE = 4,
        MINICHIMPBLOCK_DESCRIPTION = 5,
        MINICHIMPBLOCK_MAP = 6,
        MINICHIMPBLOCK_BANANAPEDIA = 7,
        MINICHIMPBLOCK_HELP = 8
    }
    
    public enum MiniChimpBlockTabType {
        MINICHIMPBLOCK_DIALOGUE,
        MINICHIMPBLOCK_DESCRIPTION,
        MINICHIMPBLOCK_MAP,
        MINICHIMPBLOCK_BANANAPEDIA,
        MINICHIMPBLOCK_HELP
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
            ObjectsReference.Instance.uiRawMaterialsInventory.RefreshUInventory();
            ObjectsReference.Instance.uiIngredientsInventory.RefreshUInventory();
            ObjectsReference.Instance.uiManufacturedItemsItemsInventory.RefreshUInventory();
            ObjectsReference.Instance.uiBlueprintsInventory.RefreshUInventory();
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
                    ObjectsReference.Instance.uiIngredientsInventory.SelectFirstSlot();
                    break;
                case ItemCategory.RAW_MATERIAL:
                    ObjectsReference.Instance.uiRawMaterialsInventory.SelectFirstSlot();
                    break;
                case ItemCategory.MANUFACTURED_ITEM:
                    ObjectsReference.Instance.uiManufacturedItemsItemsInventory.SelectFirstSlot();
                    break;
            }
        }

        public void SwitchToInventoryTab(ItemCategory itemCategory) {
            lastFocusedInventory = itemCategory;

            foreach (var (category, canvasGroup) in canvasGroupByInventoryCategory) {
                SetActive(canvasGroup, category == itemCategory);
            }

            FocusInventory();
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