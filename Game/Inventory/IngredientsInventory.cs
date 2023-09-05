using UnityEngine;

namespace Game.Inventory {
    public class IngredientsInventory : MonoBehaviour {
        public GenericDictionary<IngredientsType, int> ingredientsInventory;
        
        public GameObject lastselectedInventoryItem;

        public void AddQuantity(IngredientsType ingredientsType, int quantity) {
            if (ingredientsInventory[ingredientsType] > 10000) return;
            
            ingredientsInventory[ingredientsType] += quantity;
            ObjectsReference.Instance.uiSlotsManager.RefreshQuantityInQuickSlot();

            var ingredientItem = ObjectsReference.Instance.uiIngredientsInventory.inventorySlotsByIngredientsType[ingredientsType];
            ingredientItem.gameObject.SetActive(true);
            ingredientItem.SetQuantity(ingredientsInventory[ingredientsType]);
            
            ObjectsReference.Instance.uiQueuedMessages.AddToInventory(ingredientItem.itemScriptableObject, quantity);
        }

        public int GetQuantity(IngredientsType ingredientsType) {
            return ingredientsInventory[ingredientsType];
        }

        public void RemoveQuantity(IngredientsType ingredientsType, int quantity) {
            var ingredientItem = ObjectsReference.Instance.uiIngredientsInventory.inventorySlotsByIngredientsType[ingredientsType];
            
            if (ingredientsInventory[ingredientsType] > quantity) {
                ingredientsInventory[ingredientsType] -= quantity;
                ingredientItem.SetQuantity(ingredientsInventory[ingredientsType]);
            }
            
            else {
                ingredientsInventory[ingredientsType] = 0;
                ingredientItem.SetQuantity(0);
                ingredientItem.gameObject.SetActive(false);
            }
            
            ObjectsReference.Instance.uiSlotsManager.RefreshQuantityInQuickSlot();
        }
    }
}