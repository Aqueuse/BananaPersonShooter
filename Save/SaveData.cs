using System.Collections.Generic;
using System.IO;
using Game;
using JetBrains.Annotations;
using Newtonsoft.Json;
using PeterO.Cbor;
using Save.Templates;
using UnityEngine;

namespace Save {
    public class SaveData : MonoBehaviour {
        private string[] _buildablesDatas;
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
                    saveName = ObjectsReference.Instance.loadData.GetsaveNameByUuid(saveUuid),
                    lastSavedDate = saveDate
                };
            }
            
            var jsonSavedData = JsonConvert.SerializeObject(_savedData);
            var jsonbananaManSavedData = JsonConvert.SerializeObject(ObjectsReference.Instance.gameData.bananaManSavedData);
            
            string savefilePath = Path.Combine(savePath, "data.json");
            
            File.WriteAllText(savefilePath, jsonSavedData);
            
            savefilePath = Path.Combine(savePath, "MAPS");
            
            foreach (var map in ObjectsReference.Instance.gameData.mapSavedDatasByMapName) {
                Map mapClass = ObjectsReference.Instance.mapsManager.mapBySceneName[map.Key];
                
                // synchronize data beetween classes and templates
                map.Value.isDiscovered = mapClass.isDiscovered;
                map.Value.cleanliness = mapClass.cleanliness;
                map.Value.monkeySasiety = mapClass.monkeySasiety;
                
                var jsonMapSavedData = JsonConvert.SerializeObject(ObjectsReference.Instance.gameData.mapSavedDatasByMapName[map.Key]);
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
            
            _savedData = ObjectsReference.Instance.loadData.GetSavedDataByUuid(saveUuid);
            _savedData.saveName = saveName;
            var jsonSavedData = JsonConvert.SerializeObject(_savedData);
            
            string savefilePath = Path.Combine(savePath, "data.json");

            File.WriteAllText(savefilePath, jsonSavedData);
        }

        void SaveCameraView(string path) {
            var screenshotCamera = ObjectsReference.Instance.gameManager.cameraMain;
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

        public void SaveDataCBOR(
            string mapName,  
            List<Vector3> aspirablesPositions, 
            List<Quaternion> aspirablesRotations,
            string saveUuid,
            
            List<ItemCategory> aspirablesCategories,
            [NotNull] List<int> debrisPrefabsIndex,
            [NotNull] List<BuildableType> buildableTypes,
            [NotNull] List<ItemType> itemTypes
        ) {

            var savePath = Path.Combine(_savesPath, saveUuid);
            var mapDataSavesPath = Path.Combine(savePath, "MAPDATA");

            _buildablesDatas = new string[aspirablesCategories.Count];
            
            for (var i = 0; i < _buildablesDatas.Length; i++) {
                _buildablesDatas[i] = aspirablesPositions[i] + "/" +
                                      aspirablesRotations[i] + "/" +
                                      aspirablesCategories[i] + "/" +
                                      debrisPrefabsIndex[i] + "/" +
                                      buildableTypes[i] + "/" +
                                      itemTypes[i];
            }

            var cbor = CBORObject.NewArray();
            
            cbor.Add(mapName, _buildablesDatas);

            using var stream = new FileStream(mapDataSavesPath, FileMode.Create);
            cbor.WriteTo(stream);
        }
    }
}
