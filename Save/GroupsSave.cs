using System.Collections.Generic;
using System.IO;
using InGame.Monkeys.Chimpvisitors;
using Newtonsoft.Json;
using Save.Templates;
using UnityEngine;

namespace Save {
    public class GroupsSave : MonoBehaviour {
        public static void LoadGroups(string saveUuid) {
            var _savePath = Path.Combine(ObjectsReference.Instance.gameSave._savesPath, saveUuid);
            var saveWorldDatasPath = Path.Combine(_savePath, "WORLD_DATA");

            var loadfilePath = Path.Combine(saveWorldDatasPath, "groups.json");
            
            if (!File.Exists(loadfilePath)) return;

            var groupsDataString = File.ReadAllText(loadfilePath);
            var groupSavedDatas = JsonConvert.DeserializeObject<List<GroupSavedData>>(groupsDataString);

            foreach (var groupSavedData in groupSavedDatas) {
                var group = Instantiate(ObjectsReference.Instance.meshReferenceScriptableObject.groupPrefab);
                group.GetComponent<GroupBehaviour>().LoadSaveData(groupSavedData);
            }
        }
        
        public void SaveGroups(string saveUuid) {
            var _savePath = Path.Combine(ObjectsReference.Instance.gameSave._savesPath, saveUuid);
            var worldDataSavesPath = Path.Combine(_savePath, "WORLD_DATA");

            var groupsSavedDatas = new List<GroupSavedData>();
            
            var groupsBehaviours = ObjectsReference.Instance.gameSave.savablesItemsContainer.GetComponentsInChildren<GroupBehaviour>();
            
            foreach (var groupBehaviour in groupsBehaviours) {
                groupsSavedDatas.Add(groupBehaviour.GenerateSaveData());
            }

            var json = JsonConvert.SerializeObject(groupsSavedDatas);

            var playerSavefilePath = Path.Combine(worldDataSavesPath, "groups.json");
            File.WriteAllText(playerSavefilePath, json);
        }
    }
}
