using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Enums;
using Game;
using Newtonsoft.Json;
using Save.Templates;
using UI.Save;
using UnityEngine;

namespace Save {
    public class LoadData : MonoSingleton<LoadData> {
        private readonly NumberFormatInfo _americaNumberFormatInfo = new CultureInfo("en-US").NumberFormat;
        private string _sceneName;

        private string _appPath;
        private string _gamePath;
        private string _savesPath;
        private string _savePath;

        private void Start() {
            _appPath = Path.GetDirectoryName(Application.persistentDataPath);
            if (_appPath != null) {
                _gamePath = Path.Combine(_appPath, "Banana Man The Space Monkeys");
                _savesPath = Path.Combine(_gamePath, "Saves");
            }
            
            LoadSaves();
        }

        private void LoadSaves() {
            string[] saveFolders = Directory.GetDirectories(_savesPath);

            foreach (var folder in saveFolders) {
                string saveDataFile = Path.Combine(folder, "data.json");
                string playerDataFile = Path.Combine(folder, "player.json");
                
                if (File.Exists(saveDataFile) && File.Exists(playerDataFile)) {
                    SavedData savedData = JsonConvert.DeserializeObject<SavedData>(File.ReadAllText(saveDataFile));
                    
                    if (savedData.uuid != null) {
                        UISave.Instance.AppendSaveSlot(savedData.uuid);
                    }
                }
            }
        }

        public SavedData GetSavedDataByUuid(string saveUuid) {
            _savePath = Path.Combine(_savesPath, saveUuid);
            
            var savefilePath = Path.Combine(_savePath, "data.json");
            string savedDataString = File.ReadAllText(savefilePath);

            return JsonConvert.DeserializeObject<SavedData>(savedDataString);
        }

        public string GetsaveNameByUuid(string saveUuid) {
            _savePath = Path.Combine(_savesPath, saveUuid);
            
            var savefilePath = Path.Combine(_savePath, "data.json");

            if (File.Exists(savefilePath)) {
                string savedDataString = File.ReadAllText(savefilePath);
                return JsonConvert.DeserializeObject<SavedData>(savedDataString).saveName;
            }
            return "new save";
        }

        public BananaManSavedData GetPlayerDataByUuid(string saveUuid) {
            _savePath = Path.Combine(_savesPath, saveUuid);

            if (Directory.Exists(_savePath)) {
                    var savefilePath = Path.Combine(_savePath, "player.json");
            
                    string playerDataString = File.ReadAllText(savefilePath);
                    
                    return JsonConvert.DeserializeObject<BananaManSavedData>(playerDataString);
            }
            
            else {
                var date = DateTime.ParseExact(DateTime.Now.ToString("U"), "U", CultureInfo.CurrentCulture);
                SaveData.Instance.Save(saveUuid, date.ToString(CultureInfo.CurrentCulture));
                
                var savefilePath = Path.Combine(_savePath, "player.json");
            
                string playerDataString = File.ReadAllText(savefilePath);
            
                return JsonConvert.DeserializeObject<BananaManSavedData>(playerDataString);
            }
        }
        
        public MapSavedData GetMapDataByUuid(string saveUuid, string mapName) {
            _savePath = Path.Combine(_savesPath, saveUuid);
            var mapSavePath = Path.Combine(_savePath, "MAPS");

            if (Directory.Exists(_savePath)) {
                var savefilePath = Path.Combine(mapSavePath, mapName+".json");

                string mapDataString = File.ReadAllText(savefilePath);

                return JsonConvert.DeserializeObject<MapSavedData>(mapDataString);
            }

            else {
                var date = DateTime.ParseExact(DateTime.Now.ToString("U"), "U", CultureInfo.CurrentCulture);
                SaveData.Instance.Save(saveUuid, date.ToString(CultureInfo.CurrentCulture));
                
                var savefilePath = Path.Combine(_savePath, mapName+".json");

                string map01DataString = File.ReadAllText(savefilePath);

                return JsonConvert.DeserializeObject<MapSavedData>(map01DataString);
            }
        }

        public void LoadMapDebrisDataByUuid(string saveUuid) {
            _savePath = Path.Combine(_savesPath, saveUuid);
            
            var saveMapDatasPath = Path.Combine(_savePath, "MAPDATA");
            
            foreach (var map in MapsManager.Instance.mapBySceneName) {
                if (map.Value.hasDebris) {
                    var loadfilePath = Path.Combine(saveMapDatasPath, map.Key.ToUpper()+"_debris.data");
                    
                    if (File.Exists(loadfilePath)) {
                        using StreamReader streamReader = new StreamReader(loadfilePath);

                        var debrisData = new List<string>();

                        while (!streamReader.EndOfStream) {
                            debrisData.Add(streamReader.ReadLine());
                        }

                        MapsManager.Instance.mapBySceneName[map.Key].debrisPosition = new Vector3[debrisData.Count];
                        MapsManager.Instance.mapBySceneName[map.Key].debrisRotation = new Quaternion[debrisData.Count];
                        MapsManager.Instance.mapBySceneName[map.Key].debrisIndex = new int[debrisData.Count];

                        for (var i=0; i<debrisData.Count; i++) {
                            var dataSplit = debrisData[i].Split("/");
                
                            MapsManager.Instance.mapBySceneName[map.Key].debrisPosition[i] = Vector3FromString(dataSplit[0]);
                            MapsManager.Instance.mapBySceneName[map.Key].debrisRotation[i] = QuaternionFromString(dataSplit[1]);
                            MapsManager.Instance.mapBySceneName[map.Key].debrisIndex[i] = int.Parse(dataSplit[2]);
                        }
                    }
                }
            }
        }
        
        public void LoadMapPlateformsDataByUuid(string saveUuid) {
            _savePath = Path.Combine(_savesPath, saveUuid);
            
            var saveMapDatasPath = Path.Combine(_savePath, "MAPDATA");
            
            foreach (var map in MapsManager.Instance.mapBySceneName) {
                var loadfilePath = Path.Combine(saveMapDatasPath, map.Key.ToUpper()+"_plateforms.data");
                
                if (File.Exists(loadfilePath)) {
                    using StreamReader streamReader = new StreamReader(loadfilePath);

                    var plateformsData = new List<string>();

                    while (!streamReader.EndOfStream) {
                        plateformsData.Add(streamReader.ReadLine());
                    }

                    MapsManager.Instance.mapBySceneName[map.Key].plateformsPosition = new List<Vector3>();
                    MapsManager.Instance.mapBySceneName[map.Key].plateformsTypes = new List<PlateformType>();

                    if (plateformsData.Count > 0) {
                        for (var i=0; i<plateformsData.Count; i++) {
                            var dataSplit = plateformsData[i].Split("/");
                        
                            MapsManager.Instance.mapBySceneName[map.Key].plateformsPosition.Add(Vector3FromString(dataSplit[0]));
                            MapsManager.Instance.mapBySceneName[map.Key].plateformsTypes.Add((PlateformType)Enum.Parse(typeof(PlateformType), dataSplit[1]));
                        }
                    }
                }
            }
        }
        
        public string GetSavePathByUuid(string saveUuid) {
            return Path.Combine(_savesPath, saveUuid);
        }

        private Vector3 Vector3FromString(string vector3String) {
            string[] temp = vector3String.Substring(1,vector3String.Length-2).Split(',');
            
            return new Vector3(
                float.Parse(temp[0], _americaNumberFormatInfo), 
                float.Parse(temp[1], _americaNumberFormatInfo),
                float.Parse(temp[2], _americaNumberFormatInfo)
            );
        }

        private Quaternion QuaternionFromString(string quaternionString){
            string[] temp = quaternionString.Substring(1,quaternionString.Length-2).Split(',');
            return new Quaternion(
                float.Parse(temp[0], _americaNumberFormatInfo), 
                float.Parse(temp[1], _americaNumberFormatInfo),
                float.Parse(temp[2], _americaNumberFormatInfo),
                float.Parse(temp[3], _americaNumberFormatInfo)
            );
        }
    }
}
