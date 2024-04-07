using System;
using System.Collections.Generic;
using InGame.Items.ItemsData;
using Newtonsoft.Json;
using Save.Helpers;
using Save.Templates;

namespace InGame.Items.ItemsBehaviours.SpaceshipsBehaviours {
    public class TouristSpaceshipBehaviour : SpaceshipBehaviour {
        private List<TouristData> touristDatas;
        
        public override void Init() {
            transform.position = JsonHelper.FromStringToVector3(spaceshipSavedData.spaceshipPosition);
            transform.rotation = JsonHelper.FromStringToQuaternion(spaceshipSavedData.spaceshipRotation); 

            spaceshipName = spaceshipSavedData.spaceshipName;
            spaceshipGuid = spaceshipSavedData.spaceshipGuid;
            communicationMessagePrefabIndex = spaceshipSavedData.communicationMessageprefabIndex;
            travelState = spaceshipSavedData.travelState;

            if (spaceshipSavedData.travelState != TravelState.WAIT_IN_STATION) {
                // TODO : start incrementing timer
                // spawn character etc
            }
            
            touristDatas = spaceshipSavedData.touristDatas;
        }
    
        public override void GenerateSaveData() {
            if(string.IsNullOrEmpty(spaceshipGuid)) {
                spaceshipGuid = Guid.NewGuid().ToString();
            }

            SpaceshipSavedData touristSpaceshipData = new SpaceshipSavedData {
                spaceshipName = spaceshipName,
                spaceshipGuid = spaceshipGuid,
                communicationMessageprefabIndex = communicationMessagePrefabIndex,
                touristDatas = touristDatas,
                spaceshipPosition = JsonHelper.FromVector3ToString(transform.position),
                spaceshipRotation = JsonHelper.FromQuaternionToString(transform.rotation),
                travelState = travelState,
                timeToNextState = timeToNextState
            };

            savedData = JsonConvert.SerializeObject(touristSpaceshipData);
        }
    }
}
