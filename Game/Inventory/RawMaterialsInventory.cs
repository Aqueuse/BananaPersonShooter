using Enums;
using UnityEngine;

namespace Game.Inventory {
    public class RawMaterialsInventory : MonoBehaviour {
        public GenericDictionary<RawMaterialType, int> rawMaterialsInventory;

        public void AddQuantity(RawMaterialType rawMaterialType, int quantity) {
            if (rawMaterialsInventory[rawMaterialType] > 10000) return;

            rawMaterialsInventory[rawMaterialType] += quantity;

            var rawMaterialItem = ObjectsReference.Instance.uiRawMaterialsInventory.inventorySlotsByRawMaterialType[rawMaterialType];
            
            rawMaterialItem.gameObject.SetActive(true);
            rawMaterialItem.SetQuantity(rawMaterialsInventory[rawMaterialType]);
            
            ObjectsReference.Instance.uiQuickSlotsManager.RefreshQuantityInQuickSlot();
            
            ObjectsReference.Instance.uiQueuedMessages.AddToInventory(rawMaterialItem.itemScriptableObject, quantity);
        }

        public int GetQuantity(RawMaterialType rawMaterialType) {
            return rawMaterialsInventory[rawMaterialType];
        }

        public void RemoveQuantity(RawMaterialType rawMaterialType, int quantity) {
            var rawMaterialItem = ObjectsReference.Instance.uiRawMaterialsInventory.inventorySlotsByRawMaterialType[rawMaterialType];

            if (rawMaterialsInventory[rawMaterialType] > quantity) {
                rawMaterialsInventory[rawMaterialType] -= quantity;
                rawMaterialItem.SetQuantity(rawMaterialsInventory[rawMaterialType]);
            }

            else {
                rawMaterialsInventory[rawMaterialType] = 0;
                rawMaterialItem.SetQuantity(0);
                rawMaterialItem.gameObject.SetActive(false);
            }

            ObjectsReference.Instance.uiQuickSlotsManager.RefreshQuantityInQuickSlot();
        }
        
        public bool HasCraftingIngredients(GenericDictionary<RawMaterialType, int> craftingIngredients) {
            foreach (var craftingIngredient in craftingIngredients) {
                if (rawMaterialsInventory[craftingIngredient.Key] < craftingIngredient.Value) return false;
            }

            return true;
        }
    }
}