using System;
using System.Collections.Generic;
using System.IO;
using InGame.Items.ItemsBehaviours;
using InGame.Items.ItemsData;
using Newtonsoft.Json;
using Save.Helpers;
using UnityEngine;

namespace Save {
    public class SpaceshipDebrisSave : MonoBehaviour {
        public GameObject spaceshipDebrisContainer;
    
        private string _savePath;

        private GameObject spaceshipDebrisToSpawn;
        private GameObject spaceshipDebrisInstance;

        public GenericDictionary<CharacterType, List<string>> spaceshipDebrisDataDictionnaryByCharacterType;

        private string[] _buildablesDatas;
        
        public void LoadSpaceshipDebrisDataByUuid(string saveUuid) {
            _savePath = Path.Combine(ObjectsReference.Instance.gameSave._savesPath, saveUuid);
            var saveMapDatasPath = Path.Combine(_savePath, "WORLD_DATA");

            var loadfilePath = Path.Combine(saveMapDatasPath, "spaceshipDebris.json");

            if (!File.Exists(loadfilePath)) return;
            
            spaceshipDebrisDataDictionnaryByCharacterType.Clear();
            
            var json = File.ReadAllText(loadfilePath);

            var spaceshipDebrisDictionnary = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(json);

            foreach (var spaceshipDebrisList in spaceshipDebrisDictionnary) {
                CharacterType characterType = Enum.Parse<CharacterType>(spaceshipDebrisList.Key);

                foreach (var spaceshipDebris in spaceshipDebrisList.Value) {
                    if (spaceshipDebrisDataDictionnaryByCharacterType.ContainsKey(characterType)) {
                        spaceshipDebrisDataDictionnaryByCharacterType[characterType].Add(spaceshipDebris);
                    }
                    else {
                        spaceshipDebrisDataDictionnaryByCharacterType.Add(characterType, new List<string>{ spaceshipDebris });
                    }
                }
            }
            
            RespawnSpaceshipDebrisOnWorld();
        }

        public void SaveSpaceshipDebrisData(string saveUuid) {
            _savePath = Path.Combine(ObjectsReference.Instance.gameSave._savesPath, saveUuid);
            var mapDataSavesPath = Path.Combine(_savePath, "WORLD_DATA");
            
            var savefilePath = Path.Combine(mapDataSavesPath, "spaceshipDebris.json");
        
            var spaceshipDebrisBehaviours = FindObjectsByType<SpaceshipDebrisBehaviour>(FindObjectsSortMode.None);

            spaceshipDebrisDataDictionnaryByCharacterType.Clear();
            
            foreach (var spaceshipDebrisBehaviour in spaceshipDebrisBehaviours) {
                spaceshipDebrisBehaviour.GenerateSpaceshipDebrisData();
            }

            var spaceshipDebrisToSave = spaceshipDebrisDataDictionnaryByCharacterType;

            var json = JsonConvert.SerializeObject(spaceshipDebrisToSave);
            File.WriteAllText(savefilePath, json);
        }
    
        public void AddSpaceshipDebrisToSpaceshipDebrisDictionnary(CharacterType characterType, string spaceshipDebrisData) {
            if (spaceshipDebrisDataDictionnaryByCharacterType.ContainsKey(characterType)) {
                spaceshipDebrisDataDictionnaryByCharacterType[characterType].Add(spaceshipDebrisData);
            }
            else {
                spaceshipDebrisDataDictionnaryByCharacterType.Add(characterType, new List<string>{ spaceshipDebrisData });
            }
        }
    
        private void RespawnSpaceshipDebrisOnWorld() {
            DestroyImmediate(spaceshipDebrisContainer);


            spaceshipDebrisContainer = new GameObject("Spaceship Debris container") {
                transform = {
                    parent = transform.parent
                }   
            };

            foreach (var spaceshipDebrisToInstantiate in spaceshipDebrisDataDictionnaryByCharacterType) {
                foreach (var spaceshipDebrisString in spaceshipDebrisToInstantiate.Value) {
                    var spaceshipDebrisData = JsonConvert.DeserializeObject<SpaceshipDebrisData>(spaceshipDebrisString);

                    spaceshipDebrisToSpawn = ObjectsReference.Instance.meshReferenceScriptableObject.spaceshipDebrisPrefabsByCharacterType[spaceshipDebrisToInstantiate.Key][spaceshipDebrisData.prefabIndex];

                    spaceshipDebrisInstance = Instantiate(spaceshipDebrisToSpawn, spaceshipDebrisContainer.transform, true);

                    spaceshipDebrisInstance.transform.position = JsonHelper.FromStringToVector3(spaceshipDebrisData.spaceshipDebrisPosition);
                    spaceshipDebrisInstance.transform.rotation = JsonHelper.FromStringToQuaternion(spaceshipDebrisData.spaceshipDebrisRotation);

                    var spaceshipDebrisBehaviour = spaceshipDebrisInstance.GetComponent<SpaceshipDebrisBehaviour>();
                
                    spaceshipDebrisBehaviour.spaceshipDebrisPrefabIndex = spaceshipDebrisData.prefabIndex;
                    spaceshipDebrisBehaviour.characterType = spaceshipDebrisToInstantiate.Key;
                    spaceshipDebrisBehaviour.isInSpace = spaceshipDebrisData.isInSpace;

                    if (spaceshipDebrisBehaviour.isInSpace) {
                        var spaceshipDebrisRigidbody = spaceshipDebrisInstance.AddComponent<Rigidbody>();
                        spaceshipDebrisRigidbody.useGravity = false;
                
                        spaceshipDebrisRigidbody.AddExplosionForce(10f, transform.position, 10f);
                        spaceshipDebrisRigidbody.AddTorque(spaceshipDebrisInstance.transform.position, ForceMode.Impulse);
                        spaceshipDebrisBehaviour.isInSpace = true;
                        spaceshipDebrisBehaviour.DestroyIfUnreachable();
                    }
                }
            }
        }

        public void SpawnInitialSpaceshipDebris() {
            spaceshipDebrisInstance = Instantiate(ObjectsReference.Instance.worldData.initialSpaceshipDebrisOnWorld, spaceshipDebrisContainer.transform, true);
            
            spaceshipDebrisInstance.transform.position = spaceshipDebrisContainer.transform.position;
            spaceshipDebrisInstance.transform.rotation = spaceshipDebrisContainer.transform.rotation;
        }
    }
}
