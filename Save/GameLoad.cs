using System;
using System.Linq;
using InGame.Player;
using Save.Helpers;
using UnityEngine;

namespace Save {
    public class GameLoad : MonoBehaviour {
        private GameData gameData;
        private LoadData loadData;
        private BananaMan bananaMan;
        
        private GenericDictionary<BananaType, int> bananasInventory;
        private GenericDictionary<RawMaterialType, int> rawMaterialsInventory;
        private GenericDictionary<ManufacturedItemsType, int> manufacturedItemsInventory;
        private GenericDictionary<IngredientsType, int> ingredientsInventory;
        
        private void Start() {
            gameData = ObjectsReference.Instance.gameData;
            loadData = ObjectsReference.Instance.loadData;
            bananaMan = ObjectsReference.Instance.bananaMan;
        }

        public void LoadLastSave() {
            ObjectsReference.Instance.gameManager.loadingScreen.SetActive(true);
            ObjectsReference.Instance.uiManager.SetActive(UICanvasGroupType.DEATH, false);
            ObjectsReference.Instance.death.HideDeath();

            if (gameData.currentSaveUuid == null) ObjectsReference.Instance.gameManager.ReturnHome();
            else {
                ObjectsReference.Instance.gameManager.Play(gameData.currentSaveUuid, false);
            }
        }

        public void LoadGameData(string saveUuid) {
            gameData.currentSaveUuid = saveUuid;

            gameData.bananaManSaved = loadData.GetPlayerByUuid(saveUuid);

            LoadPositionAndRotationOnLastMap();
            LoadBananaManVitals();
            LoadActiveItem();
            LoadBitkongQuantity();

            LoadBananasInventory();
            LoadRawMaterialsInventory();
            LoadIngredientsInventory();
            LoadManufacturedItemsInventory();

            LoadSelectedBanana();

            LoadWorld(saveUuid);

            
            
            CheckTutorialFinished();

            ObjectsReference.Instance.gameSave.StartAutoSave();
        }

        public void LoadBananasInventory() {
            bananasInventory = bananaMan.inventories.bananasInventory;

            foreach (var bananaSlot in bananasInventory.ToList()) {
                if (bananaSlot.Key == BananaType.EMPTY) continue;

                bananasInventory[bananaSlot.Key] = gameData.bananaManSaved.bananaInventory[bananaSlot.Key.ToString()];
            }
        }

        public void LoadRawMaterialsInventory() {
            rawMaterialsInventory = bananaMan.inventories.rawMaterialsInventory;

            foreach (var rawMaterialSlot in rawMaterialsInventory.ToList()) {
                if (rawMaterialSlot.Key == RawMaterialType.EMPTY) continue;

                rawMaterialsInventory[rawMaterialSlot.Key] = gameData.bananaManSaved.rawMaterialsInventory[rawMaterialSlot.Key.ToString()];
            }
        }

        public void LoadIngredientsInventory() {
            ingredientsInventory = bananaMan.inventories.ingredientsInventory;

            foreach (var ingredientsSlot in ingredientsInventory.ToList()) {
                if (ingredientsSlot.Key == IngredientsType.EMPTY) continue;
                
                ingredientsInventory[ingredientsSlot.Key] = gameData.bananaManSaved.ingredientsInventory[ingredientsSlot.Key.ToString()];
            }
        }

        private void LoadManufacturedItemsInventory() {
            manufacturedItemsInventory = bananaMan.inventories.manufacturedItemsInventory;

            foreach (var manufacturedItemSlot in manufacturedItemsInventory.ToList()) {
                if (manufacturedItemSlot.Key == ManufacturedItemsType.EMPTY) continue;
                
                manufacturedItemsInventory[manufacturedItemSlot.Key] = gameData.bananaManSaved.manufacturedInventory[manufacturedItemSlot.Key.ToString()];
            }
        }
        
        private void LoadSelectedBanana() {
            var bananaSlotType = Enum.Parse<BananaType>(gameData.bananaManSaved.bananaSlot);
            var bananaScriptableObject = ObjectsReference.Instance.meshReferenceScriptableObject.bananasPropertiesScriptableObjects[bananaSlotType];
            bananaMan.activeItem = bananaScriptableObject;
            ObjectsReference.Instance.uiTools.SetBananaQuantity(bananaMan.inventories.bananasInventory[bananaScriptableObject.bananaType]);
        }

        private void LoadBananaManVitals() {
            bananaMan.health = gameData.bananaManSaved.health;
            bananaMan.resistance = gameData.bananaManSaved.resistance;
            
            bananaMan.SetBananaSkinHealth();
        }

        private void LoadPositionAndRotationOnLastMap() {
            gameData.lastPositionOnMap = new Vector3(
                gameData.bananaManSaved.xWorldPosition,
                gameData.bananaManSaved.yWorldPosition,
                gameData.bananaManSaved.zworldPosition);

            gameData.lastRotationOnMap = new Vector3(
                gameData.bananaManSaved.xWorldRotation,
                gameData.bananaManSaved.yWorldRotation,
                gameData.bananaManSaved.zWorldRotation
             );
        }
        
        private void LoadActiveItem() {
            var activeItemType = gameData.bananaManSaved.activeBanana;
            
            bananaMan.activeItem = ObjectsReference.Instance.meshReferenceScriptableObject.bananasPropertiesScriptableObjects[activeItemType];
            ObjectsReference.Instance.uiTools.SetPlateformPlacementAvailability(ObjectsReference.Instance.rawMaterialsInventory.HasCraftingIngredients(BuildableType.PLATEFORM));
        }

        private void LoadBitkongQuantity() {
            bananaMan.inventories.bitKongQuantity = gameData.bananaManSaved.bitKongQuantity;
        }
        
        private void CheckTutorialFinished() {
            bananaMan.tutorialFinished = gameData.bananaManSaved.hasFinishedTutorial;

            if (bananaMan.tutorialFinished) {
                ObjectsReference.Instance.bananaGun.bananaGunInBack.SetActive(true);
                ObjectsReference.Instance.uiManager.SetActive(UICanvasGroupType.HUD, true);
            }

            else {
                ObjectsReference.Instance.bananaGun.bananaGunInBack.SetActive(false);
                ObjectsReference.Instance.uiManager.SetActive(UICanvasGroupType.HUD, false);
                ObjectsReference.Instance.uiCrosshairs.SetCrosshair(BananaType.EMPTY);
            }
        }
        
        private void LoadWorld(string saveUuid) {
            if (loadData.WorldDataExist(saveUuid)) {
                var savedWorldData = loadData.GetWorldSavedByUuid(gameData.currentSaveUuid);

                var worldToLoad = gameData.worldData;

                worldToLoad.piratesDebris = savedWorldData.piratesDebris;
                worldToLoad.visitorsDebris = savedWorldData.visitorsDebris;

                worldToLoad.chimployeesQuantity = savedWorldData.chimployeesQuantity;
                worldToLoad.piratesQuantity = savedWorldData.piratesQuantity;
                worldToLoad.visitorsQuantity = savedWorldData.visitorsQuantity;

                if (savedWorldData.monkeysPositionByMonkeyId != null) {
                    foreach (var monkeyPosition in savedWorldData.monkeysPositionByMonkeyId) {
                        worldToLoad.monkeysPositionByMonkeyId.Add(monkeyPosition.Key, JsonHelper.FromStringToVector3(monkeyPosition.Value));
                    }
                }

                if (savedWorldData.monkeysSasietyTimerByMonkeyId != null) {
                    foreach (var monkeySasiety in savedWorldData.monkeysSasietyTimerByMonkeyId) {
                        worldToLoad.monkeysSasietyTimerByMonkeyId.Add(monkeySasiety.Key, monkeySasiety.Value);
                    }
                }

                loadData.LoadBuildableDataFromJsonDictionnaryByUuid(saveUuid);
                loadData.LoadDebrisDataFromJsonDictionnaryByUuid(saveUuid);

                LoadSpaceships(saveUuid);
            }
        }

        private void LoadSpaceships(string saveUuid) {
            loadData.LoadpaceshipsData(saveUuid);
        }
    }
}
