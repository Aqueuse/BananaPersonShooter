using System.Linq;
using UnityEngine;

namespace Save {
    public class GameLoad : MonoBehaviour {
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

            ObjectsReference.Instance.gameData.bananaManSaved = ObjectsReference.Instance.loadData.GetPlayerByUuid(saveUuid);

            LoadPositionAndRotationOnLastMap();
            LoadBananaManVitals();
            LoadActiveItem();

            LoadBananasInventory();
            LoadRawMaterialsInventory();
            LoadIngredientsInventory();

            LoadSelectedBanana();

            LoadMaps(saveUuid);

            CheckTutorialFinished();

            ObjectsReference.Instance.gameSave.StartAutoSave();
        }

        public void LoadBananasInventory() {
            foreach (var bananaSlot in ObjectsReference.Instance.bananasInventory.bananasInventory.ToList()) {
                if (bananaSlot.Key == BananaType.EMPTY) continue;

                var bananaQuantity = ObjectsReference.Instance.gameData.bananaManSaved.bananaInventory[bananaSlot.Key.ToString()];

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

                var rawMaterialQuantity = ObjectsReference.Instance.gameData.bananaManSaved.rawMaterialsInventory[rawMaterialSlot.Key.ToString()];

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
                
                var ingredientsQuantity = ObjectsReference.Instance.gameData.bananaManSaved.ingredientsInventory[ingredientsSlot.Key.ToString()];

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
            ObjectsReference.Instance.bananaMan.health = ObjectsReference.Instance.gameData.bananaManSaved.health;
            ObjectsReference.Instance.bananaMan.resistance = ObjectsReference.Instance.gameData.bananaManSaved.resistance;
            
            ObjectsReference.Instance.bananaMan.SetBananaSkinHealth();
        }

        private static void LoadPositionAndRotationOnLastMap() {
            ObjectsReference.Instance.gameData.lastPositionOnMap = new Vector3(
                ObjectsReference.Instance.gameData.bananaManSaved.xWorldPosition,
                ObjectsReference.Instance.gameData.bananaManSaved.yWorldPosition,
                ObjectsReference.Instance.gameData.bananaManSaved.zworldPosition);

            ObjectsReference.Instance.gameData.lastRotationOnMap = new Vector3(
                ObjectsReference.Instance.gameData.bananaManSaved.xWorldRotation,
                ObjectsReference.Instance.gameData.bananaManSaved.yWorldRotation,
                ObjectsReference.Instance.gameData.bananaManSaved.zWorldRotation
             );
        }
        
        private static void LoadActiveItem() {
            var activeItemType = ObjectsReference.Instance.gameData.bananaManSaved.activeBanana;
            
            ObjectsReference.Instance.bananaMan.activeItem = ObjectsReference.Instance.scriptableObjectManager.GetBananaScriptableObject(activeItemType);
        }
        
        private static void CheckTutorialFinished() {
            ObjectsReference.Instance.bananaMan.tutorialFinished = ObjectsReference.Instance.gameData.bananaManSaved.hasFinishedTutorial;

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
        
        private static void LoadMaps(string saveUuid) {
            foreach (var map in ObjectsReference.Instance.gameData.mapBySceneName) {
                var sceneName = map.Value.mapPropertiesScriptableObject.sceneName.ToString().ToUpper();

                if (ObjectsReference.Instance.loadData.MapDataExist(sceneName, saveUuid)) {
                    var savedMapData = ObjectsReference.Instance.loadData.GetMapSavedByUuid(ObjectsReference.Instance.gameData.currentSaveUuid, map.Key);

                    ObjectsReference.Instance.gameData.mapBySceneName[map.Key].mapPropertiesScriptableObject.piratesDebris = savedMapData.piratesDebris;
                    ObjectsReference.Instance.gameData.mapBySceneName[map.Key].mapPropertiesScriptableObject.visitorsDebris = savedMapData.visitorsDebris;

                    ObjectsReference.Instance.gameData.mapBySceneName[map.Key].mapPropertiesScriptableObject.chimployeesQuantity = savedMapData.chimployeesQuantity;
                    ObjectsReference.Instance.gameData.mapBySceneName[map.Key].mapPropertiesScriptableObject.piratesQuantity = savedMapData.piratesQuantity;
                    ObjectsReference.Instance.gameData.mapBySceneName[map.Key].mapPropertiesScriptableObject.visitorsQuantity = savedMapData.visitorsQuantity;
                    
                    foreach (var monkey in ObjectsReference.Instance.gameData.mapBySceneName[map.Key].mapPropertiesScriptableObject.monkeyPropertiesScriptableObjectsByMonkeyId) {
                        monkey.Value.sasiety = savedMapData.monkeysSasietyByMonkeyId[monkey.Key];
                    }
                    
                    map.Value.isDiscovered = savedMapData.isDiscovered;
                }
                
                ObjectsReference.Instance.loadData.LoadBuildableDataFromJsonDictionnaryByUuid(map.Value, saveUuid);
                ObjectsReference.Instance.loadData.LoadDebrisDataFromJsonDictionnaryByUuid(map.Value, saveUuid);
            }
        }

    }
}
