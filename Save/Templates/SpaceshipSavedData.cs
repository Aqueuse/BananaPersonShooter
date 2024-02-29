using System.Collections.Generic;
using InGame.Items.ItemsData;

namespace Save.Templates {
    public class SpaceshipSavedData {
        public string spaceshipGuid;
        public string spaceshipName;
        
        public TravelState travelState;
        public float timeToNextState;

        public float distance;

        public int prefabIndex;

        public CharacterType characterType;

        public int hangarNumber;
        
        public MerchantData merchantData;
        public List<VisitorData> visitorDatas;
        public List<PirateData> pirateDatas;
    }
}
