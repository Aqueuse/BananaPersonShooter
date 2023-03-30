using Game;
using Save;
using UnityEngine;

namespace Input.UIActions {
    public class UIDeathActions : MonoBehaviour {
        void Update() {
            PlayAgain();
        }
        
        private void PlayAgain() {
            if (UnityEngine.Input.GetKeyDown(KeyCode.E) || UnityEngine.Input.GetKeyDown(KeyCode.JoystickButton2)) {
                if (GameData.Instance.currentSaveUuid == null) ScenesSwitch.Instance.ReturnHome();
                else {
                    GameLoad.Instance.LoadLastSave();
                }
            }
        }
    }
}
