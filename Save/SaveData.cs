using System.Collections.Generic;
using System.IO;
using InGame.Items.ItemsData;
using Newtonsoft.Json;
using Save.Helpers;
using Save.Templates;
using UnityEngine;

namespace Save {
    public class SaveData : MonoBehaviour {
        private string[] _buildablesDatas;
        private string[] _debrisDatas;
        private string[] _plateformsDatas;

        private string _appPath;
        public string gamePath;
        private string _savesPath;

        private Saved _saved;
        private string savePath;

        private void Start() {
            _appPath = Path.GetDirectoryName(Application.persistentDataPath);
            if (_appPath != null) {
                gamePath = Path.Combine(_appPath, "Banana Man The Space Monkeys");

                _savesPath = Path.Combine(gamePath, "Saves");
                if (!Directory.Exists(_savesPath)) {
                    Directory.CreateDirectory(_savesPath);
                }
            }
        }

        public void DeleteSave(string saveUuid) {
            var worldDataSavesPath = Path.Combine(_savesPath, saveUuid);
            Directory.Delete(worldDataSavesPath, true);
        }

        public void Save(string saveUuid, string saveDate) {
            CreateSave(saveUuid, saveDate);
            
            SavePlayer();
            
            SaveWorld();
            
            SaveSpaceships();
            
            SaveCameraView();
        }


        private void CreateSave(string saveUuid, string saveDate) {
            savePath = Path.Combine(_savesPath, saveUuid);

            _saved = new Saved { uuid = saveUuid, saveName = "new save", lastSavedDate = saveDate };
            
            if (!Directory.Exists(savePath)) {
                Directory.CreateDirectory(savePath);
                Directory.CreateDirectory(Path.Combine(savePath, "WORLD_DATA"));
            }

            else _saved.saveName = ObjectsReference.Instance.loadData.GetsaveNameByUuid(saveUuid);

            var jsonSaved = JsonConvert.SerializeObject(_saved);
            var dataSavefilePath = Path.Combine(savePath, "data.json");
            File.WriteAllText(dataSavefilePath, jsonSaved);
        }

        private void SavePlayer() {
            var jsonbananaManSaved = JsonConvert.SerializeObject(ObjectsReference.Instance.gameData.bananaManSaved);
            var playerSavefilePath = Path.Combine(savePath, "player.json");
            File.WriteAllText(playerSavefilePath, jsonbananaManSaved);
        }

        private void SaveWorld() {
            World.Instance.SaveAspirablesOnWorld();
            World.Instance.SaveMapMonkeysData();

            var worldData = ObjectsReference.Instance.gameData.worldData;
            
            var worldSavedData = new WorldSavedData();

            foreach (var monkeyPosition in worldData.monkeysPositionByMonkeyId) {
                worldSavedData.monkeysPositionByMonkeyId.Add(monkeyPosition.Key,
                    JsonHelper.FromVector3ToString(monkeyPosition.Value));
            }

            foreach (var monkeySasiety in worldData.monkeysSasietyTimerByMonkeyId) {
                worldSavedData.monkeysSasietyTimerByMonkeyId.Add(monkeySasiety.Key, monkeySasiety.Value);
            }
            
            worldSavedData.visitorsDebris = worldData.visitorsDebrisToSpawn;
            worldSavedData.piratesDebris = worldData.piratesDebrisToSpawn;
            worldSavedData.merchantDebris = worldData.merchantsDebrisToSpawn;

            worldSavedData.chimployeesQuantity = worldData.chimployeesQuantity;
            worldSavedData.visitorsQuantity = worldData.visitorsQuantity;
            worldSavedData.piratesQuantity = worldData.piratesQuantity;
                
            var jsonMapSaved = JsonConvert.SerializeObject(worldSavedData);
            var worldSavefilePath = Path.Combine(savePath, "world.json");
            File.WriteAllText(worldSavefilePath, jsonMapSaved);
                
            SaveWorldData(worldData);
        }

        private void SaveWorldData(WorldData worldDataToSave) {
            SaveBuildableDataAsDictionnary(worldDataToSave);
            SaveDebrisDataAsDictionnary(worldDataToSave);
        }

        public void SaveName(string saveUuid, string saveName) {
            savePath = Path.Combine(_savesPath, saveUuid);

            _saved = ObjectsReference.Instance.loadData.GetSavedByUuid(saveUuid);
            _saved.saveName = saveName;
            var jsonSavedData = JsonConvert.SerializeObject(_saved);

            var savefilePath = Path.Combine(savePath, "data.json");

            File.WriteAllText(savefilePath, jsonSavedData);
        }

        private void SaveCameraView() {
            var screenshotFilePath = Path.Combine(savePath, "screenshot.png");

            var screenshotCamera = ObjectsReference.Instance.gameManager.cameraMain;
            var screenTexture = new RenderTexture(150, 150, 16);
            screenshotCamera.targetTexture = screenTexture;
            RenderTexture.active = screenTexture;
            screenshotCamera.Render();
            var renderedTexture = new Texture2D(150, 150);
            renderedTexture.ReadPixels(new Rect(0, 0, 150, 150), 0, 0);
            RenderTexture.active = null;
            var byteArray = renderedTexture.EncodeToPNG();
            File.WriteAllBytes(screenshotFilePath, byteArray);
            screenshotCamera.targetTexture = null;
        }

        private void SaveBuildableDataAsDictionnary(WorldData worldDataToSave) {
            var worldDataSavesPath = Path.Combine(savePath, "WORLD_DATA");

            var savefilePath = Path.Combine(worldDataSavesPath, "buildables.json");

            var buildablesToSave = worldDataToSave.buildablesDataDictionaryByBuildableType;

            var json = JsonConvert.SerializeObject(buildablesToSave);
            File.WriteAllText(savefilePath, json);
        }

        private void SaveDebrisDataAsDictionnary(WorldData worldDataToSave) {
            var mapDataSavesPath = Path.Combine(savePath, "WORLD_DATA");

            var savefilePath = Path.Combine(mapDataSavesPath, "debris.json");

            var debrisToSave = worldDataToSave.debrisDataDictionnaryByCharacterType;

            var json = JsonConvert.SerializeObject(debrisToSave);
            File.WriteAllText(savefilePath, json);
        }

        private void SaveSpaceships() {
            var mapDataSavesPath = Path.Combine(savePath, "WORLD_DATA");

            List<string> jsonSpaceshipsSaved = new List<string>();

            foreach (var spaceshipsBehaviour in ObjectsReference.Instance.spaceTrafficControlManager.spaceshipBehavioursByGuid) {
                spaceshipsBehaviour.Value.GenerateSaveData();
                jsonSpaceshipsSaved.Add(spaceshipsBehaviour.Value.savedData);
            }

            var json = JsonConvert.SerializeObject(jsonSpaceshipsSaved);

            var playerSavefilePath = Path.Combine(mapDataSavesPath, "spaceships.json");
            File.WriteAllText(playerSavefilePath, json);
        }
    }
}