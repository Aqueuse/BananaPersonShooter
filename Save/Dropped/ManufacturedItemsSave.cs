using System;
using System.Collections.Generic;
using System.IO;
using InGame.Items.ItemsBehaviours.DroppedBehaviours;
using Newtonsoft.Json;
using UnityEngine;

namespace Save.Dropped {
    public class ManufacturedItemsSave : MonoBehaviour {
        public GameObject manufacturedItemsContainer;
    
        private string _savePath;

        private GameObject manufacturedItemsToSpawn;
        private GameObject manufacturedItemsInstance;

        public GenericDictionary<ManufacturedItemsType, List<string>> manufacturedItemsDataDictionaryByManufacturedType;
        
        public void LoadManufacturedItemDataByUuid(string saveUuid) {
            _savePath = Path.Combine(ObjectsReference.Instance.gameSave._savesPath, saveUuid);
            var saveMapDatasPath = Path.Combine(_savePath, "WORLD_DATA");

            var loadfilePath = Path.Combine(saveMapDatasPath, "manufacturedItems.json");

            if (!File.Exists(loadfilePath)) return;
            
            var json = File.ReadAllText(loadfilePath);
            
            var manufacturedItemsDictionnary = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(json);
            
            manufacturedItemsDictionnary.Clear();
            
            foreach (var manufacturedItemsList in manufacturedItemsDictionnary) {
                var manufacturedItemsType = Enum.Parse<ManufacturedItemsType>(manufacturedItemsList.Key);

                foreach (var manufacturedItems in manufacturedItemsList.Value) {
                    if (manufacturedItemsDataDictionaryByManufacturedType.ContainsKey(manufacturedItemsType)) {
                        manufacturedItemsDataDictionaryByManufacturedType[manufacturedItemsType].Add(manufacturedItems);
                    }
                    else {
                        manufacturedItemsDataDictionaryByManufacturedType.Add(manufacturedItemsType, new List<string>{ manufacturedItems });
                    }
                }

                RespawnManufacturedItemsOnWorld();
            }
        }

        private void RespawnManufacturedItemsOnWorld() {
            DestroyImmediate(manufacturedItemsContainer);

            manufacturedItemsContainer = new GameObject("ManufacturedItems container") {
                transform = {
                    parent = transform.parent
                }
            };

            foreach (var manufacturedItemsList in manufacturedItemsDataDictionaryByManufacturedType) {
                foreach (var manufacturedItemsString in manufacturedItemsList.Value) {
                    manufacturedItemsToSpawn = ObjectsReference.Instance.meshReferenceScriptableObject.manufacturedItemPrefabByManufacturedItemType[manufacturedItemsList.Key];
                    
                    manufacturedItemsInstance = Instantiate(manufacturedItemsToSpawn, manufacturedItemsContainer.transform, true);
                    manufacturedItemsInstance.GetComponent<BananaBehaviour>().LoadSavedData(manufacturedItemsString);
                }
            }
        }

        public void SaveManufacturedItemData(string saveUuid) {
            _savePath = Path.Combine(ObjectsReference.Instance.gameSave._savesPath, saveUuid);
            var mapDataSavesPath = Path.Combine(_savePath, "WORLD_DATA");
            
            var savefilePath = Path.Combine(mapDataSavesPath, "manufacturedItems.json");
            
            var manufacturedItemsBehaviours = FindObjectsByType<ManufacturedItemBehaviour>(FindObjectsSortMode.None);
            
            manufacturedItemsDataDictionaryByManufacturedType.Clear();

            foreach (var manufacturedItemsBehaviour in manufacturedItemsBehaviours) manufacturedItemsBehaviour.GenerateDroppedData();
            
            var json = JsonConvert.SerializeObject(manufacturedItemsDataDictionaryByManufacturedType);
            File.WriteAllText(savefilePath, json);
        }
        
        public void AddManufacturedItemToDictionnary(ManufacturedItemsType manufacturedItemsType, string manufacturedItemsData) {
            if (manufacturedItemsDataDictionaryByManufacturedType.ContainsKey(manufacturedItemsType)) {
                manufacturedItemsDataDictionaryByManufacturedType[manufacturedItemsType].Add(manufacturedItemsData);
            }
            else {
                manufacturedItemsDataDictionaryByManufacturedType.Add(manufacturedItemsType, new List<string>{ manufacturedItemsData });
            }
        }
    }
}
