using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.BananaCannonMiniGame {
    public class SpaceshipsSpawner : MonoBehaviour {
        public RectTransform entryTransform;
        public RectTransform exitTransform;

        [SerializeField] private float spaceshipPropulsionSpeed = 0.4f;

        [SerializeField] private GameObject pirateSpaceshipPrefab;
        [SerializeField] private GameObject visitorSpaceshipPrefab;

        [SerializeField] private Transform spaceshipsContainer;

        private GameObject _spaceshipInstance;
        
        public void SpawnSpaceships(List<CharacterType> spaceshipsToSpawn) {
            foreach (var spaceship in spaceshipsToSpawn) {
                Invoke(spaceship == CharacterType.PIRATE ? nameof(SpawnPirateSpaceship) : nameof(SpawnVisitorSpaceship), 2);
            }
        }
        
        private Vector3 GetRandomizeEntryPoint() {
            var entryRandomTransform = entryTransform.localPosition;
            entryRandomTransform.y = Random.Range(-50, 50);
            entryTransform.localPosition = entryRandomTransform;
            return entryTransform.position;
        }
    
        private Vector3 GetRandomizeExitPoint() {
            var exitRandomTransform = exitTransform.localPosition;
            exitRandomTransform.y = Random.Range(-20, 20);
            exitTransform.localPosition = exitRandomTransform;

            return exitTransform.position;
        }

        private void SpawnVisitorSpaceship() {
            _spaceshipInstance = Instantiate(original: visitorSpaceshipPrefab, position: GetRandomizeEntryPoint(), 
                rotation: Quaternion.identity, parent: spaceshipsContainer);
            
            _spaceshipInstance.GetComponent<Spaceship>().InitiatePropulsion(GetRandomizeExitPoint(), spaceshipPropulsionSpeed);
        }

        private void SpawnPirateSpaceship() {
            _spaceshipInstance = Instantiate(original: pirateSpaceshipPrefab, position: GetRandomizeEntryPoint(), 
                rotation: Quaternion.identity, parent: spaceshipsContainer);
            
            _spaceshipInstance.GetComponent<Spaceship>().InitiatePropulsion(GetRandomizeExitPoint(), spaceshipPropulsionSpeed);
        }

        public void DestroyAllSpaceships() {
            foreach (var spaceship in spaceshipsContainer.GetComponentsInChildren<Spaceship>()) {
                Destroy(spaceship);
            }
        }
    }
}
