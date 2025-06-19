using System.Collections.Generic;

namespace Save.Templates {
    public class BananaManSavedData {
        public float health = 100;
        public float resistance = 100;

        public float xWorldPosition;
        public float yWorldPosition;
        public float zworldPosition;

        public float xWorldRotation = 0;
        public float yWorldRotation = 0;
        public float zWorldRotation = 0;

        public List<string> slots = new ();
        public int activeSlotIndex;

        public int bitKongQuantity;

        public bool hasFinishedTutorial = false;

        public List<string> discoveredRawMaterials = new();

        public Dictionary<string, int> bananaInventory = new() {
            {BananaType.BARANGAN.ToString(), 0},
            {BananaType.BLUE_JAVA.ToString(), 0},
            {BananaType.BURRO.ToString(), 0},
            {BananaType.CAVENDISH.ToString(), 0},
            {BananaType.GOLD_FINGER.ToString(), 0},
            {BananaType.GROS_MICHEL.ToString(), 0},
            {BananaType.LADY_FINGER.ToString(), 0},
            {BananaType.MATOKE.ToString(), 0},
            {BananaType.MUSA_VELUTINA.ToString(), 0},
            {BananaType.NANJANGUD.ToString(), 0},
            {BananaType.PISANG_RAJA.ToString(), 0},
            {BananaType.PLANTAIN.ToString(), 0},
            {BananaType.PRAYING_HANDS.ToString(), 0},
            {BananaType.RED.ToString(), 0},
            {BananaType.RINO_HORN.ToString(), 0},
            {BananaType.TINDOK.ToString(), 0}
        };

        public Dictionary<string, int> rawMaterialsInventory = new() {
            {RawMaterialType.ELECTRONIC.ToString(), 0},
            {RawMaterialType.BANANA_PEEL.ToString(), 0},
            {RawMaterialType.METAL.ToString(), 0},
            {RawMaterialType.FABRIC.ToString(), 0},
            {RawMaterialType.BATTERY.ToString(), 0},
            {RawMaterialType.SILICE.ToString(), 0},
            {RawMaterialType.YELLOW_DYE.ToString(), 0},
            {RawMaterialType.RED_DYE.ToString(), 0},
            {RawMaterialType.GREEN_DYE.ToString(), 0},
            {RawMaterialType.BLUE_DYE.ToString(), 0}
        };  

        public Dictionary<string, int> ingredientsInventory = new() {
            {IngredientsType.BANANA_DOG_BREAD.ToString(), 0},   
            {IngredientsType.BANANA_WITHOUT_SKIN.ToString(), 0}
        };

        public Dictionary<string, int> manufacturedInventory = new() {
            {ManufacturedItemsType.SPACESHIP_TOY.ToString(), 0},
            {ManufacturedItemsType.LAPINOU.ToString(), 0},
            {ManufacturedItemsType.BANANARAIGNEE.ToString(), 0},
            {ManufacturedItemsType.BANANAVIAIRE.ToString(), 0}
        };

        public Dictionary<string, int> foodInventory = new() {
            {FoodType.BANANA_DOG.ToString(), 0}
        };
    }
}