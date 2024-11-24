using System;
using System.Collections.Generic;
using System.IO;
using InGame.Items.ItemsBehaviours.DroppedBehaviours;
using Newtonsoft.Json;
using UnityEngine;

namespace Save.Dropped {
    public class RawMaterialsSave : MonoBehaviour {
        public GameObject rawMaterialsContainer;
    
        private string _savePath;

        private GameObject rawMaterialToSpawn;
        private GameObject rawMaterialInstance;

        public GenericDictionary<RawMaterialType, List<string>> rawMaterialsDataDictionaryByRawMaterialType;
        
        public void LoadRawMaterialDataByUuid(string saveUuid) {
            _savePath = Path.Combine(ObjectsReference.Instance.gameSave._savesPath, saveUuid);
            var saveMapDatasPath = Path.Combine(_savePath, "WORLD_DATA");

            var loadfilePath = Path.Combine(saveMapDatasPath, "rawMaterials.json");

            if (!File.Exists(loadfilePath)) return;
            
            var json = File.ReadAllText(loadfilePath);
            
            var rawMaterialsDictionnary = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(json);
            
            rawMaterialsDictionnary.Clear();
            
            foreach (var rawMaterialList in rawMaterialsDictionnary) {
                var rawMaterialType = Enum.Parse<RawMaterialType>(rawMaterialList.Key);

                foreach (var rawMaterial in rawMaterialList.Value) {
                    if (rawMaterialsDataDictionaryByRawMaterialType.ContainsKey(rawMaterialType)) {
                        rawMaterialsDataDictionaryByRawMaterialType[rawMaterialType].Add(rawMaterial);
                    }
                    else {
                        rawMaterialsDataDictionaryByRawMaterialType.Add(rawMaterialType, new List<string>{ rawMaterial });
                    }
                }

                RespawnRawMaterialsOnWorld();
            }
        }

        private void RespawnRawMaterialsOnWorld() {
            DestroyImmediate(rawMaterialsContainer);

            rawMaterialsContainer = new GameObject("RawMaterials container") {
                transform = {
                    parent = transform.parent
                }
            };

            foreach (var rawMaterialList in rawMaterialsDataDictionaryByRawMaterialType) {
                foreach (var rawMaterialString in rawMaterialList.Value) {
                    rawMaterialToSpawn = ObjectsReference.Instance.meshReferenceScriptableObject.rawMaterialPrefabByRawMaterialType[rawMaterialList.Key];
                    
                    rawMaterialInstance = Instantiate(rawMaterialToSpawn, rawMaterialsContainer.transform, true);
                    rawMaterialInstance.GetComponent<RawMaterialBehaviour>().LoadSavedData(rawMaterialString);
                }
            }
        }

        public void SaveRawMaterialData(string saveUuid) {
            _savePath = Path.Combine(ObjectsReference.Instance.gameSave._savesPath, saveUuid);
            var mapDataSavesPath = Path.Combine(_savePath, "WORLD_DATA");
            
            var savefilePath = Path.Combine(mapDataSavesPath, "rawMaterials.json");
            
            var rawMaterialBehaviours = FindObjectsByType<RawMaterialBehaviour>(FindObjectsSortMode.None);
            
            rawMaterialsDataDictionaryByRawMaterialType.Clear();

            foreach (var rawMaterialBehaviour in rawMaterialBehaviours) rawMaterialBehaviour.GenerateDroppedData();
            
            var json = JsonConvert.SerializeObject(rawMaterialsDataDictionaryByRawMaterialType);
            File.WriteAllText(savefilePath, json);
        }
        
        public void AddRawMaterialToRawMaterialDictionnary(RawMaterialType rawMaterialType, string rawMaterialData) {
            if (rawMaterialsDataDictionaryByRawMaterialType.ContainsKey(rawMaterialType)) {
                rawMaterialsDataDictionaryByRawMaterialType[rawMaterialType].Add(rawMaterialData);
            }
            else {
                rawMaterialsDataDictionaryByRawMaterialType.Add(rawMaterialType, new List<string>{ rawMaterialData });
            }
        }
    }
}
