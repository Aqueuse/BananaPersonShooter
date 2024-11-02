using InGame.Items.ItemsProperties;
using UnityEngine;

namespace InGame.Monkeys.Merchimps {
    public class MerchimpsManager : MonoBehaviour {
        [HideInInspector] public MerchimpBehaviour activeMerchimpBehaviour;
        [HideInInspector] public ItemScriptableObject activeItemScriptableObject;
        
        public int GetMerchantItemQuantity(ItemScriptableObject itemScriptableObject) {
            switch (itemScriptableObject.itemCategory) {
                case ItemCategory.INGREDIENT:
                    return activeMerchimpBehaviour.monkeyMenBehaviour.monkeyMenData.ingredientsInventory[itemScriptableObject.ingredientsType];

                case ItemCategory.MANUFACTURED_ITEM:
                    return activeMerchimpBehaviour.monkeyMenBehaviour.monkeyMenData.manufacturedItemsInventory[itemScriptableObject.manufacturedItemsType];

                case ItemCategory.DROPPED:
                    return activeMerchimpBehaviour.monkeyMenBehaviour.monkeyMenData.droppedInventory[itemScriptableObject.droppedType];
            }

            return 0;
        }

        public int GetBananaManItemQuantity(ItemScriptableObject itemScriptableObject) {
            switch (itemScriptableObject.itemCategory) {
                case ItemCategory.BANANA:
                    return ObjectsReference.Instance.bananaMan.bananaManData.bananasInventory[itemScriptableObject.bananaType];

                case ItemCategory.MANUFACTURED_ITEM:
                    return ObjectsReference.Instance.bananaMan.bananaManData.manufacturedItemsInventory[itemScriptableObject.manufacturedItemsType];

                case ItemCategory.INGREDIENT:
                    return ObjectsReference.Instance.bananaMan.bananaManData.ingredientsInventory[itemScriptableObject.ingredientsType];
            }

            return 0;
        }
    }
}
