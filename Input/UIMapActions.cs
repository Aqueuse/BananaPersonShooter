using Game;
using UI.InGame.MapUI;
using UnityEngine;

namespace Input {
    public class UIMapActions : MonoBehaviour {
        private void Update() {
            ShowHideDebris();
            ShowHideBananaTrees();

            Teleport();
        }

        private void ShowHideDebris() {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha1) || UnityEngine.Input.GetKeyDown(KeyCode.JoystickButton0)) {
                GetComponent<DotsPositionnement>().ShowHideDebris();
            }
        }

        private void ShowHideBananaTrees() {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha2) || UnityEngine.Input.GetKeyDown(KeyCode.JoystickButton1)) {
                GetComponent<DotsPositionnement>().ShowHideBananaTrees();
            }
        }

        private void Teleport() {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha3) || UnityEngine.Input.GetKeyDown(KeyCode.JoystickButton2)) {
                ScenesSwitch.Instance.Teleport(GetComponent<DotsPositionnement>().mapName);
            }
        }
    }
}
