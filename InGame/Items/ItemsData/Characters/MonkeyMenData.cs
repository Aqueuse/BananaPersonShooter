using System.Collections.Generic;
using UnityEngine;

namespace InGame.Items.ItemsData.Characters {
    [System.Serializable]
    public class MonkeyMenData {
        public string uid;
        public string monkeyMenName;
        public CharacterType characterType;
        public MonkeyMenType monkeyMenType;

        public int appearanceScriptableObjectIndex;
        
        public PirateState pirateState = PirateState.GO_TO_TELEPORTER;
        public TouristState touristState = TouristState.GO_TO_TELEPORTER;

        public GenericDictionary<NeedType, int> needs = new ();
        public Vector3 destination;

        public Dictionary<RawMaterialType, int> rawMaterialsInventory;
        public Dictionary<ManufacturedItemsType, int> manufacturedItemsInventory;
        public Dictionary<IngredientsType, int> ingredientsInventory;
        public Dictionary<BananaType, int> bananasInventory;

        public int bitKongQuantity;

        public string spaceshipGuid;
        public Vector3 position;
        public Quaternion rotation;
    }
}