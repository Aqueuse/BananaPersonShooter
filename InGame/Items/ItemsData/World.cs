using System.Collections.Generic;
using InGame.Items.ItemsBehaviours;
using InGame.Items.ItemsBehaviours.BuildablesBehaviours;
using InGame.MiniGames.SpaceTrafficControlMiniGame.debris;
using InGame.Monkeys.Ancestors;
using Newtonsoft.Json;
using Save.Helpers;
using UnityEngine;

namespace InGame.Items.ItemsData {
    public class World : MonoSingleton<World> {
        private GameObject aspirable;
        private GameObject prefab;

        public GameObject buildablesContainer;

        public DebrisSpawner debrisSpawner;
        public Monkey[] monkeysInMap;

        public Collider cameraBounds;

        public void SaveAspirablesOnWorld() {
            var buildablesBehaviours = FindObjectsByType<BuildableBehaviour>(FindObjectsSortMode.None);
            var debrisBehaviours = FindObjectsByType<DebrisBehaviour>(FindObjectsSortMode.None);

            ObjectsReference.Instance.gameData.worldData.buildablesDataDictionaryByBuildableType.Clear();
            ObjectsReference.Instance.gameData.worldData.debrisDataDictionnaryByCharacterType.Clear();

            foreach (var buildableBehaviour in buildablesBehaviours) {
                buildableBehaviour.GenerateSaveData();
            }

            foreach (var debris in debrisBehaviours) {
                debris.GenerateDebrisData();
            }
        }

        public void SpawnInitialBuidablesAndDebrisOnWorld() {
            aspirable = Instantiate(
                ObjectsReference.Instance.gameData.worldData.initialAspirablesOnWorld,
                buildablesContainer.transform,
                true
            );

            aspirable.transform.position = buildablesContainer.transform.position;
            aspirable.transform.rotation = buildablesContainer.transform.rotation;
        }
        
        public void RespawnBuildablesOnWorld() {
            var worldData = ObjectsReference.Instance.gameData.worldData;

            DestroyImmediate(buildablesContainer);

            buildablesContainer = new GameObject("buildables") {
                transform = {
                    parent = transform
                }
            };

            var buildablesDictionnary = worldData.buildablesDataDictionaryByBuildableType;

            foreach (var buildableToInstantiate in buildablesDictionnary) {
                foreach (var buildableString in buildableToInstantiate.Value) {
                    prefab = ObjectsReference.Instance.meshReferenceScriptableObject.buildablePrefabByBuildableType[buildableToInstantiate.Key];

                    aspirable = Instantiate(prefab, buildablesContainer.transform, true);
                    aspirable.GetComponent<BuildableBehaviour>().LoadSavedData(buildableString);
                }
            }
        }

        public void RespawnDebrisOnWorld() {
            var worldData = ObjectsReference.Instance.gameData.worldData;
            var debrisDataInMapDictionnary = worldData.debrisDataDictionnaryByCharacterType;

            foreach (var debrisToInstantiate in debrisDataInMapDictionnary) {
                foreach (var debrisString in debrisToInstantiate.Value) {
                    var debrisData = JsonConvert.DeserializeObject<DebrisData>(debrisString);

                    prefab = ObjectsReference.Instance.meshReferenceScriptableObject.debrisPrefabsBycharacterType[debrisToInstantiate.Key][debrisData.prefabIndex];

                    aspirable = Instantiate(prefab, buildablesContainer.transform, true);

                    aspirable.transform.position = JsonHelper.FromStringToVector3(debrisData.debrisPosition);
                    aspirable.transform.rotation = JsonHelper.FromStringToQuaternion(debrisData.debrisRotation);
                    aspirable.GetComponent<DebrisBehaviour>().debrisPrefabIndex = debrisData.prefabIndex;
                    aspirable.GetComponent<DebrisBehaviour>().characterType = debrisToInstantiate.Key;
                }
            }
        }

        public void SpawnNewDebris() {
            var mapData = ObjectsReference.Instance.gameData.worldData;
            var newDebrisQuantity = mapData.piratesDebrisToSpawn + mapData.visitorsDebrisToSpawn + mapData.merchantsDebrisToSpawn; 

            if (newDebrisQuantity > 0) {
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
                    ObjectsReference.Instance.gameData.worldData.monkeysPositionByMonkeyId[monkey.monkeyId] =
                        monkey.transform.position;

                    ObjectsReference.Instance.gameData.worldData.monkeysSasietyTimerByMonkeyId[monkey.monkeyId] =
                        monkey.sasietyTimer;
                }
            }
        }
    }
}