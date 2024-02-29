using InGame.Items.ItemsProperties.Buildables;
using UnityEngine;

namespace InGame.Inventory {
    public class RawMaterialsInventory : MonoBehaviour {
        private GenericDictionary<RawMaterialType, int> rawMaterialsInventory;
        [HideInInspector] public GenericDictionary<RawMaterialType, int> rawMaterialsForPlateform;

        private void Start() {
            rawMaterialsForPlateform = ObjectsReference.Instance.meshReferenceScriptableObject
                .buildablePropertiesScriptableObjects[BuildableType.PLATEFORM].rawMaterialsWithQuantity;
            
            rawMaterialsInventory = ObjectsReference.Instance.bananaMan.inventories.rawMaterialsInventory;
            ObjectsReference.Instance.uiBananasInventory.inventoryScriptableObject = ObjectsReference.Instance.bananaMan.inventories;
        }

        public void AddQuantity(RawMaterialType rawMaterialType, int quantity) {
            if (rawMaterialsInventory[rawMaterialType] > 10000) return;

            rawMaterialsInventory[rawMaterialType] += quantity;

            var rawMaterialItem = ObjectsReference.Instance.uiRawMaterialsInventory.uInventorySlots[rawMaterialType];
            
            rawMaterialItem.gameObject.SetActive(true);
            rawMaterialItem.SetQuantity(rawMaterialsInventory[rawMaterialType]);

            ObjectsReference.Instance.uiQueuedMessages.AddToInventory(rawMaterialItem.itemScriptableObject, quantity);
        }
        
        public void RemoveQuantity(RawMaterialType rawMaterialType, int quantity) {
            var rawMaterialItem = ObjectsReference.Instance.uiRawMaterialsInventory.uInventorySlots[rawMaterialType];

            if (rawMaterialsInventory[rawMaterialType] > quantity) {
                rawMaterialsInventory[rawMaterialType] -= quantity;
                rawMaterialItem.SetQuantity(rawMaterialsInventory[rawMaterialType]);
            }

            else {
                rawMaterialsInventory[rawMaterialType] = 0;
                rawMaterialItem.SetQuantity(0);
                rawMaterialItem.gameObject.SetActive(false);
            }
            
            ObjectsReference.Instance.uiQueuedMessages.RemoveFromInventory(rawMaterialItem.itemScriptableObject, quantity);
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
        
        public bool HasCraftingIngredientsForPlateform() {
            foreach (var craftingIngredient in rawMaterialsForPlateform) {
                if (rawMaterialsInventory[craftingIngredient.Key] < craftingIngredient.Value) return false;
            }

            return true;
        }
    }
}