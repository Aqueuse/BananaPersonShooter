using System.Collections.Generic;
using Enums;

namespace Save.Templates {
    public class BananaManSavedData {
        public float health = 100;
        public float resistance = 100;

        public float xWorldPosition = -1019.6099853515625f;
        public float yWorldPosition = 1.169921875f;
        public float zworldPosition = -2645.9099121f;
        
        public string last_map = "COMMANDROOM";
        public int active_item = 0;
        
        public Dictionary<string, int> inventory = new Dictionary<string, int> {
            {ItemThrowableType.EMPTY.ToString(), 0},
            {ItemThrowableType.DEBRIS.ToString(), 0},
            {ItemThrowableType.INGOT.ToString(), 0},
            {ItemThrowableType.BARANGAN.ToString(), 0},
            {ItemThrowableType.BLUE_JAVA.ToString(), 0},
            {ItemThrowableType.BURRO.ToString(), 0},
            {ItemThrowableType.CAVENDISH.ToString(), 0},
            {ItemThrowableType.GOLD_FINGER.ToString(), 0},
            {ItemThrowableType.GROS_MICHEL.ToString(), 0},
            {ItemThrowableType.LADY_FINGER.ToString(), 0},
            {ItemThrowableType.MANZANO.ToString(), 0},
            {ItemThrowableType.MATOKE.ToString(), 0},
            {ItemThrowableType.MUSA_VELUTINA.ToString(), 0},
            {ItemThrowableType.NANJANGUD.ToString(), 0},
            {ItemThrowableType.PISANG_RAJA.ToString(), 0},
            {ItemThrowableType.PLANTAIN.ToString(), 0},
            {ItemThrowableType.PRAYING_HANDS.ToString(), 0},
            {ItemThrowableType.RED.ToString(), 0},
            {ItemThrowableType.RINO_HORN.ToString(), 0},
            {ItemThrowableType.TINDOK.ToString(), 0},
            {ItemThrowableType.PLATEFORM.ToString(), 0}
        };
        
        public Dictionary<string, int> slots = new Dictionary<string, int> {
            { "inventorySlot0", 0},
            { "inventorySlot1", 0},
            { "inventorySlot2", 0},
            { "inventorySlot3", 0}
        };
    }
}
