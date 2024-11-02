using System.Collections.Generic;
using InGame.Items.ItemsProperties.Bananas;
using InGame.Items.ItemsProperties.Buildables;
using UnityEngine;

namespace InGame.Player {
    public class BananaManData : MonoBehaviour {
        public BananasPropertiesScriptableObject activeBanana;
        public BuildablePropertiesScriptableObject activeBuildable;

        public Dictionary<BananaType, int> bananasInventory = new() {
            {BananaType.BARANGAN, 0},
            {BananaType.BLUE_JAVA, 0},
            {BananaType.BURRO, 0},
            {BananaType.CAVENDISH, 0},
            {BananaType.GOLD_FINGER, 0},
            {BananaType.GROS_MICHEL, 0},
            {BananaType.LADY_FINGER, 0},
            {BananaType.MATOKE, 0},
            {BananaType.MUSA_VELUTINA, 0},
            {BananaType.NANJANGUD, 0},
            {BananaType.PISANG_RAJA, 0},
            {BananaType.PLANTAIN, 0},
            {BananaType.PRAYING_HANDS, 0},
            {BananaType.RED, 0},
            {BananaType.RINO_HORN, 0},
            {BananaType.TINDOK, 0}
        };
        
        public Dictionary<DroppedType, int> droppedInventory = new() {
            { DroppedType.METAL, 0},
            { DroppedType.BATTERY, 0},
            { DroppedType.ELECTRONIC, 0},
            { DroppedType.FABRIC, 0},
            { DroppedType.BANANA_PEEL, 0},
            { DroppedType.SILICE, 0 }
        };

        public Dictionary<IngredientsType, int> ingredientsInventory = new() {
            { IngredientsType.BANANA_DOG_BREAD, 0 },
            { IngredientsType.BANANA_WITHOUT_SKIN, 0 }
        };
        
        public Dictionary<ManufacturedItemsType, int> manufacturedItemsInventory = new() {
            { ManufacturedItemsType.SPACESHIP_TOY, 0}
        };

        public int bitKongQuantity;
    }
}
