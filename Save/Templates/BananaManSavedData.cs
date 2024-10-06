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
        
        public BananaType activeBanana = BananaType.EMPTY;
        public BuildableType activeBuildable = BuildableType.BUMPER;
        public int bitKongQuantity;
        
        public bool hasFinishedTutorial = false;
        
        public Dictionary<string, int> bananaInventory = new() {
            {BananaType.EMPTY.ToString(), 0},
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
        
        public Dictionary<string, int> droppedInventory = new() {
            {DroppedType.ELECTRONIC.ToString(), 0},
            {DroppedType.BANANA_PEEL.ToString(), 0},
            {DroppedType.METAL.ToString(), 0},
            {DroppedType.FABRIC.ToString(), 0},
            {DroppedType.BATTERY.ToString(), 0},
            {DroppedType.SILICE.ToString(), 0}
        };  

        public Dictionary<string, int> ingredientsInventory = new() {
            {IngredientsType.BANANA_DOG_BREAD.ToString(), 0},   
            {IngredientsType.BANANA_WITHOUT_SKIN.ToString(), 0}
        };

        public Dictionary<string, int> manufacturedInventory = new() {
            {ManufacturedItemsType.SPACESHIP_TOY.ToString(), 0}
        };

        public string bananaSlot = BananaType.EMPTY.ToString();

        public string bananaGunMode = BananaGunMode.SHOOT.ToString();
    }
}