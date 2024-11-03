using UnityEngine;

namespace UI.InGame {
    public class UIDeath : MonoBehaviour {
        private static void PlayAgain() {
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.JoystickButton2)) {
                if (ObjectsReference.Instance.gameSave.currentSaveUuid == null) ObjectsReference.Instance.gameManager.ReturnHome();
                else {
                    ObjectsReference.Instance.gameSave.LoadLastSave();
                }
            }

            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton1)) {
                ObjectsReference.Instance.gameManager.ReturnHome();
            }
        }
    }
}
