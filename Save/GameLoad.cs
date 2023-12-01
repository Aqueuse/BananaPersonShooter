using System.Linq;
using Gestion;
using Gestion.Buildables.Plateforms;
using Data;
using UnityEngine;

namespace Save {
    public class GameLoad : MonoBehaviour {
        private GameObject aspirable;
        private GameObject prefab;

        private ItemScriptableObject itemScriptableObject;

        public void LoadLastSave() {
            ObjectsReference.Instance.gameManager.loadingScreen.SetActive(true);
            ObjectsReference.Instance.uiManager.SetActive(UICanvasGroupType.DEATH, false);
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

            LoadSelectedBanana();

            LoadMapsData();
            LoadMonkeysSatiety();

            ObjectsReference.Instance.loadData.LoadMapBuildablesDataByUuid(saveUuid);
            ObjectsReference.Instance.loadData.LoadMapDebrisDataByUuid(saveUuid);

            CheckTutorialFinished();

            ObjectsReference.Instance.gameSave.StartAutoSave();
        }

        public void LoadBananasInventory() {
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

        public void LoadRawMaterialsInventory() {
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

        public void LoadIngredientsInventory() {
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
        
        private void LoadSelectedBanana() {
            var bananaScriptableObject = ObjectsReference.Instance.quickSlotsManager.bananaSlotItemScriptableObject;
            ObjectsReference.Instance.quickSlotsManager.SetBananaSlot(bananaScriptableObject);
            ObjectsReference.Instance.quickSlotsManager.SetBananaQuantity(ObjectsReference.Instance.bananasInventory.bananasInventory[bananaScriptableObject.bananaType]);
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
            
            ObjectsReference.Instance.bananaMan.activeItem = ObjectsReference.Instance.scriptableObjectManager.GetBananaScriptableObject(activeItemType);
        }

        private static void LoadMonkeysSatiety() {
            foreach (var mapData in ObjectsReference.Instance.mapsManager.mapBySceneName) {
                if (mapData.Value.mapDataScriptableObject.monkeyDataScriptableObjectsByMonkeyId.Count > 0) {
                    foreach (var monkeyDataScriptableObject in mapData.Value.mapDataScriptableObject.monkeyDataScriptableObjectsByMonkeyId) {
                        mapData.Value.mapDataScriptableObject.monkeyDataScriptableObjectsByMonkeyId[monkeyDataScriptableObject.Key].sasiety = monkeyDataScriptableObject.Value.sasiety;
                    }
                }
            }
        }

        private static void CheckTutorialFinished() {
            ObjectsReference.Instance.bananaMan.tutorialFinished = ObjectsReference.Instance.gameData.bananaManSavedData.hasFinishedTutorial;

            if (ObjectsReference.Instance.bananaMan.tutorialFinished) {
                ObjectsReference.Instance.bananaGun.bananaGunInBack.SetActive(true);
                ObjectsReference.Instance.uiManager.SetActive(UICanvasGroupType.HUD, true);
            }

            else {
                ObjectsReference.Instance.bananaGun.bananaGunInBack.SetActive(false);
                ObjectsReference.Instance.uiManager.SetActive(UICanvasGroupType.HUD, false);
                ObjectsReference.Instance.uiCrosshairs.SetCrosshair(BananaType.EMPTY);
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

                for (var i = 0; i < mapData.itemsCategories.Count; i++) {
                    if (mapData.itemsCategories[i] == ItemCategory.BUILDABLE) {
                        prefab = ObjectsReference.Instance.scriptableObjectManager.BuildablePrefabByBuildableType(mapData.itemsBuildableTypes[i]);
                    }
                    
                    aspirable = Instantiate(prefab, MapItems.Instance.aspirablesContainer.transform, true);

                    if (mapData.itemsBuildableTypes[i] == BuildableType.PLATEFORM &&
                        mapData.itemBananaTypes[i] != BananaType.EMPTY) {
                        aspirable.GetComponent<Plateform>().ActivePlateform(ObjectsReference.Instance.scriptableObjectManager._meshReferenceScriptableObject.bananasDataScriptableObjects[mapData.itemBananaTypes[i]]);
                    }

                    aspirable.transform.position = mapData.itemsPositions[i];
                    aspirable.transform.rotation = mapData.itemsRotations[i];
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
        }
    }
}
