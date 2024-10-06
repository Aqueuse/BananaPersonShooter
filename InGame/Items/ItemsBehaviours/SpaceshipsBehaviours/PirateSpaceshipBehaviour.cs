using System;
using Save.Helpers;
using Save.Templates;
using UnityEngine;

namespace InGame.Items.ItemsBehaviours.SpaceshipsBehaviours {
    public class PirateSpaceshipBehaviour : SpaceshipBehaviour {
        private GameObject pirateInstance;
        private GameObject piratePrefab;

        private int pirateNumber;

        public override void Init() {
            if (spaceshipSavedData.travelState == TravelState.WAIT_IN_STATION) {
                ObjectsReference.Instance.spaceTrafficControlManager.AssignSpaceshipToHangar(assignatedHangar);
            }
        }
        
        public override void GenerateSaveData() {
            if(string.IsNullOrEmpty(spaceshipGuid)) {
                spaceshipGuid = Guid.NewGuid().ToString();
            }
            
            spaceshipSavedData = new SpaceshipSavedData {
                spaceshipName = spaceshipName,
                spaceshipGuid = spaceshipGuid,
                communicationMessagePrefabIndex = communicationMessagePrefabIndex,
                uiColor = JsonHelper.FromColorToString(spaceshipUIcolor),
                characterType = characterType,
                spaceshipPosition = JsonHelper.FromVector3ToString(transform.position),
                spaceshipRotation = JsonHelper.FromQuaternionToString(transform.rotation),
                travelState = travelState,
                arrivalPoint = JsonHelper.FromVector3ToString(arrivalPosition),
                hangarNumber = assignatedHangar
            };

            savedData = spaceshipSavedData;
        }
    }
}
