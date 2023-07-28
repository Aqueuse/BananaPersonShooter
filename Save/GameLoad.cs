using System;
using System.Collections.Generic;
using System.Linq;
using Building;
using Enums;
using Items;
using UnityEngine;

namespace Save {
    public class GameLoad : MonoBehaviour {
        private GameObject aspirable;
        private GameObject prefab;

        private BuildablesManager buildablesManager;

        private void Start() {
            buildablesManager = ObjectsReference.Instance.buildablesManager;
        }

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

            ObjectsReference.Instance.gameData.bananaManSavedData = ObjectsReference.Instance.loadData.GetPlayerDataByUuid(saveUuid);

            LoadPositionAndRotationOnLastMap();
            LoadBananaManVitals();
            LoadActiveItem();
            LoadInventory();
            LoadBlueprints();
            LoadSlots();

            LoadMapsData();
            LoadMonkeysSatiety();

            ObjectsReference.Instance.loadData.LoadMapAspirablesDataByUuid(saveUuid);

            CheckTutorialFinished();

            ObjectsReference.Instance.gameSave.StartAutoSave();
        }

        private static void LoadInventory() {
            foreach (var bananaSlot in ObjectsReference.Instance.inventory.bananaManInventory.ToList()) {
                ObjectsReference.Instance.inventory.bananaManInventory[bananaSlot.Key] =
                    ObjectsReference.Instance.gameData.bananaManSavedData.inventory[bananaSlot.Key.ToString()];
            }
        }

        private void LoadBlueprints() {
            buildablesManager.playerBlueprints = new List<BuildableType>();

            var playerBlueprints = ObjectsReference.Instance.gameData.bananaManSavedData.blueprints;
            
            foreach (var blueprint in playerBlueprints) {
                buildablesManager.playerBlueprints.Add(Enum.Parse<BuildableType>(blueprint));
                ObjectsReference.Instance.uiBlueprints.SetVisible(Enum.Parse<BuildableType>(blueprint));
            }

            var buildablesAvailable = ObjectsReference.Instance.scriptableObjectManager._meshReferenceScriptableObject.buildablesDataScriptableObject;

            foreach (var buildableDataScriptableObject in buildablesAvailable) {
                if (!buildablesManager.playerBlueprints.Contains(buildableDataScriptableObject.Value.buildableType)) {
                    buildablesManager.buildablesToGive.Add(buildableDataScriptableObject.Value.buildableType);
                }
            }
        }

        private static void LoadSlots() {
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
            
            ObjectsReference.Instance.uiSlotsManager.SetActiveSlot();
        }

        private static void LoadBananaManVitals() {
            ObjectsReference.Instance.bananaMan.health = ObjectsReference.Instance.gameData.bananaManSavedData.health;
            ObjectsReference.Instance.bananaMan.resistance = ObjectsReference.Instance.gameData.bananaManSavedData.resistance;
            
            ObjectsReference.Instance.bananaMan.SetBananaSkinHealth();
        }

        private static void LoadPositionAndRotationOnLastMap() {
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
        
        private static void LoadActiveItem() {
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

        private static void LoadMonkeysSatiety() {
            foreach (var mapData in ObjectsReference.Instance.mapsManager.mapBySceneName) {
                mapData.Value.monkeySasiety = ObjectsReference.Instance.gameData.mapSavedDatasByMapName[mapData.Key].monkeySasiety;
            }
        }

        private static void CheckTutorialFinished() {
            ObjectsReference.Instance.bananaMan.tutorialFinished = ObjectsReference.Instance.gameData.bananaManSavedData.hasFinishedTutorial;
            ObjectsReference.Instance.bananaMan.hasRepairedBananaGun = ObjectsReference.Instance.gameData.bananaManSavedData.hasRepairedBananaGun;

            if (ObjectsReference.Instance.bananaMan.hasRepairedBananaGun) {
                ObjectsReference.Instance.bananaGun.bananaGunInBack.SetActive(true);
                ObjectsReference.Instance.uiManager.Set_active(UICanvasGroupType.HUD, true);
            }

            else {
                ObjectsReference.Instance.bananaGun.bananaGunInBack.SetActive(false);
                ObjectsReference.Instance.uiManager.Set_active(UICanvasGroupType.HUD, false);
                ObjectsReference.Instance.uiCrosshairs.SetCrosshair(ItemCategory.EMPTY, ItemType.EMPTY);
            }
        }
        
        private static void LoadMapsData() {
            foreach (var map in ObjectsReference.Instance.mapsManager.mapBySceneName) {
                var savedMapData = ObjectsReference.Instance.loadData.GetMapDataByUuid(ObjectsReference.Instance.gameData.currentSaveUuid, map.Key);
                
                ObjectsReference.Instance.gameData.mapSavedDatasByMapName[map.Key] = savedMapData;
                
                ObjectsReference.Instance.mapsManager.mapBySceneName[map.Key].isDiscovered = savedMapData.isDiscovered;
            }
        }

        public void RespawnAspirablesOnMap() {
            var mapData = ObjectsReference.Instance.mapsManager.currentMap;

            if (mapData.isDiscovered) {
                DestroyImmediate(MapItems.Instance.aspirablesContainer);

                MapItems.Instance.aspirablesContainer = new GameObject("aspirables") {
                    transform = {
                        parent = MapItems.Instance.transform
                    }
                };

                for (var i = 0; i < mapData.aspirablesCategories.Count; i++) {
                    if (mapData.aspirablesCategories[i] == ItemCategory.DEBRIS) {
                        aspirable = Instantiate(
                            ObjectsReference.Instance.scriptableObjectManager._meshReferenceScriptableObject.debrisPrefab[mapData.aspirablesPrefabsIndex[i]], 
                            MapItems.Instance.aspirablesContainer.transform, 
                            true
                        );

                        aspirable.transform.position = mapData.aspirablesPositions[i];
                        aspirable.transform.rotation = mapData.aspirablesRotations[i];
                    }
                    
                    if (mapData.aspirablesCategories[i] == ItemCategory.BUILDABLE) {
                        prefab = ObjectsReference.Instance.buildablesManager.BuildablePrefabByBuildableType(mapData.aspirablesBuildableTypes[i]);

                        aspirable = Instantiate(prefab, MapItems.Instance.aspirablesContainer.transform, true);
                    
                        aspirable.transform.position = mapData.aspirablesPositions[i];
                        aspirable.transform.rotation = mapData.aspirablesRotations[i];
                    }

                    if (mapData.aspirablesCategories[i] == ItemCategory.RUINE) {
                        prefab = ObjectsReference.Instance.scriptableObjectManager._meshReferenceScriptableObject.ruinesPrefab[mapData.aspirablesPrefabsIndex[i]];

                        aspirable = Instantiate(prefab, MapItems.Instance.aspirablesContainer.transform, true);
                    
                        aspirable.transform.position = mapData.aspirablesPositions[i];
                        aspirable.transform.rotation = mapData.aspirablesRotations[i];
                    }

                    if (mapData.aspirablesCategories[i] == ItemCategory.CHIMPLOYEE) {
                        prefab = ObjectsReference.Instance.scriptableObjectManager._meshReferenceScriptableObject.chimployeePrefab;

                        aspirable = Instantiate(prefab, MapItems.Instance.aspirablesContainer.transform, true);

                        aspirable.transform.position = mapData.aspirablesPositions[i];
                        aspirable.transform.rotation = mapData.aspirablesRotations[i];
                    }
                }
            }
            
            else {
                if (mapData.initialAspirablesOnMap != null) {
                    aspirable = Instantiate(
                        mapData.initialAspirablesOnMap, 
                        MapItems.Instance.aspirablesContainer.transform,
                        true
                    );

                    aspirable.transform.position = MapItems.Instance.aspirablesContainer.transform.position;
                    aspirable.transform.rotation = MapItems.Instance.aspirablesContainer.transform.rotation;
                }
            }

            if (mapData.debrisToSpawn > 0) {
                if (ObjectsReference.Instance.gameSettings.areDebrisFallingOnTheTrees) MapItems.Instance.debrisSpawner.SpawnNewDebrisOnRaycastHit(MapItems.Instance.aspirablesContainer.transform);

                else {
                    MapItems.Instance.debrisSpawner.SpawnNewDebripOnNavMesh(MapItems.Instance.aspirablesContainer.transform);
                }
            }
        }
    }
}
