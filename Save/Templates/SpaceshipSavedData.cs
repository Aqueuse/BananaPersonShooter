using System.Collections.Generic;
using UnityEngine;

namespace Save.Templates {
    public class SpaceshipSavedData {
        public string spaceshipGuid;
        public string spaceshipName;
        
        public int communicationMessagePrefabIndex;
        public string uiColor; 
        
        public TravelState travelState;
        
        public CharacterType characterType;
        public SpaceshipType spaceshipType;
        
        public List<MonkeyMenSavedData> MonkeyMenSavedDatas;

        public GroupTravelState groupTravelState;
        public SpawnPoint[] guichetsMapsToVisit;
        public Vector3[] mapPointInterests;
        
        public int hangarNumber;

        public string spaceshipPosition;
        public string spaceshipRotation;

        public string arrivalPoint;
    }
}
