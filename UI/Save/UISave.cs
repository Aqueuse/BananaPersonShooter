using System;
using System.Globalization;
using UnityEngine;

namespace UI.Save {
    public class UISave : MonoBehaviour {
        [SerializeField] private GameObject savePrefab;
        [SerializeField] private GameObject autosavePrefab;
        public GameObject newSaveButton;

        public void UnselectAll() {
            foreach (var saveSlot in GetComponentsInChildren<UISaveSlot>()) {
                saveSlot.Unselect();
            }
        }
        
        public void CreateNewSave(string saveUuid) {
            var date = DateTime.ParseExact(DateTime.Now.ToString("U"), "U", CultureInfo.CurrentCulture).ToString(CultureInfo.CurrentCulture);
            if (saveUuid.Length == 0) saveUuid = DateTime.Now.ToString("yyyyMMddHHmmss");

            ObjectsReference.Instance.gameSave.SaveGameData(saveUuid);

            var save = Instantiate(savePrefab, transform);
            save.GetComponent<UISaveSlot>().UpdateToNewSave(saveUuid, date);
        }
        
        public void AppendSaveSlot(string saveUuid) {
            if (saveUuid == "auto_save") {
                var save = Instantiate(autosavePrefab, transform);
                save.GetComponent<UIAutoSaveSlot>().saveUuid = saveUuid;
            }

            else {
                var save = Instantiate(savePrefab, transform);
                save.GetComponent<UISaveSlot>().SetToExistingSave(saveUuid);
            }
        }
    }
}
