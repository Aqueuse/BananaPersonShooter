using System;
using System.Globalization;
using Game;
using Player;
using UI.InGame.Inventory;
using UI.InGame.QuickSlots;
using UI.Save;

namespace Save {
    public class GameSave : MonoSingleton<GameSave> {
        public void NewSave(string saveUuid) {
            if (saveUuid.Length == 0) saveUuid = DateTime.Now.ToString("yyyyMMddHHmmss");

            SaveGameData(saveUuid);
            UISave.Instance.CreateNewSave(saveUuid);
        }

        public void SaveGameData(string saveUuid) {
            var date = DateTime.ParseExact(DateTime.Now.ToString("U"), "U", CultureInfo.CurrentCulture).ToString(CultureInfo.CurrentCulture);
            
            SaveInventory();
            SaveSlots();
            SaveBananaManVitals();
            SavePosition();
            SaveActiveItem();
            SaveMonkeysSatiety();

            SaveDebrisPositionAndRotationByUuid(saveUuid);

            SaveData.Instance.Save(saveUuid, date);
        }

        private void SaveInventory() {
            foreach (var inventorySlot in Inventory.Instance.bananaManInventory) {
                GameData.Instance.BananaManSavedData.inventory[inventorySlot.Key.ToString()] = inventorySlot.Value;
            }
        }

        private void SaveSlots() {
            foreach (var slot in UISlotsManager.Instance.slotsMappingToInventory) {
                GameData.Instance.BananaManSavedData.slots["inventorySlot" + slot.Key] = slot.Value;
            }
        }

        private void SaveBananaManVitals() {
            GameData.Instance.BananaManSavedData.health = BananaMan.Instance.health; 
            GameData.Instance.BananaManSavedData.resistance = BananaMan.Instance.resistance; 
        }

        private void SavePosition() {
            GameData.Instance.lastPositionOnMap = BananaMan.Instance.transform.position;
            GameData.Instance.BananaManSavedData.xWorldPosition = GameData.Instance.lastPositionOnMap.x;
            GameData.Instance.BananaManSavedData.yWorldPosition = GameData.Instance.lastPositionOnMap.y;
            GameData.Instance.BananaManSavedData.zworldPosition = GameData.Instance.lastPositionOnMap.z;
        }

        private void SaveActiveItem() {
            UInventory.Instance.ActivateAllInventory();

            GameData.Instance.BananaManSavedData.active_item =
                UInventory.Instance.GetSlotIndex(BananaMan.Instance.activeItemThrowableType); 
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
                    SaveData.Instance.SaveMapDataByUuid(
                        map.Value.debrisPosition,
                        map.Value.debrisRotation,
                        map.Value.debrisIndex,
                        map.Key,
                        saveUuid
                    );
                }
            }
        }
    }
}
