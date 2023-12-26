using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Data;
using Newtonsoft.Json;
using Save.Templates;
using UnityEngine;

namespace Save {
    public class LoadData : MonoBehaviour {
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
            var saveFolders = Directory.GetDirectories(_savesPath);

            foreach (var folder in saveFolders) {
                var saveDataFile = Path.Combine(folder, "data.json");
                var playerDataFile = Path.Combine(folder, "player.json");

                if (File.Exists(saveDataFile) && File.Exists(playerDataFile)) {
                    var savedData = JsonConvert.DeserializeObject<Saved>(File.ReadAllText(saveDataFile));

                    if (savedData.uuid != null) {
                        ObjectsReference.Instance.uiSave.AppendSaveSlot(savedData.uuid);
                    }
                }
            }
        }
        
        public Saved GetSavedByUuid(string saveUuid) {
            _savePath = Path.Combine(_savesPath, saveUuid);

            var savefilePath = Path.Combine(_savePath, "data.json");
            var savedDataString = File.ReadAllText(savefilePath);

            return JsonConvert.DeserializeObject<Saved>(savedDataString);
        }

        public string GetsaveNameByUuid(string saveUuid) {
            _savePath = Path.Combine(_savesPath, saveUuid);

            var savefilePath = Path.Combine(_savePath, "data.json");

            if (File.Exists(savefilePath)) {
                var savedDataString = File.ReadAllText(savefilePath);
                return JsonConvert.DeserializeObject<Saved>(savedDataString).saveName;
            }

            return "new save";
        }

        public BananaManSavedData GetPlayerByUuid(string saveUuid) {
            _savePath = Path.Combine(_savesPath, saveUuid);

            if (Directory.Exists(_savePath)) {
                var savefilePath = Path.Combine(_savePath, "player.json");

                var playerString = File.ReadAllText(savefilePath);

                return JsonConvert.DeserializeObject<BananaManSavedData>(playerString);
            }

            else {
                var date = DateTime.ParseExact(DateTime.Now.ToString("U"), "U", CultureInfo.CurrentCulture);
                ObjectsReference.Instance.saveData.Save(saveUuid, date.ToString(CultureInfo.CurrentCulture));

                var savefilePath = Path.Combine(_savePath, "player.json");

                var playerDataString = File.ReadAllText(savefilePath);

                return JsonConvert.DeserializeObject<BananaManSavedData>(playerDataString);
            }
        }
        
        public MapSavedData GetMapSavedByUuid(string saveUuid, SceneType mapName) {
            _savePath = Path.Combine(_savesPath, saveUuid);
            var mapsSavePath = Path.Combine(_savePath, "MAPS");

            var savefilePath = Path.Combine(mapsSavePath, mapName + ".json");

            var mapSavedDataString = File.ReadAllText(savefilePath);

            return JsonConvert.DeserializeObject<MapSavedData>(mapSavedDataString);
        }

        public void LoadDebrisDataFromJsonDictionnaryByUuid(MapData mapDataToLoad, string saveUuid) {
            _savePath = Path.Combine(_savesPath, saveUuid);

            var saveMapDatasPath = Path.Combine(_savePath, "MAPDATA");

            var loadfilePath = Path.Combine(saveMapDatasPath,
                mapDataToLoad.mapPropertiesScriptableObject.sceneName.ToString().ToLower() + "_debris.json");

            if (!File.Exists(loadfilePath)) return;
            
            mapDataToLoad.debrisDataInMapDictionnaryByCharacterType.Clear();
            
            var json = File.ReadAllText(loadfilePath);

            var debrisDictionnary = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(json);

            foreach (var debrisList in debrisDictionnary) {
                CharacterType debrisType = Enum.Parse<CharacterType>(debrisList.Key);

                foreach (var debris in debrisList.Value) {
                    mapDataToLoad.AddDebrisToDebrisDictionnary(debrisType, debris);
                }
            }
        }

        public void LoadBuildableDataFromJsonDictionnaryByUuid(MapData mapDataToLoad, string saveUuid) {
            _savePath = Path.Combine(_savesPath, saveUuid);

            var saveMapDatasPath = Path.Combine(_savePath, "MAPDATA");

            var loadfilePath = Path.Combine(saveMapDatasPath,
                mapDataToLoad.mapPropertiesScriptableObject.sceneName.ToString().ToLower() + "_buildables.json");
            
            if (!File.Exists(loadfilePath)) return;
            
            var json = File.ReadAllText(loadfilePath);

            mapDataToLoad.buildablesDataInMapDictionaryByBuildableType.Clear();
            
            var buildablesDictionnary = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(json);
            
            foreach (var buildableList in buildablesDictionnary) {
                BuildableType buildableType = Enum.Parse<BuildableType>(buildableList.Key);

                foreach (var buildable in buildableList.Value) {
                    mapDataToLoad.AddBuildableToBuildableDictionnary(buildableType, buildable);
                }
            }
        }

        public bool MapDataExist(string sceneName, string saveUuid) {
            _savePath = Path.Combine(_savesPath, saveUuid);
            var mapsSavePath = Path.Combine(_savePath, "MAPS");

            var savefilePath = Path.Combine(mapsSavePath, sceneName.ToLower() + ".json");

            return File.Exists(savefilePath);
        }

        public string GetSavePathByUuid(string saveUuid) {
            return Path.Combine(_savesPath, saveUuid);
        }

        public bool SaveExists(string saveuuid) {
            return Directory.Exists(Path.Combine(_savesPath, saveuuid));
        }
    }
}