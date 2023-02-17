using System.IO;
using Game;
using Newtonsoft.Json;
using Save.Templates;
using UnityEngine;

namespace Save {
    public class SaveData : MonoSingleton<SaveData> {
        private string[] debrisDatas;

        private string appPath;
        private string gamePath;
        private string savesPath;

        private SavedData savedData;

        private void Start() {
            appPath = Path.GetDirectoryName(Application.persistentDataPath);
            if (appPath != null) {
                gamePath = Path.Combine(appPath, "Banana Man The Space Monkeys");
            
                savesPath = Path.Combine(gamePath, "Saves");
                if (!Directory.Exists(savesPath)) {
                    Directory.CreateDirectory(savesPath);
                }
            }
        }

        public void DeleteSave(string saveUuid) {
            var mapDataSavesPath = Path.Combine(savesPath, saveUuid);
            Directory.Delete(mapDataSavesPath, true);
        }

        public void Save(string saveUuid, string saveDate) {
            var savePath = Path.Combine(savesPath, saveUuid);
            
            if (!Directory.Exists(savePath)) {
                Directory.CreateDirectory(savePath);
                Directory.CreateDirectory(Path.Combine(savePath, "MAPDATA"));
                Directory.CreateDirectory(Path.Combine(savePath, "MAPS"));
                
                savedData = new SavedData {
                    uuid = saveUuid,
                    saveName = "new save",
                    lastSavedDate = saveDate
                };
            }

            else {
                savedData = new SavedData {
                    uuid = saveUuid,
                    saveName = LoadData.Instance.GetsaveNameByUuid(saveUuid),
                    lastSavedDate = saveDate
                };
            }
            
            var jsonSavedData = JsonConvert.SerializeObject(savedData);
            var jsonbananaManSavedData = JsonConvert.SerializeObject(GameData.Instance.BananaManSavedData);

            string savefilePath = Path.Combine(savePath, "data.json");
            File.WriteAllText(savefilePath, jsonSavedData);

            savefilePath = Path.Combine(savePath, "MAPS");

            foreach (var map in GameData.Instance.mapSavedDatasByMapName) {
                // synchronize data beetween classes and templates
                map.Value.isDiscovered = MapsManager.Instance.mapBySceneName[map.Key].isDiscovered;
                map.Value.cleanliness = MapsManager.Instance.mapBySceneName[map.Key].cleanliness;
                map.Value.monkey_sasiety = MapsManager.Instance.mapBySceneName[map.Key].monkeySasiety;
                
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
            var savePath = Path.Combine(savesPath, saveUuid);
            
            savedData = LoadData.Instance.GetSavedDataByUuid(saveUuid);
            savedData.saveName = saveName;
            var jsonSavedData = JsonConvert.SerializeObject(savedData);
            
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
        
        public void SaveMapDataByUuid(Vector3[] debrisPosition, Quaternion[] debrisRotation, int[] debrisIndex, string mapName, string saveUuid) {
            var savePath = Path.Combine(savesPath, saveUuid);
            var mapDataSavesPath = Path.Combine(savePath, "MAPDATA");

            if (!File.Exists(mapDataSavesPath)) {
                Directory.CreateDirectory(mapDataSavesPath);
            }
            
            debrisDatas = new string[debrisPosition.Length];
        
            for (var i = 0; i < debrisPosition.Length; i++) {
                debrisDatas[i] = debrisPosition[i] + "/" + debrisRotation[i] + "/" + debrisIndex[i];
            }
            
            string savefilePath = Path.Combine(mapDataSavesPath, mapName+"_debris.data");

            using StreamWriter streamWriter = new StreamWriter(savefilePath, append:false);

            foreach (var debrisData in debrisDatas) {
                streamWriter.WriteLine(debrisData);
            }
            
            streamWriter.Flush();
        }
    }
}
