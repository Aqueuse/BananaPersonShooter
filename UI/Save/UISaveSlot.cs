using TMPro;
using UnityEngine;

namespace UI.Save {
    public class UISaveSlot : MonoBehaviour {
        public TextMeshProUGUI saveName;

        public string saveUuid = "";
        public string savedDate;

        public void Click() {
            ObjectsReference.Instance.uiSave.selectedSaveSlot = this;
            
            ObjectsReference.Instance.uiSave.UpdateThumbail(savedDate);
            
            ObjectsReference.Instance.uiSave.ShowOptions(saveUuid == "autosave");
        }

        public void Unselect() {
//            ObjectsReference.Instance.uiSave.selectedSaveSlot = null;

            ObjectsReference.Instance.uiSave.HideThumbnail();
            
            ObjectsReference.Instance.uiSave.HideOptions();
        }
    }
}
