using ItemsProperties.Buildables;
using ItemsProperties.Buildables.VisitorsBuildable;
using UnityEngine;

namespace Game.Inventory {
    public class RawMaterialsInventory : MonoBehaviour {
        public GenericDictionary<RawMaterialType, int> rawMaterialsInventory;
        public GenericDictionary<RawMaterialType, int> rawMaterialsForPlateform;

        private void Start() {
            rawMaterialsForPlateform = ObjectsReference.Instance.scriptableObjectManager._meshReferenceScriptableObject
                .buildablePropertiesScriptableObjects[BuildableType.PLATEFORM].rawMaterialsWithQuantity;
        }

        public void AddQuantity(RawMaterialType rawMaterialType, int quantity) {
            if (rawMaterialsInventory[rawMaterialType] > 10000) return;

            rawMaterialsInventory[rawMaterialType] += quantity;

            var rawMaterialItem = ObjectsReference.Instance.uiRawMaterialsInventory.inventorySlotsByRawMaterialType[rawMaterialType];
            
            rawMaterialItem.gameObject.SetActive(true);
            rawMaterialItem.SetQuantity(rawMaterialsInventory[rawMaterialType]);
            
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
        }
            
        public bool HasCraftingIngredients(BuildablePropertiesScriptableObject buildablePropertiesScriptableObject) {
            var _craftingIngredients = buildablePropertiesScriptableObject.rawMaterialsWithQuantity;

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