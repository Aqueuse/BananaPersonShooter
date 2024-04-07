using System;
using System.Globalization;
using System.IO;
using Newtonsoft.Json;
using Save.Templates;
using UnityEngine;

namespace Save {
    public class DataSave : MonoBehaviour {
        private string _savePath;
        
        public string GetsaveNameByUuid(string saveUuid) {
            _savePath = Path.Combine(ObjectsReference.Instance.gameSave._savesPath, saveUuid);

            var savefilePath = Path.Combine(_savePath, "data.json");

            if (File.Exists(savefilePath)) {
                var savedDataString = File.ReadAllText(savefilePath);
                return JsonConvert.DeserializeObject<SavedData>(savedDataString).saveName;
            }

            return "new save";
        }

        public void SetSaveName(string saveUuid, string saveName) {
            _savePath = Path.Combine(ObjectsReference.Instance.gameSave._savesPath, saveUuid);

            var _savedData = ObjectsReference.Instance.gameSave.GetSavedByUuid(saveUuid);
            _savedData.saveName = saveName;
            var jsonSavedData = JsonConvert.SerializeObject(_savedData);

            var savefilePath = Path.Combine(_savePath, "data.json");

            File.WriteAllText(savefilePath, jsonSavedData);
        }
        
        public void UpdateSaveDate(string saveUuid) {
            _savePath = Path.Combine(ObjectsReference.Instance.gameSave._savesPath, saveUuid);

            var date = DateTime.ParseExact(DateTime.Now.ToString("U"), "U", CultureInfo.CurrentCulture).ToString(CultureInfo.CurrentCulture);

            var _savedData = ObjectsReference.Instance.gameSave.GetSavedByUuid(saveUuid);
            _savedData.lastSavedDate = date;
            var jsonSavedData = JsonConvert.SerializeObject(_savedData);

            var savefilePath = Path.Combine(_savePath, "data.json");

            File.WriteAllText(savefilePath, jsonSavedData);
        }
    }
}
