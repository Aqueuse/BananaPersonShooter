using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace UI.InGame.Inventory {
    public class UInventoriesManager : MonoBehaviour {
        [SerializeField] private GenericDictionary<ItemCategory, Image> buttonImageByInventoryCategory;
        [SerializeField] private GenericDictionary<ItemCategory, TextMeshProUGUI> buttonTextByInventoryCategory;

        [SerializeField] private GenericDictionary<ItemCategory, CanvasGroup> canvasGroupByInventoryCategory;

        public GenericDictionary<ItemCategory, GameObject> lastSelectedItemByInventoryCategory;

        [SerializeField] private TextMeshProUGUI bitkongQuantityText;
        
        [SerializeField] private Color activatedColor;
        [SerializeField] private Color unactivatedColor;
        
        public ItemCategory lastFocusedInventory;

        [SerializeField] private UIHelper gamepadUIHelper;
        [SerializeField] private UIHelper keyboardUIHelper;
        
        private void Start() {
            Switch_To_Bananas_Inventory();
            
            ObjectsReference.Instance.uiBananasInventory.RefreshUInventory();
            ObjectsReference.Instance.uiRawMaterialsInventory.RefreshUInventory();
            ObjectsReference.Instance.uiIngredientsInventory.RefreshUInventory();
            ObjectsReference.Instance.uiManufacturedItemsItemsInventory.RefreshUInventory();
        }
        
        public void SetLastSelectedItem(ItemCategory itemCategory, GameObject lastSelectedItem) {
            lastFocusedInventory = itemCategory;
            lastSelectedItemByInventoryCategory[itemCategory] = lastSelectedItem;
        }

        public void FocusInventory() {
            if (lastSelectedItemByInventoryCategory[lastFocusedInventory] != null) {
                EventSystem.current.SetSelectedGameObject(lastSelectedItemByInventoryCategory[lastFocusedInventory]);
                FocusFirstSlotInInventory(lastFocusedInventory);
            }
        }

        public void UnselectInventorySlots(ItemCategory inventoryCategory) {
            switch (inventoryCategory) {
                case ItemCategory.BANANA:
                    ObjectsReference.Instance.uiBananasInventory.UnselectAllSlots();
                    break;
                case ItemCategory.INGREDIENT:
                    ObjectsReference.Instance.uiIngredientsInventory.UnselectAllSlots();
                    break;
                case ItemCategory.RAW_MATERIAL:
                    ObjectsReference.Instance.uiRawMaterialsInventory.UnselectAllSlots();
                    break;
                case ItemCategory.MANUFACTURED_ITEM:
                    ObjectsReference.Instance.uiManufacturedItemsItemsInventory.UnselectAllSlots();
                    break;
            }
        }

        private void FocusFirstSlotInInventory(ItemCategory inventoryCategory) {
            switch (inventoryCategory) {
                case ItemCategory.BANANA:
                    ObjectsReference.Instance.uiBananasInventory.SelectFirstSlot();
                    break;
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

        public void Switch_To_Left_Tab() {
            switch (lastFocusedInventory) {
                case ItemCategory.BANANA:
                    Switch_To_Tab(ItemCategory.MANUFACTURED_ITEM);
                    break;
                case ItemCategory.RAW_MATERIAL:
                    Switch_To_Tab(ItemCategory.BANANA);
                    break;
                case ItemCategory.INGREDIENT:
                    Switch_To_Tab(ItemCategory.RAW_MATERIAL);
                    break;
                case ItemCategory.MANUFACTURED_ITEM:
                    Switch_To_Tab(ItemCategory.INGREDIENT);
                    break;
            }
        }

        public void Switch_To_Right_Tab() {
            switch (lastFocusedInventory) {
                case ItemCategory.BANANA:
                    Switch_To_Tab(ItemCategory.RAW_MATERIAL);
                    break;
                case ItemCategory.RAW_MATERIAL:
                    Switch_To_Tab(ItemCategory.INGREDIENT);
                    break;
                case ItemCategory.INGREDIENT:
                    Switch_To_Tab(ItemCategory.MANUFACTURED_ITEM);
                    break;
                case ItemCategory.MANUFACTURED_ITEM:
                    Switch_To_Tab(ItemCategory.BANANA);
                    break;
            }
        }

        public void Switch_To_Tab(ItemCategory itemCategory) {
            UnselectInventorySlots(lastFocusedInventory);

            lastFocusedInventory = itemCategory;

            foreach (var (category, canvasGroup) in canvasGroupByInventoryCategory) {
                if (category == itemCategory) {
                    canvasGroup.alpha = 1;
                    canvasGroup.interactable = true;
                    canvasGroup.blocksRaycasts = true;
                    
                    buttonImageByInventoryCategory[category].color = activatedColor;
                    buttonTextByInventoryCategory[category].color = Color.black;
                }

                else {
                    canvasGroup.alpha = 0;
                    canvasGroup.interactable = false;
                    canvasGroup.blocksRaycasts = false;
                    
                    buttonImageByInventoryCategory[category].color = unactivatedColor;
                    buttonTextByInventoryCategory[category].color = activatedColor;
                }
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

        public void Switch_To_Bananas_Inventory() {
            Switch_To_Tab(ItemCategory.BANANA);
        }

        public void Switch_To_Raw_Materials_Inventory() {
            Switch_To_Tab(ItemCategory.RAW_MATERIAL);
        }

        public void Switch_To_Ingredients_Inventory() {
            Switch_To_Tab(ItemCategory.INGREDIENT);
        }
        
        public void Switch_To_Manufactured_Items_Inventory() {
            Switch_To_Tab(ItemCategory.MANUFACTURED_ITEM);
        } 

        public void SetBitKongQuantity(int bitkongQuantity) {
            bitkongQuantityText.text = bitkongQuantity + " BTK";
        }
    }
}