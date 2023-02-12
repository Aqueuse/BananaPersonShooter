using System.IO;
using Newtonsoft.Json;
using Save.Templates;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Save {
    public class SaveData : MonoSingleton<SaveData> {
        [SerializeField] private GameObject screenshotCameraGameObject;
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
                
                savedData = new SavedData {
                    uuid = saveUuid,
                    saveName = "new save",
                    lastSavedDate = saveDate
                };
            }

            else {
                savedData = new SavedData {
                    uuid = saveUuid,
                    saveName = LoadData.Instance.GetSavedDataByUuid(saveUuid).saveName,
                    lastSavedDate = saveDate
                };
            }
            
            var jsonSavedData = JsonConvert.SerializeObject(savedData);
            var jsonbananaManSavedData = JsonConvert.SerializeObject(GameData.Instance.bananaManSavedData);
            var jsonMap01SavedData = JsonConvert.SerializeObject(GameData.Instance.map01SavedData);
            
            string savefilePath = Path.Combine(savePath, "data.json");
            File.WriteAllText(savefilePath, jsonSavedData);
            
            savefilePath = Path.Combine(savePath, "player.json");
            File.WriteAllText(savefilePath, jsonbananaManSavedData);
            
            savefilePath = Path.Combine(savePath, "MAP01.json");
            File.WriteAllText(savefilePath, jsonMap01SavedData);

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
            screenshotCameraGameObject.SetActive(true);
            var screenshotCamera = screenshotCameraGameObject.GetComponent<Camera>();
            RenderTexture screenTexture = new RenderTexture(150, 150, 16);
            screenshotCamera.targetTexture = screenTexture;
            RenderTexture.active = screenTexture;
            screenshotCamera.Render();
            Texture2D renderedTexture = new Texture2D(150, 150);
            renderedTexture.ReadPixels(new Rect(0, 0, 150, 150), 0, 0);
            RenderTexture.active = null;
            byte[] byteArray = renderedTexture.EncodeToPNG();
            File.WriteAllBytes(path, byteArray);
            screenshotCameraGameObject.SetActive(false);
        }
        
        public void SaveMapDataByUuid(Vector3[] debrisPosition, Quaternion[] debrisRotation, int[] debrisIndex, string saveUuid) {
            var savePath = Path.Combine(savesPath, saveUuid);
            var mapDataSavesPath = Path.Combine(savePath, "MAPDATA");

            if (!File.Exists(mapDataSavesPath)) {
                Directory.CreateDirectory(mapDataSavesPath);
            }
            
            debrisDatas = new string[debrisPosition.Length];
        
            for (var i = 0; i < debrisPosition.Length; i++) {
                debrisDatas[i] = debrisPosition[i] + "/" + debrisRotation[i] + "/" + debrisIndex[i];
            }
            
            string savefilePath = Path.Combine(mapDataSavesPath, SceneManager.GetActiveScene().name.ToUpper()+"_debris.data");

            using StreamWriter streamWriter = new StreamWriter(savefilePath, append:false);

            foreach (var debrisData in debrisDatas) {
                streamWriter.WriteLine(debrisData);
            }
            
            streamWriter.Flush();
        }
    }
}
