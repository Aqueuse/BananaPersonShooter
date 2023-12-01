using System.Collections.Generic;
using Game.BananaCannonMiniGame;
using Monkeys;
using Tags;
using UnityEngine;

namespace Gestion {
    public class MapItems : MonoSingleton<MapItems> {
        public GameObject aspirablesContainer;

        public DebrisSpawner debrisSpawner;
        public Monkey[] monkeys;

        public Collider cameraBounds;

        public void SpawnDebris() {
            if (ObjectsReference.Instance.mapsManager.currentMap.piratesDebrisToSpawn > 0) {
                if (ObjectsReference.Instance.gameSettings.areDebrisFallingOnTheTrees) debrisSpawner.SpawnNewDebrisOnRaycastHit();

                else {
                    debrisSpawner.SpawnNewDebrisOnNavMesh();
                }
            }
        }

        public List<GameObject> GetAllItemsInAspirableContainer() {
            var gameObjects = aspirablesContainer.GetComponentsInChildren<Tag>();
            
            var gameObjectsWithTag = new List<GameObject>();
            
            foreach (var objectTag in gameObjects) {
                gameObjectsWithTag.Add(objectTag.gameObject);
            }

            return gameObjectsWithTag;
        }
    }
}