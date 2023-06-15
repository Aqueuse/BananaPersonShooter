using UnityEngine;

namespace Input.UIActions {
    public class UILoadActions : MonoBehaviour {
        private bool isDeleting;
        
        private void Update() {
            Escape();

            if (ObjectsReference.Instance.uiSave.selectedSaveSlot == null) return;
            Save();
            Load();
            Rename();

            Delete();

            if (isDeleting) {
                ConfirmDelete();
                CancelDelete();
            }
        }

        private static void Save() {
            if (UnityEngine.Input.GetKeyDown(KeyCode.JoystickButton0)) { // (A)
                ObjectsReference.Instance.uiSave.selectedSaveSlot.Save();
            }
        }

        private static void Load() {
            if (UnityEngine.Input.GetKeyDown(KeyCode.JoystickButton2)) { // (X)
                ObjectsReference.Instance.uiSave.selectedSaveSlot.Load();
            }
        }
        
        private void Delete() {
            if (UnityEngine.Input.GetKeyDown(KeyCode.JoystickButton3)) { // (Y)
                isDeleting = true;
                ObjectsReference.Instance.uiSave.selectedSaveSlot.ShowDeleteConfirmationButtons();
            }
        }

        private static void Rename() {
            if (UnityEngine.Input.GetKeyDown(KeyCode.JoystickButton6)) { // (select)
                ObjectsReference.Instance.uiSave.selectedSaveSlot.Rename();
            }
        }

        private void ConfirmDelete() {
            if (UnityEngine.Input.GetAxis("DpadHorizontal") < 0) {
                isDeleting = false;
            
                ObjectsReference.Instance.uiSave.selectedSaveSlot.Delete();
                ObjectsReference.Instance.uiSave.selectedSaveSlot = null;
            }
        }

        private void CancelDelete() {
            if (UnityEngine.Input.GetAxis("DpadHorizontal") > 0) {
                isDeleting = false;
                ObjectsReference.Instance.uiSave.selectedSaveSlot.HideDeleteConfirmationButtons();
            }
        }

        private static void Escape() {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Escape) || UnityEngine.Input.GetKeyDown(KeyCode.JoystickButton1)) {
                ObjectsReference.Instance.uiManager.Hide_menus();
            }
        }
    }
}
