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
        private string _savePath;
        private Transform debrisContainer;

        private GameObject spaceshipDebrisToSpawn;
        private GameObject spaceshipDebrisInstance;
        private SpaceshipDebrisBehaviour spaceshipDebrisBehaviourInstance;

        public GenericDictionary<SpaceshipType, List<string>> spaceshipDebrisDataDictionnaryBySpaceshipType;

        private string[] _buildablesDatas;

        private void Start() {
            debrisContainer = ObjectsReference.Instance.gameSave.debrisContainer;
        }

        public void LoadSpaceshipDebrisDataByUuid(string saveUuid) {
            _savePath = Path.Combine(ObjectsReference.Instance.gameSave._savesPath, saveUuid);
            var saveMapDatasPath = Path.Combine(_savePath, "WORLD_DATA");

            var loadfilePath = Path.Combine(saveMapDatasPath, "spaceshipDebris.json");

            if (!File.Exists(loadfilePath)) return;
            
            spaceshipDebrisDataDictionnaryBySpaceshipType.Clear();
            
            var json = File.ReadAllText(loadfilePath);

            var spaceshipDebrisDictionnary = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(json);

            foreach (var spaceshipDebrisList in spaceshipDebrisDictionnary) {
                var spaceshipType = Enum.Parse<SpaceshipType>(spaceshipDebrisList.Key);

                foreach (var spaceshipDebris in spaceshipDebrisList.Value) {
                    if (spaceshipDebrisDataDictionnaryBySpaceshipType.ContainsKey(spaceshipType)) {
                        spaceshipDebrisDataDictionnaryBySpaceshipType[spaceshipType].Add(spaceshipDebris);
                    }
                    else {
                        spaceshipDebrisDataDictionnaryBySpaceshipType.Add(spaceshipType, new List<string>{ spaceshipDebris });
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

            spaceshipDebrisDataDictionnaryBySpaceshipType.Clear();
            
            foreach (var spaceshipDebrisBehaviour in spaceshipDebrisBehaviours) {
                spaceshipDebrisBehaviour.GenerateSpaceshipDebrisData();
            }

            var spaceshipDebrisToSave = spaceshipDebrisDataDictionnaryBySpaceshipType;

            var json = JsonConvert.SerializeObject(spaceshipDebrisToSave);
            File.WriteAllText(savefilePath, json);
        }
    
        public void AddSpaceshipDebrisToSpaceshipDebrisDictionnary(SpaceshipType spaceshipType, string spaceshipDebrisData) {
            if (spaceshipDebrisDataDictionnaryBySpaceshipType.ContainsKey(spaceshipType)) {
                spaceshipDebrisDataDictionnaryBySpaceshipType[spaceshipType].Add(spaceshipDebrisData);
            }
            else {
                spaceshipDebrisDataDictionnaryBySpaceshipType.Add(spaceshipType, new List<string>{ spaceshipDebrisData });
            }
        }
    
        private void RespawnSpaceshipDebrisOnWorld() {
            
            foreach (var spaceshipDebrisToInstantiate in spaceshipDebrisDataDictionnaryBySpaceshipType) {
                foreach (var spaceshipDebrisString in spaceshipDebrisToInstantiate.Value) {
                    var spaceshipDebrisData = JsonConvert.DeserializeObject<SpaceshipDebrisData>(spaceshipDebrisString);

                    spaceshipDebrisToSpawn = 
                        ObjectsReference.Instance.meshReferenceScriptableObject.spaceshipDebrisBySpaceshipType
                            [spaceshipDebrisData.spaceshipType]
                            [spaceshipDebrisData.prefabIndex];

                    spaceshipDebrisInstance = Instantiate(
                        spaceshipDebrisToSpawn, 
                        JsonHelper.FromStringToVector3(spaceshipDebrisData.spaceshipDebrisPosition),
                        JsonHelper.FromStringToQuaternion(spaceshipDebrisData.spaceshipDebrisRotation),
                        debrisContainer
                    );
                    
                    spaceshipDebrisBehaviourInstance = spaceshipDebrisInstance.GetComponent<SpaceshipDebrisBehaviour>();
                
                    spaceshipDebrisBehaviourInstance.prefabIndex = spaceshipDebrisData.prefabIndex;
                    spaceshipDebrisBehaviourInstance.spaceshipType = spaceshipDebrisData.spaceshipType;
                    spaceshipDebrisBehaviourInstance.isInSpace = spaceshipDebrisData.isInSpace;
                    spaceshipDebrisBehaviourInstance.bananaEffect = spaceshipDebrisData.bananaEffect;
                    spaceshipDebrisBehaviourInstance.effectSource =
                        JsonHelper.FromStringToVector3(spaceshipDebrisData.effectSourcePosition);

                    if (spaceshipDebrisBehaviourInstance.isInSpace) {
                        var spaceshipDebrisRigidbody = spaceshipDebrisInstance.AddComponent<Rigidbody>();
                        spaceshipDebrisRigidbody.useGravity = false;
                        spaceshipDebrisRigidbody.AddExplosionForce(10f, transform.position, 10f);
                        spaceshipDebrisRigidbody.AddTorque(spaceshipDebrisInstance.transform.position, ForceMode.Impulse);
                        
                        spaceshipDebrisBehaviourInstance.DestroyIfUnreachable();
                    }
                }
            }
        }

        public void SpawnInitialSpaceshipDebris() {
            Instantiate(
                ObjectsReference.Instance.worldData.initialSpaceshipDebrisOnWorld,
                debrisContainer.transform.position,
                debrisContainer.transform.rotation,
                debrisContainer
            );
        }
    }
}
