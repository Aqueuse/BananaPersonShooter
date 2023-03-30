using System;
using System.Globalization;
using Enums;
using Game;
using Player;
using UI.InGame.QuickSlots;

namespace Save {
    public class GameSave : MonoSingleton<GameSave> {
        public void SaveGameData(string saveUuid) {
            var date = DateTime.ParseExact(DateTime.Now.ToString("U"), "U", CultureInfo.CurrentCulture).ToString(CultureInfo.CurrentCulture);
            
            SaveInventory();
            SaveSlots();
            SaveBananaManVitals();
            SavePositionAndRotation();
            SaveActiveItem();
            SaveMonkeysSatiety();
            
            SaveDebrisPositionAndRotationByUuid(saveUuid);
            SavePlateformsPositionAndTypeByUuid(saveUuid);

            SaveData.Instance.Save(saveUuid, date);
        }

        private void SaveInventory() {
            foreach (var inventorySlot in Inventory.Instance.bananaManInventory) {
                GameData.Instance.bananaManSavedData.inventory[inventorySlot.Key.ToString()] = inventorySlot.Value;
            }
        }

        private void SaveSlots() {
        GameData.Instance.bananaManSavedData.slots = new() {
                ItemThrowableType.EMPTY.ToString(), 
                ItemThrowableType.EMPTY.ToString(), 
                ItemThrowableType.EMPTY.ToString(), 
                ItemThrowableType.EMPTY.ToString() 
            };
            
            for (int i = 0; i < UISlotsManager.Instance.uiSlotsScripts.Count; i++) {
                GameData.Instance.bananaManSavedData.slots[i] =
                    UISlotsManager.Instance.uiSlotsScripts[i].itemThrowableType.ToString();
            }
        }

        private void SaveBananaManVitals() {
            GameData.Instance.bananaManSavedData.health = BananaMan.Instance.health; 
            GameData.Instance.bananaManSavedData.resistance = BananaMan.Instance.resistance; 
        }

        private void SavePositionAndRotation() {
            var bananaManTransform = BananaMan.Instance.transform;
            GameData.Instance.lastPositionOnMap = bananaManTransform.position;
            GameData.Instance.lastRotationOnMap = bananaManTransform.rotation.eulerAngles;
            
            GameData.Instance.bananaManSavedData.xWorldPosition = GameData.Instance.lastPositionOnMap.x;
            GameData.Instance.bananaManSavedData.yWorldPosition = GameData.Instance.lastPositionOnMap.y;
            GameData.Instance.bananaManSavedData.zworldPosition = GameData.Instance.lastPositionOnMap.z;

            GameData.Instance.bananaManSavedData.xWorldRotation = GameData.Instance.lastRotationOnMap.x;
            GameData.Instance.bananaManSavedData.yWorldRotation = GameData.Instance.lastRotationOnMap.y;
            GameData.Instance.bananaManSavedData.zWorldRotation = GameData.Instance.lastRotationOnMap.z;
        }

        private void SaveActiveItem() {
            GameData.Instance.bananaManSavedData.activeItem = BananaMan.Instance.activeItemThrowableType;
        }

        private void SaveMonkeysSatiety() {
            foreach (var map in MapsManager.Instance.mapBySceneName) {
                // add other monkeys and other maps
                GameData.Instance.mapSavedDatasByMapName[map.Key].monkey_sasiety = map.Value.monkeySasiety;
            }
        }

        /////////////////// DEBRIS ///////////////////////

        private void SaveDebrisPositionAndRotationByUuid(string saveUuid) {
            foreach (var map in MapsManager.Instance.mapBySceneName) {
                if (map.Value.hasDebris && map.Value.debrisIndex.Length != 0) {
                    SaveData.Instance.SaveMapDebrisDataByUuid(
                        map.Value.debrisPosition,
                        map.Value.debrisRotation,
                        map.Value.debrisIndex,
                        map.Key,
                        saveUuid
                    );
                }
            }
        }
        
        /////////////////// PLATEFORMS ///////////////////////

        private void SavePlateformsPositionAndTypeByUuid(string saveUuid) {
            foreach (var map in MapsManager.Instance.mapBySceneName) {
                SaveData.Instance.SaveMapPlateformsDataByUuid(map.Value.plateformsPosition, map.Value.plateformsTypes, map.Key, saveUuid);
            }
        }
    }
}
