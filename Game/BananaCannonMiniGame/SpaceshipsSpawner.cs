using Enums;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.BananaCannonMiniGame {
    public enum SpaceshipDestination {
        ENTRY,
        EXIT
    }

    public class SpaceshipsSpawner : MonoBehaviour {
        public RectTransform entryTransform;
        public RectTransform exitTransform;

        [SerializeField] private Transform minRotation;
        [SerializeField] private Transform maxRotation;

        [SerializeField] private float speed = 0.01f;

        [SerializeField] private GameObject spaceshipPrefab;

        [SerializeField] private Transform spaceshipsContainer;

        private GameObject _spaceshipInstance;

        private int _spaceshipsMaxQuantity;
        
        public void SpawnSpaceships(int quantity) {
            _spaceshipsMaxQuantity = quantity;
            
            InvokeRepeating(nameof(GetRandomizeEntryPoint), 0, 1);
            InvokeRepeating(nameof(GetRandomizeExitPoint), 0, 1);
            
            InvokeRepeating(nameof(SpawnSpaceship), 0, 3);
        }

        private void Update() {
            if (ObjectsReference.Instance.gameManager.gameContext != GameContext.IN_MINI_GAME) return;
            
            transform.rotation = Quaternion.Lerp(minRotation.rotation, maxRotation.rotation, Time.deltaTime * speed);
        }

        private void GetRandomizeEntryPoint() {
            var entryRandomTransform = entryTransform.localPosition;
            entryRandomTransform.y = Random.Range(-50, 50);
            entryTransform.localPosition = entryRandomTransform;
        }
    
        private void GetRandomizeExitPoint() {
            var exitRandomTransform = exitTransform.localPosition;
            exitRandomTransform.y = Random.Range(-50, 50);
            exitTransform.localPosition = exitRandomTransform;
        }

        private void SpawnSpaceship() {
            if (spaceshipsContainer.GetComponentsInChildren<Spaceship>().Length+1 > _spaceshipsMaxQuantity) {
                CancelInvoke(nameof(SpawnSpaceship));
                return;
            }

            _spaceshipInstance = Instantiate(original: spaceshipPrefab, position: entryTransform.transform.position, 
                rotation: Quaternion.identity, parent: spaceshipsContainer);
            
            _spaceshipInstance.GetComponent<Spaceship>().InitiatePropulsion(exitTransform.transform.position, 0.2f);
        }
        
        public void DestroyAllSpaceships() {
            foreach (var spaceship in spaceshipsContainer.GetComponentsInChildren<Spaceship>()) {
                Destroy(spaceship);
            }
        }
    }
}
