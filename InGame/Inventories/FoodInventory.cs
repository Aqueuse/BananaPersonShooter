using System.Collections.Generic;
using InGame.Items.ItemsProperties;

namespace InGame.Inventories {
    public class FoodInventory : Inventory {
        public Dictionary<FoodType, int> foodInventory = new () {
            {FoodType.BANANA_DOG, 0}
        };

        public FoodType foodType;
        
        public override int AddQuantity(ItemScriptableObject itemScriptableObject, int quantity) {
            foodType = itemScriptableObject.foodType;
            
            foodInventory.TryAdd(foodType, 0);
            
            if (foodInventory[foodType] > 10000) return foodInventory[foodType];
            
            foodInventory[foodType] += quantity;
            
            ObjectsReference.Instance.bottomSlots.RefreshSlotsQuantities();

            return foodInventory[foodType];
        }

        public override int GetQuantity(ItemScriptableObject itemScriptableObject) {
            return foodInventory[foodType];
        }

        public override int RemoveQuantity(ItemScriptableObject itemScriptableObject, int quantity) {
            foodType = itemScriptableObject.foodType;

            if (foodInventory[foodType] > quantity) {
                foodInventory[foodType] -= quantity;
            }

            else {
                foodInventory[foodType] = 0;
            }

            ObjectsReference.Instance.bottomSlots.RefreshSlotsQuantities();

            return foodInventory[foodType];
        }

        public override void ResetInventory() {
            foodInventory = new () {
                {FoodType.BANANA_DOG, 0}
            };
        }
    }
}