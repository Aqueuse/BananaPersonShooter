using System;
using System.Collections.Generic;
using System.IO;
using InGame.Items.ItemsBehaviours;
using InGame.Items.ItemsData;
using Newtonsoft.Json;
using Save.Helpers;
using UnityEngine;

namespace Save {
    public class DebrisSave : MonoBehaviour {
        public GameObject debrisContainer;
    
        private string _savePath;

        private GameObject debrisToSpawn;
        private GameObject debrisInstance;

        public GenericDictionary<CharacterType, List<string>> debrisDataDictionnaryByCharacterType;

        private string[] _buildablesDatas;
        
        public void LoadDebrisDataByUuid(string saveUuid) {
            _savePath = Path.Combine(ObjectsReference.Instance.gameSave._savesPath, saveUuid);
            var saveMapDatasPath = Path.Combine(_savePath, "WORLD_DATA");

            var loadfilePath = Path.Combine(saveMapDatasPath, "debris.json");

            if (!File.Exists(loadfilePath)) return;
            
            debrisDataDictionnaryByCharacterType.Clear();
            
            var json = File.ReadAllText(loadfilePath);

            var debrisDictionnary = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(json);

            foreach (var debrisList in debrisDictionnary) {
                CharacterType debrisType = Enum.Parse<CharacterType>(debrisList.Key);

                foreach (var debris in debrisList.Value) {
                    if (debrisDataDictionnaryByCharacterType.ContainsKey(debrisType)) {
                        debrisDataDictionnaryByCharacterType[debrisType].Add(debris);
                    }
                    else {
                        debrisDataDictionnaryByCharacterType.Add(debrisType, new List<string>{ debris });
                    }
                }
            }
            
            RespawnDebrisOnWorld();
        }

        public void SaveDebrisData(string saveUuid) {
            _savePath = Path.Combine(ObjectsReference.Instance.gameSave._savesPath, saveUuid);
            var mapDataSavesPath = Path.Combine(_savePath, "WORLD_DATA");
            
            var savefilePath = Path.Combine(mapDataSavesPath, "debris.json");
        
            var debrisBehaviours = FindObjectsByType<DebrisBehaviour>(FindObjectsSortMode.None);

            debrisDataDictionnaryByCharacterType.Clear();
            
            foreach (var debris in debrisBehaviours) {
                debris.GenerateDebrisData();
            }

            var debrisToSave = debrisDataDictionnaryByCharacterType;

            var json = JsonConvert.SerializeObject(debrisToSave);
            File.WriteAllText(savefilePath, json);
        }
    
        public void AddDebrisToDebrisDictionnary(CharacterType debrisType, string debrisData) {
            if (debrisDataDictionnaryByCharacterType.ContainsKey(debrisType)) {
                debrisDataDictionnaryByCharacterType[debrisType].Add(debrisData);
            }
            else {
                debrisDataDictionnaryByCharacterType.Add(debrisType, new List<string>{ debrisData });
            }
        }
    
        private void RespawnDebrisOnWorld() {
            DestroyImmediate(debrisContainer);

            debrisContainer = new GameObject("Debris container") {
                transform = {
                    parent = transform.parent
                }
            };

            foreach (var debrisToInstantiate in debrisDataDictionnaryByCharacterType) {
                foreach (var debrisString in debrisToInstantiate.Value) {
                    var debrisData = JsonConvert.DeserializeObject<DebrisData>(debrisString);

                    debrisToSpawn = ObjectsReference.Instance.meshReferenceScriptableObject.debrisPrefabsByCharacterType[debrisToInstantiate.Key][debrisData.prefabIndex];

                    debrisInstance = Instantiate(debrisToSpawn, debrisContainer.transform, true);

                    debrisInstance.transform.position = JsonHelper.FromStringToVector3(debrisData.debrisPosition);
                    debrisInstance.transform.rotation = JsonHelper.FromStringToQuaternion(debrisData.debrisRotation);

                    var debrisBehaviour = debrisInstance.GetComponent<DebrisBehaviour>();
                
                    debrisBehaviour.debrisPrefabIndex = debrisData.prefabIndex;
                    debrisBehaviour.characterType = debrisToInstantiate.Key;
                    debrisBehaviour.isInSpace = debrisData.isInSpace;

                    if (debrisBehaviour.isInSpace) {
                        var debrisRigidbody = debrisInstance.AddComponent<Rigidbody>();
                        debrisRigidbody.useGravity = false;
                
                        debrisRigidbody.AddExplosionForce(10f, transform.position, 10f);
                        debrisRigidbody.AddTorque(debrisInstance.transform.position, ForceMode.Impulse);
                        debrisBehaviour.isInSpace = true;
                        debrisBehaviour.DestroyIfUnreachable();
                    }
                }
            }
        }

        public void SpawnInitialDebris() {
            debrisInstance = Instantiate(ObjectsReference.Instance.worldData.initialDebrisOnWorld, debrisContainer.transform, true);
            
            debrisInstance.transform.position = debrisContainer.transform.position;
            debrisInstance.transform.rotation = debrisContainer.transform.rotation;
        }
    }
}
