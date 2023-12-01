using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Game;
using Newtonsoft.Json;
using Save.Templates;
using UnityEngine;

namespace Save {
    public class LoadData : MonoBehaviour {
        private readonly NumberFormatInfo _americaNumberFormatInfo = new CultureInfo("en-US").NumberFormat;
        private string _sceneName;

        private string _appPath;
        private string _gamePath;
        private string _savesPath;
        private string _savePath;

        private Map mapToLoad;

        private void Start() {
            _appPath = Path.GetDirectoryName(Application.persistentDataPath);
            if (_appPath != null) {
                _gamePath = Path.Combine(_appPath, "Banana Man The Space Monkeys");
                _savesPath = Path.Combine(_gamePath, "Saves");
            }
            
            LoadSaves();
        }

        private void LoadSaves() {
            var saveFolders = Directory.GetDirectories(_savesPath);

            foreach (var folder in saveFolders) {
                var saveDataFile = Path.Combine(folder, "data.json");
                var playerDataFile = Path.Combine(folder, "player.json");
                
                if (File.Exists(saveDataFile) && File.Exists(playerDataFile)) {
                    var savedData = JsonConvert.DeserializeObject<SavedData>(File.ReadAllText(saveDataFile));
                    
                    if (savedData.uuid != null) {
                        ObjectsReference.Instance.uiSave.AppendSaveSlot(savedData.uuid);
                    }
                }
            }
        }

        public SavedData GetSavedDataByUuid(string saveUuid) {
            _savePath = Path.Combine(_savesPath, saveUuid);
            
            var savefilePath = Path.Combine(_savePath, "data.json");
            var savedDataString = File.ReadAllText(savefilePath);

            return JsonConvert.DeserializeObject<SavedData>(savedDataString);
        }

        public string GetsaveNameByUuid(string saveUuid) {
            _savePath = Path.Combine(_savesPath, saveUuid);
            
            var savefilePath = Path.Combine(_savePath, "data.json");

            if (File.Exists(savefilePath)) {
                var savedDataString = File.ReadAllText(savefilePath);
                return JsonConvert.DeserializeObject<SavedData>(savedDataString).saveName;
            }
            return "new save";
        }

        public BananaManSavedData GetPlayerDataByUuid(string saveUuid) {
            _savePath = Path.Combine(_savesPath, saveUuid);

            if (Directory.Exists(_savePath)) {
                    var savefilePath = Path.Combine(_savePath, "player.json");
            
                    var playerDataString = File.ReadAllText(savefilePath);
                    
                    return JsonConvert.DeserializeObject<BananaManSavedData>(playerDataString);
            }
            
            else {
                var date = DateTime.ParseExact(DateTime.Now.ToString("U"), "U", CultureInfo.CurrentCulture);
                ObjectsReference.Instance.saveData.Save(saveUuid, date.ToString(CultureInfo.CurrentCulture));
                
                var savefilePath = Path.Combine(_savePath, "player.json");
            
                var playerDataString = File.ReadAllText(savefilePath);
            
                return JsonConvert.DeserializeObject<BananaManSavedData>(playerDataString);
            }
        }
        
        public MapSavedData GetMapDataByUuid(string saveUuid, string mapName) {
            _savePath = Path.Combine(_savesPath, saveUuid);
            var mapSavePath = Path.Combine(_savePath, "MAPS");

            if (Directory.Exists(_savePath)) {
                var savefilePath = Path.Combine(mapSavePath, mapName+".json");

                var mapDataString = File.ReadAllText(savefilePath);

                return JsonConvert.DeserializeObject<MapSavedData>(mapDataString);
            }

            else {
                var date = DateTime.ParseExact(DateTime.Now.ToString("U"), "U", CultureInfo.CurrentCulture);
                ObjectsReference.Instance.saveData.Save(saveUuid, date.ToString(CultureInfo.CurrentCulture));

                var savefilePath = Path.Combine(_savePath, mapName+".json");

                var map01DataString = File.ReadAllText(savefilePath);

                return JsonConvert.DeserializeObject<MapSavedData>(map01DataString);
            }
        }

        public void LoadMapBuildablesDataByUuid(string saveUuid) {
            _savePath = Path.Combine(_savesPath, saveUuid);
            
            var saveMapDatasPath = Path.Combine(_savePath, "MAPDATA");
            
            foreach (var map in ObjectsReference.Instance.mapsManager.mapBySceneName) {
                var loadfilePath = Path.Combine(saveMapDatasPath, map.Key.ToUpper()+"_buildables.data");
                
                if (File.Exists(loadfilePath)) {
                    using var streamReader = new StreamReader(loadfilePath);

                    var buildablesData = new List<string>();

                    while (!streamReader.EndOfStream) {
                        buildablesData.Add(streamReader.ReadLine());
                    }

                    var buildablesQuantity = buildablesData.Count;
                    
                    if (buildablesQuantity > 0) {
                        mapToLoad = ObjectsReference.Instance.mapsManager.mapBySceneName[map.Key];
                        
                        mapToLoad.itemsPositions = new List<Vector3>();
                        mapToLoad.itemsRotations = new List<Quaternion>();

                        mapToLoad.itemsCategories = new List<ItemCategory>();
                        mapToLoad.itemsBuildableTypes = new List<BuildableType>();
                        mapToLoad.itemBananaTypes = new List<BananaType>();

                        foreach (var buidable in buildablesData) {
                            var dataSplit = buidable.Split("/");
                    
                            mapToLoad.itemsPositions.Add(Vector3FromString(dataSplit[0]));
                            mapToLoad.itemsRotations.Add(QuaternionFromString(dataSplit[1]));
                        
                            mapToLoad.itemsCategories.Add((ItemCategory)Enum.Parse(typeof(ItemCategory), dataSplit[2]));
                            mapToLoad.itemsBuildableTypes.Add((BuildableType)Enum.Parse(typeof(BuildableType), dataSplit[3]));
                            mapToLoad.itemBananaTypes.Add((BananaType)Enum.Parse(typeof(BananaType), dataSplit[4]));
                        }
                    }
                }
            }
        }
        
        public void LoadMapDebrisDataByUuid(string saveUuid) {
            _savePath = Path.Combine(_savesPath, saveUuid);
            
            var saveMapDatasPath = Path.Combine(_savePath, "MAPDATA");
            
            foreach (var map in ObjectsReference.Instance.mapsManager.mapBySceneName) {
                var loadfilePath = Path.Combine(saveMapDatasPath, map.Key.ToUpper()+"_debris.data");
                
                if (File.Exists(loadfilePath)) {
                    using var streamReader = new StreamReader(loadfilePath);

                    var debrisData = new List<string>();

                    while (!streamReader.EndOfStream) {
                        debrisData.Add(streamReader.ReadLine());
                    }

                    var debrisQuantity = debrisData.Count;
                    
                    if (debrisQuantity > 0) {
                        mapToLoad = ObjectsReference.Instance.mapsManager.mapBySceneName[map.Key];
                        
                        mapToLoad.debrisPositions = new List<Vector3>();
                        mapToLoad.debrisRotations = new List<Quaternion>();

                        mapToLoad.debrisTypes = new List<CharacterType>();

                        foreach (var debris in debrisData) {
                            var dataSplit = debris.Split("/");

                            mapToLoad.debrisPositions.Add(Vector3FromString(dataSplit[0]));
                            mapToLoad.debrisRotations.Add(QuaternionFromString(dataSplit[1]));
                        
                            mapToLoad.debrisTypes.Add((CharacterType)Enum.Parse(typeof(CharacterType), dataSplit[2]));
                        }
                    }
                }
            }
        }

        public string GetSavePathByUuid(string saveUuid) {
            return Path.Combine(_savesPath, saveUuid);
        }

        public bool SaveExists(string saveuuid) {
            return Directory.Exists(Path.Combine(_savesPath, saveuuid));
        }

        private Vector3 Vector3FromString(string vector3String) {
            var temp = vector3String.Substring(1,vector3String.Length-2).Split(',');
            
            return new Vector3(
                float.Parse(temp[0], _americaNumberFormatInfo), 
                float.Parse(temp[1], _americaNumberFormatInfo),
                float.Parse(temp[2], _americaNumberFormatInfo)
            );
        }

        private Quaternion QuaternionFromString(string quaternionString){
            var temp = quaternionString.Substring(1,quaternionString.Length-2).Split(',');
            return new Quaternion(
                float.Parse(temp[0], _americaNumberFormatInfo), 
                float.Parse(temp[1], _americaNumberFormatInfo),
                float.Parse(temp[2], _americaNumberFormatInfo),
                float.Parse(temp[3], _americaNumberFormatInfo)
            );
        }
    }
}
