using System;
using System.Globalization;
using System.IO;
using System.Linq;
using Building;
using Building.Buildables;
using Enums;
using UnityEngine;

namespace Save {
    public class GameLoad : MonoBehaviour {
        public void LoadLastSave() {
            ObjectsReference.Instance.gameManager.loadingScreen.SetActive(true);
            ObjectsReference.Instance.uiManager.Set_active(UICanvasGroupType.DEATH, false);
            ObjectsReference.Instance.death.HideDeath();
            
            if (ObjectsReference.Instance.gameData.currentSaveUuid == null) ObjectsReference.Instance.scenesSwitch.ReturnHome();
            else {
                ObjectsReference.Instance.gameManager.Play(ObjectsReference.Instance.gameData.currentSaveUuid, false);
            }
        }
        
        public void LoadGameData(string saveUuid) {
            ObjectsReference.Instance.gameData.currentSaveUuid = saveUuid;
            
            var date = DateTime.ParseExact(DateTime.Now.ToString("U"), "U", CultureInfo.CurrentCulture).ToString(CultureInfo.CurrentCulture);

            if (saveUuid == "auto_save" && !Directory.Exists(ObjectsReference.Instance.loadData.GetSavePathByUuid("auto_save"))) {
                ObjectsReference.Instance.saveData.Save(saveUuid, date);
                ObjectsReference.Instance.uiSave.AppendSaveSlot(saveUuid);
            }

            ObjectsReference.Instance.gameData.bananaManSavedData = ObjectsReference.Instance.loadData.GetPlayerDataByUuid(saveUuid);
            
            LoadInventory();
            LoadBlueprints();
            LoadSlots();
            LoadBananaManVitals();
            LoadPositionAndRotationOnLastMap();
            LoadActiveItem();

            LoadMapsData();
            LoadMonkeysSatiety();

            ObjectsReference.Instance.loadData.LoadMapDebrisDataByUuid(saveUuid);
            ObjectsReference.Instance.loadData.LoadMapPlateformsDataByUuid(saveUuid);
        }

        private void LoadInventory() {
            foreach (var bananaSlot in ObjectsReference.Instance.inventory.bananaManInventory.ToList()) {
                ObjectsReference.Instance.inventory.bananaManInventory[bananaSlot.Key] =
                    ObjectsReference.Instance.gameData.bananaManSavedData.inventory[bananaSlot.Key.ToString()];
            }
        }
        
        private void LoadBlueprints() {
            foreach (var blueprint in ObjectsReference.Instance.gameData.bananaManSavedData.blueprints) {
                ObjectsReference.Instance.uiBlueprints.SetVisible(Enum.Parse<BuildableType>(blueprint));
            }
        }

        private void LoadSlots() {
            for (var i = 0; i < ObjectsReference.Instance.uiSlotsManager.uiSlotsScripts.Count; i++) {
                var itemCategoryAndType = ObjectsReference.Instance.gameData.bananaManSavedData.slots[i].Split(",");
                var itemCategoryString = itemCategoryAndType[0];
                var itemTypeString = itemCategoryAndType[1];
                
                var itemCategory = (ItemCategory)Enum.Parse(typeof(ItemCategory), itemCategoryString); 
                
                if (itemCategory == ItemCategory.BUILDABLE) {
                    var itemType = (BuildableType)Enum.Parse(typeof(BuildableType), itemTypeString);
                    ObjectsReference.Instance.uiSlotsManager.uiSlotsScripts[i].SetSlot(itemCategory, slotBuildableType:itemType);
                }
                else {
                    var itemType = (ItemType)Enum.Parse(typeof(ItemType), itemTypeString);
                    ObjectsReference.Instance.uiSlotsManager.uiSlotsScripts[i].SetSlot(itemCategory, slotItemType:itemType);
                }
            }
        }

        private void LoadBananaManVitals() {
            ObjectsReference.Instance.bananaMan.health = ObjectsReference.Instance.gameData.bananaManSavedData.health;
            ObjectsReference.Instance.bananaMan.resistance = ObjectsReference.Instance.gameData.bananaManSavedData.resistance;
            
            ObjectsReference.Instance.bananaMan.SetBananaSkinHealth();
        }

        private void LoadPositionAndRotationOnLastMap() {
            ObjectsReference.Instance.gameData.lastPositionOnMap = new Vector3(
                ObjectsReference.Instance.gameData.bananaManSavedData.xWorldPosition,
                ObjectsReference.Instance.gameData.bananaManSavedData.yWorldPosition,
                ObjectsReference.Instance.gameData.bananaManSavedData.zworldPosition);

            ObjectsReference.Instance.gameData.lastRotationOnMap = new Vector3(
                ObjectsReference.Instance.gameData.bananaManSavedData.xWorldRotation,
                ObjectsReference.Instance.gameData.bananaManSavedData.yWorldRotation,
                ObjectsReference.Instance.gameData.bananaManSavedData.zWorldRotation
             );
        }
        
        private void LoadActiveItem() {
            var activeItemType = ObjectsReference.Instance.gameData.bananaManSavedData.activeItem;
            var activeItemCategory = ObjectsReference.Instance.gameData.bananaManSavedData.activeItemCategory;
            var activeBuildableType = ObjectsReference.Instance.gameData.bananaManSavedData.activeBuildableType;

            if (activeItemCategory == ItemCategory.BANANA) {
                ObjectsReference.Instance.bananaMan.activeItem = ObjectsReference.Instance.scriptableObjectManager.GetBananaScriptableObject(activeItemType);
            }

            ObjectsReference.Instance.bananaMan.activeItemType = activeItemType;
            ObjectsReference.Instance.bananaMan.activeItemCategory = activeItemCategory;
            ObjectsReference.Instance.bananaMan.activeBuildableType = activeBuildableType;
        }

        private void LoadMonkeysSatiety() {
            foreach (var mapData in ObjectsReference.Instance.mapsManager.mapBySceneName) {
                // add other monkeys and other maps
                mapData.Value.monkeySasiety = ObjectsReference.Instance.gameData.mapSavedDatasByMapName[mapData.Key].monkeySasiety;
            }
        }

        /////////////////// MAPS ///////////////////////

        private void LoadMapsData() {
            foreach (var map in ObjectsReference.Instance.mapsManager.mapBySceneName) {
                var savedMapData = ObjectsReference.Instance.loadData.GetMapDataByUuid(ObjectsReference.Instance.gameData.currentSaveUuid, map.Key);
                
                ObjectsReference.Instance.gameData.mapSavedDatasByMapName[map.Key] = savedMapData;
                
                ObjectsReference.Instance.mapsManager.mapBySceneName[map.Key].isDiscovered = savedMapData.isDiscovered;
            }
        }

        public void RespawnDebrisOnMap() {
            var mapData = ObjectsReference.Instance.mapsManager.currentMap;

            Destroy(MapItems.Instance.debrisContainer);

            MapItems.Instance.debrisContainer = new GameObject("debris") {
                transform = {
                    parent = MapItems.Instance.transform
                }
            };

            for (var i=0; i<mapData.debrisIndex.Length; i++) {
                var debris = Instantiate(ObjectsReference.Instance.gameData.debrisPrefab[mapData.debrisIndex[i]], MapItems.Instance.debrisContainer.transform, true);
                debris.transform.position = mapData.debrisPosition[i];
                debris.transform.rotation = mapData.debrisRotation[i];
            }
        }

        public void RespawnPlateformsOnMap() {
            var mapData = ObjectsReference.Instance.mapsManager.currentMap;
            
            for (var i = 0; i < mapData.plateformsPosition.Count; i++) {
                var plateforme = Instantiate(ObjectsReference.Instance.gameData.plateformPrefab, MapItems.Instance.plateformsContainer.transform, true);
                plateforme.transform.position = mapData.plateformsPosition[i];

                var plateformClass = plateforme.GetComponent<Buildable>();
                var plateformType = mapData.plateformsTypes[i];
                
                plateformClass.RespawnBuildable(plateformType);
            }
        }
    }
}
