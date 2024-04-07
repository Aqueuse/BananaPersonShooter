using System;
using System.Collections.Generic;
using InGame.Items.ItemsData;
using Newtonsoft.Json;
using Save.Helpers;
using Save.Templates;

namespace InGame.Items.ItemsBehaviours.SpaceshipsBehaviours {
    public class PirateSpaceshipBehaviour : SpaceshipBehaviour {
        public List<PirateData> piratesData = new List<PirateData>();
        
        public override void Init() {
            transform.position = JsonHelper.FromStringToVector3(spaceshipSavedData.spaceshipPosition);
            transform.rotation = JsonHelper.FromStringToQuaternion(spaceshipSavedData.spaceshipRotation); 

            spaceshipName = spaceshipSavedData.spaceshipName;
            spaceshipGuid = spaceshipSavedData.spaceshipGuid;
            communicationMessagePrefabIndex = spaceshipSavedData.communicationMessageprefabIndex;
            characterType = spaceshipSavedData.characterType;
            travelState = spaceshipSavedData.travelState;

            if (spaceshipSavedData.travelState != TravelState.WAIT_IN_STATION) {
                // TODO : start incrementing timer
                // spawn character etc
            }

            else {
                var arrivalPoint = JsonHelper.FromStringToVector3(spaceshipSavedData.arrivalPoint);
                
                InitiatePropulsion(arrivalPoint);
            }
            
            piratesData = spaceshipSavedData.pirateDatas;
        }

        public override void GenerateSaveData() {
            if(string.IsNullOrEmpty(spaceshipGuid)) {
                spaceshipGuid = Guid.NewGuid().ToString();
            }

            spaceshipSavedData = new SpaceshipSavedData {
                spaceshipName = spaceshipName,
                spaceshipGuid = spaceshipGuid,
                communicationMessageprefabIndex = communicationMessagePrefabIndex,
                characterType = characterType,
                pirateDatas = piratesData,
                spaceshipPosition = JsonHelper.FromVector3ToString(transform.position),
                spaceshipRotation = JsonHelper.FromQuaternionToString(transform.rotation),
                travelState = travelState,
                timeToNextState = timeToNextState
            };

            savedData = JsonConvert.SerializeObject(spaceshipSavedData);
        }
    }
}
