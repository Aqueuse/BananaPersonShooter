using System.Collections.Generic;
using InGame.Items.ItemsProperties.Buildables;
using UnityEngine;

namespace InGame.Inventory {
    public class RawMaterialInventory : MonoBehaviour {
        private Dictionary<RawMaterialType, int> rawMaterialsInventory;

        private void Start() {
            rawMaterialsInventory = ObjectsReference.Instance.bananaMan.bananaManData.rawMaterialInventory;
        }

        public void AddQuantity(RawMaterialType rawMaterialType, int quantity) {
            if (rawMaterialsInventory[rawMaterialType] > 10000) return;

            rawMaterialsInventory[rawMaterialType] += quantity;

            var droppedItem = ObjectsReference.Instance.bananaManUIRawMaterialsInventory.uInventorySlots[rawMaterialType];
            
            droppedItem.gameObject.SetActive(true);
            droppedItem.SetQuantity(rawMaterialsInventory[rawMaterialType]);

            ObjectsReference.Instance.uiQueuedMessages.AddToInventory(droppedItem.itemScriptableObject, quantity);

            ObjectsReference.Instance.uiFlippers.RefreshActiveBuildableAvailability();
        }
        
        public void RemoveQuantity(RawMaterialType rawMaterialType, int quantity) {
            var droppedItem = ObjectsReference.Instance.bananaManUIRawMaterialsInventory.uInventorySlots[rawMaterialType];

            if (rawMaterialsInventory[rawMaterialType] > quantity) {
                rawMaterialsInventory[rawMaterialType] -= quantity;
                droppedItem.SetQuantity(rawMaterialsInventory[rawMaterialType]);
            }

            else {
                rawMaterialsInventory[rawMaterialType] = 0;
                droppedItem.SetQuantity(0);
                droppedItem.gameObject.SetActive(false);
            }
            
            ObjectsReference.Instance.uiQueuedMessages.RemoveFromInventory(droppedItem.itemScriptableObject, quantity);
            
            ObjectsReference.Instance.uiFlippers.RefreshActiveBuildableAvailability();
        }
            
        public bool HasCraftingIngredients(BuildablePropertiesScriptableObject buildablePropertiesScriptableObject) {
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