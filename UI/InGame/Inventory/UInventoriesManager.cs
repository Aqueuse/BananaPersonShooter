using Enums;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.InGame.Inventory {
    public enum SchemaContext {
        KEYBOARD,
        GAMEPAD
    }

    public class UInventoriesManager : MonoBehaviour {
        [SerializeField] private GenericDictionary<ItemCategory, Image> buttonImageByInventoryCategory;
        [SerializeField] private GenericDictionary<ItemCategory, TextMeshProUGUI> buttonTextByInventoryCategory;
        public GenericDictionary<ItemCategory, GameObject> firstItemByInventoryCategory;

        [SerializeField] private GenericDictionary<ItemCategory, CanvasGroup> canvasGroupByInventoryCategory;

        [SerializeField] private GenericDictionary<ItemCategory, GameObject> lastSelectedItemByInventoryCategory;
        
        [SerializeField] private Color activatedColor;
        [SerializeField] private Color unactivatedColor;
        
        public ItemCategory lastFocusedInventory;
     
        [SerializeField] private GenericDictionary<SchemaContext, UIHelper> uiHelpersBySchemaContext;
        
        private void Start() {
            lastFocusedInventory = ItemCategory.BANANA;
            
            Switch_To_Bananas_Inventory();
        }

        public GameObject GetLastSelectedItem(ItemCategory itemCategory) {
            return lastSelectedItemByInventoryCategory[itemCategory];
        }

        public void SetLastSelectedItem(ItemCategory itemCategory, GameObject lastSelectedItem) {
            lastFocusedInventory = itemCategory;

            lastSelectedItemByInventoryCategory[itemCategory] = lastSelectedItem;
        }

        public void Focus_interface() {
            if (lastSelectedItemByInventoryCategory[lastFocusedInventory] != null)
                EventSystem.current.SetSelectedGameObject(lastSelectedItemByInventoryCategory[lastFocusedInventory]);
        }
        
        public void Switch_To_Left_Tab() {
            switch (lastFocusedInventory) {
                case ItemCategory.BANANA:
                    Switch_To_Tab(ItemCategory.BUILDABLE);
                    break;
                case ItemCategory.RAW_MATERIAL:
                    Switch_To_Tab(ItemCategory.BANANA);
                    break;
                case ItemCategory.INGREDIENT:
                    Switch_To_Tab(ItemCategory.RAW_MATERIAL);
                    break;
                case ItemCategory.BUILDABLE:
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
                    Switch_To_Tab(ItemCategory.BUILDABLE);
                    break;
                case ItemCategory.BUILDABLE:
                    Switch_To_Tab(ItemCategory.BANANA);
                    break;
            }
        }

        public void Switch_To_Tab(ItemCategory itemCategory) {
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

            Focus_interface();
        }
        
        public UIHelper GetCurrentUIHelper() {
            return uiHelpersBySchemaContext[ObjectsReference.Instance.inputManager.schemaContext];
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

        public void Switch_To_Blueprints_Inventory() {
            Switch_To_Tab(ItemCategory.BUILDABLE);
        }
    }
}