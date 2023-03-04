using System.Linq;
using Enums;
using Game;
using Player;
using Save.Templates;
using UI.InGame.QuickSlots;
using UnityEngine;

namespace Save {
    public class GameReset : MonoSingleton<GameReset> {
        private Vector3[] debrisPosition;
        private Quaternion[] debrisRotation;
        private int[] debrisPrefabIndex;
        
        private GameObject debrisContainer;
        
        private BananaManSavedData bananaManSavedData;
        private MapSavedData mapSavedData;

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
            foreach (var bananaSlot in Inventory.Instance.bananaManInventory.ToList()) {
                Inventory.Instance.bananaManInventory[bananaSlot.Key] = 0;
                GameData.Instance.BananaManSavedData.inventory[bananaSlot.Key.ToString()] = 0;
            }
        }

        private void ResetSlots() {
            foreach (var slot in GameData.Instance.BananaManSavedData.slots.ToList()) {
                GameData.Instance.BananaManSavedData.slots["inventorySlot" + slot.Key] = 0;
            }

            foreach (UISlot uiSlot in UISlotsManager.Instance.uiSlotsScripts) {
                uiSlot.EmptySlot();
            }
        }

        private void ResetBananaManVitals() {
            BananaMan.Instance.health = 100;
            BananaMan.Instance.resistance = 100;

            GameData.Instance.BananaManSavedData.health = 100;
            GameData.Instance.BananaManSavedData.resistance = 100;
        }

        private void ResetPositionAndLastMap() {
            GameData.Instance.lastPositionOnMap = ScenesSwitch.Instance.teleportSpawnPointBySceneName["COMMANDROOM"].position;
            GameData.Instance.BananaManSavedData.last_map = "COMMANDROOM";
            BananaMan.Instance.transform.position = GameData.Instance.lastPositionOnMap;
        }

        private void ResetAdvancementType() {
            GameData.Instance.BananaManSavedData.advancementState = AdvancementState.NEW_GAME;
        }

        private void ResetActiveItem() {
            BananaMan.Instance.activeItemThrowableType = ItemThrowableType.EMPTY;
            BananaMan.Instance.activeItemThrowableCategory = ItemThrowableCategory.EMPTY;

            GameData.Instance.BananaManSavedData.active_item = 0;
        }

        private void ResetMonkeysSasiety() {
            foreach (var mapData in MapsManager.Instance.mapBySceneName) {
                mapData.Value.monkeySasiety = 50;
            }
        }
    }
}