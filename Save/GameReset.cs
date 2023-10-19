using System.Linq;
using Enums;
using Save.Templates;
using UnityEngine;

namespace Save {
    public class GameReset : MonoBehaviour {
        private Vector3[] _debrisPosition;
        private Quaternion[] _debrisRotation;
        private int[] _debrisPrefabIndex;
        
        private GameObject _debrisContainer;
        
        private BananaManSavedData _bananaManSavedData;
        private MapSavedData _mapSavedData;

        public void ResetGameData() {
            ResetInventory();
            ResetBlueprints();
            ResetSlots();
            ResetBananaManVitals();
            ResetTutorial();
            
            ResetPositionAndLastMap();
            ResetActiveItem();
            ResetMonkeysSasiety();

            // lock maps
            // reinit spaceship state
            // reinit assets positions on maps
        }

        private static void ResetInventory() {
            foreach (var bananaSlot in ObjectsReference.Instance.bananasInventory.bananasInventory.ToList()) {
                ObjectsReference.Instance.bananasInventory.bananasInventory[bananaSlot.Key] = 0;
                ObjectsReference.Instance.gameData.bananaManSavedData.bananaInventory[bananaSlot.Key.ToString()] = 0;
            }
        }

        private static void ResetBlueprints() {
            ObjectsReference.Instance.uiBlueprintsInventory.HideAllBlueprints();
        }

        private static void ResetSlots() {
            for (var i = 0; i < ObjectsReference.Instance.gameData.bananaManSavedData.slots.Count; i++) {
                ObjectsReference.Instance.gameData.bananaManSavedData.slots[i] = BananaType.EMPTY.ToString();
            }
            
            foreach (var uiSlot in ObjectsReference.Instance.uiQuickSlotsManager.uiQuickSlotsScripts) {
                uiSlot.EmptySlot();
            }
        }

        private static void ResetBananaManVitals() {
            ObjectsReference.Instance.bananaMan.health = 100;
            ObjectsReference.Instance.bananaMan.resistance = 100;

            ObjectsReference.Instance.gameData.bananaManSavedData.health = 100;
            ObjectsReference.Instance.gameData.bananaManSavedData.resistance = 100;
            
            ObjectsReference.Instance.bananaMan.SetBananaSkinHealth();
        }

        private static void ResetPositionAndLastMap() {
            ObjectsReference.Instance.gameData.lastPositionOnMap = ObjectsReference.Instance.scenesSwitch.spawnPointsBySpawnType[SpawnPoint.NEW_GAME].position;
            ObjectsReference.Instance.gameData.bananaManSavedData.lastMap = "COROLLE";
            ObjectsReference.Instance.bananaMan.transform.position = ObjectsReference.Instance.gameData.lastPositionOnMap;
        }

        private static void ResetTutorial() {
            ObjectsReference.Instance.bananaMan.tutorialFinished = false;
            
            ObjectsReference.Instance.gameData.bananaManSavedData.hasFinishedTutorial = false;
        }

        private static void ResetActiveItem() {
            ObjectsReference.Instance.bananaMan.activeBananaType = BananaType.EMPTY;
            ObjectsReference.Instance.bananaMan.activeItemCategory = ItemCategory.EMPTY;

            ObjectsReference.Instance.gameData.bananaManSavedData.activeBanana = 0;
        }

        private static void ResetMonkeysSasiety() {
            foreach (var mapData in ObjectsReference.Instance.mapsManager.mapBySceneName) {
                if (mapData.Value.mapDataScriptableObject.monkeyDataScriptableObjectsByMonkeyId.Count > 0) {
                    foreach (var monkeyDataScriptableObject in mapData.Value.mapDataScriptableObject.monkeyDataScriptableObjectsByMonkeyId) {
                        monkeyDataScriptableObject.Value.sasiety = 0;
                    }
                }
            }
        }
    }
}