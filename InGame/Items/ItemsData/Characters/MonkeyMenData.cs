using System.Collections.Generic;
using UnityEngine;

namespace InGame.Items.ItemsData.Characters {
    [System.Serializable]
    public class MonkeyMenData {
        public string uid;
        public string monkeyMenName;
        public CharacterType characterType;

        // APPARENCE
        public float textureRotation;
        public int prefabIndex;
        public int propertiesIndex;
        
        public NeedType need;
        public bool isSatisfied;
        public Vector3 destination;
        
        public Dictionary<RawMaterialType, int> rawMaterialsInventory = new() {
            {RawMaterialType.ELECTRONIC, 0},
            {RawMaterialType.BANANA_PEEL, 0},
            {RawMaterialType.METAL, 0},
            {RawMaterialType.FABRIC, 0},
            {RawMaterialType.BATTERY, 0}
        };

        public Dictionary<IngredientsType, int> ingredientsInventory = new() {
            {IngredientsType.BANANA_DOG_BREAD, 0},
            {IngredientsType.BANANA_WITHOUT_SKIN, 0}
        };

        public Dictionary<ManufacturedItemsType, int> manufacturedItemsInventory = new() {
            {ManufacturedItemsType.SPACESHIP_TOY, 0},
            {ManufacturedItemsType.BANANARAIGNEE, 0},
            {ManufacturedItemsType.LAPINOU, 0},
            {ManufacturedItemsType.BANANAVIAIRE, 0}
        };
        
        public int bitKongQuantity;

        public string spaceshipGuid;
        public Vector3 spaceshipPosition;
        public Vector3 position;
        public Quaternion rotation;
    }
}