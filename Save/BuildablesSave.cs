using System;
using System.Collections.Generic;
using System.IO;
using InGame.Items.ItemsBehaviours.BuildablesBehaviours;
using Newtonsoft.Json;
using UnityEngine;

namespace Save {
    public class BuildablesSave : MonoBehaviour {
        public GameObject buildablesContainer;

        private string _savePath;
        
        private GameObject buildableToSpawn;
        private GameObject buildableInstance;
        
        public GenericDictionary<BuildableType, List<string>> buildablesDataDictionaryByBuildableType;
        private string[] _buildablesDatas;
        
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
    
        public void SaveBuildablesData(string saveUuid) {
            var savePath = Path.Combine(ObjectsReference.Instance.gameSave._savesPath, saveUuid);
            var worldDataSavesPath = Path.Combine(savePath, "WORLD_DATA");

            var savefilePath = Path.Combine(worldDataSavesPath, "buildables.json");
        
            var buildablesBehaviours = FindObjectsByType<BuildableBehaviour>(FindObjectsSortMode.None);

            buildablesDataDictionaryByBuildableType.Clear();
            foreach (var buildableBehaviour in buildablesBehaviours) {
                buildableBehaviour.GenerateSaveData();
            }

            var json = JsonConvert.SerializeObject(buildablesDataDictionaryByBuildableType);
            File.WriteAllText(savefilePath, json);
        }
        
        private void RespawnBuildablesOnWorld() {
            DestroyImmediate(buildablesContainer);

            buildablesContainer = new GameObject("Buildables container") {
                transform = {
                    parent = transform.parent
                }
            };

            var buildablesDictionnary = buildablesDataDictionaryByBuildableType;

            foreach (var buildableToInstantiate in buildablesDictionnary) {
                foreach (var buildableString in buildableToInstantiate.Value) {
                    buildableToSpawn = ObjectsReference.Instance.meshReferenceScriptableObject.buildablePrefabByBuildableType[buildableToInstantiate.Key];

                    buildableInstance = Instantiate(buildableToSpawn, buildablesContainer.transform, true);
                    buildableInstance.GetComponent<BuildableBehaviour>().LoadSavedData(buildableString);
                }
            }
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
            buildableInstance = Instantiate(ObjectsReference.Instance.worldData.initialBuildablesOnWorld, buildablesContainer.transform, true);
            
            buildableInstance.transform.position = buildablesContainer.transform.position;
            buildableInstance.transform.rotation = buildablesContainer.transform.rotation;
        }
    }
}
