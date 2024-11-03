using System.Collections.Generic;
using System.IO;
using InGame.Items.ItemsData;
using InGame.Monkeys;
using Newtonsoft.Json;
using Save.Helpers;
using Save.Templates;
using UnityEngine;

namespace Save {
    public class monkeyMensSave : MonoBehaviour {
        public static void LoadMonkeyMens(string saveUuid) {
            var _savePath = Path.Combine(ObjectsReference.Instance.gameSave._savesPath, saveUuid);
            var saveWorldDatasPath = Path.Combine(_savePath, "WORLD_DATA");

            var loadfilePath = Path.Combine(saveWorldDatasPath, "monkeymens.json");
            
            if (!File.Exists(loadfilePath)) return;

            var monkeymensDataString = File.ReadAllText(loadfilePath);
            var monkeymensList = JsonConvert.DeserializeObject<List<string>>(monkeymensDataString);

            foreach (var monkeymen in monkeymensList) {
                var monkeyMenSavedData = JsonConvert.DeserializeObject<MonkeyMenSavedData>(monkeymen);
                
                var monkeyMenData = new MonkeyMenData {
                    uid = monkeyMenSavedData.uid,
                    monkeyMenName = monkeyMenSavedData.name,
                    characterType = monkeyMenSavedData.characterType,
                    appearanceScriptableObjectIndex = monkeyMenSavedData.appearanceScriptableObjectIndex,
                    
                    isInSpaceship = monkeyMenSavedData.isInSpaceship,
                    pirateState = monkeyMenSavedData.pirateState,
                    touristState = monkeyMenSavedData.touristState
                };

                foreach (var need in monkeyMenSavedData.needs) {
                    monkeyMenData.needs.Add(need);
                }

                monkeyMenData.destination = JsonHelper.FromStringToVector3(monkeyMenSavedData.destination);
                monkeyMenData.droppedInventory = monkeyMenSavedData.droppedInventory;
                monkeyMenData.manufacturedItemsInventory = monkeyMenSavedData.manufacturedItemsInventory;
                monkeyMenData.ingredientsInventory = monkeyMenSavedData.ingredientsInventory;
                monkeyMenData.bananasInventory = monkeyMenSavedData.bananasInventory;
                monkeyMenData.bitKongQuantity = monkeyMenSavedData.bitKongQuantity;
                monkeyMenData.spaceshipGuid = monkeyMenSavedData.spaceshipGuid;
                monkeyMenData.position = JsonHelper.FromStringToVector3(monkeyMenSavedData.position);
                monkeyMenData.rotation = JsonHelper.FromStringToQuaternion(monkeyMenSavedData.rotation);

                if (monkeyMenSavedData.isInSpaceship) {
                    // just add the monkeyData to the spaceship
                    ObjectsReference.Instance.spaceTrafficControlManager.spaceshipBehavioursByGuid[monkeyMenData.spaceshipGuid].monkeyMensData.Add(monkeyMenData);
                }

                else {
                    var monkeymenInstance = Instantiate(
                        ObjectsReference.Instance.meshReferenceScriptableObject.chimpmenPrefabByChimpmenType[monkeyMenSavedData.monkeyMenType],
                        ObjectsReference.Instance.gameSave.chimpmensContainer
                    );
                    
                    monkeymenInstance.GetComponent<MonkeyMenBehaviour>().monkeyMenData = monkeyMenData;
                }
            }
        }
        
        public void SaveMonkeyMens(string saveUuid) {
            var _savePath = Path.Combine(ObjectsReference.Instance.gameSave._savesPath, saveUuid);
            var worldDataSavesPath = Path.Combine(_savePath, "WORLD_DATA");

            var monkeyMenSavedDatas = new List<MonkeyMenSavedData>();

            var monkeyMensBehaviours = FindObjectsOfType<MonkeyMenBehaviour>();
            
            foreach (var monkeyMenBehaviour in monkeyMensBehaviours) {
                monkeyMenBehaviour.GenerateSavedData();
                monkeyMenSavedDatas.Add(monkeyMenBehaviour.monkeyMenSavedData);
            }

            var json = JsonConvert.SerializeObject(monkeyMenSavedDatas);

            var playerSavefilePath = Path.Combine(worldDataSavesPath, "monkeymens.json");
            File.WriteAllText(playerSavefilePath, json);
        }
    }
}
