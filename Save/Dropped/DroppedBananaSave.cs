using System;
using System.Collections.Generic;
using System.IO;
using InGame.Items.ItemsBehaviours.DroppedBehaviours;
using Newtonsoft.Json;
using UnityEngine;

namespace Save.Dropped {
    public class DroppedBananaSave : MonoBehaviour {
        public GameObject bananasContainer;
    
        private string _savePath;

        private GameObject bananaToSpawn;
        private GameObject bananaInstance;

        public GenericDictionary<BananaType, List<string>> bananasDataDictionaryByBananaType;
        
        public void LoadBananasDataByUuid(string saveUuid) {
            _savePath = Path.Combine(ObjectsReference.Instance.gameSave._savesPath, saveUuid);
            var saveMapDatasPath = Path.Combine(_savePath, "WORLD_DATA");

            var loadfilePath = Path.Combine(saveMapDatasPath, "bananas.json");

            if (!File.Exists(loadfilePath)) return;
            
            var json = File.ReadAllText(loadfilePath);
            
            var bananasDictionnary = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(json);
            
            bananasDictionnary.Clear();
            
            foreach (var bananaList in bananasDictionnary) {
                var bananaType = Enum.Parse<BananaType>(bananaList.Key);
                
                foreach (var banana in bananaList.Value) {
                    if (bananasDataDictionaryByBananaType.ContainsKey(bananaType)) {
                        bananasDataDictionaryByBananaType[bananaType].Add(banana);
                    }
                    else {
                        bananasDataDictionaryByBananaType.Add(bananaType, new List<string>{ banana });
                    }
                }
                
                RespawnBananasOnWorld();
            }
        }

        private void RespawnBananasOnWorld() {
            DestroyImmediate(bananasContainer);

            bananasContainer = new GameObject("Bananas container") {
                transform = {
                    parent = transform.parent
                }
            };

            foreach (var bananaList in bananasDataDictionaryByBananaType) {
                foreach (var bananaString in bananaList.Value) {
                    bananaToSpawn = ObjectsReference.Instance.meshReferenceScriptableObject.bananaPrefabByBananaType[bananaList.Key];
                    
                    bananaInstance = Instantiate(bananaToSpawn, bananasContainer.transform, true);
                    bananaInstance.GetComponent<BananaBehaviour>().LoadSavedData(bananaString);
                }
            }
        }

        public void SaveBananaData(string saveUuid) {
            _savePath = Path.Combine(ObjectsReference.Instance.gameSave._savesPath, saveUuid);
            var mapDataSavesPath = Path.Combine(_savePath, "WORLD_DATA");
            
            var savefilePath = Path.Combine(mapDataSavesPath, "bananas.json");
            
            var bananaBehaviours = FindObjectsByType<BananaBehaviour>(FindObjectsSortMode.None);
            
            bananasDataDictionaryByBananaType.Clear();

            foreach (var bananaBehaviour in bananaBehaviours) bananaBehaviour.GenerateDroppedData();
            
            var json = JsonConvert.SerializeObject(bananasDataDictionaryByBananaType);
            File.WriteAllText(savefilePath, json);
        }
        
        public void AddBananaToBananaDictionnary(BananaType bananaType, string bananaData) {
            if (bananasDataDictionaryByBananaType.ContainsKey(bananaType)) {
                bananasDataDictionaryByBananaType[bananaType].Add(bananaData);
            }
            else {
                bananasDataDictionaryByBananaType.Add(bananaType, new List<string>{ bananaData });
            }
        }
    }
}
