using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
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
        
        public WorldSavedData GetWorldSavedByUuid(string saveUuid) {
            _savePath = Path.Combine(_savesPath, saveUuid);

            var savefilePath = Path.Combine(_savePath, "world.json");

            var worldSavedDataString = File.ReadAllText(savefilePath);

            return JsonConvert.DeserializeObject<WorldSavedData>(worldSavedDataString);
        }

        public void LoadDebrisDataFromJsonDictionnaryByUuid(string saveUuid) {
            _savePath = Path.Combine(_savesPath, saveUuid);
            var saveMapDatasPath = Path.Combine(_savePath, "WORLD_DATA");

            var loadfilePath = Path.Combine(saveMapDatasPath, "debris.json");

            if (!File.Exists(loadfilePath)) return;
            
            ObjectsReference.Instance.gameData.worldData.debrisDataDictionnaryByCharacterType.Clear();
            
            var json = File.ReadAllText(loadfilePath);

            var debrisDictionnary = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(json);

            foreach (var debrisList in debrisDictionnary) {
                CharacterType debrisType = Enum.Parse<CharacterType>(debrisList.Key);

                foreach (var debris in debrisList.Value) {
                    ObjectsReference.Instance.gameData.worldData.AddDebrisToDebrisDictionnary(debrisType, debris);
                }
            }
        }

        public void LoadBuildableDataFromJsonDictionnaryByUuid(string saveUuid) {
            _savePath = Path.Combine(_savesPath, saveUuid);
            var saveWorldDatasPath = Path.Combine(_savePath, "WORLD_DATA");

            var loadfilePath = Path.Combine(saveWorldDatasPath, "buildables.json");
            
            if (!File.Exists(loadfilePath)) return;
            
            var json = File.ReadAllText(loadfilePath);

            ObjectsReference.Instance.gameData.worldData.buildablesDataDictionaryByBuildableType.Clear();
            
            var buildablesDictionnary = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(json);
            
            foreach (var buildableList in buildablesDictionnary) {
                BuildableType buildableType = Enum.Parse<BuildableType>(buildableList.Key);

                foreach (var buildable in buildableList.Value) {
                    ObjectsReference.Instance.gameData.worldData.AddBuildableToBuildableDictionnary(buildableType, buildable);
                }
            }
        }

        public void LoadpaceshipsData(string saveUuid) {
            _savePath = Path.Combine(_savesPath, saveUuid);
            var saveWorldDatasPath = Path.Combine(_savePath, "WORLD_DATA");

            var loadfilePath = Path.Combine(saveWorldDatasPath, "spaceships.json");
            
            var spaceshipsDataString = File.ReadAllText(loadfilePath);
            var spaceshipsList = JsonConvert.DeserializeObject<List<string>>(spaceshipsDataString);
            
            foreach (var spaceship in spaceshipsList) {
                SpaceshipSavedData spaceshipSavedData = JsonConvert.DeserializeObject<SpaceshipSavedData>(spaceship);
                
                ObjectsReference.Instance.spaceTrafficControlManager.LoadSpaceshipBehaviour(spaceshipSavedData.characterType, spaceshipSavedData);
            }
        }
        
        public bool WorldDataExist(string saveUuid) {
            _savePath = Path.Combine(_savesPath, saveUuid);
            var worldSavePath = Path.Combine(_savePath, "WORLD_DATA");
            
            return File.Exists(worldSavePath);
        }

        public string GetSavePathByUuid(string saveUuid) {
            return Path.Combine(_savesPath, saveUuid);
        }

        public bool SaveExists(string saveuuid) {
            return Directory.Exists(Path.Combine(_savesPath, saveuuid));
        }
    }
}