using System.Collections.Generic;
using InGame.Items.ItemsProperties;

namespace InGame.Inventories {
    public class IngredientsInventory : Inventory {
        public Dictionary<IngredientsType, int> ingredientsInventory= new () {
            {IngredientsType.BANANA_DOG_BREAD, 0},   
            {IngredientsType.BANANA_WITHOUT_SKIN, 0}
        };

        private IngredientsType ingredientsType;

        public override int AddQuantity(ItemScriptableObject itemScriptableObject, int quantity) {
            ingredientsType = itemScriptableObject.ingredientsType;
            
            if (ingredientsInventory[ingredientsType] > 10000) return ingredientsInventory[ingredientsType];

            ingredientsInventory[ingredientsType] += quantity;

            return ingredientsInventory[ingredientsType];
        }

        public override int GetQuantity(ItemScriptableObject itemScriptableObject) {
            return ingredientsInventory[ingredientsType];
        }

        public override int RemoveQuantity(ItemScriptableObject itemScriptableObject, int quantity) {
            ingredientsType = itemScriptableObject.ingredientsType;
            
            if (ingredientsInventory[ingredientsType] > quantity) {
                ingredientsInventory[ingredientsType] -= quantity;
                return ingredientsInventory[ingredientsType];
            }

            ingredientsInventory[ingredientsType] = 0;
            return 0;
        }

        public override void ResetInventory() {
            ingredientsInventory= new () {
                {IngredientsType.BANANA_DOG_BREAD, 0},   
                {IngredientsType.BANANA_WITHOUT_SKIN, 0}
            };
        }
    }
}