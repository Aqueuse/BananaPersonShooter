using UnityEngine;

namespace UI.InGame {
    public class UIDeath : MonoBehaviour {
        private static void PlayAgain() {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Return) || UnityEngine.Input.GetKeyDown(KeyCode.JoystickButton2)) {
                if (ObjectsReference.Instance.gameData.currentSaveUuid == null) ObjectsReference.Instance.gameManager.ReturnHome();
                else {
                    ObjectsReference.Instance.gameLoad.LoadLastSave();
                }
            }

            if (UnityEngine.Input.GetKeyDown(KeyCode.Escape) || UnityEngine.Input.GetKeyDown(KeyCode.JoystickButton1)) {
                ObjectsReference.Instance.gameManager.ReturnHome();
            }
        }
    }
}
