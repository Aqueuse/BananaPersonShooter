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
            touristDatas = spaceshipSavedData.touristDatas;

            if (spaceshipSavedData.travelState == TravelState.WAIT_IN_STATION) {
                ObjectsReference.Instance.spaceTrafficControlManager.AssignSpaceshipToHangar(assignatedHangar);
                
                // TODO : start incrementing timer
                // spawn character etc
            }
        }

        public int GetTouristsNumber() {
            return touristDatas.Count;
        }

        public override void GenerateSaveData() {
            if(string.IsNullOrEmpty(spaceshipGuid)) {
                spaceshipGuid = Guid.NewGuid().ToString();
            }

            SpaceshipSavedData touristSpaceshipData = new SpaceshipSavedData {
                spaceshipName = spaceshipName,
                spaceshipGuid = spaceshipGuid,
                communicationMessagePrefabIndex = communicationMessagePrefabIndex,
                uiColor = JsonHelper.FromColorToString(spaceshipUIcolor),
                touristDatas = touristDatas,
                characterType = characterType,
                spaceshipPosition = JsonHelper.FromVector3ToString(transform.position),
                spaceshipRotation = JsonHelper.FromQuaternionToString(transform.rotation),
                travelState = travelState,
                arrivalPoint = JsonHelper.FromVector3ToString(arrivalPosition),
                hangarNumber = assignatedHangar
            };

            savedData = touristSpaceshipData;
        }
    }
}
