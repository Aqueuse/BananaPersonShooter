using System.Collections.Generic;
using System.IO;
using System.Linq;
using InGame.Items.ItemsData;
using InGame.Items.ItemsData.Characters;
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
            var monkeymensList = JsonConvert.DeserializeObject<List<MonkeyMenSavedData>>(monkeymensDataString);

            foreach (var monkeymen in monkeymensList) {
                // si le vaisseau du touriste ou du pirate n'existe plus, on ne le respawn pas
                if (!ObjectsReference.Instance.spaceTrafficControlManager.spaceshipBehavioursByGuid.ContainsKey(monkeymen.spaceshipGuid)
                    & monkeymen.characterType != CharacterType.CHIMPLOYEE)
                    continue;
                
                var monkeyMenData = new MonkeyMenData {
                    uid = monkeymen.uid,
                    monkeyMenName = monkeymen.name,
                    characterType = monkeymen.characterType,
                    appearanceScriptableObjectIndex = monkeymen.appearanceScriptableObjectIndex,
                    pirateState = monkeymen.pirateState,
                    touristState = monkeymen.touristState,
                    destination = JsonHelper.FromStringToVector3(monkeymen.destination),
                    rawMaterialsInventory = monkeymen.rawMaterialsInventory,
                    manufacturedItemsInventory = monkeymen.manufacturedItemsInventory,
                    ingredientsInventory = monkeymen.ingredientsInventory,
                    bananasInventory = monkeymen.bananasInventory,
                    bitKongQuantity = monkeymen.bitKongQuantity,
                    spaceshipGuid = monkeymen.spaceshipGuid,
                    position = JsonHelper.FromStringToVector3(monkeymen.position),
                    rotation = JsonHelper.FromStringToQuaternion(monkeymen.rotation),
                };

                foreach (var need in monkeymen.needs) {
                    monkeyMenData.needs.Add(need);
                }
                
                var monkeymenInstance = Instantiate(
                    ObjectsReference.Instance.meshReferenceScriptableObject.monkeyMenPrefabByMonkeyMenType[monkeymen.monkeyMenType],
                    position: monkeyMenData.position,
                    rotation: monkeyMenData.rotation,
                    parent: ObjectsReference.Instance.gameSave.chimpmensContainer
                );

                monkeymenInstance.GetComponent<MonkeyMenBehaviour>().monkeyMenData = monkeyMenData;
                monkeymenInstance.GetComponent<MonkeyMenBehaviour>().Init();
            }
            
            var spaceshipBehavioursCopy = ObjectsReference.Instance.spaceTrafficControlManager.spaceshipBehavioursByGuid.ToList();
            
            foreach (var spaceshipBehaviour in spaceshipBehavioursCopy) {
                if (spaceshipBehaviour.Value.travelers.Count == 0) {
                    spaceshipBehaviour.Value.LeaveRegion();
                }
            }
            
            // refresh after eventually removing unpopulated spaceships
            ObjectsReference.Instance.uiSpaceTrafficControlPanel.RefreshCommunicationButton();
            ObjectsReference.Instance.uiSpaceTrafficControlPanel.RefreshHangarAvailability();
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
