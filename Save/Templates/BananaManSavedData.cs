using System.Collections.Generic;

namespace Save.Templates {
    public class BananaManSavedData {
        public float health = 100;
        public float resistance = 100;
        
        public float xWorldPosition = -1022.773f;
        public float yWorldPosition = 0.809f;
        public float zworldPosition = -2651.568f;

        public float xWorldRotation = 0;
        public float yWorldRotation = -0.495293289f;
        public float zWorldRotation = 0;
        
        public RegionType lastMap = RegionType.COROLLE;
        public BananaType activeBanana = BananaType.EMPTY;
        public int bitKongQuantity;
        
        public bool hasFinishedTutorial = false;
        
        public Dictionary<string, int> bananaInventory = new() {
            {BananaType.BARANGAN.ToString(), 0},
            {BananaType.BLUE_JAVA.ToString(), 0},
            {BananaType.BURRO.ToString(), 0},
            {BananaType.CAVENDISH.ToString(), 0},
            {BananaType.GOLD_FINGER.ToString(), 0},
            {BananaType.GROS_MICHEL.ToString(), 0},
            {BananaType.LADY_FINGER.ToString(), 0},
            {BananaType.MANZANO.ToString(), 0},
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
            {RawMaterialType.BATTERY.ToString(), 0}
        };

        public Dictionary<string, int> ingredientsInventory = new() {
            {IngredientsType.BANANA_DOG_BREAD.ToString(), 0},
            {IngredientsType.BANANA_WITHOUT_SKIN.ToString(), 0},
            {IngredientsType.EMPTY.ToString(), 0}
        };

        public Dictionary<string, int> manufacturedInventory = new() {
            {ManufacturedItemsType.SPACESHIP_TOY.ToString(), 0},
            {IngredientsType.EMPTY.ToString(), 0}
        };

        public string bananaSlot = BananaType.EMPTY.ToString();
    }
}