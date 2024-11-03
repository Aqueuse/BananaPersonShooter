using System.Collections.Generic;
using InGame.Items.ItemsData;

namespace Save.Templates {
    public class SpaceshipSavedData {
        public string spaceshipGuid;
        public string spaceshipName;
        
        public int communicationMessagePrefabIndex;
        public string uiColor; 
        
        public TravelState travelState;
        
        public CharacterType characterType;
        public SpaceshipType spaceshipType;

        public int hangarNumber;
        
        public MerchantData merchantData;
        public List<TouristData> touristDatas;

        public string spaceshipPosition;
        public string spaceshipRotation;

        public string arrivalPoint;
    }
}
