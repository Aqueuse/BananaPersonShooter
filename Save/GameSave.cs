using System;
using System.Globalization;
using System.IO;
using InGame.SpaceTrafficControl;
using Newtonsoft.Json;
using Save.Dropped;
using Save.Templates;
using Tags;
using UnityEngine;

namespace Save {
    public class GameSave : MonoBehaviour {
        [SerializeField] private GameObject autoSaveBanana;
        [SerializeField] private GameObject savablesItemsContainerPrefab;

        public Transform savablesItemsContainer;
        
        public string _appPath;
        public string gamePath;
        public string _savesPath;
        public string savePath;

        private SavedData _savedData;
        
        private SpaceTrafficControlManager spaceTrafficControlManager;
        
        public DataSave dataSave;
        public PlayerSave playerSave;
        public BuildablesSave buildablesSave;
        public SpaceshipDebrisSave spaceshipDebrisSave;
        public DroppedBananaSave droppedBananaSave;
        public DroppedIngredientsSave droppedIngredientsSave;
        public DroppedManufacturedItemsSave droppedManufacturedItemSave;
        public DroppedFoodSave droppedFoodSave;
        public DroppedRawMaterialsSave droppedRawMaterialSave;
        
        public WorldSave worldSave;
        public SpaceshipsSave spaceshipsSave;
        
        private void Start() {
            _appPath = Path.GetDirectoryName(Application.persistentDataPath);
            if (_appPath != null) {
                gamePath = Path.Combine(_appPath, "Banana Man The Space Monkeys");

                _savesPath = Path.Combine(gamePath, "Saves");
                if (!Directory.Exists(_savesPath)) {
                    Directory.CreateDirectory(_savesPath);
                }
            }
            
            LoadSaves();
        }
        
        private void LoadSaves() {
            var saveFolders = Directory.GetDirectories(_savesPath);

            foreach (var folder in saveFolders) {
                var saveDataFile = Path.Combine(folder, "data.json");
                var playerDataFile = Path.Combine(folder, "player.json");

                if (File.Exists(saveDataFile) & File.Exists(playerDataFile)) {
                    var savedData = JsonConvert.DeserializeObject<SavedData>(File.ReadAllText(saveDataFile));

                    if (savedData.uuid != null) {
                        ObjectsReference.Instance.uiSave.AppendSaveSlot(savedData.uuid);
                    }
                }
            }
        }
        
        public void SaveGame(string saveUuid, string saveName, string date) {
            CreateSave(saveUuid, saveName, date);

            playerSave.SavePlayerByUuid(saveUuid);
            worldSave.SaveWorld(saveUuid);
            spaceshipDebrisSave.SaveSpaceshipDebrisData(saveUuid);

            droppedBananaSave.SaveBananaData(saveUuid);
            droppedRawMaterialSave.SaveRawMaterialData(saveUuid);
            droppedIngredientsSave.SaveIngredientsData(saveUuid);
            droppedManufacturedItemSave.SaveManufacturedItemData(saveUuid);
            droppedFoodSave.SaveFoodData(saveUuid);

            buildablesSave.SaveBuildablesData(saveUuid);
            spaceshipsSave.SaveSpaceships(saveUuid);
            
            SaveCameraView(saveUuid);
            
            dataSave.UpdateSaveDate(saveUuid);
        }
        
        public void StartAutoSave() {
            if (ObjectsReference.Instance.gameSettings.saveDelayMinute > 0) InvokeRepeating(nameof(AutoSave), ObjectsReference.Instance.gameSettings.saveDelayMinute, ObjectsReference.Instance.gameSettings.saveDelayMinute);
        }
        
        public void CancelAutoSave() {
            CancelInvoke();
        }
        
        public void AutoSave() {
            autoSaveBanana.SetActive(true);
            
            var date = DateTime.ParseExact(DateTime.Now.ToString("U"), "U", CultureInfo.CurrentCulture).ToString(CultureInfo.CurrentCulture);
            
            SaveGame("autosave", "autosave", date);
            
            // reflect on UI
            ObjectsReference.Instance.uiSave.UpdateAutoSave(date);

            Invoke(nameof(HideAutoSaveBanana), 5);
        }

        public void DeleteSave(string saveUuid) {
            var worldDataSavesPath = Path.Combine(_savesPath, saveUuid);

            if (Directory.Exists(worldDataSavesPath)) {
                Directory.Delete(worldDataSavesPath, true);
            }
        }

        private void HideAutoSaveBanana() {
            autoSaveBanana.SetActive(false);
        }

        public string GetSavePathByUuid(string saveUuid) {
            return Path.Combine(_savesPath, saveUuid);
        }
        
        public void LoadSave(string saveUuid) {
            playerSave.LoadPlayer(saveUuid);
            worldSave.LoadWorld(saveUuid);
            
            Destroy(TagsManager.GetAllGameObjectsWithTag(GAME_OBJECT_TAG.SAVABLES_ITEMS_CONTAINER)[0].gameObject);
            savablesItemsContainer = Instantiate(savablesItemsContainerPrefab).transform;

            buildablesSave.LoadBuildablesDataByUuid(saveUuid);
            spaceshipDebrisSave.LoadSpaceshipDebrisDataByUuid(saveUuid);
            
            droppedBananaSave.LoadBananasDataByUuid(saveUuid);
            droppedRawMaterialSave.LoadRawMaterialDataByUuid(saveUuid);
            droppedIngredientsSave.LoadIngredientsDataByUuid(saveUuid);
            droppedManufacturedItemSave.LoadManufacturedItemDataByUuid(saveUuid);
            droppedFoodSave.LoadFoodDataByUuid(saveUuid);
            
            spaceshipsSave.LoadpaceshipsData(saveUuid);
            
            ObjectsReference.Instance.gameSave.StartAutoSave();
        }
        
        private void CreateSave(string saveUuid, string saveName, string saveDate) {
            savePath = Path.Combine(_savesPath, saveUuid);

            _savedData = new SavedData { uuid = saveUuid, saveName = saveName, lastSavedDate = saveDate };
            
            if (!Directory.Exists(savePath)) {
                Directory.CreateDirectory(savePath);
                Directory.CreateDirectory(Path.Combine(savePath, "WORLD_DATA"));
            }

            else _savedData.saveName = dataSave.GetsaveNameByUuid(saveUuid);

            var jsonSaved = JsonConvert.SerializeObject(_savedData);
            var dataSavefilePath = Path.Combine(savePath, "data.json");
            File.WriteAllText(dataSavefilePath, jsonSaved);
            
            SaveCameraView(saveUuid);
        }
        
        private void SaveCameraView(string saveUuid) {
            var _savePath = Path.Combine(_savesPath, saveUuid);
            var screenshotFilePath = Path.Combine(_savePath, "screenshot.png");

            var currentResolution = ObjectsReference.Instance.gameSettings.GetCurrentResolution();
            
            var screenshotCamera = ObjectsReference.Instance.gameManager.cameraMain;
            var screenTexture = new RenderTexture(currentResolution.width, currentResolution.height, 16);
            screenshotCamera.targetTexture = screenTexture;
            RenderTexture.active = screenTexture;
            screenshotCamera.Render();
            var renderedTexture = new Texture2D(currentResolution.width, currentResolution.height);
            renderedTexture.ReadPixels(new Rect(0, 0, currentResolution.width, currentResolution.height), 0, 0);
            RenderTexture.active = null;
            var byteArray = renderedTexture.EncodeToPNG();
            File.WriteAllBytes(screenshotFilePath, byteArray);
            screenshotCamera.targetTexture = null;
        }
        
        public SavedData GetSavedByUuid(string saveUuid) {
            savePath = Path.Combine(_savesPath, saveUuid);

            var savefilePath = Path.Combine(savePath, "data.json");
            var savedDataString = File.ReadAllText(savefilePath);

            return JsonConvert.DeserializeObject<SavedData>(savedDataString);
        }
    }
}
