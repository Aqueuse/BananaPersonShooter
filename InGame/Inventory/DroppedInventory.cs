using System.Collections.Generic;
using InGame.Items.ItemsProperties.Buildables;
using UnityEngine;

namespace InGame.Inventory {
    public class DroppedInventory : MonoBehaviour {
        private Dictionary<DroppedType, int> droppedInventory;

        private void Start() {
            droppedInventory = ObjectsReference.Instance.bananaMan.bananaManData.droppedInventory;
        }

        public void AddQuantity(DroppedType droppedType, int quantity) {
            if (droppedInventory[droppedType] > 10000) return;

            droppedInventory[droppedType] += quantity;

            var droppedItem = ObjectsReference.Instance.uiDroppedInventory.uInventorySlots[droppedType];
            
            droppedItem.gameObject.SetActive(true);
            droppedItem.SetQuantity(droppedInventory[droppedType]);

            ObjectsReference.Instance.uiQueuedMessages.AddToInventory(droppedItem.itemScriptableObject, quantity);

            ObjectsReference.Instance.uiFlippers.RefreshActiveBuildableAvailability();
            if (ObjectsReference.Instance.gameManager.gameContext == GameContext.IN_MINICHIMP_VIEW) {
                ObjectsReference.Instance.uiStats.RefreshStats();
            }
        }
        
        public void RemoveQuantity(DroppedType droppedType, int quantity) {
            var droppedItem = ObjectsReference.Instance.uiDroppedInventory.uInventorySlots[droppedType];

            if (droppedInventory[droppedType] > quantity) {
                droppedInventory[droppedType] -= quantity;
                droppedItem.SetQuantity(droppedInventory[droppedType]);
            }

            else {
                droppedInventory[droppedType] = 0;
                droppedItem.SetQuantity(0);
                droppedItem.gameObject.SetActive(false);
            }
            
            ObjectsReference.Instance.uiQueuedMessages.RemoveFromInventory(droppedItem.itemScriptableObject, quantity);
            
            ObjectsReference.Instance.uiFlippers.RefreshActiveBuildableAvailability();
            if (ObjectsReference.Instance.gameManager.gameContext == GameContext.IN_MINICHIMP_VIEW) {
                ObjectsReference.Instance.uiStats.RefreshStats();
            }
        }
            
        public bool HasCraftingIngredients(BuildablePropertiesScriptableObject buildablePropertiesScriptableObject) {
            var _craftingIngredients = buildablePropertiesScriptableObject.rawMaterialsWithQuantity;

            foreach (var craftingIngredient in _craftingIngredients) {
                if (droppedInventory[craftingIngredient.Key] < craftingIngredient.Value) return false;
            }

            return true;
        }
        
        public bool HasCraftingIngredients(BuildableType buildableType) {
            var _craftingIngredients = ObjectsReference.Instance.meshReferenceScriptableObject.buildablePropertiesScriptableObjects[buildableType].rawMaterialsWithQuantity;

            foreach (var craftingIngredient in _craftingIngredients) {
                if (droppedInventory[craftingIngredient.Key] < craftingIngredient.Value) return false;
            }

            return true;
        }
        
        public int GetQuantity(DroppedType droppedType) {
            return droppedInventory[droppedType];
        }
    }
}