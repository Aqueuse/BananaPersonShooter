using System.Collections.Generic;
using System.IO;
using Enums;
using Game;
using Newtonsoft.Json;
using Save.Templates;
using UnityEngine;

namespace Save {
    public class SaveData : MonoSingleton<SaveData> {
        private string[] _debrisDatas;
        private string[] _plateformsDatas;

        private string _appPath;
        public string gamePath;
        private string _savesPath;

        private SavedData _savedData;

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
            var savePath = Path.Combine(_savesPath, saveUuid);

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
                    saveName = LoadData.Instance.GetsaveNameByUuid(saveUuid),
                    lastSavedDate = saveDate
                };
            }
            
            var jsonSavedData = JsonConvert.SerializeObject(_savedData);
            var jsonbananaManSavedData = JsonConvert.SerializeObject(GameData.Instance.bananaManSavedData);
            
            string savefilePath = Path.Combine(savePath, "data.json");
            
            File.WriteAllText(savefilePath, jsonSavedData);
            
            savefilePath = Path.Combine(savePath, "MAPS");
            
            foreach (var map in GameData.Instance.mapSavedDatasByMapName) {
                Map mapClass = MapsManager.Instance.mapBySceneName[map.Key];
                
                // synchronize data beetween classes and templates
                map.Value.isDiscovered = mapClass.isDiscovered;
                map.Value.cleanliness = mapClass.cleanliness;
                map.Value.monkey_sasiety = mapClass.monkeySasiety;
            
                map.Value.isShowingDebris = mapClass.isShowingDebris;
                map.Value.isShowingBananaTrees = mapClass.isShowingBananaTrees;
                
                var jsonMapSavedData = JsonConvert.SerializeObject(GameData.Instance.mapSavedDatasByMapName[map.Key]);
                var mapSavefilePath = Path.Combine(savefilePath, map.Key+".json");
                File.WriteAllText(mapSavefilePath, jsonMapSavedData);
            }
            
            savefilePath = Path.Combine(savePath, "player.json");
            File.WriteAllText(savefilePath, jsonbananaManSavedData);
            
            string screenshotFilePath = Path.Combine(savePath, "screenshot.png");
            SaveCameraView(screenshotFilePath);
        }

        public void SaveName(string saveUuid, string saveName) {
            var savePath = Path.Combine(_savesPath, saveUuid);
            
            _savedData = LoadData.Instance.GetSavedDataByUuid(saveUuid);
            _savedData.saveName = saveName;
            var jsonSavedData = JsonConvert.SerializeObject(_savedData);
            
            string savefilePath = Path.Combine(savePath, "data.json");

            File.WriteAllText(savefilePath, jsonSavedData);
        }

        void SaveCameraView(string path) {
            var screenshotCamera = GameManager.Instance.cameraMain;
            RenderTexture screenTexture = new RenderTexture(150, 150, 16);
            screenshotCamera.targetTexture = screenTexture;
            RenderTexture.active = screenTexture;
            screenshotCamera.Render();
            Texture2D renderedTexture = new Texture2D(150, 150);
            renderedTexture.ReadPixels(new Rect(0, 0, 150, 150), 0, 0);
            RenderTexture.active = null;
            byte[] byteArray = renderedTexture.EncodeToPNG();
            File.WriteAllBytes(path, byteArray);
            screenshotCamera.targetTexture = null;
        }
        
        public void SaveMapDebrisDataByUuid(Vector3[] debrisPosition, Quaternion[] debrisRotation, int[] debrisIndex, string mapName, string saveUuid) {
            var savePath = Path.Combine(_savesPath, saveUuid);
            var mapDataSavesPath = Path.Combine(savePath, "MAPDATA");
            
            _debrisDatas = new string[debrisPosition.Length];

            for (var i = 0; i < debrisPosition.Length; i++) {
                _debrisDatas[i] = debrisPosition[i] + "/" + debrisRotation[i] + "/" + debrisIndex[i];
            }

            string savefilePath = Path.Combine(mapDataSavesPath, mapName+"_debris.data");
            
            using StreamWriter streamWriter = new StreamWriter(savefilePath, append:false);
            
            foreach (var debrisData in _debrisDatas) {
                streamWriter.WriteLine(debrisData);
            }
            
            streamWriter.Flush();
        }
        
        public void SaveMapPlateformsDataByUuid(List<Vector3> plateformsPosition, List<PlateformType> plateformTypes, string mapName, string saveUuid) {
            var savePath = Path.Combine(_savesPath, saveUuid);
            var mapDataSavesPath = Path.Combine(savePath, "MAPDATA");
            
            _plateformsDatas = new string[plateformsPosition.Count];
            
            for (var i = 0; i < plateformsPosition.Count; i++) {
                _plateformsDatas[i] = plateformsPosition[i] + "/" + plateformTypes[i];
            }

            if (_plateformsDatas.Length > 0) {
                string savefilePath = Path.Combine(mapDataSavesPath, mapName+"_plateforms.data");

                using StreamWriter streamWriter = new StreamWriter(savefilePath, append:false);

                foreach (var plateformsData in _plateformsDatas) {
                    streamWriter.WriteLine(plateformsData);
                }

                streamWriter.Flush();
            }
        }
    }
}
