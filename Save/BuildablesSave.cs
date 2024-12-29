using System;
using System.Collections.Generic;
using System.IO;
using InGame.Items.ItemsBehaviours.BuildablesBehaviours;
using Newtonsoft.Json;
using UnityEngine;

namespace Save {
    public class BuildablesSave : MonoBehaviour {
        private string _savePath;
        
        private GameObject buildableToSpawn;
        private GameObject buildableInstance;
        
        public GenericDictionary<BuildableType, List<string>> buildablesDataDictionaryByBuildableType;
        
        public void LoadBuildablesDataByUuid(string saveUuid) {
            _savePath = Path.Combine(ObjectsReference.Instance.gameSave._savesPath, saveUuid);
            var saveWorldDatasPath = Path.Combine(_savePath, "WORLD_DATA");

            var loadfilePath = Path.Combine(saveWorldDatasPath, "buildables.json");
            
            if (!File.Exists(loadfilePath)) return;

            var json = File.ReadAllText(loadfilePath);

            var buildablesDictionnary = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(json);

            buildablesDataDictionaryByBuildableType.Clear();
        
            foreach (var buildableList in buildablesDictionnary) {
                var buildableType = Enum.Parse<BuildableType>(buildableList.Key);

                foreach (var buildable in buildableList.Value) {
                    if (buildablesDataDictionaryByBuildableType.ContainsKey(buildableType)) {
                        buildablesDataDictionaryByBuildableType[buildableType].Add(buildable);
                    }
                    else {
                        buildablesDataDictionaryByBuildableType.Add(buildableType, new List<string>{ buildable });
                    }
                }
            }
            
            RespawnBuildablesOnWorld();
        }
        
        private void RespawnBuildablesOnWorld() {
            foreach (var buildableList in buildablesDataDictionaryByBuildableType) {
                foreach (var buildableString in buildableList.Value) {
                    buildableToSpawn = ObjectsReference.Instance.meshReferenceScriptableObject.buildablePrefabByBuildableType[buildableList.Key];

                    buildableInstance = Instantiate(buildableToSpawn, ObjectsReference.Instance.gameSave.savablesItemsContainer, true);
                    buildableInstance.GetComponent<BuildableBehaviour>().LoadSavedData(buildableString);
                }
            }
        }
        
        public void SaveBuildablesData(string saveUuid) {
            var savePath = Path.Combine(ObjectsReference.Instance.gameSave._savesPath, saveUuid);
            var worldDataSavesPath = Path.Combine(savePath, "WORLD_DATA");

            var savefilePath = Path.Combine(worldDataSavesPath, "buildables.json");
        
            var buildablesBehaviours = FindObjectsByType<BuildableBehaviour>(FindObjectsSortMode.None);

            buildablesDataDictionaryByBuildableType.Clear();
            
            foreach (var buildableBehaviour in buildablesBehaviours) buildableBehaviour.GenerateSaveData();
            
            var json = JsonConvert.SerializeObject(buildablesDataDictionaryByBuildableType);
            File.WriteAllText(savefilePath, json);
        }
        
        public void AddBuildableToBuildableDictionnary(BuildableType buildableType, string buildableData) {
            if (buildablesDataDictionaryByBuildableType.ContainsKey(buildableType)) {
                buildablesDataDictionaryByBuildableType[buildableType].Add(buildableData);
            }
            else {
                buildablesDataDictionaryByBuildableType.Add(buildableType, new List<string>{ buildableData });
            }
        }

        public void SpawnInitialBuildables() {
            buildableInstance = Instantiate(
                ObjectsReference.Instance.worldData.initialBuildablesOnWorld,
                ObjectsReference.Instance.gameSave.savablesItemsContainer, 
                true
            );
            
            buildableInstance.transform.position = ObjectsReference.Instance.gameSave.savablesItemsContainer.position;
            buildableInstance.transform.rotation = ObjectsReference.Instance.gameSave.savablesItemsContainer.rotation;
        }
    }
}
