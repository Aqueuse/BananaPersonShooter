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
        private MapSavedData _mapSavedData;

        public void ResetGameData() {
            ResetInventory();
            ResetBananaSlot();
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
                ObjectsReference.Instance.gameData.bananaManSaved.bananaInventory[bananaSlot.Key.ToString()] = 0;
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
            ObjectsReference.Instance.gameData.lastPositionOnMap = ObjectsReference.Instance.scenesSwitch.spawnPointsBySpawnType[SpawnPoint.NEW_GAME].position;
            ObjectsReference.Instance.gameData.bananaManSaved.lastMap = SceneType.COROLLE;
            ObjectsReference.Instance.bananaMan.transform.position = ObjectsReference.Instance.gameData.lastPositionOnMap;
        }

        private static void ResetTutorial() {
            ObjectsReference.Instance.bananaMan.tutorialFinished = false;
            
            ObjectsReference.Instance.gameData.bananaManSaved.hasFinishedTutorial = false;
        }

        private static void ResetActiveItem() {
            ObjectsReference.Instance.bananaMan.activeItem = ObjectsReference.Instance.scriptableObjectManager.GetBananaScriptableObject(BananaType.EMPTY);

            ObjectsReference.Instance.gameData.bananaManSaved.activeBanana = 0;
        }

        private static void ResetMonkeysSasiety() {
            foreach (var mapData in ObjectsReference.Instance.gameData.mapBySceneName) {
                if (mapData.Value.mapPropertiesScriptableObject.monkeyPropertiesScriptableObjectsByMonkeyId.Count > 0) {
                    foreach (var monkeyDataScriptableObject in mapData.Value.mapPropertiesScriptableObject.monkeyPropertiesScriptableObjectsByMonkeyId) {
                        monkeyDataScriptableObject.Value.sasiety = 0;
                    }
                }
            }
        }
    }
}