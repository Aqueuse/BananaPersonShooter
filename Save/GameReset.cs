using System.Linq;
using Save.Templates;
using UnityEngine;

namespace Save {
    public class GameReset : MonoBehaviour {
        private Vector3[] _debrisPosition;
        private Quaternion[] _debrisRotation;
        private int[] _debrisPrefabIndex;
        
        private GameObject _debrisContainer;
        
        private BananaManSavedData _bananaManSavedData;
        private WorldSavedData _worldSavedData;

        public void ResetGameData() {
            ResetInventories();
            ResetBananaSlot();
            ResetBananaManVitals();
            ResetActiveItem();

            ResetTutorial();
            
            ResetPositionAndLastMap();
            
            ResetMonkeysSasiety();
            ResetMonkeysPositions();
        }

        private static void ResetInventories() {
            foreach (var bananaSlot in ObjectsReference.Instance.bananaMan.inventories.bananasInventory.ToList()) {
                ObjectsReference.Instance.bananaMan.inventories.bananasInventory[bananaSlot.Key] = 0;
                ObjectsReference.Instance.gameData.bananaManSaved.bananaInventory[bananaSlot.Key.ToString()] = 0;
            }
            
            foreach (var ingredientSlot in ObjectsReference.Instance.bananaMan.inventories.ingredientsInventory.ToList()) {
                ObjectsReference.Instance.bananaMan.inventories.ingredientsInventory[ingredientSlot.Key] = 0;
                ObjectsReference.Instance.gameData.bananaManSaved.ingredientsInventory[ingredientSlot.Key.ToString()] = 0;
            }

            foreach (var rawMaterialsSlot in ObjectsReference.Instance.bananaMan.inventories.rawMaterialsInventory.ToList()) {
                ObjectsReference.Instance.bananaMan.inventories.rawMaterialsInventory[rawMaterialsSlot.Key] = 0;
                ObjectsReference.Instance.gameData.bananaManSaved.rawMaterialsInventory[rawMaterialsSlot.Key.ToString()] = 0;
            }

            foreach (var manufacturedItemSlot in ObjectsReference.Instance.bananaMan.inventories.manufacturedItemsInventory.ToList()) {
                ObjectsReference.Instance.bananaMan.inventories.manufacturedItemsInventory[manufacturedItemSlot.Key] = 0;
                ObjectsReference.Instance.gameData.bananaManSaved.manufacturedInventory[manufacturedItemSlot.Key.ToString()] = 0;
            }
        }

        private static void ResetBananaSlot() {
            ObjectsReference.Instance.gameData.bananaManSaved.bananaSlot = BananaType.CAVENDISH.ToString();
        }

        private static void ResetBananaManVitals() {
            ObjectsReference.Instance.bananaMan.health = 100;
            ObjectsReference.Instance.bananaMan.resistance = 100;

            ObjectsReference.Instance.gameData.bananaManSaved.health = 100;
            ObjectsReference.Instance.gameData.bananaManSaved.resistance = 100;
            
            ObjectsReference.Instance.bananaMan.SetBananaSkinHealth();
        }

        private static void ResetPositionAndLastMap() {
            ObjectsReference.Instance.gameData.lastPositionOnMap = ObjectsReference.Instance.gameManager.spawnPointsBySpawnType[SpawnPoint.NEW_GAME].position;
            ObjectsReference.Instance.gameData.bananaManSaved.lastMap = RegionType.COROLLE;
            ObjectsReference.Instance.bananaMan.transform.position = ObjectsReference.Instance.gameData.lastPositionOnMap;
        }

        private static void ResetTutorial() {
            ObjectsReference.Instance.bananaMan.tutorialFinished = false;
            
            ObjectsReference.Instance.gameData.bananaManSaved.hasFinishedTutorial = false;
        }

        private static void ResetActiveItem() {
            ObjectsReference.Instance.bananaMan.activeItem = ObjectsReference.Instance.meshReferenceScriptableObject.bananasPropertiesScriptableObjects[BananaType.EMPTY];

            ObjectsReference.Instance.gameData.bananaManSaved.activeBanana = 0;
        }

        private static void ResetMonkeysSasiety() {
            ObjectsReference.Instance.gameData.worldData.monkeysSasietyTimerByMonkeyId = new GenericDictionary<string, int>();
        }
        
        private static void ResetMonkeysPositions() {
            ObjectsReference.Instance.gameData.worldData.monkeysPositionByMonkeyId = new GenericDictionary<string, Vector3>();
        }
    }
}