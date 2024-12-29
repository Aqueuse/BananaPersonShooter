using System;
using UnityEngine;

namespace UI.Save {
    public class UISave : MonoBehaviour {
        [SerializeField] private GameObject savePrefab;
        public GameObject newSaveButton;
        
        public void UnselectAll() {
            foreach (var saveSlot in GetComponentsInChildren<UISaveSlot>()) {
                saveSlot.Unselect();
            }
        }
        
        public void CreateNewSave() {
            var saveUuid = DateTime.Now.ToString("yyyyMMddHHmmss");
            ObjectsReference.Instance.gameSave.currentSaveUuid = saveUuid;

            var save = Instantiate(savePrefab, transform);
            save.GetComponent<UISaveSlot>().saveUuid = saveUuid;
            
            save.GetComponent<UISaveSlot>().Save();
        }
        
        public void AppendSaveSlot(string saveUuid) {
            var save = Instantiate(savePrefab, transform);
            var savedData = ObjectsReference.Instance.gameSave.GetSavedByUuid(saveUuid);

            save.GetComponent<UISaveSlot>().saveUuid = saveUuid;
            save.GetComponent<UISaveSlot>().saveDate.text = savedData.lastSavedDate;
            save.GetComponent<UISaveSlot>().saveName.text = savedData.saveName;
            
            save.GetComponent<UISaveSlot>().UpdateThumbail();
        }
    }
}
