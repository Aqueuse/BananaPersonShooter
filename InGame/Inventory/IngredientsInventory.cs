using System.Collections.Generic;
using UnityEngine;

namespace InGame.Inventory {
    public class IngredientsInventory : MonoBehaviour {
        private Dictionary<IngredientsType, int> ingredientsInventory;
        
        private void Start() {
            ingredientsInventory = ObjectsReference.Instance.bananaMan.bananaManData.ingredientsInventory;
        }
        
        public void AddQuantity(IngredientsType ingredientsType, int quantity) {
            if (ingredientsInventory[ingredientsType] > 10000) return;
            
            ingredientsInventory[ingredientsType] += quantity;

            var ingredientItem = ObjectsReference.Instance.uiIngredientsInventory.uInventorySlots[ingredientsType];
            ingredientItem.gameObject.SetActive(true);
            ingredientItem.SetQuantity(ingredientsInventory[ingredientsType]);
            
            ObjectsReference.Instance.uiQueuedMessages.AddToInventory(ingredientItem.itemScriptableObject, quantity);
        }

        public void RemoveQuantity(IngredientsType ingredientsType, int quantity) {
            var ingredientItem = ObjectsReference.Instance.uiIngredientsInventory.uInventorySlots[ingredientsType];
            
            if (ingredientsInventory[ingredientsType] > quantity) {
                ingredientsInventory[ingredientsType] -= quantity;
                ingredientItem.SetQuantity(ingredientsInventory[ingredientsType]);
            }
            
            else {
                ingredientsInventory[ingredientsType] = 0;
                ingredientItem.SetQuantity(0);
                ingredientItem.gameObject.SetActive(false);
            }
        }
    }
}