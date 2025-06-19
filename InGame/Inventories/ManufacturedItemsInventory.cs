using System.Collections.Generic;
using InGame.Items.ItemsProperties;

namespace InGame.Inventories {
    public class ManufacturedItemsInventory : Inventory {
        public Dictionary<ManufacturedItemsType, int> manufacturedItemsInventory = new () {
            {ManufacturedItemsType.SPACESHIP_TOY, 0},
            {ManufacturedItemsType.LAPINOU, 0},
            {ManufacturedItemsType.BANANARAIGNEE, 0},
            {ManufacturedItemsType.BANANAVIAIRE, 0}
        };

        private ManufacturedItemsType manufacturedItemsType;
        
        public override int AddQuantity(ItemScriptableObject itemScriptableObject, int quantity) {
            manufacturedItemsType = itemScriptableObject.manufacturedItemsType;
            
            if (manufacturedItemsInventory[manufacturedItemsType] > 10000) return 9999;

            manufacturedItemsInventory[manufacturedItemsType] += quantity;
            
            ObjectsReference.Instance.bottomSlots.RefreshSlotsQuantities();

            return manufacturedItemsInventory[manufacturedItemsType];
        }
        
        public override int RemoveQuantity(ItemScriptableObject itemScriptableObject, int quantity) {
            manufacturedItemsType = itemScriptableObject.manufacturedItemsType;
            
            if (manufacturedItemsInventory[manufacturedItemsType] > quantity) {
                manufacturedItemsInventory[manufacturedItemsType] -= quantity;
            }

            else {
                manufacturedItemsInventory[manufacturedItemsType] = 0;
            }
            
            ObjectsReference.Instance.bottomSlots.RefreshSlotsQuantities();

            return manufacturedItemsInventory[manufacturedItemsType];
        }

        public override int GetQuantity(ItemScriptableObject itemScriptableObject) {
            manufacturedItemsType = itemScriptableObject.manufacturedItemsType;
            
            return manufacturedItemsInventory[manufacturedItemsType];
        }

        public override void ResetInventory() {
            manufacturedItemsInventory = new () {
                {ManufacturedItemsType.SPACESHIP_TOY, 0},
                {ManufacturedItemsType.LAPINOU, 0},
                {ManufacturedItemsType.BANANARAIGNEE, 0},
                {ManufacturedItemsType.BANANAVIAIRE, 0}
            };
        }
    }
}