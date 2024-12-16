using Save.Templates;
using UnityEngine;
using UnityEngine.AI;

namespace Save {
    public class GameReset : MonoBehaviour {
        private Vector3[] _droppedPosition;
        private Quaternion[] _droppedRotation;
        private int[] _droppedPrefabIndex;
        
        private GameObject _droppedContainer;
        
        private BananaManSavedData _bananaManSavedData;
        private WorldSavedData _worldSavedData;

        public void ResetGameData() {
            ResetInventories();
            ResetBananaManVitals();
            ResetActiveItem();

            ResetTutorial();
            
            ResetPosition();
            
            ResetMonkeysSasiety();
            ResetMonkeysPositions();
        }

        private static void ResetInventories() {
            ObjectsReference.Instance.BananaManBananasInventory.ResetInventory();
            ObjectsReference.Instance.bananaManIngredientsInventory.ResetInventory();
            ObjectsReference.Instance.bananaManRawMaterialInventory.ResetInventory();
            ObjectsReference.Instance.bananaManManufacturedItemsInventory.ResetInventory();
        }
        
        private static void ResetBananaManVitals() {
            ObjectsReference.Instance.bananaMan.health = 100;
            ObjectsReference.Instance.bananaMan.resistance = 100;
            
            ObjectsReference.Instance.bananaMan.SetBananaSkinHealth();
        }

        private static void ResetPosition() {
            ObjectsReference.Instance.bananaMan.transform.position = ObjectsReference.Instance.gameManager.spawnPointsBySpawnType[SpawnPoint.NEW_GAME].position;
        }

        private static void ResetTutorial() {
            ObjectsReference.Instance.bananaMan.tutorialFinished = false;
        }

        private static void ResetActiveItem() {
            ObjectsReference.Instance.bananaMan.bananaManData.activeBanana = BananaType.CAVENDISH;
            ObjectsReference.Instance.bananaMan.bananaManData.activeBuildable = BuildableType.BUMPER;
            ObjectsReference.Instance.bananaMan.bananaManData.activeIngredient = IngredientsType.BANANA_DOG_BREAD;
            ObjectsReference.Instance.bananaMan.bananaManData.activeRawMaterial = RawMaterialType.METAL;
            ObjectsReference.Instance.bananaMan.bananaManData.activeManufacturedItem = ManufacturedItemsType.SPACESHIP_TOY;

        }

        private static void ResetMonkeysSasiety() {
            foreach (var monkey in ObjectsReference.Instance.worldData.monkeys) {
                monkey.monkeyPropertiesScriptableObject.sasietyTimer = 120;
                monkey.sasietyTimer = 120;
            }
        }
        
        private static void ResetMonkeysPositions() {
            foreach (var monkey in ObjectsReference.Instance.worldData.monkeys) {
                monkey.monkeyPropertiesScriptableObject.lastPosition = monkey.monkeyPropertiesScriptableObject.initialPosition;
                monkey.GetComponent<NavMeshAgent>().Warp(monkey.monkeyPropertiesScriptableObject.initialPosition);
            }
        }
    }
}