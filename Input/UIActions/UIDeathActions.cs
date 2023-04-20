using UnityEngine;

namespace Input.UIActions {
    public class UIDeathActions : MonoBehaviour {
        void Update() {
            PlayAgain();
        }
        
        private void PlayAgain() {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Return) || UnityEngine.Input.GetKeyDown(KeyCode.JoystickButton2)) {
                if (ObjectsReference.Instance.gameData.currentSaveUuid == null) ObjectsReference.Instance.scenesSwitch.ReturnHome();
                else {
                    ObjectsReference.Instance.gameLoad.LoadLastSave();
                }
            }

            if (UnityEngine.Input.GetKeyDown(KeyCode.Escape) || UnityEngine.Input.GetKeyDown(KeyCode.JoystickButton1)) {
                ObjectsReference.Instance.scenesSwitch.ReturnHome();
            }
        }
    }
}
