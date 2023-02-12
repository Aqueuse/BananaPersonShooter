using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
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
                string map01DataFile = Path.Combine(folder, "MAP01.json");
                string playerDataFile = Path.Combine(folder, "player.json");
                
                if (File.Exists(saveDataFile) && File.Exists(map01DataFile) && File.Exists(playerDataFile)) {
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

        public Sprite GetSavedThumbailByUuid(string saveUuid) {
            savePath = GetSavePathByUuid(saveUuid);
            string screenshotFilePath = Path.Combine(savePath, "screenshot.png");
            
            var bytes = File.ReadAllBytes(screenshotFilePath);
            Texture2D texture2D = new Texture2D(2, 2);
            texture2D.LoadImage(bytes);
            
            return Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), new Vector2(0.5f, 0.5f));
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
        
        public MAP01SavedData GetMap01DataByUuid(string saveUuid) {
            savePath = Path.Combine(savesPath, saveUuid);

            if (Directory.Exists(savePath)) {
                var savefilePath = Path.Combine(savePath, "MAP01.json");

                string playerDataString = File.ReadAllText(savefilePath);

                return JsonConvert.DeserializeObject<MAP01SavedData>(playerDataString);
            }

            else {
                var date = DateTime.ParseExact(DateTime.Now.ToString("U"), "U", CultureInfo.CurrentCulture);
                SaveData.Instance.Save(saveUuid, date.ToString(CultureInfo.CurrentCulture));
                
                var savefilePath = Path.Combine(savePath, "MAP01.json");

                string map01DataString = File.ReadAllText(savefilePath);

                return JsonConvert.DeserializeObject<MAP01SavedData>(map01DataString);
            }
        }
        
        public void LoadMapDataByUuid(string saveUuid) {
            savePath = Path.Combine(savesPath, saveUuid);
            
            var saveMapDatasPath = Path.Combine(savePath, "MAPDATA");
            
            foreach (var mapData in GameData.Instance.mapDatasBySceneNames) {
                if (mapData.Value.hasDebris) {
                    string savefilePath = Path.Combine(saveMapDatasPath, mapData.Key.ToUpper()+"_debris.data");

                    if (File.Exists(savefilePath)) {
                        using StreamReader streamReader = new StreamReader(savefilePath);

                        List<string> debrisData = new List<string>();

                        while (!streamReader.EndOfStream) {
                            debrisData.Add(streamReader.ReadLine());
                        }

                        GameData.Instance.mapDatasBySceneNames[mapData.Key.ToUpper()].debrisPosition = new Vector3[debrisData.Count];
                        GameData.Instance.mapDatasBySceneNames[mapData.Key.ToUpper()].debrisRotation = new Quaternion[debrisData.Count];
                        GameData.Instance.mapDatasBySceneNames[mapData.Key.ToUpper()].debrisIndex = new int[debrisData.Count];

                        for (var i=0; i<debrisData.Count; i++) {
                            var dataSplit = debrisData[i].Split("/");
                
                            GameData.Instance.mapDatasBySceneNames[mapData.Key.ToUpper()].debrisPosition[i] = Vector3FromString(dataSplit[0]);
                            GameData.Instance.mapDatasBySceneNames[mapData.Key.ToUpper()].debrisRotation[i] = QuaternionFromString(dataSplit[1]);
                            GameData.Instance.mapDatasBySceneNames[mapData.Key.ToUpper()].debrisIndex[i] = int.Parse(dataSplit[2]);
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
