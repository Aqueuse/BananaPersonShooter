using System.Collections.Generic;
using Enums;
using UnityEngine;

namespace UI.InGame.Inventory {
    public class UIIngredientsInventory : MonoBehaviour {
        [SerializeField] private Transform inventoryContentTransform; 

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
        
        public void UnselectAllSlots() {
            foreach (var inventoryItem in inventorySlotsByIngredientsType) {
                inventoryItem.Value.UnselectInventorySlot();
            }
        }

        public void SelectFirstSlot() {
            UnselectAllSlots();

            if (inventoryContentTransform.childCount == 0) return;
            
            foreach (var slot in inventoryContentTransform.GetComponentsInChildren<UInventorySlot>()) {
                if (slot.gameObject.activeInHierarchy) {
                    slot.GetComponent<UInventorySlot>().SelectInventorySlot();
                    break;
                }
            }
        }
    }
}
