using System.Collections.Generic;
using System.IO;
using InGame.Items.ItemsBehaviours;
using Newtonsoft.Json;
using Save.Helpers;
using Save.Templates;
using UnityEngine;

namespace Save {
    public class SpaceshipsSave : MonoBehaviour {
        private string _savePath;
        private SpaceshipBehaviour spaceshipBehaviourInstance;
        
        public void LoadpaceshipsData(string saveUuid) {
            _savePath = Path.Combine(ObjectsReference.Instance.gameSave._savesPath, saveUuid);
            var saveWorldDatasPath = Path.Combine(_savePath, "WORLD_DATA");

            var loadfilePath = Path.Combine(saveWorldDatasPath, "spaceships.json");

            if (!File.Exists(loadfilePath)) return;

            var spaceshipsDataString = File.ReadAllText(loadfilePath);
            
            var spaceshipsList = JsonConvert.DeserializeObject<List<SpaceshipSavedData>>(spaceshipsDataString);

            foreach (var spaceship in spaceshipsList) {
                var spaceshipInstance = Instantiate(
                    ObjectsReference.Instance.meshReferenceScriptableObject.spaceshipPrefabBySpaceshipType[spaceship.spaceshipType],
                    JsonHelper.FromStringToVector3(spaceship.spaceshipPosition),
                    JsonHelper.FromStringToQuaternion(spaceship.spaceshipRotation),
                    ObjectsReference.Instance.gameSave.spaceshipsContainer
                );
            
                spaceshipBehaviourInstance = spaceshipInstance.GetComponent<SpaceshipBehaviour>();
                spaceshipBehaviourInstance.spaceshipSavedData = spaceship;
                
                spaceshipBehaviourInstance.spaceshipName = spaceship.spaceshipName;
                spaceshipBehaviourInstance.spaceshipGuid = spaceship.spaceshipGuid;
            
                spaceshipBehaviourInstance.communicationMessagePrefabIndex = spaceship.communicationMessagePrefabIndex;
                spaceshipBehaviourInstance.spaceshipUIcolor = JsonHelper.FromStringToColor(spaceship.uiColor);
                
                spaceshipBehaviourInstance.travelState = spaceship.travelState;
                spaceshipBehaviourInstance.characterType = spaceship.characterType;
            
                spaceshipBehaviourInstance.arrivalPosition = JsonHelper.FromStringToVector3(spaceship.arrivalPoint);
                spaceshipBehaviourInstance.assignatedHangar = spaceship.hangarNumber;
                
                ObjectsReference.Instance.spaceTrafficControlManager.spaceshipBehavioursByGuid.Add(
                    spaceshipInstance.GetComponent<SpaceshipBehaviour>().spaceshipGuid, 
                    spaceshipInstance.GetComponent<SpaceshipBehaviour>());
                
                if (spaceship.travelState == TravelState.GO_TO_PATH) {
                    spaceshipBehaviourInstance.GoToPath(spaceship.hangarNumber);
                }
            
                if (spaceship.travelState == TravelState.TRAVEL_ON_PATH) {
                    spaceshipBehaviourInstance.MoveToElevator();
                }
            
                if (spaceship.travelState == TravelState.TRAVEL_ON_ELEVATOR) {
                    spaceshipBehaviourInstance.MoveOnElevatorToHangar();
                }
            
                if (spaceship.travelState == TravelState.TRAVEL_BACK_ON_ELEVATOR) {
                    spaceshipBehaviourInstance.travelState = TravelState.LEAVES_THE_REGION;
                    transform.position = new Vector3(transform.position.x, -10f, transform.position.z);
                    spaceshipBehaviourInstance.arrivalPosition.y = -10f;
                }
            
                if (spaceship.travelState == TravelState.LEAVES_THE_REGION) {
                    spaceshipBehaviourInstance.arrivalPosition.y = -10f;
                }
            
                if (spaceship.travelState == TravelState.FREE_FLIGHT || spaceship.travelState == TravelState.LEAVES_THE_REGION) {
                    ObjectsReference.Instance.uiSpaceTrafficControlPanel.AddNewCommunication(spaceshipBehaviourInstance);
                }
            }
            
            ObjectsReference.Instance.uiSpaceTrafficControlPanel.RefreshCommunicationButton();
            ObjectsReference.Instance.uiSpaceTrafficControlPanel.RefreshHangarAvailability();
        }

        public void SaveSpaceships(string saveUuid) {
            _savePath = Path.Combine(ObjectsReference.Instance.gameSave._savesPath, saveUuid);
            var mapDataSavesPath = Path.Combine(_savePath, "WORLD_DATA");

            var jsonSpaceshipsSaved = new List<SpaceshipSavedData>();

            foreach (var spaceshipsBehaviour in ObjectsReference.Instance.spaceTrafficControlManager.spaceshipBehavioursByGuid) {
                if (spaceshipsBehaviour.Value == null) continue;
                
                spaceshipsBehaviour.Value.GenerateSaveData();
                jsonSpaceshipsSaved.Add(spaceshipsBehaviour.Value.spaceshipSavedData);
            }

            var json = JsonConvert.SerializeObject(jsonSpaceshipsSaved);

            var playerSavefilePath = Path.Combine(mapDataSavesPath, "spaceships.json");
            File.WriteAllText(playerSavefilePath, json);
        }
    }
}
