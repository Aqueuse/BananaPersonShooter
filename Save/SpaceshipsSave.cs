using System.Collections.Generic;
using System.IO;
using InGame.Items.ItemsBehaviours.SpaceshipsBehaviours;
using Newtonsoft.Json;
using Save.Templates;
using UnityEngine;

namespace Save {
    public class SpaceshipsSave : MonoBehaviour {
        private string _savePath;
        
        public void LoadpaceshipsData(string saveUuid) {
            _savePath = Path.Combine(ObjectsReference.Instance.gameSave._savesPath, saveUuid);
            var saveWorldDatasPath = Path.Combine(_savePath, "WORLD_DATA");

            var loadfilePath = Path.Combine(saveWorldDatasPath, "spaceships.json");
            
            var spaceshipsDataString = File.ReadAllText(loadfilePath);
            var spaceshipsList = JsonConvert.DeserializeObject<List<string>>(spaceshipsDataString);
            
            foreach (var spaceship in spaceshipsList) {
                SpaceshipSavedData spaceshipSavedData = JsonConvert.DeserializeObject<SpaceshipSavedData>(spaceship);

                var spaceshipInstance = Instantiate(ObjectsReference.Instance.meshReferenceScriptableObject.spaceshipByCharacterType[spaceshipSavedData.characterType]);

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
