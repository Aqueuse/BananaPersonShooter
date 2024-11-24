using System.Collections.Generic;
using InGame.Items.ItemsProperties.Buildables;
using InGame.Items.ItemsProperties.Dropped;
using InGame.Items.ItemsProperties.Dropped.Raw_Materials;
using UnityEngine;

namespace InGame.Player {
    public class BananaManData : MonoBehaviour {
        public DroppedPropertiesScriptableObject activeDropped;
        public BuildablePropertiesScriptableObject activeBuildable;

        public BananaType activeBanana = BananaType.EMPTY;
        public RawMaterialType activeRawMaterial = RawMaterialType.EMPTY;
        public IngredientsType activeIngredient = IngredientsType.EMPTY;
        public ManufacturedItemsType activeManufacturedItem = ManufacturedItemsType.EMPTY;
        
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
        
        public Dictionary<RawMaterialType, int> rawMaterialInventory = new() {
            { RawMaterialType.METAL, 0},
            { RawMaterialType.BATTERY, 0},
            { RawMaterialType.ELECTRONIC, 0},
            { RawMaterialType.FABRIC, 0},
            { RawMaterialType.BANANA_PEEL, 0},
            { RawMaterialType.SILICE, 0 }
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
