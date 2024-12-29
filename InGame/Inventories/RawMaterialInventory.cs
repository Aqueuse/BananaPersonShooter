using System.Collections.Generic;
using InGame.Items.ItemsProperties;
using InGame.Items.ItemsProperties.Buildables;

namespace InGame.Inventories {
    public class RawMaterialInventory : Inventory {
        private RawMaterialType rawMaterialType;
        
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
        
        public override int AddQuantity(ItemScriptableObject itemScriptableObject, int quantity) {
            rawMaterialType = itemScriptableObject.rawMaterialType;
            
            if (rawMaterialsInventory[rawMaterialType] > 10000) return 9999;

            rawMaterialsInventory[rawMaterialType] += quantity;

            return rawMaterialsInventory[rawMaterialType];
        }
        
        public override int RemoveQuantity(ItemScriptableObject itemScriptableObject, int quantity) {
            rawMaterialType = itemScriptableObject.rawMaterialType;
            
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
                if (rawMaterialsInventory[craftingIngredient.Key.rawMaterialType] < craftingIngredient.Value) return false;
            }

            return true;
        }
        
        public override int GetQuantity(ItemScriptableObject itemScriptableObject) {
            return rawMaterialsInventory[itemScriptableObject.rawMaterialType];
        }

        public override void ResetInventory() {
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