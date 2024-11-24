using System.Linq;
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
            foreach (var bananaSlot in ObjectsReference.Instance.bananaMan.bananaManData.bananasInventory.ToList()) {
                ObjectsReference.Instance.bananaMan.bananaManData.bananasInventory[bananaSlot.Key] = 0;
            }
            
            foreach (var ingredientSlot in ObjectsReference.Instance.bananaMan.bananaManData.ingredientsInventory.ToList()) {
                ObjectsReference.Instance.bananaMan.bananaManData.ingredientsInventory[ingredientSlot.Key] = 0;
            }

            foreach (var rawMaterialsSlot in ObjectsReference.Instance.bananaMan.bananaManData.rawMaterialInventory.ToList()) {
                ObjectsReference.Instance.bananaMan.bananaManData.rawMaterialInventory[rawMaterialsSlot.Key] = 0;
            }

            foreach (var manufacturedItemSlot in ObjectsReference.Instance.bananaMan.bananaManData.manufacturedItemsInventory.ToList()) {
                ObjectsReference.Instance.bananaMan.bananaManData.manufacturedItemsInventory[manufacturedItemSlot.Key] = 0;
            }
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
            ObjectsReference.Instance.bananaMan.bananaManData.activeDropped = ObjectsReference.Instance.meshReferenceScriptableObject.droppedPropertiesScriptableObjects[DroppedType.EMPTY];
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