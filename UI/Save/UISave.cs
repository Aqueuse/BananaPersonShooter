using UnityEngine;

namespace UI.Save {
    public class UISave : MonoSingleton<UISave> {
        [SerializeField] private GameObject savePrefab;
        [SerializeField] private GameObject autosavePrefab;
        public GameObject newSaveButton;
        
        
        public void UnselectAll() {
            foreach (var saveSlot in GetComponentsInChildren<UISaveSlot>()) {
                saveSlot.Unselect();
            }
        }
        
        public void CreateNewSave() {
            var save = Instantiate(savePrefab, transform);
            save.GetComponent<UISaveSlot>().UpdateToNewSave();
        }
        
        public void AppendSaveSlot(string saveUuid) {
            if (saveUuid == "auto_save") {
                var save = Instantiate(autosavePrefab, transform);
                save.GetComponent<UIAutoSaveSlot>().saveUuid = saveUuid;
                save.GetComponent<UIAutoSaveSlot>().AutoSave();
            }

            else {
                var save = Instantiate(savePrefab, transform);
                save.GetComponent<UISaveSlot>().UpdateToExistingSave(saveUuid);
            }
        }
    }
}
