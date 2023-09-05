using System;
using System.Linq;
using Building;
using Building.Buildables.Plateforms;
using Data;
using Enums;
using UnityEngine;

namespace Save {
    public class GameLoad : MonoBehaviour {
        private GameObject aspirable;
        private GameObject prefab;
        
        private ItemScriptableObject itemScriptableObject;
        
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
            
            LoadBananasInventory();
            LoadRawMaterialsInventory();
            LoadIngredientsInventory();
            LoadBuildablesInventory();
            
            LoadSlots();

            LoadMapsData();
            LoadMonkeysSatiety();

            ObjectsReference.Instance.loadData.LoadMapAspirablesDataByUuid(saveUuid);

            CheckTutorialFinished();

            ObjectsReference.Instance.gameSave.StartAutoSave();
        }

        private static void LoadBananasInventory() {
            foreach (var bananaSlot in ObjectsReference.Instance.bananasInventory.bananasInventory.ToList()) {
                if (bananaSlot.Key == BananaType.EMPTY) continue;
                
                var bananaQuantity = ObjectsReference.Instance.gameData.bananaManSavedData.bananaInventory[bananaSlot.Key.ToString()];

                ObjectsReference.Instance.bananasInventory.bananasInventory[bananaSlot.Key] = bananaQuantity;

                var inventorySlot = ObjectsReference.Instance.uiBananasInventory.inventorySlotsByBananaType[bananaSlot.Key]; 

                if (bananaQuantity > 0) {
                    inventorySlot.gameObject.SetActive(true);
                    inventorySlot.SetQuantity(bananaQuantity);
                }
                else {
                    inventorySlot.gameObject.SetActive(false);
                }
            }
        }

        private static void LoadRawMaterialsInventory() {
            foreach (var rawMaterialSlot in ObjectsReference.Instance.rawMaterialsInventory.rawMaterialsInventory.ToList()) {
                if (rawMaterialSlot.Key == RawMaterialType.EMPTY) continue;
                
                var rawMaterialQuantity = ObjectsReference.Instance.gameData.bananaManSavedData.rawMaterialsInventory[rawMaterialSlot.Key.ToString()];

                ObjectsReference.Instance.rawMaterialsInventory.rawMaterialsInventory[rawMaterialSlot.Key] = rawMaterialQuantity;

                var inventorySlot = ObjectsReference.Instance.uiRawMaterialsInventory.inventorySlotsByRawMaterialType[rawMaterialSlot.Key]; 

                if (rawMaterialQuantity > 0) {
                    inventorySlot.gameObject.SetActive(true);
                    inventorySlot.SetQuantity(rawMaterialQuantity);
                }
                else {
                    inventorySlot.gameObject.SetActive(false);
                }
            }
        }

        private static void LoadIngredientsInventory() {
            foreach (var ingredientsSlot in ObjectsReference.Instance.ingredientsInventory.ingredientsInventory.ToList()) {
                if (ingredientsSlot.Key == IngredientsType.EMPTY) continue;
                
                var ingredientsQuantity = ObjectsReference.Instance.gameData.bananaManSavedData.ingredientsInventory[ingredientsSlot.Key.ToString()];

                ObjectsReference.Instance.ingredientsInventory.ingredientsInventory[ingredientsSlot.Key] = ingredientsQuantity;

                var inventorySlot = ObjectsReference.Instance.uiIngredientsInventory.inventorySlotsByIngredientsType[ingredientsSlot.Key]; 

                if (ingredientsQuantity > 0) {
                    inventorySlot.gameObject.SetActive(true);
                    inventorySlot.SetQuantity(ingredientsQuantity);
                }
                else {
                    inventorySlot.gameObject.SetActive(false);
                }
            }
        }

        private static void LoadBuildablesInventory() {
            foreach (var buildableSlot in ObjectsReference.Instance.blueprintsInventory.blueprintsInventory.ToList()) {
                if (buildableSlot.Key == BuildableType.EMPTY) continue;
                
                var blueprintQuantity = ObjectsReference.Instance.gameData.bananaManSavedData.blueprintsInventory[buildableSlot.Key.ToString()];

                ObjectsReference.Instance.blueprintsInventory.blueprintsInventory[buildableSlot.Key] = blueprintQuantity;

                var inventorySlot = ObjectsReference.Instance.uiBlueprintsInventory.inventorySlotsByBuildableType[buildableSlot.Key]; 

                if (blueprintQuantity > 0) {
                    inventorySlot.gameObject.SetActive(true);
                    inventorySlot.SetQuantity(blueprintQuantity);
                }
                else {
                    inventorySlot.gameObject.SetActive(false);
                }
            }
        }

        private void LoadSlots() {
            for (var i = 0; i < ObjectsReference.Instance.uiSlotsManager.uiSlotsScripts.Count; i++) {
                var itemCategoriesAndTypes = ObjectsReference.Instance.gameData.bananaManSavedData.slots[i].Split(",");
                var itemCategoryString = itemCategoriesAndTypes[0];
                var itemTypeString = itemCategoriesAndTypes[1];

                var itemCategory = (ItemCategory)Enum.Parse(typeof(ItemCategory), itemCategoryString);

                if (itemCategory == ItemCategory.BANANA) {
                    var bananaType = (BananaType)Enum.Parse(typeof(BananaType), itemTypeString);

                    var bananaScriptableObject = ObjectsReference.Instance.uiBananasInventory
                        .inventorySlotsByBananaType[bananaType].itemScriptableObject;
                    
                    ObjectsReference.Instance.uiSlotsManager.uiSlotsScripts[i].SetSlot(bananaScriptableObject);
                }
                
                if (itemCategory == ItemCategory.RAW_MATERIAL) {
                    var rawMaterialType = (RawMaterialType)Enum.Parse(typeof(RawMaterialType), itemTypeString);

                    var rawMaterialScriptableObject = ObjectsReference.Instance.uiRawMaterialsInventory.
                        inventorySlotsByRawMaterialType[rawMaterialType].itemScriptableObject;
                    
                    ObjectsReference.Instance.uiSlotsManager.uiSlotsScripts[i].SetSlot(rawMaterialScriptableObject);
                }

                if (itemCategory == ItemCategory.INGREDIENT) {
                    var ingredientsType = (IngredientsType)Enum.Parse(typeof(IngredientsType), itemTypeString);

                    var ingredientScriptableObject = ObjectsReference.Instance.uiIngredientsInventory
                        .inventorySlotsByIngredientsType[ingredientsType].itemScriptableObject;
                    
                    ObjectsReference.Instance.uiSlotsManager.uiSlotsScripts[i].SetSlot(ingredientScriptableObject);
                }
                
                if (itemCategory == ItemCategory.BUILDABLE) {
                    var buildableType = (BuildableType)Enum.Parse(typeof(BuildableType), itemTypeString);

                    var buildableScriptableObject = ObjectsReference.Instance.uiBlueprintsInventory
                        .inventorySlotsByBuildableType[buildableType].itemScriptableObject;
                    
                    ObjectsReference.Instance.uiSlotsManager.uiSlotsScripts[i].SetSlot(buildableScriptableObject);
                }
            }

            ObjectsReference.Instance.uiSlotsManager.Switch_to_Slot_Index(0);
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
            var activeItemType = ObjectsReference.Instance.gameData.bananaManSavedData.activeBanana;
            var activeItemCategory = ObjectsReference.Instance.gameData.bananaManSavedData.activeItemCategory;
            var activeBuildableType = ObjectsReference.Instance.gameData.bananaManSavedData.activeBuildableType;

            if (activeItemCategory == ItemCategory.BANANA) {
                ObjectsReference.Instance.bananaMan.activeItem = ObjectsReference.Instance.scriptableObjectManager.GetBananaScriptableObject(activeItemType);
            }

            ObjectsReference.Instance.bananaMan.activeBananaType = activeItemType;
            ObjectsReference.Instance.bananaMan.activeItemCategory = activeItemCategory;
            ObjectsReference.Instance.bananaMan.activeBuildableType = activeBuildableType;
        }

        private static void LoadMonkeysSatiety() {
            foreach (var mapData in ObjectsReference.Instance.mapsManager.mapBySceneName) {
                if (mapData.Value.mapDataScriptableObject.monkeyDataScriptableObject != null)
                    mapData.Value.mapDataScriptableObject.monkeyDataScriptableObject.sasiety = ObjectsReference.Instance.gameData.mapSavedDatasByMapName[mapData.Key].monkeySasiety;
            }
        }

        private static void CheckTutorialFinished() {
            ObjectsReference.Instance.bananaMan.tutorialFinished = ObjectsReference.Instance.gameData.bananaManSavedData.hasFinishedTutorial;

            if (ObjectsReference.Instance.bananaMan.tutorialFinished) {
                ObjectsReference.Instance.bananaGun.bananaGunInBack.SetActive(true);
                ObjectsReference.Instance.uiManager.Set_active(UICanvasGroupType.HUD, true);
            }

            else {
                ObjectsReference.Instance.bananaGun.bananaGunInBack.SetActive(false);
                ObjectsReference.Instance.uiManager.Set_active(UICanvasGroupType.HUD, false);
                ObjectsReference.Instance.uiCrosshairs.SetCrosshair(ItemCategory.EMPTY, BananaType.EMPTY);
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
                        prefab = ObjectsReference.Instance.scriptableObjectManager.BuildablePrefabByBuildableType(mapData.aspirablesBuildableTypes[i]);

                        aspirable = Instantiate(prefab, MapItems.Instance.aspirablesContainer.transform, true);

                        aspirable.transform.position = mapData.aspirablesPositions[i];
                        aspirable.transform.rotation = mapData.aspirablesRotations[i];

                        if (mapData.aspirablesBuildableTypes[i] == BuildableType.PLATEFORM) {
                            aspirable.GetComponent<Plateform>().ActivePlateform(ObjectsReference.Instance.scriptableObjectManager._meshReferenceScriptableObject.bananasDataScriptableObjects[mapData.aspirablesItemTypes[i]]);
                        }
                    }

                    if (mapData.aspirablesCategories[i] == ItemCategory.RUINE) {
                        prefab = ObjectsReference.Instance.scriptableObjectManager._meshReferenceScriptableObject.ruinesPrefab[mapData.aspirablesPrefabsIndex[i]];

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
