using System.Collections.Generic;
using System.IO;
using InGame.Items.ItemsBehaviours;
using InGame.Items.ItemsData;
using Newtonsoft.Json;
using Save.Helpers;
using UnityEngine;

namespace Save {
    public class DroppedSave : MonoBehaviour {
        public GameObject droppedContainer;
    
        private string _savePath;

        private GameObject droppedToSpawn;
        private GameObject droppedInstance;

        public List<string> droppedDataList;

        private string[] _buildablesDatas;
        
        public void LoadDroppedDataByUuid(string saveUuid) {
            _savePath = Path.Combine(ObjectsReference.Instance.gameSave._savesPath, saveUuid);
            var saveMapDatasPath = Path.Combine(_savePath, "WORLD_DATA");

            var loadfilePath = Path.Combine(saveMapDatasPath, "dropped.json");

            if (!File.Exists(loadfilePath)) return;
            
            droppedDataList.Clear();
            
            var json = File.ReadAllText(loadfilePath);

            var droppedList = JsonConvert.DeserializeObject<List<string>>(json);
            
            foreach (var dropped in droppedList) {
                droppedDataList.Add(dropped);
            }
            
            RespawnDroppedOnWorld();
        }

        public void SaveDroppedData(string saveUuid) {
            _savePath = Path.Combine(ObjectsReference.Instance.gameSave._savesPath, saveUuid);
            var mapDataSavesPath = Path.Combine(_savePath, "WORLD_DATA");
            
            var savefilePath = Path.Combine(mapDataSavesPath, "dropped.json");
        
            var droppedBehaviours = FindObjectsByType<DroppedBehaviour>(FindObjectsSortMode.None);

            droppedDataList.Clear();
            
            foreach (var droppedBehaviour in droppedBehaviours) {
                droppedBehaviour.GenerateDroppedData();
            }

            var droppedToSave = droppedDataList;

            var json = JsonConvert.SerializeObject(droppedToSave);
            File.WriteAllText(savefilePath, json);
        }
    
        public void AddDroppedToDroppedDictionnary(string spaceshipDebrisData) {
            droppedDataList.Add(spaceshipDebrisData);
        }
    
        private void RespawnDroppedOnWorld() {
            DestroyImmediate(droppedContainer);
            
            droppedContainer = new GameObject("Dropped container") {
                transform = {
                    parent = transform.parent
                }
            };

            foreach (var droppedToInstantiate in droppedDataList) {
                var droppedData = JsonConvert.DeserializeObject<DroppedData>(droppedToInstantiate);

                droppedToSpawn = ObjectsReference.Instance.meshReferenceScriptableObject.droppedPrefabByDroppedType[droppedData.droppedType];

                droppedInstance = Instantiate(droppedToSpawn, droppedContainer.transform, true);

                droppedInstance.transform.position = JsonHelper.FromStringToVector3(droppedData.droppedPosition);
                droppedInstance.transform.rotation = JsonHelper.FromStringToQuaternion(droppedData.droppedRotation);
                droppedInstance.GetComponent<DroppedBehaviour>().droppedType = droppedData.droppedType;
            }
        }
    }
}
