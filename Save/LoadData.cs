using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Game;
using Newtonsoft.Json;
using Save.Templates;
using UI.Save;
using UnityEngine;

namespace Save {
    public class LoadData : MonoSingleton<LoadData> {
        private readonly NumberFormatInfo americaNumberFormatInfo = new CultureInfo("en-US").NumberFormat;
        private string sceneName;

        private string appPath;
        private string gamePath;
        private string savesPath;
        private string savePath;

        private void Start() {
            appPath = Path.GetDirectoryName(Application.persistentDataPath);
            if (appPath != null) {
                gamePath = Path.Combine(appPath, "Banana Man The Space Monkeys");
                savesPath = Path.Combine(gamePath, "Saves");
            }
            
            LoadSaves();
        }

        private void LoadSaves() {
            string[] saveFolders = Directory.GetDirectories(savesPath);

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
            savePath = Path.Combine(savesPath, saveUuid);
            
            var savefilePath = Path.Combine(savePath, "data.json");
            string savedDataString = File.ReadAllText(savefilePath);

            return JsonConvert.DeserializeObject<SavedData>(savedDataString);
        }

        public string GetsaveNameByUuid(string saveUuid) {
            savePath = Path.Combine(savesPath, saveUuid);
            
            var savefilePath = Path.Combine(savePath, "data.json");

            if (File.Exists(savefilePath)) {
                string savedDataString = File.ReadAllText(savefilePath);
                return JsonConvert.DeserializeObject<SavedData>(savedDataString).saveName;
            }
            return "new save";
        }

        public BananaManSavedData GetPlayerDataByUuid(string saveUuid) {
            savePath = Path.Combine(savesPath, saveUuid);

            if (Directory.Exists(savePath)) {
                    var savefilePath = Path.Combine(savePath, "player.json");
            
                    string playerDataString = File.ReadAllText(savefilePath);
                    
                    return JsonConvert.DeserializeObject<BananaManSavedData>(playerDataString);
            }
            
            else {
                var date = DateTime.ParseExact(DateTime.Now.ToString("U"), "U", CultureInfo.CurrentCulture);
                SaveData.Instance.Save(saveUuid, date.ToString(CultureInfo.CurrentCulture));
                
                var savefilePath = Path.Combine(savePath, "player.json");
            
                string playerDataString = File.ReadAllText(savefilePath);
            
                return JsonConvert.DeserializeObject<BananaManSavedData>(playerDataString);
            }
        }
        
        public MAPSavedData GetMapDataByUuid(string saveUuid, string mapName) {
            savePath = Path.Combine(savesPath, saveUuid);
            var mapSavePath = Path.Combine(savePath, "MAPS");

            if (Directory.Exists(savePath)) {
                var savefilePath = Path.Combine(mapSavePath, mapName+".json");

                string mapDataString = File.ReadAllText(savefilePath);

                return JsonConvert.DeserializeObject<MAPSavedData>(mapDataString);
            }

            else {
                var date = DateTime.ParseExact(DateTime.Now.ToString("U"), "U", CultureInfo.CurrentCulture);
                SaveData.Instance.Save(saveUuid, date.ToString(CultureInfo.CurrentCulture));
                
                var savefilePath = Path.Combine(savePath, mapName+".json");

                string map01DataString = File.ReadAllText(savefilePath);

                return JsonConvert.DeserializeObject<MAPSavedData>(map01DataString);
            }
        }

        public void LoadMapDebrisDataByUuid(string saveUuid) {
            savePath = Path.Combine(savesPath, saveUuid);
            
            var saveMapDatasPath = Path.Combine(savePath, "MAPDATA");
            
            foreach (var map in MapsManager.Instance.mapBySceneName) {
                if (map.Value.hasDebris && map.Value.isDiscovered) {
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

        public string GetSavePathByUuid(string saveUuid) {
            return Path.Combine(savesPath, saveUuid);
        }

        private Vector3 Vector3FromString(string vector3String) {
            string[] temp = vector3String.Substring(1,vector3String.Length-2).Split(',');
            
            return new Vector3(
                float.Parse(temp[0], americaNumberFormatInfo), 
                float.Parse(temp[1], americaNumberFormatInfo),
                float.Parse(temp[2], americaNumberFormatInfo)
            );
        }

        private Quaternion QuaternionFromString(string quaternionString){
            string[] temp = quaternionString.Substring(1,quaternionString.Length-2).Split(',');
            return new Quaternion(
                float.Parse(temp[0], americaNumberFormatInfo), 
                float.Parse(temp[1], americaNumberFormatInfo),
                float.Parse(temp[2], americaNumberFormatInfo),
                float.Parse(temp[3], americaNumberFormatInfo)
            );
        }
    }
}
