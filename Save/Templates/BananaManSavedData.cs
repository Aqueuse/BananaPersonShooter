using System.Collections.Generic;
using Enums;

namespace Save.Templates {
    public class BananaManSavedData {
        public float health = 100;
        public float resistance = 100;

        public AdvancementState advancementState = AdvancementState.NEW_GAME;
        
        public float xWorldPosition = -1022.773f;
        public float yWorldPosition = 0.809f;
        public float zworldPosition = -2651.568f;

        // Quaternion(0,-0.495293289,0,0.868725777)
        
        public float xWorldRotation = 0;
        public float yWorldRotation = -0.495293289f;
        public float zWorldRotation = 0;
        
        public string lastMap = "COMMANDROOM";
        public ItemThrowableType activeItem = ItemThrowableType.EMPTY;
        
        public Dictionary<string, int> inventory = new() {
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
        
        public List<string> slots = new List<string>();
    }
}