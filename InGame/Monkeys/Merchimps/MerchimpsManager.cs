using InGame.Items.ItemsProperties;
using UnityEngine;

namespace InGame.Monkeys.Merchimps {
    public class MerchimpsManager : MonoBehaviour {
        public Merchimp activeMerchimp;
        public ItemScriptableObject activeItemScriptableObject;
        
        public int GetMerchantItemQuantity(ItemScriptableObject itemScriptableObject) {
            switch (itemScriptableObject.itemCategory) {
                case ItemCategory.INGREDIENT:
                    return activeMerchimp.merchantPropertiesScriptableObject.ingredientsInventory[itemScriptableObject.ingredientsType];

                case ItemCategory.MANUFACTURED_ITEM:
                    return activeMerchimp.merchantPropertiesScriptableObject.manufacturedItemsInventory[itemScriptableObject.manufacturedItemsType];

                case ItemCategory.RAW_MATERIAL:
                    return activeMerchimp.merchantPropertiesScriptableObject.rawMaterialsInventory[itemScriptableObject.rawMaterialType];
            }

            return 0;
        }

        public int GetBananaManItemQuantity(ItemScriptableObject itemScriptableObject) {
            switch (itemScriptableObject.itemCategory) {
                case ItemCategory.BANANA:
                    return ObjectsReference.Instance.bananaMan.inventories.bananasInventory[itemScriptableObject.bananaType];

                case ItemCategory.MANUFACTURED_ITEM:
                    return ObjectsReference.Instance.bananaMan.inventories.manufacturedItemsInventory[itemScriptableObject.manufacturedItemsType];

                case ItemCategory.INGREDIENT:
                    return ObjectsReference.Instance.bananaMan.inventories.ingredientsInventory[itemScriptableObject.ingredientsType];
            }

            return 0;
        }
    }
}
