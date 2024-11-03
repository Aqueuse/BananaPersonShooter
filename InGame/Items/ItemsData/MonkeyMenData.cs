using System.Collections.Generic;
using UnityEngine;

namespace InGame.Items.ItemsData {
    public class MonkeyMenData {
        public string uid;
        public string monkeyMenName;
        public CharacterType characterType;

        public int appearanceScriptableObjectIndex;
        
        public bool isInSpaceship;

        public PirateState pirateState = PirateState.GO_TO_TELEPORTER;
        public TouristState touristState = TouristState.GO_TO_WAITING_LINE;

        public GenericDictionary<NeedType, int> needs;
        public Vector3 destination;

        public Dictionary<DroppedType, int> droppedInventory;
        public Dictionary<ManufacturedItemsType, int> manufacturedItemsInventory;
        public Dictionary<IngredientsType, int> ingredientsInventory;
        public Dictionary<BananaType, int> bananasInventory;

        public int bitKongQuantity;

        public string spaceshipGuid;
        public Vector3 position;
        public Quaternion rotation;
    }
}