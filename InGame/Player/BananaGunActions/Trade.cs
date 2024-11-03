using InGame.Items.ItemsData;
using InGame.Items.ItemsProperties;
using UI.InGame.Merchimps;
using UnityEngine;

namespace InGame.Player.BananaGunActions {
    public class Trade : MonoBehaviour {
        public MonkeyMenData monkeyMenData;

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
            
            bananaMan.bananaManData.bitKongQuantity += bitkongValue;
            monkeyMenData.bitKongQuantity -= bitkongValue;
            ObjectsReference.Instance.uInventoriesManager.SetBitKongQuantity(bananaMan.bananaManData.bitKongQuantity);
            uiMerchant.RefreshBitkongQuantities();

            sellUiMerchantSliderMenu.quantitySlider.value = 0;

            switch (itemScriptableObject.itemCategory) {
                case ItemCategory.INGREDIENT:
                    var ingredientType = itemScriptableObject.ingredientsType;
                    ObjectsReference.Instance.ingredientsInventory.RemoveQuantity(ingredientType, quantityToSell);

                    newItemQuantity = monkeyMenData.ingredientsInventory[ingredientType] + quantityToSell;
                    monkeyMenData.ingredientsInventory[ingredientType] = newItemQuantity;

                    uiMerchant.merchantSellUiIngredientsInventory.uInventorySlots[ingredientType].SetQuantity(newItemQuantity);
                    uiMerchant.merchantSellUiIngredientsInventory.RefreshUInventory();
                    break;
                
                case ItemCategory.MANUFACTURED_ITEM:
                    var manufacturedItemType = itemScriptableObject.manufacturedItemsType;
                    ObjectsReference.Instance.manufacturedItemsInventory.RemoveQuantity(manufacturedItemType, quantityToSell);

                    newItemQuantity = monkeyMenData.manufacturedItemsInventory[manufacturedItemType] + quantityToSell;
                    monkeyMenData.manufacturedItemsInventory[manufacturedItemType] = newItemQuantity;

                    uiMerchant.merchantSellUiManufacturedItemsInventory.uInventorySlots[manufacturedItemType].SetQuantity(newItemQuantity);
                    uiMerchant.merchantSellUiManufacturedItemsInventory.RefreshUInventory();
                    break;
            }
        }

        public void Buy(ItemScriptableObject itemScriptableObject) {
            var quantityToBuy = (int)buyUiMerchantSliderMenu.quantitySlider.value;
            var bitkongValue = itemScriptableObject.bitKongValue * quantityToBuy;
            
            // not enough monkey money (；′⌒`)
            if (bananaMan.bananaManData.bitKongQuantity < bitkongValue) return;

            bananaMan.bananaManData.bitKongQuantity -= itemScriptableObject.bitKongValue;
            monkeyMenData.bitKongQuantity += bitkongValue;
            ObjectsReference.Instance.uInventoriesManager.SetBitKongQuantity(bananaMan.bananaManData.bitKongQuantity);
            uiMerchant.RefreshBitkongQuantities();

            buyUiMerchantSliderMenu.quantitySlider.value = 0;

            // buy
            switch (itemScriptableObject.itemCategory) {
                case ItemCategory.INGREDIENT:
                    var ingredientType = itemScriptableObject.ingredientsType;
                    ObjectsReference.Instance.ingredientsInventory.AddQuantity(ingredientType, quantityToBuy);

                    newItemQuantity = monkeyMenData.ingredientsInventory[ingredientType] - quantityToBuy;
                    monkeyMenData.ingredientsInventory[ingredientType] = newItemQuantity;
                    
                    uiMerchant.merchantBuyUiIngredientsInventory.uInventorySlots[ingredientType].SetQuantity(newItemQuantity);
                    uiMerchant.merchantBuyUiIngredientsInventory.RefreshUInventory();
                    uiMerchant.merchantSellUiIngredientsInventory.RefreshUInventory();
                    break;

                case ItemCategory.MANUFACTURED_ITEM:
                    var manufacturedItemType = itemScriptableObject.manufacturedItemsType;
                    ObjectsReference.Instance.manufacturedItemsInventory.AddQuantity(manufacturedItemType, quantityToBuy);

                    newItemQuantity = monkeyMenData.manufacturedItemsInventory[manufacturedItemType] - quantityToBuy;
                    monkeyMenData.manufacturedItemsInventory[manufacturedItemType] = newItemQuantity;
                    
                    uiMerchant.merchantBuyUiManufacturedItemsInventory.uInventorySlots[manufacturedItemType].SetQuantity(newItemQuantity);
                    uiMerchant.merchantBuyUiManufacturedItemsInventory.RefreshUInventory();
                    uiMerchant.merchantSellUiManufacturedItemsInventory.RefreshUInventory();
                    break;

                case ItemCategory.DROPPED:
                    var droppedType = itemScriptableObject.droppedType;
                    ObjectsReference.Instance.droppedInventory.AddQuantity(droppedType, quantityToBuy);

                    newItemQuantity = monkeyMenData.droppedInventory[droppedType] - quantityToBuy;
                    monkeyMenData.droppedInventory[droppedType] = newItemQuantity;
                    
                    uiMerchant.merchantBuyUiDroppedInventory.uInventorySlots[droppedType].SetQuantity(newItemQuantity);
                    uiMerchant.merchantBuyUiDroppedInventory.RefreshUInventory();
                    break;
            }
        }
    }
}