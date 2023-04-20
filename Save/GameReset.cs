using System.Linq;
using Enums;
using Save.Templates;
using UI.InGame.QuickSlots;
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
            ResetSlots();
            ResetBananaManVitals();
            ResetAdvancementType();
            
            ResetPositionAndLastMap();
            ResetActiveItem();
            ResetMonkeysSasiety();

            // lock maps
            // reinit spaceship state
            // reinit assets positions on maps
        }

        private void ResetInventory() {
            foreach (var bananaSlot in ObjectsReference.Instance.inventory.bananaManInventory.ToList()) {
                ObjectsReference.Instance.inventory.bananaManInventory[bananaSlot.Key] = 0;
                ObjectsReference.Instance.gameData.bananaManSavedData.inventory[bananaSlot.Key.ToString()] = 0;
            }
        }

        private void ResetSlots() {
            for (var i = 0; i < ObjectsReference.Instance.gameData.bananaManSavedData.slots.Count; i++) {
                ObjectsReference.Instance.gameData.bananaManSavedData.slots[i] = ItemType.EMPTY.ToString();
            }
            
            foreach (UISlot uiSlot in ObjectsReference.Instance.uiSlotsManager.uiSlotsScripts) {
                uiSlot.EmptySlot();
            }
        }

        private void ResetBananaManVitals() {
            ObjectsReference.Instance.bananaMan.health = 100;
            ObjectsReference.Instance.bananaMan.resistance = 100;

            ObjectsReference.Instance.gameData.bananaManSavedData.health = 100;
            ObjectsReference.Instance.gameData.bananaManSavedData.resistance = 100;
        }

        private void ResetPositionAndLastMap() {
            ObjectsReference.Instance.gameData.lastPositionOnMap = ObjectsReference.Instance.scenesSwitch.spawnPointsBySpawnType[SpawnPoint.COMMAND_ROOM_TELEPORTATION].position;
            ObjectsReference.Instance.gameData.bananaManSavedData.lastMap = "COMMANDROOM";
            ObjectsReference.Instance.bananaMan.transform.position = ObjectsReference.Instance.gameData.lastPositionOnMap;
        }

        private void ResetAdvancementType() {
            ObjectsReference.Instance.gameData.bananaManSavedData.playerAdvancements.Clear();
        }

        private void ResetActiveItem() {
            ObjectsReference.Instance.bananaMan.activeItemType = ItemType.EMPTY;
            ObjectsReference.Instance.bananaMan.activeItemCategory = ItemCategory.EMPTY;

            ObjectsReference.Instance.gameData.bananaManSavedData.activeItem = 0;
        }

        private void ResetMonkeysSasiety() {
            foreach (var mapData in ObjectsReference.Instance.mapsManager.mapBySceneName) {
                mapData.Value.monkeySasiety = 50;
            }
        }
    }
}