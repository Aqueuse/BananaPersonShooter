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

            foreach (var spaceshipSavedData in spaceshipsList) {
                var spaceshipInstance = Instantiate(
                    ObjectsReference.Instance.meshReferenceScriptableObject.spaceshipPrefabBySpaceshipType[spaceshipSavedData.spaceshipType],
                    JsonHelper.FromStringToVector3(spaceshipSavedData.spaceshipPosition),
                    JsonHelper.FromStringToQuaternion(spaceshipSavedData.spaceshipRotation),
                    ObjectsReference.Instance.gameSave.savablesItemsContainer
                );

                spaceshipInstance.GetComponent<SpaceshipBehaviour>().LoadSavedData(spaceshipSavedData);
            }
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
