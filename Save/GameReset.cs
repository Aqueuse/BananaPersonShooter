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
            ResetSlots();

            ResetDiscoveredMaterials();
            
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

        private static void ResetSlots() {
            foreach (var bottomSlot in ObjectsReference.Instance.bottomSlots.uiBottomSlots) {
                bottomSlot.ResetSlot();
            }
            
            ObjectsReference.Instance.bottomSlots.ActivateSlot(0);
            ObjectsReference.Instance.bottomSlots.activeSlotIndex = 0;
        }

        private static void ResetDiscoveredMaterials() {
            ObjectsReference.Instance.bananaMan.bananaManData.discoveredRawMaterials.Clear();
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