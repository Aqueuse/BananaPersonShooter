using System;
using System.Globalization;
using System.IO;
using System.Linq;
using Building;
using Building.Plateforms;
using Data;
using Enums;
using Game;
using Items;
using Player;
using UI;
using UI.InGame.QuickSlots;
using UI.Save;
using UnityEngine;

namespace Save {
    public class GameLoad : MonoSingleton<GameLoad> {
        public void LoadLastSave() {
            GameManager.Instance.loadingScreen.SetActive(true);
            UIManager.Instance.Set_active(UICanvasGroupType.DEATH, false);
            Death.Instance.HideDeath();
            
            if (GameData.Instance.currentSaveUuid == null) ScenesSwitch.Instance.ReturnHome();
            else {
                
                GameManager.Instance.Play(GameData.Instance.currentSaveUuid, false);
            }
        }
        
        public void LoadGameData(string saveUuid) {
            GameData.Instance.currentSaveUuid = saveUuid;
            
            var date = DateTime.ParseExact(DateTime.Now.ToString("U"), "U", CultureInfo.CurrentCulture).ToString(CultureInfo.CurrentCulture);

            if (saveUuid == "auto_save" && !Directory.Exists(LoadData.Instance.GetSavePathByUuid("auto_save"))) {
                SaveData.Instance.Save(saveUuid, date);
                UISave.Instance.AppendSaveSlot(saveUuid);
            }

            GameData.Instance.bananaManSavedData = LoadData.Instance.GetPlayerDataByUuid(saveUuid);
            
            LoadInventory();
            LoadSlots();
            LoadBananaManVitals();
            LoadPositionAndRotationOnLastMap();
            LoadActiveItem();
            LoadMonkeysSatiety();

            LoadMapsData();
            
            LoadData.Instance.LoadMapDebrisDataByUuid(saveUuid);
            LoadData.Instance.LoadMapPlateformsDataByUuid(saveUuid);
        }

        private void LoadInventory() {
            foreach (var bananaSlot in Inventory.Instance.bananaManInventory.ToList()) {
                Inventory.Instance.bananaManInventory[bananaSlot.Key] =
                    GameData.Instance.bananaManSavedData.inventory[bananaSlot.Key.ToString()];
            }
        }

        private void LoadSlots() {
            for (var i = 0; i < UISlotsManager.Instance.uiSlotsScripts.Count; i++) {
                var itemType = (ItemThrowableType)Enum.Parse(typeof(ItemThrowableType), GameData.Instance.bananaManSavedData.slots[i]);
                
                UISlotsManager.Instance.uiSlotsScripts[i].SetSlot(itemType);
                UISlotsManager.Instance.uiSlotsScripts[i].SetAmmoQuantity(Inventory.Instance.GetQuantity(itemType));
                UISlotsManager.Instance.uiSlotsScripts[i].SetSprite(ItemsManager.Instance.itemsDataScriptableObject.itemSpriteByItemType[itemType]);
            }
        }

        private void LoadBananaManVitals() {
            BananaMan.Instance.health = GameData.Instance.bananaManSavedData.health;
            BananaMan.Instance.resistance = GameData.Instance.bananaManSavedData.resistance;
        }

        private void LoadPositionAndRotationOnLastMap() {
             GameData.Instance.lastPositionOnMap = new Vector3(
                GameData.Instance.bananaManSavedData.xWorldPosition,
                GameData.Instance.bananaManSavedData.yWorldPosition,
                GameData.Instance.bananaManSavedData.zworldPosition);

             GameData.Instance.lastRotationOnMap = new Vector3(
                 GameData.Instance.bananaManSavedData.xWorldRotation,
                 GameData.Instance.bananaManSavedData.yWorldRotation,
                 GameData.Instance.bananaManSavedData.zWorldRotation
             );
        }
        
        private void LoadActiveItem() {
            var activeItemType = GameData.Instance.bananaManSavedData.activeItem;
            var activeItemCategory = ItemsManager.Instance.itemsDataScriptableObject.itemsThrowableCategoriesByType[activeItemType];

            if (activeItemCategory == ItemThrowableCategory.BANANA) {
                BananaMan.Instance.activeItem = ScriptableObjectManager.Instance.GetBananaScriptableObject(activeItemType);
            }

            BananaMan.Instance.activeItemThrowableType = activeItemType;
            BananaMan.Instance.activeItemThrowableCategory = activeItemCategory;
        }

        private void LoadMonkeysSatiety() {
            foreach (var mapData in MapsManager.Instance.mapBySceneName) {
                // add other monkeys and other maps
                mapData.Value.monkeySasiety = GameData.Instance.mapSavedDatasByMapName[mapData.Key].monkey_sasiety;
            }
        }

        /////////////////// MAPS ///////////////////////

        private void LoadMapsData() {
            foreach (var map in MapsManager.Instance.mapBySceneName) {
                var savedMapData = LoadData.Instance.GetMapDataByUuid(GameData.Instance.currentSaveUuid, map.Key);
                
                GameData.Instance.mapSavedDatasByMapName[map.Key] = savedMapData;
                
                MapsManager.Instance.mapBySceneName[map.Key].isDiscovered = savedMapData.isDiscovered;
            }
        }

        public void RespawnDebrisOnMap() {
            var mapData = MapsManager.Instance.currentMap;

            Destroy(MapItems.Instance.debrisContainer);

            MapItems.Instance.debrisContainer = new GameObject("debris") {
                transform = {
                    parent = MapItems.Instance.transform
                }
            };

            for (var i=0; i<mapData.debrisIndex.Length; i++) {
                var debris = Instantiate(GameData.Instance.debrisPrefab[mapData.debrisIndex[i]], MapItems.Instance.debrisContainer.transform, true);
                debris.transform.position = mapData.debrisPosition[i];
                debris.transform.rotation = mapData.debrisRotation[i];
            }
        }

        public void RespawnPlateformsOnMap() {
            var mapData = MapsManager.Instance.currentMap;

            for (var i = 0; i < mapData.plateformsPosition.Count; i++) {
                var plateforme = Instantiate(GameData.Instance.plateformPrefab, MapItems.Instance.plateformsContainer.transform, true);
                plateforme.transform.position = mapData.plateformsPosition[i];

                var plateformClass = plateforme.GetComponent<Plateform>();
                var plateformType = mapData.plateformsTypes[i];
                
                plateformClass.RespawnPlateform(plateformType);
            }
        }
    }
}
