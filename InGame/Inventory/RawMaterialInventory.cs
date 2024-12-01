using System.Collections.Generic;
using InGame.Items.ItemsProperties.Buildables;
using UnityEngine;

namespace InGame.Inventory {
    public class RawMaterialInventory : MonoBehaviour {
        public Dictionary<RawMaterialType, int> rawMaterialsInventory;
        
        public void AddQuantity(RawMaterialType rawMaterialType, int quantity) {
            if (rawMaterialsInventory[rawMaterialType] > 10000) return;

            rawMaterialsInventory[rawMaterialType] += quantity;

            var droppedItem = ObjectsReference.Instance.bananaManUIRawMaterialsInventory.uInventorySlots[rawMaterialType];
            
            droppedItem.gameObject.SetActive(true);
            droppedItem.SetQuantity(rawMaterialsInventory[rawMaterialType]);

            ObjectsReference.Instance.uiQueuedMessages.AddToInventory(droppedItem.itemScriptableObject, quantity);

            ObjectsReference.Instance.uiFlippers.RefreshActiveBuildableAvailability();
        }
        
        public int RemoveQuantity(RawMaterialType rawMaterialType, int quantity) {
            if (rawMaterialsInventory[rawMaterialType] > quantity) {
                rawMaterialsInventory[rawMaterialType] -= quantity;
            }

            else {
                rawMaterialsInventory[rawMaterialType] = 0;
            }

            return rawMaterialsInventory[rawMaterialType];
        }
            
        public bool HasCraftingIngredients(BuildablePropertiesScriptableObject buildablePropertiesScriptableObject) {
            if (buildablePropertiesScriptableObject.buildableType == BuildableType.EMPTY) return false;
            
            var _craftingIngredients = buildablePropertiesScriptableObject.rawMaterialsWithQuantity;

            foreach (var craftingIngredient in _craftingIngredients) {
                if (rawMaterialsInventory[craftingIngredient.Key] < craftingIngredient.Value) return false;
            }

            return true;
        }
        
        public bool HasCraftingIngredients(BuildableType buildableType) {
            var _craftingIngredients = ObjectsReference.Instance.meshReferenceScriptableObject.buildablePropertiesScriptableObjects[buildableType].rawMaterialsWithQuantity;

            foreach (var craftingIngredient in _craftingIngredients) {
                if (rawMaterialsInventory[craftingIngredient.Key] < craftingIngredient.Value) return false;
            }

            return true;
        }
        
        public int GetQuantity(RawMaterialType rawMaterialType) {
            return rawMaterialsInventory[rawMaterialType];
        }
    }
}