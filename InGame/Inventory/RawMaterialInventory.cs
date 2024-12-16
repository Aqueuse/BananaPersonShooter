using System.Collections.Generic;
using InGame.Items.ItemsProperties.Buildables;
using UnityEngine;

namespace InGame.Inventory {
    public class RawMaterialInventory : MonoBehaviour {
        public Dictionary<RawMaterialType, int> rawMaterialsInventory = new () {
            {RawMaterialType.ELECTRONIC, 0},
            {RawMaterialType.BANANA_PEEL, 0},
            {RawMaterialType.METAL, 0},
            {RawMaterialType.FABRIC, 0},
            {RawMaterialType.BATTERY, 0},
            {RawMaterialType.SILICE, 0},
            {RawMaterialType.YELLOW_DYE, 0},
            {RawMaterialType.RED_DYE, 0},
            {RawMaterialType.GREEN_DYE, 0},
            {RawMaterialType.BLUE_DYE, 0}
        };
        
        public void AddQuantity(RawMaterialType rawMaterialType, int quantity) {
            if (rawMaterialsInventory[rawMaterialType] > 10000) return;

            rawMaterialsInventory[rawMaterialType] += quantity;
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

        public void ResetInventory() {
            rawMaterialsInventory = new () {
                {RawMaterialType.ELECTRONIC, 0},
                {RawMaterialType.BANANA_PEEL, 0},
                {RawMaterialType.METAL, 0},
                {RawMaterialType.FABRIC, 0},
                {RawMaterialType.BATTERY, 0},
                {RawMaterialType.SILICE, 0},
                {RawMaterialType.YELLOW_DYE, 0},
                {RawMaterialType.RED_DYE, 0},
                {RawMaterialType.GREEN_DYE, 0},
                {RawMaterialType.BLUE_DYE, 0}
            };
        }
    }
}