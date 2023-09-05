using System.Collections.Generic;
using Enums;
using UnityEngine;

namespace UI.InGame.Inventory {
    public class UIIngredientsInventory : MonoBehaviour {
        public GenericDictionary<IngredientsType, UInventorySlot> inventorySlotsByIngredientsType;
        private Dictionary<IngredientsType, int> _itemsIndexByType;

        public void RefreshUInventory() {
            var inventory = ObjectsReference.Instance.ingredientsInventory.ingredientsInventory;

            foreach (var inventoryItem in inventory) {
                if (inventoryItem.Value > 0) {
                    inventorySlotsByIngredientsType[inventoryItem.Key].gameObject.SetActive(true);
                    inventorySlotsByIngredientsType[inventoryItem.Key].GetComponent<UInventorySlot>().SetQuantity(inventoryItem.Value);
                }

                else inventorySlotsByIngredientsType[inventoryItem.Key].gameObject.SetActive(false);
            }
        }
    }
}
