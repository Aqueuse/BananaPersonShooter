using System;
using System.Globalization;
using System.IO;
using System.Linq;
using Data;
using Enums;
using Player;
using UI.InGame;
using UI.InGame.Inventory;
using UI.InGame.QuickSlots;
using UI.Save;
using UnityEngine;

namespace Save {
    public class GameLoad : MonoSingleton<GameLoad> {
        public void LoadGameData(string saveUuid) {
            var date = DateTime.ParseExact(DateTime.Now.ToString("U"), "U", CultureInfo.CurrentCulture).ToString(CultureInfo.CurrentCulture);

            if (saveUuid == "auto_save" && !Directory.Exists(LoadData.Instance.GetSavePathByUuid("auto_save"))) {
                SaveData.Instance.Save(saveUuid, date);
                UISave.Instance.AppendSaveSlot(saveUuid);
            }

            GameData.Instance.bananaManSavedData = LoadData.Instance.GetPlayerDataByUuid(saveUuid);
            GameData.Instance.map01SavedData = LoadData.Instance.GetMap01DataByUuid(saveUuid);

            LoadInventory();
            LoadSlots();
            LoadBananaManVitals();
            LoadPositionAndLastMap();
            LoadActiveItem();
            LoadMonkeysSatiety();

            LoadData.Instance.LoadMapDataByUuid(saveUuid);
        }

        private void LoadInventory() {
            foreach (var bananaSlot in Inventory.Instance.bananaManInventory.ToList()) {
                Inventory.Instance.bananaManInventory[bananaSlot.Key] =
                    GameData.Instance.bananaManSavedData.inventory[bananaSlot.Key.ToString()];
            }
        }

        private void LoadSlots() {
            UInventory.Instance.ActivateAllInventory();  // activate temporally all the inventory to find the index of slots

            foreach (var slot in UISlotsManager.Instance.slotsMappingToInventory.ToList()) {
                var itemType = UInventory.Instance.GetItemThrowableTypeByIndex(GameData.Instance.bananaManSavedData.slots["inventorySlot"+slot.Key]);
                var itemCategory = UInventory.Instance.GetItemThrowableCategoryByIndex(GameData.Instance.bananaManSavedData.slots["inventorySlot"+slot.Key]);

                if (Inventory.Instance.bananaManInventory[itemType] > 0) {
                    UISlotsManager.Instance.slotsMappingToInventory[slot.Key] = GameData.Instance.bananaManSavedData.slots["inventorySlot"+slot.Key];
                    UISlotsManager.Instance.uiSlotsScripts[slot.Key].SetSlot(itemType, itemCategory);
                    UISlotsManager.Instance.uiSlotsScripts[slot.Key].SetSprite(UInventory.Instance.GetItemSprite(itemType));
                }
            }

            UInventory.Instance.ActivateAllInventory();  // activate temporally all the inventory to find the index of slots
        }

        private void LoadBananaManVitals() {
            BananaMan.Instance.health = GameData.Instance.bananaManSavedData.health;
            BananaMan.Instance.resistance = GameData.Instance.bananaManSavedData.resistance;

            UIVitals.Instance.Set_Health(BananaMan.Instance.health);
            UIVitals.Instance.Set_Resistance(BananaMan.Instance.resistance);
        }

        private void LoadPositionAndLastMap() {
             GameData.Instance.lastPositionOnMap = new Vector3(
                GameData.Instance.bananaManSavedData.xWorldPosition,
                GameData.Instance.bananaManSavedData.yWorldPosition,
                GameData.Instance.bananaManSavedData.zworldPosition);

             BananaMan.Instance.transform.position = GameData.Instance.lastPositionOnMap;
        }
        
        private void LoadActiveItem() {
            var activeItemType = UInventory.Instance.GetItemThrowableTypeByIndex(GameData.Instance.bananaManSavedData.active_item);
            var activeItemCategory = UInventory.Instance.GetItemThrowableCategoryByIndex(GameData.Instance.bananaManSavedData.active_item);

            if (activeItemCategory == ItemThrowableCategory.BANANA) {
                BananaMan.Instance.activeItem = ScriptableObjectManager.Instance.GetBananaScriptableObject(activeItemType);
            }

            BananaMan.Instance.activeItemThrowableType = activeItemType;
            BananaMan.Instance.activeItemThrowableCategory = activeItemCategory;
        }

        private void LoadMonkeysSatiety() {
            foreach (var mapData in GameData.Instance.mapDatasBySceneNames) {
                // TODO : add other monkeys and other maps
                mapData.Value.monkeySasiety = GameData.Instance.map01SavedData.monkey_sasiety;
            }
        }

        /////////////////// DEBRIS ///////////////////////

        public void RespawnDebrisOnMap(string sceneName) {
            if (GameObject.FindGameObjectWithTag("debrisContainer") != null) {
                GameData.Instance.debrisContainer = GameObject.FindGameObjectWithTag("debrisContainer");

                Destroy(GameData.Instance.debrisContainer);

                GameData.Instance.debrisContainer = new GameObject("debris");
                GameData.Instance.debrisContainer.transform.parent = null;
                GameData.Instance.debrisContainer.tag = "debrisContainer";

                var mapData = GameData.Instance.mapDatasBySceneNames[sceneName];

                for (var i=0; i<GameData.Instance.mapDatasBySceneNames[sceneName].debrisIndex.Length; i++) {
                    var debris = Instantiate(GameData.Instance.debrisPrefab[mapData.debrisIndex[i]], GameData.Instance.debrisContainer.transform, true);
                    debris.transform.position = mapData.debrisPosition[i];
                    debris.transform.rotation = mapData.debrisRotation[i];
                }
            }
        }
    }
}
