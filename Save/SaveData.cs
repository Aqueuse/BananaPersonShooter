using System.Collections.Generic;
using System.IO;
using JetBrains.Annotations;
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

        private SavedData _savedData;
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
            savePath = Path.Combine(_savesPath, saveUuid);

            if (!Directory.Exists(savePath)) {
                Directory.CreateDirectory(savePath);
                Directory.CreateDirectory(Path.Combine(savePath, "MAPDATA"));
                Directory.CreateDirectory(Path.Combine(savePath, "MAPS"));

                _savedData = new SavedData {
                    uuid = saveUuid,
                    saveName = "new save",
                    lastSavedDate = saveDate
                };
            }

            else {
                _savedData = new SavedData {
                    uuid = saveUuid,
                    saveName = ObjectsReference.Instance.loadData.GetsaveNameByUuid(saveUuid),
                    lastSavedDate = saveDate
                };
            }
            
            var jsonSavedData = JsonConvert.SerializeObject(_savedData);
            var jsonbananaManSavedData = JsonConvert.SerializeObject(ObjectsReference.Instance.gameData.bananaManSavedData);
            
            var savefilePath = Path.Combine(savePath, "data.json");
            
            File.WriteAllText(savefilePath, jsonSavedData);

            savefilePath = Path.Combine(savePath, "MAPS");

            foreach (var map in ObjectsReference.Instance.gameData.mapSavedDatasByMapName) {
                var mapClass = ObjectsReference.Instance.mapsManager.mapBySceneName[map.Key];

                // synchronize data beetween classes and templates
                map.Value.isDiscovered = mapClass.isDiscovered;
                map.Value.piratesDebris = mapClass.piratesDebrisToSpawn;
                map.Value.visitorsDebris = mapClass.visitorsDebrisToSpawn;

                if (mapClass.mapDataScriptableObject.monkeyDataScriptableObjectsByMonkeyId.Count > 0) {
                    foreach (var monkeyDataScriptableObject in mapClass.mapDataScriptableObject.monkeyDataScriptableObjectsByMonkeyId) {
                        map.Value.monkeysSasietyByMonkeyId[monkeyDataScriptableObject.Key] = monkeyDataScriptableObject.Value.sasiety;
                    }
                }
                
                var jsonMapSavedData = JsonConvert.SerializeObject(ObjectsReference.Instance.gameData.mapSavedDatasByMapName[map.Key]);
                var mapSavefilePath = Path.Combine(savefilePath, map.Key+".json");
                File.WriteAllText(mapSavefilePath, jsonMapSavedData);
            }

            savefilePath = Path.Combine(savePath, "player.json");
            File.WriteAllText(savefilePath, jsonbananaManSavedData);
            
            var screenshotFilePath = Path.Combine(savePath, "screenshot.png");
            SaveCameraView(screenshotFilePath);
            
            foreach (var map in ObjectsReference.Instance.mapsManager.mapBySceneName) {
                if (map.Value.itemsCategories.Count == 0) {
                    // delete file
                    var mapDataSavesPath = Path.Combine(savePath, "MAPDATA");
                    var filePath = Path.Combine(mapDataSavesPath, map.Value.mapDataScriptableObject.sceneName+"_buildables.data");

                    File.Delete(filePath);
                }

                else {
                    var mapToSave = map.Value;

                    SaveMapBuildablesData(
                        mapName: mapToSave.mapDataScriptableObject.sceneName,
                        buildablesPositions: mapToSave.itemsPositions,
                        buildablesRotations:mapToSave.itemsRotations,
                        buildablesCategories:mapToSave.itemsCategories,
                        buildableTypes: mapToSave.itemsBuildableTypes,
                        itemTypes:mapToSave.itemBananaTypes
                    );
                }
            }
            
            foreach (var map in ObjectsReference.Instance.mapsManager.mapBySceneName) {
                if (map.Value.itemsCategories.Count == 0) {
                    // delete file
                    var mapDataSavesPath = Path.Combine(savePath, "MAPDATA");
                    var filePath = Path.Combine(mapDataSavesPath, map.Value.mapDataScriptableObject.sceneName+"_debris.data");

                    File.Delete(filePath);
                }

                else {
                    var mapToSave = map.Value;

                    SaveMapDebrisData(
                        mapName: mapToSave.mapDataScriptableObject.sceneName,
                        debrisPositions: mapToSave.debrisPositions,
                        debrisRotations:mapToSave.debrisRotations,
                        debrisType: mapToSave.debrisTypes
                    );
                }
            }
        }

        public void SaveName(string saveUuid, string saveName) {
            savePath = Path.Combine(_savesPath, saveUuid);
            
            _savedData = ObjectsReference.Instance.loadData.GetSavedDataByUuid(saveUuid);
            _savedData.saveName = saveName;
            var jsonSavedData = JsonConvert.SerializeObject(_savedData);
            
            var savefilePath = Path.Combine(savePath, "data.json");

            File.WriteAllText(savefilePath, jsonSavedData);
        }

        private static void SaveCameraView(string path) {
            var screenshotCamera = ObjectsReference.Instance.gameManager.cameraMain;
            var screenTexture = new RenderTexture(150, 150, 16);
            screenshotCamera.targetTexture = screenTexture;
            RenderTexture.active = screenTexture;
            screenshotCamera.Render();
            var renderedTexture = new Texture2D(150, 150);
            renderedTexture.ReadPixels(new Rect(0, 0, 150, 150), 0, 0);
            RenderTexture.active = null;
            var byteArray = renderedTexture.EncodeToPNG();
            File.WriteAllBytes(path, byteArray);
            screenshotCamera.targetTexture = null;
        }

        private void SaveMapBuildablesData(
            string mapName,  
            List<Vector3> buildablesPositions, 
            List<Quaternion> buildablesRotations,
            
            List<ItemCategory> buildablesCategories,
            [NotNull] List<BuildableType> buildableTypes,
            [NotNull] List<BananaType> itemTypes
        ) {

            var mapDataSavesPath = Path.Combine(savePath, "MAPDATA");
            
            if (buildablesCategories.Count > 0) {
                _buildablesDatas = new string[buildablesCategories.Count];
            
                for (var i = 0; i < _buildablesDatas.Length; i++) {
                    _buildablesDatas[i] = buildablesPositions[i] + "/" +
                                          buildablesRotations[i] + "/" +
                                          buildablesCategories[i] + "/" +
                                          buildableTypes[i] + "/" +
                                          itemTypes[i];
                }

                var savefilePath = Path.Combine(mapDataSavesPath, mapName+"_buildables.data");

                using var streamWriter = new StreamWriter(savefilePath, append:false);

                foreach (var data in _buildablesDatas) {
                    streamWriter.WriteLine(data);
                }

                streamWriter.Flush();
            }
        }
        
        private void SaveMapDebrisData(
            string mapName,  
            List<Vector3> debrisPositions, 
            List<Quaternion> debrisRotations,
            List<CharacterType> debrisType
        ) {

            var mapDataSavesPath = Path.Combine(savePath, "MAPDATA");
            
            if (debrisPositions.Count > 0) {
                _debrisDatas = new string[debrisPositions.Count];
            
                for (var i = 0; i < _debrisDatas.Length; i++) {
                    _debrisDatas[i] = debrisPositions[i] + "/" +
                                      debrisRotations[i] + "/" +
                                      debrisType[i];
                }

                var savefilePath = Path.Combine(mapDataSavesPath, mapName+"_debris.data");

                using var streamWriter = new StreamWriter(savefilePath, append:false);

                foreach (var data in _debrisDatas) {
                    streamWriter.WriteLine(data);
                }

                streamWriter.Flush();
            }
        }
    }
}
