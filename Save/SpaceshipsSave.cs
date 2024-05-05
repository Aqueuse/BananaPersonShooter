using System.Collections.Generic;
using System.IO;
using InGame.Items.ItemsBehaviours.SpaceshipsBehaviours;
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
            var spaceshipsList = JsonConvert.DeserializeObject<List<string>>(spaceshipsDataString);

            foreach (var spaceship in spaceshipsList) {
                SpaceshipSavedData spaceshipSavedData = JsonConvert.DeserializeObject<SpaceshipSavedData>(spaceship);

                var spaceshipInstance = Instantiate(ObjectsReference.Instance.meshReferenceScriptableObject.spaceshipByCharacterType[spaceshipSavedData.characterType]);

                spaceshipBehaviourInstance = spaceshipInstance.GetComponent<SpaceshipBehaviour>();
                spaceshipBehaviourInstance.spaceshipSavedData = spaceshipSavedData;

                spaceshipBehaviourInstance.transform.position = JsonHelper.FromStringToVector3(spaceshipSavedData.spaceshipPosition);
                spaceshipBehaviourInstance.transform.rotation = JsonHelper.FromStringToQuaternion(spaceshipSavedData.spaceshipRotation);

                spaceshipBehaviourInstance.spaceshipName = spaceshipSavedData.spaceshipName;
                spaceshipBehaviourInstance.spaceshipGuid = spaceshipSavedData.spaceshipGuid;

                spaceshipBehaviourInstance.communicationMessagePrefabIndex = spaceshipSavedData.communicationMessagePrefabIndex;
                spaceshipBehaviourInstance.spaceshipUIcolor = JsonHelper.FromStringToColor(spaceshipSavedData.uiColor);
                
                spaceshipBehaviourInstance.travelState = spaceshipSavedData.travelState;

                spaceshipBehaviourInstance.arrivalPosition = JsonHelper.FromStringToVector3(spaceshipSavedData.arrivalPoint);
                spaceshipBehaviourInstance.assignatedHangar = spaceshipSavedData.hangarNumber;
                
                if (spaceshipSavedData.travelState == TravelState.GO_TO_PATH) {
                    spaceshipBehaviourInstance.GoToPath(spaceshipSavedData.hangarNumber);
                }

                if (spaceshipSavedData.travelState == TravelState.TRAVEL_ON_PATH) {
                    spaceshipBehaviourInstance.MoveToElevator();
                }

                if (spaceshipSavedData.travelState == TravelState.TRAVEL_ON_ELEVATOR) {
                    spaceshipBehaviourInstance.MoveOnElevatorToHangar();
                }

                if (spaceshipSavedData.travelState == TravelState.TRAVEL_BACK_ON_ELEVATOR) {
                    spaceshipBehaviourInstance.travelState = TravelState.LEAVES_THE_REGION;
                    transform.position = new Vector3(transform.position.x, -10f, transform.position.z);
                    spaceshipBehaviourInstance.arrivalPosition.y = -10f;
                }

                if (spaceshipSavedData.travelState == TravelState.LEAVES_THE_REGION) {
                    spaceshipBehaviourInstance.arrivalPosition.y = -10f;
                }

                if (spaceshipSavedData.travelState == TravelState.FREE_FLIGHT || spaceshipSavedData.travelState == TravelState.LEAVES_THE_REGION) {
                    ObjectsReference.Instance.uiSpaceTrafficControlPanel.AddNewCommunication(spaceshipBehaviourInstance);
                }

                spaceshipInstance.GetComponent<SpaceshipBehaviour>().Init();

                ObjectsReference.Instance.spaceTrafficControlManager.spaceshipBehavioursByGuid.Add(
                    spaceshipInstance.GetComponent<SpaceshipBehaviour>().spaceshipGuid, 
                    spaceshipInstance.GetComponent<SpaceshipBehaviour>());
            }
        }

        public void SaveSpaceships(string saveUuid) {
            _savePath = Path.Combine(ObjectsReference.Instance.gameSave._savesPath, saveUuid);
            var mapDataSavesPath = Path.Combine(_savePath, "WORLD_DATA");

            List<string> jsonSpaceshipsSaved = new List<string>();

            foreach (var spaceshipsBehaviour in ObjectsReference.Instance.spaceTrafficControlManager.spaceshipBehavioursByGuid) {
                spaceshipsBehaviour.Value.GenerateSaveData();
                jsonSpaceshipsSaved.Add(spaceshipsBehaviour.Value.savedData);
            }

            var json = JsonConvert.SerializeObject(jsonSpaceshipsSaved);

            var playerSavefilePath = Path.Combine(mapDataSavesPath, "spaceships.json");
            File.WriteAllText(playerSavefilePath, json);
        }
    }
}
