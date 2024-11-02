using System.Collections.Generic;

namespace Save.Templates {
    // use to save monkeyMens
    public class MonkeyMenSavedData {
        public string uid;
        public string name;
        public CharacterType characterType;

        public int prefabNumber;
        public int clothColorsPreset;

        public bool isInSpaceship;
        
        public PirateState pirateState;
        public TouristState touristState;
    
        public Dictionary<NeedType, int> needs;
        public string destination;

        public Dictionary<DroppedType, int> droppedInventory;
        public Dictionary<ManufacturedItemsType, int> manufacturedItemsInventory;
        public Dictionary<IngredientsType, int> ingredientsInventory;
        public Dictionary<BananaType, int> bananasInventory;
        
        public int bitKongQuantity;

        public string spaceshipGuid;
        public string position;
        public string rotation;
    }
}
