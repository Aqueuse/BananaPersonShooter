using System.Linq;
using Enums;
using Game;
using Player;
using Save.Templates;
using UI.InGame.QuickSlots;
using UnityEngine;

namespace Save {
    public class GameReset : MonoSingleton<GameReset> {
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
            foreach (var bananaSlot in Inventory.Instance.bananaManInventory.ToList()) {
                Inventory.Instance.bananaManInventory[bananaSlot.Key] = 0;
                GameData.Instance.bananaManSavedData.inventory[bananaSlot.Key.ToString()] = 0;
            }
        }

        private void ResetSlots() {
            for (var i = 0; i < GameData.Instance.bananaManSavedData.slots.Count; i++) {
                GameData.Instance.bananaManSavedData.slots[i] = ItemThrowableType.EMPTY.ToString();
            }
            
            foreach (UISlot uiSlot in UISlotsManager.Instance.uiSlotsScripts) {
                uiSlot.EmptySlot();
            }
        }

        private void ResetBananaManVitals() {
            BananaMan.Instance.health = 100;
            BananaMan.Instance.resistance = 100;

            GameData.Instance.bananaManSavedData.health = 100;
            GameData.Instance.bananaManSavedData.resistance = 100;
        }

        private void ResetPositionAndLastMap() {
            GameData.Instance.lastPositionOnMap = ScenesSwitch.Instance.teleportSpawnPointBySceneName["COMMANDROOM"].position;
            GameData.Instance.bananaManSavedData.lastMap = "COMMANDROOM";
            BananaMan.Instance.transform.position = GameData.Instance.lastPositionOnMap;
        }

        private void ResetAdvancementType() {
            GameData.Instance.bananaManSavedData.advancementState = AdvancementState.NEW_GAME;
        }

        private void ResetActiveItem() {
            BananaMan.Instance.activeItemThrowableType = ItemThrowableType.EMPTY;
            BananaMan.Instance.activeItemThrowableCategory = ItemThrowableCategory.EMPTY;

            GameData.Instance.bananaManSavedData.activeItem = 0;
        }

        private void ResetMonkeysSasiety() {
            foreach (var mapData in MapsManager.Instance.mapBySceneName) {
                mapData.Value.monkeySasiety = 50;
            }
        }
    }
}