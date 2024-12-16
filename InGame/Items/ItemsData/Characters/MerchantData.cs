using System.Collections.Generic;

namespace InGame.Items.ItemsData.Characters {
    
    public class MerchantData {
        public Dictionary<string, int> rawMaterialsInventory = new() {
            {RawMaterialType.ELECTRONIC.ToString(), 0},
            {RawMaterialType.BANANA_PEEL.ToString(), 0},
            {RawMaterialType.METAL.ToString(), 0},
            {RawMaterialType.FABRIC.ToString(), 0},
            {RawMaterialType.BATTERY.ToString(), 0}
        };

        public Dictionary<string, int> ingredientsInventory = new() {
            {IngredientsType.BANANA_DOG_BREAD.ToString(), 0},
            {IngredientsType.BANANA_WITHOUT_SKIN.ToString(), 0}
        };

        public Dictionary<string, int> manufacturedInventory = new() {
            {ManufacturedItemsType.SPACESHIP_TOY.ToString(), 0},
            {ManufacturedItemsType.BANANARAIGNEE.ToString(), 0},
            {ManufacturedItemsType.LAPINOU.ToString(), 0},
            {ManufacturedItemsType.BANANAVIAIRE.ToString(), 0}
        };
    }
}