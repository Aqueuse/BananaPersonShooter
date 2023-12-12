using System.Collections.Generic;
using System.IO;
using Game;
using Gestion;
using Newtonsoft.Json;
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
            var mapDataSavesPath = Path.Combine(_savesPath, saveUuid);
            Directory.Delete(mapDataSavesPath, true);
        }

        public void Save(string saveUuid, string saveDate) {
            CreateSave(saveUuid, saveDate);
            
            SavePlayer();
            
            SaveMaps();
            
            SaveCameraView();
        }


        private void CreateSave(string saveUuid, string saveDate) {
            savePath = Path.Combine(_savesPath, saveUuid);

            _saved = new Saved { uuid = saveUuid, saveName = "new save", lastSavedDate = saveDate };
            
            if (!Directory.Exists(savePath)) {
                Directory.CreateDirectory(savePath);
                Directory.CreateDirectory(Path.Combine(savePath, "MAPDATA"));
                Directory.CreateDirectory(Path.Combine(savePath, "MAPS"));
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

        private void SaveMaps() {
            Map.Instance.SaveAspirablesOnMap();
            
            var MAPS_Save_file_Path = Path.Combine(savePath, "MAPS");

            foreach (var map in ObjectsReference.Instance.gameData.mapBySceneName) {
                var mapSavedData = new MapSavedData();
                
                if (map.Value.mapPropertiesScriptableObject.monkeyPropertiesScriptableObjectsByMonkeyId.Count > 0) {
                    mapSavedData.monkeysSasietyByMonkeyId = new Dictionary<string, float>();
                    
                    foreach (var monkey in map.Value.mapPropertiesScriptableObject.monkeyPropertiesScriptableObjectsByMonkeyId) {
                        mapSavedData.monkeysSasietyByMonkeyId.Add(monkey.Key, monkey.Value.sasiety);
                    }
                }
                
                mapSavedData.isDiscovered = map.Value.isDiscovered;
                mapSavedData.visitorsDebris = map.Value.visitorsDebrisToSpawn;
                mapSavedData.piratesDebris = map.Value.piratesDebrisToSpawn;
                
                var jsonMapSaved = JsonConvert.SerializeObject(mapSavedData);
                var mapSavefilePath = Path.Combine(MAPS_Save_file_Path, map.Key + ".json");
                File.WriteAllText(mapSavefilePath, jsonMapSaved);
                
                SaveMapData(map.Value);
            }
        }

        private void SaveMapData(MapData mapDataToSave) {
            var mapDataSavesPath = Path.Combine(savePath, "MAPDATA");
            
            if (mapDataToSave.buildablesDataInMapDictionaryByBuildableType.Count == 0) {
                var buildableFilePath = Path.Combine(mapDataSavesPath,
                    mapDataToSave.mapPropertiesScriptableObject.sceneName.ToString().ToLower() + "_buildables.json");

                if (File.Exists(buildableFilePath)) File.Delete(buildableFilePath);
            }

            else {
                SaveBuildableDataAsDictionnary(mapDataToSave);
            }

            if (mapDataToSave.debrisDataInMapDictionnaryByCharacterType.Count == 0) {
                var debrisFilePath = Path.Combine(mapDataSavesPath,
                    mapDataToSave.mapPropertiesScriptableObject.sceneName.ToString().ToLower() + "_debris.json");

                if (File.Exists(debrisFilePath)) File.Delete(debrisFilePath);
            }

            else {
                SaveDebrisDataAsDictionnary(mapDataToSave);
            }
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

        private void SaveBuildableDataAsDictionnary(MapData mapDataToSave) {
            var mapDataSavesPath = Path.Combine(savePath, "MAPDATA");
            
            var savefilePath = Path.Combine(mapDataSavesPath, mapDataToSave.mapPropertiesScriptableObject.sceneName.ToString().ToLower() + "_buildables.json");

            var buildablesToSave = mapDataToSave.buildablesDataInMapDictionaryByBuildableType;
            
            var json = JsonConvert.SerializeObject(buildablesToSave);
            File.WriteAllText(savefilePath, json);
        }
        
        private void SaveDebrisDataAsDictionnary(MapData mapDataToSave) {
            var mapDataSavesPath = Path.Combine(savePath, "MAPDATA");
            
            var savefilePath = Path.Combine(mapDataSavesPath, mapDataToSave.mapPropertiesScriptableObject.sceneName.ToString().ToLower() + "_debris.json");

            var debrisToSave = mapDataToSave.debrisDataInMapDictionnaryByCharacterType;
            
            var json = JsonConvert.SerializeObject(debrisToSave);
            File.WriteAllText(savefilePath, json);
        }
    }
}