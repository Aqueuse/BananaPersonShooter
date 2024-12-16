using System.Collections.Generic;
using UnityEngine;

namespace InGame.Inventory {
    public class IngredientsInventory : MonoBehaviour {
        public Dictionary<IngredientsType, int> ingredientsInventory= new () {
            {IngredientsType.BANANA_DOG_BREAD, 0},   
            {IngredientsType.BANANA_WITHOUT_SKIN, 0}
        };
        
        public int AddQuantity(IngredientsType ingredientsType, int quantity) {
            if (ingredientsInventory[ingredientsType] > 10000) return ingredientsInventory[ingredientsType];
            
            ingredientsInventory[ingredientsType] += quantity;

            return ingredientsInventory[ingredientsType];
        }
        
        public int GetQuantity(IngredientsType ingredientsType) {
            return ingredientsInventory[ingredientsType];
        }
        
        public int RemoveQuantity(IngredientsType ingredientsType, int quantity) {
            if (ingredientsInventory[ingredientsType] > quantity) {
                ingredientsInventory[ingredientsType] -= quantity;
                return ingredientsInventory[ingredientsType];
            }

            ingredientsInventory[ingredientsType] = 0;
            return 0;
        }

        public void ResetInventory() {
            ingredientsInventory= new () {
                {IngredientsType.BANANA_DOG_BREAD, 0},   
                {IngredientsType.BANANA_WITHOUT_SKIN, 0}
            };
        }
    }
}