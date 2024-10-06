using System.IO;
using InGame.Items.ItemsData;
using Newtonsoft.Json;
using Save.Helpers;
using Save.Templates;
using UnityEngine;

namespace Save {
    public class WorldSave : MonoBehaviour {
        private string _savePath;
    
        public WorldData worldData;
        
        public void LoadWorld(string saveUuid) {
            _savePath = Path.Combine(ObjectsReference.Instance.gameSave._savesPath, saveUuid);
            var savefilePath = Path.Combine(_savePath, "world.json");

            var worldSavedDataString = File.ReadAllText(savefilePath);
            var savedWorldData = JsonConvert.DeserializeObject<WorldSavedData>(worldSavedDataString);
            
            foreach (var monkeyPositionById in savedWorldData.monkeysPositionByMonkeyId) {
                foreach (var monkey in worldData.monkeys) {
                    if (monkey.monkeyPropertiesScriptableObject.monkeyId == monkeyPositionById.Key) {
                        monkey.monkeyPropertiesScriptableObject.lastPosition =
                            JsonHelper.FromStringToVector3(monkeyPositionById.Value);
                    }
                }
            }

            foreach (var monkeySasietyById in savedWorldData.monkeysSasietyTimerByMonkeyId) {
                foreach (var monkey in worldData.monkeys) {
                    if (monkey.monkeyPropertiesScriptableObject.monkeyId == monkeySasietyById.Key) {
                        monkey.monkeyPropertiesScriptableObject.sasietyTimer = monkeySasietyById.Value;
                        monkey.sasietyTimer = monkeySasietyById.Value;
                    }
                }
            }
        }
    
        public void SaveWorld(string saveUuid) {
            _savePath = Path.Combine(ObjectsReference.Instance.gameSave._savesPath, saveUuid);
            var savefilePath = Path.Combine(_savePath, "world.json");

            var worldSavedData = new WorldSavedData();

            foreach (var monkey in ObjectsReference.Instance.worldData.monkeys) {
                worldSavedData.monkeysPositionByMonkeyId.Add(monkey.monkeyId, JsonHelper.FromVector3ToString(monkey.transform.position));
                worldSavedData.monkeysSasietyTimerByMonkeyId.Add(monkey.monkeyId, (int)monkey.sasietyTimer);
            }

            var jsonMapSaved = JsonConvert.SerializeObject(worldSavedData);

            File.WriteAllText(savefilePath, jsonMapSaved);
        }
    }
}