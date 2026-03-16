using System;
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
            
            foreach (var bananaGoop in savedWorldData.bananaGoopCannonInventory) {
                ObjectsReference.Instance.cannonsManager.bananaGoopInventory.bananaGoopInventory[Enum.Parse<BananaEffect>(bananaGoop.Key)] = bananaGoop.Value;
            }
            
            foreach (var cannon in ObjectsReference.Instance.cannonsManager.cannonsByRegionType) {
                cannon.Value.SetRotation(
                    savedWorldData.cannonsSocleYRotation[cannon.Key.ToString()],
                    savedWorldData.cannonsXRotation[cannon.Key.ToString()]
                );
            }

            ObjectsReference.Instance.worldData.stationLightSetting = savedWorldData.stationLightSetting;
            
            ObjectsReference.Instance.uiCannons.RefreshBananaGoopsQuantity();
            ObjectsReference.Instance.cannonsManager.ActivateCannon(savedWorldData.activeCannonRegion);
        }
    
        public void SaveWorld(string saveUuid) {
            _savePath = Path.Combine(ObjectsReference.Instance.gameSave._savesPath, saveUuid);
            var savefilePath = Path.Combine(_savePath, "world.json");

            var worldSavedData = new WorldSavedData();

            foreach (var monkey in ObjectsReference.Instance.worldData.monkeys) {
                worldSavedData.monkeysPositionByMonkeyId.Add(monkey.monkeyId, JsonHelper.FromVector3ToString(monkey.transform.position));
                worldSavedData.monkeysSasietyTimerByMonkeyId.Add(monkey.monkeyId, (int)monkey.sasietyTimer);
            }

            foreach (var bananaGoop in ObjectsReference.Instance.cannonsManager.bananaGoopInventory.bananaGoopInventory) {
                worldSavedData.bananaGoopCannonInventory[bananaGoop.Key.ToString()] = bananaGoop.Value;
            }

            worldSavedData.activeCannonRegion = ObjectsReference.Instance.cannonsManager.activeCannonRegion;
            
            foreach (var cannon in ObjectsReference.Instance.cannonsManager.cannonsByRegionType) {
                (float, float) rotationsCanon = cannon.Value.GetRotation();

                worldSavedData.cannonsSocleYRotation[cannon.Key.ToString()] = rotationsCanon.Item1;
                worldSavedData.cannonsXRotation[cannon.Key.ToString()] = rotationsCanon.Item2;
            }
            
            worldSavedData.stationLightSetting = worldData.stationLightSetting;
            
            var jsonMapSaved = JsonConvert.SerializeObject(worldSavedData);

            File.WriteAllText(savefilePath, jsonMapSaved);
        }
    }
}