using System.Collections.Generic;
using UnityEngine;

namespace Save.Templates {
    // use to save monkeyMens
    public class MonkeyMenSavedData {
        public string uid;
        public string name;
        public CharacterType characterType;
        
        public MonkeyMenType monkeyMenType;
        public int prefabIndex;
        public Color[] colorsSet;
        
        public PirateState pirateState;
        public TouristState touristState;
    
        public NeedType need;
        public bool isSatisfied;
        public string destination;

        public Dictionary<RawMaterialType, int> rawMaterialsInventory;
        public Dictionary<ManufacturedItemsType, int> manufacturedItemsInventory;
        public Dictionary<IngredientsType, int> ingredientsInventory;
        public Dictionary<BananaType, int> bananasInventory;
        
        public int bitKongQuantity;

        public string spaceshipGuid;
        public string position;
        public string rotation;
    }
}
