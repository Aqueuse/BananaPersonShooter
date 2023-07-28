using System.Collections.Generic;
using Enums;

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
        
        public string lastMap = "COMMANDROOM";
        public ItemType activeItem = ItemType.EMPTY;
        public ItemCategory activeItemCategory = ItemCategory.EMPTY;
        public BuildableType activeBuildableType = BuildableType.EMPTY;

        public bool hasRepairedBananaGun = false;
        public bool hasFinishedTutorial = false;
        
        public Dictionary<string, int> inventory = new() {
            {ItemType.EMPTY.ToString(), 0},
            {ItemType.BARANGAN.ToString(), 0},
            {ItemType.BLUE_JAVA.ToString(), 0},
            {ItemType.BURRO.ToString(), 0},
            {ItemType.CAVENDISH.ToString(), 0},
            {ItemType.GOLD_FINGER.ToString(), 0},
            {ItemType.GROS_MICHEL.ToString(), 0},
            {ItemType.LADY_FINGER.ToString(), 0},
            {ItemType.MANZANO.ToString(), 0},
            {ItemType.MATOKE.ToString(), 0},
            {ItemType.MUSA_VELUTINA.ToString(), 0},
            {ItemType.NANJANGUD.ToString(), 0},
            {ItemType.PISANG_RAJA.ToString(), 0},
            {ItemType.PLANTAIN.ToString(), 0},
            {ItemType.PRAYING_HANDS.ToString(), 0},
            {ItemType.RED.ToString(), 0},
            {ItemType.RINO_HORN.ToString(), 0},
            {ItemType.TINDOK.ToString(), 0},
            {ItemType.METAL.ToString(), 0},
            {ItemType.BATTERY.ToString(), 0},
            {ItemType.FABRIC.ToString(), 0},
            {ItemType.ELECTRONIC.ToString(), 0},
            {ItemType.BANANA_PEEL.ToString(), 0}
        };

        public List<string> blueprints = new List<string>();
        
        public List<string> slots = new List<string>();
    }
}