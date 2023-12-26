using System.Collections.Generic;
using Game.BananaCannonMiniGame;
using Game.Monkeys.Ancestors;
using Gestion;
using Gestion.BuildablesBehaviours;
using Newtonsoft.Json;
using Save.Helpers;
using UnityEngine;

namespace Data {
    public class Map : MonoSingleton<Map> {
        private GameObject aspirable;
        private GameObject prefab;

        public GameObject aspirablesContainer;

        public DebrisSpawner debrisSpawner;
        public Monkey[] monkeysInMap;

        public Collider cameraBounds;

        public void SaveAspirablesOnMap() {
            var buildablesBehaviours = FindObjectsByType<BuildableBehaviour>(FindObjectsSortMode.None);
            var debrisBehaviours = FindObjectsByType<DebrisBehaviour>(FindObjectsSortMode.None);

            ObjectsReference.Instance.gameData.currentMapData.buildablesDataInMapDictionaryByBuildableType.Clear();
            ObjectsReference.Instance.gameData.currentMapData.debrisDataInMapDictionnaryByCharacterType.Clear();

            foreach (var buildable in buildablesBehaviours) {
                buildable.GenerateSaveData();
            }

            foreach (var debris in debrisBehaviours) {
                debris.GenerateDebrisData();
            }
        }

        public void RespawnAspirablesOnMap() {
            var mapData = ObjectsReference.Instance.gameData.currentMapData;

            if (mapData.isDiscovered) {
                DestroyImmediate(aspirablesContainer);

                aspirablesContainer = new GameObject("aspirables") {
                    transform = {
                        parent = transform
                    }
                };

                RespawnBuildablesOnMap(mapData);
                RespawnDebrisOnMap(mapData);
            }

            else {
                if (mapData.initialAspirablesOnMap != null) {
                    aspirable = Instantiate(
                        mapData.initialAspirablesOnMap,
                        aspirablesContainer.transform,
                        true
                    );

                    aspirable.transform.position = aspirablesContainer.transform.position;
                    aspirable.transform.rotation = aspirablesContainer.transform.rotation;
                }
            }
        }

        private void RespawnBuildablesOnMap(MapData mapData) {
            var buildablesDictionnary = mapData.buildablesDataInMapDictionaryByBuildableType;

            foreach (var buildable in buildablesDictionnary) {
                foreach (var buildableString in buildable.Value) {
                    prefab = ObjectsReference.Instance.meshReferenceScriptableObject.buildablePrefabByBuildableType[buildable.Key];

                    aspirable = Instantiate(prefab, aspirablesContainer.transform, true);
                    aspirable.GetComponent<BuildableBehaviour>().LoadSavedData(buildableString);
                }
            }
            
        }

        private void RespawnDebrisOnMap(MapData mapData) {
            var debrisDataInMapDictionnary = mapData.debrisDataInMapDictionnaryByCharacterType;
                
            foreach (var debris in debrisDataInMapDictionnary) {
                foreach (var debrisString in debris.Value) {
                    var debrisData = JsonConvert.DeserializeObject<DebrisData>(debrisString);
                    
                    prefab = ObjectsReference.Instance.meshReferenceScriptableObject.debrisPrefabsBycharacterType[debris.Key][debrisData.prefabIndex];

                    aspirable = Instantiate(prefab, aspirablesContainer.transform, true);

                    aspirable.transform.position = JsonHelper.FromStringToVector3(debrisData.debrisPosition);
                    aspirable.transform.rotation = JsonHelper.FromStringToQuaternion(debrisData.debrisRotation);
                    aspirable.GetComponent<DebrisBehaviour>().debrisPrefabIndex = debrisData.prefabIndex;
                    aspirable.GetComponent<DebrisBehaviour>().characterType = debris.Key;
                }
            }
        }

        public void SpawnNewDebris() {
            if (ObjectsReference.Instance.gameData.currentMapData.piratesDebrisToSpawn > 0) {
                if (ObjectsReference.Instance.gameSettings.areDebrisFallingOnTheTrees) debrisSpawner.SpawnNewDebrisOnRaycastHit();

                else {
                    debrisSpawner.SpawnNewDebrisOnNavMesh();
                }
            }
        }

        public List<GameObject> GetAllBuildablesByTypeInAspirableContainer(BuildableType buildableType) {
            var buildablesBehaviours = FindObjectsByType<BuildableBehaviour>(FindObjectsSortMode.None);
            
            var gameObjectsWithBuildableType = new List<GameObject>();

            foreach (var buildable in buildablesBehaviours) {
                if (buildable.buildableType == buildableType) {
                    gameObjectsWithBuildableType.Add(buildable.gameObject);
                }
            }

            return gameObjectsWithBuildableType;
        }
        
        public void SaveMapMonkeysData() {
            if (monkeysInMap != null) {
                foreach (var monkey in monkeysInMap) {
                    ObjectsReference.Instance.gameData.currentMapData.monkeysPositionByMonkeyId[monkey.monkeyId] =
                        monkey.transform.position;

                    ObjectsReference.Instance.gameData.currentMapData.monkeysSasietyTimerByMonkeyId[monkey.monkeyId] =
                        monkey.sasietyTimer;
                }
            }
        }
    }
}