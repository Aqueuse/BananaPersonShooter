using InGame.Items.ItemsData;
using InGame.Items.ItemsData.Characters;
using InGame.Items.ItemsProperties;
using UI.InGame.Merchimps;
using UnityEngine;

namespace InGame.Player.BananaGunActions {
    public class Trade : MonoBehaviour {
        public MonkeyMenData monkeyMenData;
        
        private BananaMan bananaMan;
        private UIMerchant uiMerchant;

        [Range(0, 999)] private int newItemQuantity;

        private void Start() {
            uiMerchant = ObjectsReference.Instance.uiMerchant;
            bananaMan = ObjectsReference.Instance.bananaMan;
        }

        public void Sell(ItemScriptableObject itemScriptableObject) {
            var quantityToSell = (int)uiMerchant.sellUiMerchantSliderMenu.quantitySlider.value;
            var bitkongValue = itemScriptableObject.bitKongValue * quantityToSell;
            
            bananaMan.bananaManData.bitKongQuantity += bitkongValue;
            monkeyMenData.bitKongQuantity -= bitkongValue;
            ObjectsReference.Instance.uInventoriesManager.SetBitKongQuantity(bananaMan.bananaManData.bitKongQuantity);
            uiMerchant.RefreshBitkongQuantities();

            uiMerchant.sellUiMerchantSliderMenu.quantitySlider.value = 0;

            switch (itemScriptableObject.droppedType) {
                case DroppedType.INGREDIENTS:
                    var ingredientType = itemScriptableObject.ingredientsType;
                    ObjectsReference.Instance.ingredientsInventory.RemoveQuantity(ingredientType, quantityToSell);

                    newItemQuantity = monkeyMenData.ingredientsInventory[ingredientType] + quantityToSell;
                    monkeyMenData.ingredientsInventory[ingredientType] = newItemQuantity;

                    uiMerchant.merchantSellUiIngredientsInventory.uInventorySlots[ingredientType].SetQuantity(newItemQuantity);
                    uiMerchant.merchantSellUiIngredientsInventory.RefreshUInventory();
                    break;
                
                case DroppedType.MANUFACTURED_ITEMS:
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
            var quantityToBuy = (int)uiMerchant.buyUiMerchantSliderMenu.quantitySlider.value;
            var bitkongValue = itemScriptableObject.bitKongValue * quantityToBuy;
            
            // not enough monkey money (；′⌒`)
            if (bananaMan.bananaManData.bitKongQuantity < bitkongValue) return;

            bananaMan.bananaManData.bitKongQuantity -= itemScriptableObject.bitKongValue;
            monkeyMenData.bitKongQuantity += bitkongValue;
            ObjectsReference.Instance.uInventoriesManager.SetBitKongQuantity(bananaMan.bananaManData.bitKongQuantity);
            uiMerchant.RefreshBitkongQuantities();

            uiMerchant.buyUiMerchantSliderMenu.quantitySlider.value = 0;

            // buy
            switch (itemScriptableObject.droppedType) {
                case DroppedType.INGREDIENTS:
                    var ingredientType = itemScriptableObject.ingredientsType;
                    ObjectsReference.Instance.ingredientsInventory.AddQuantity(ingredientType, quantityToBuy);

                    newItemQuantity = monkeyMenData.ingredientsInventory[ingredientType] - quantityToBuy;
                    monkeyMenData.ingredientsInventory[ingredientType] = newItemQuantity;
                    
                    uiMerchant.merchantBuyUiIngredientsInventory.uInventorySlots[ingredientType].SetQuantity(newItemQuantity);
                    uiMerchant.merchantBuyUiIngredientsInventory.RefreshUInventory();
                    uiMerchant.merchantSellUiIngredientsInventory.RefreshUInventory();
                    break;

                case DroppedType.MANUFACTURED_ITEMS:
                    var manufacturedItemType = itemScriptableObject.manufacturedItemsType;
                    ObjectsReference.Instance.manufacturedItemsInventory.AddQuantity(manufacturedItemType, quantityToBuy);

                    newItemQuantity = monkeyMenData.manufacturedItemsInventory[manufacturedItemType] - quantityToBuy;
                    monkeyMenData.manufacturedItemsInventory[manufacturedItemType] = newItemQuantity;
                    
                    uiMerchant.merchantBuyUiManufacturedItemsInventory.uInventorySlots[manufacturedItemType].SetQuantity(newItemQuantity);
                    uiMerchant.merchantBuyUiManufacturedItemsInventory.RefreshUInventory();
                    uiMerchant.merchantSellUiManufacturedItemsInventory.RefreshUInventory();
                    break;

                case DroppedType.RAW_MATERIAL:
                    var rawMaterialType = itemScriptableObject.rawMaterialType;
                    ObjectsReference.Instance.rawMaterialInventory.AddQuantity(rawMaterialType, quantityToBuy);

                    newItemQuantity = monkeyMenData.rawMaterialsInventory[rawMaterialType] - quantityToBuy;
                    monkeyMenData.rawMaterialsInventory[rawMaterialType] = newItemQuantity;
                    
                    uiMerchant.merchantBuyUIRawMaterialsInventory.uInventorySlots[rawMaterialType].SetQuantity(newItemQuantity);
                    uiMerchant.merchantBuyUIRawMaterialsInventory.RefreshUInventory();
                    break;
            }
        }
    }
}