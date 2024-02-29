using InGame.Items.ItemsProperties;
using InGame.Items.ItemsProperties.Characters;
using UI.InGame.Merchimps;
using UnityEngine;

namespace InGame.Player.PlayerActions {
    public class Trade : MonoBehaviour {
        public InventoryScriptableObject merchantPropertiesScriptableObject;

        private UIMerchantSliderMenu buyUiMerchantSliderMenu;
        private UIMerchantSliderMenu sellUiMerchantSliderMenu;
        
        private BananaMan bananaMan;
        private UIMerchant uiMerchant;

        [Range(0, 999)] private int newItemQuantity;

        private void Start() {
            uiMerchant = ObjectsReference.Instance.uiMerchant;
            bananaMan = ObjectsReference.Instance.bananaMan;

            buyUiMerchantSliderMenu = uiMerchant.buyUiMerchantSliderMenu;
            sellUiMerchantSliderMenu = uiMerchant.sellUiMerchantSliderMenu;
        }

        public void Sell(ItemScriptableObject itemScriptableObject) {
            var quantityToSell = (int)sellUiMerchantSliderMenu.quantitySlider.value;
            var bitkongValue = itemScriptableObject.bitKongValue * quantityToSell;
            
            bananaMan.inventories.bitKongQuantity += bitkongValue;
            merchantPropertiesScriptableObject.bitKongQuantity -= bitkongValue;
            ObjectsReference.Instance.uInventoriesManager.SetBitKongQuantity(bananaMan.inventories.bitKongQuantity);
            uiMerchant.RefreshBitkongQuantities();

            sellUiMerchantSliderMenu.quantitySlider.value = 0;

            switch (itemScriptableObject.itemCategory) {
                case ItemCategory.BANANA:
                    var bananaType = itemScriptableObject.bananaType;
                    ObjectsReference.Instance.bananasInventory.RemoveQuantity(bananaType, quantityToSell);

                    newItemQuantity = merchantPropertiesScriptableObject.bananasInventory[bananaType] + quantityToSell;
                    merchantPropertiesScriptableObject.bananasInventory[bananaType] = newItemQuantity;

                    uiMerchant.merchantSellUiBananasInventory.uInventorySlots[bananaType].SetQuantity(newItemQuantity);
                    uiMerchant.merchantSellUiBananasInventory.RefreshUInventory();
                    break;

                case ItemCategory.INGREDIENT:
                    var ingredientType = itemScriptableObject.ingredientsType;
                    ObjectsReference.Instance.ingredientsInventory.RemoveQuantity(ingredientType, quantityToSell);

                    newItemQuantity = merchantPropertiesScriptableObject.ingredientsInventory[ingredientType] + quantityToSell;
                    merchantPropertiesScriptableObject.ingredientsInventory[ingredientType] = newItemQuantity;

                    uiMerchant.merchantSellUiIngredientsInventory.uInventorySlots[ingredientType].SetQuantity(newItemQuantity);
                    uiMerchant.merchantSellUiIngredientsInventory.RefreshUInventory();
                    break;
                
                case ItemCategory.MANUFACTURED_ITEM:
                    var manufacturedItemType = itemScriptableObject.manufacturedItemsType;
                    ObjectsReference.Instance.manufacturedItemsInventory.RemoveQuantity(manufacturedItemType, quantityToSell);

                    newItemQuantity = merchantPropertiesScriptableObject.manufacturedItemsInventory[manufacturedItemType] + quantityToSell;
                    merchantPropertiesScriptableObject.manufacturedItemsInventory[manufacturedItemType] = newItemQuantity;

                    uiMerchant.merchantSellUiManufacturedItemsInventory.uInventorySlots[manufacturedItemType].SetQuantity(newItemQuantity);
                    uiMerchant.merchantSellUiManufacturedItemsInventory.RefreshUInventory();
                    break;
            }
        }

        public void Buy(ItemScriptableObject itemScriptableObject) {
            var quantityToBuy = (int)buyUiMerchantSliderMenu.quantitySlider.value;
            var bitkongValue = itemScriptableObject.bitKongValue * quantityToBuy;
            
            // not enough monkey money (；′⌒`)
            if (bananaMan.inventories.bitKongQuantity < bitkongValue) return;

            bananaMan.inventories.bitKongQuantity -= itemScriptableObject.bitKongValue;
            merchantPropertiesScriptableObject.bitKongQuantity += bitkongValue;
            ObjectsReference.Instance.uInventoriesManager.SetBitKongQuantity(bananaMan.inventories.bitKongQuantity);
            uiMerchant.RefreshBitkongQuantities();

            buyUiMerchantSliderMenu.quantitySlider.value = 0;

            // buy
            switch (itemScriptableObject.itemCategory) {
                case ItemCategory.INGREDIENT:
                    var ingredientType = itemScriptableObject.ingredientsType;
                    ObjectsReference.Instance.ingredientsInventory.AddQuantity(ingredientType, quantityToBuy);

                    newItemQuantity = merchantPropertiesScriptableObject.ingredientsInventory[ingredientType] - quantityToBuy;
                    merchantPropertiesScriptableObject.ingredientsInventory[ingredientType] = newItemQuantity;
                    
                    uiMerchant.merchantBuyUiIngredientsInventory.uInventorySlots[ingredientType].SetQuantity(newItemQuantity);
                    uiMerchant.merchantBuyUiIngredientsInventory.RefreshUInventory();
                    uiMerchant.merchantSellUiIngredientsInventory.RefreshUInventory();
                    break;

                case ItemCategory.MANUFACTURED_ITEM:
                    var manufacturedItemType = itemScriptableObject.manufacturedItemsType;
                    ObjectsReference.Instance.manufacturedItemsInventory.AddQuantity(manufacturedItemType, quantityToBuy);

                    newItemQuantity = merchantPropertiesScriptableObject.manufacturedItemsInventory[manufacturedItemType] - quantityToBuy;
                    merchantPropertiesScriptableObject.manufacturedItemsInventory[manufacturedItemType] = newItemQuantity;
                    
                    uiMerchant.merchantBuyUiManufacturedItemsInventory.uInventorySlots[manufacturedItemType].SetQuantity(newItemQuantity);
                    uiMerchant.merchantBuyUiManufacturedItemsInventory.RefreshUInventory();
                    uiMerchant.merchantSellUiManufacturedItemsInventory.RefreshUInventory();
                    break;

                case ItemCategory.RAW_MATERIAL:
                    var rawMaterialType = itemScriptableObject.rawMaterialType;
                    ObjectsReference.Instance.rawMaterialsInventory.AddQuantity(rawMaterialType, quantityToBuy);

                    newItemQuantity = merchantPropertiesScriptableObject.rawMaterialsInventory[rawMaterialType] - quantityToBuy;
                    merchantPropertiesScriptableObject.rawMaterialsInventory[rawMaterialType] = newItemQuantity;
                    
                    uiMerchant.merchantBuyUiRawMaterialsInventory.uInventorySlots[rawMaterialType].SetQuantity(newItemQuantity);
                    uiMerchant.merchantBuyUiRawMaterialsInventory.RefreshUInventory();
                    break;
            }
        }
    }
}