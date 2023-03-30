using Settings;
using UI;
using UI.InGame;
using UnityEngine;

namespace Input.UIActions {
    public class UIStatisticsActions : MonoBehaviour {
        void Update() {
            ShowHideDebris();
            ShowHideBananaTrees();
            Close();
        }

        private void ShowHideDebris() {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha1) || UnityEngine.Input.GetKeyDown(KeyCode.JoystickButton2)) {
                GameSettings.Instance.isShowingDebris = !GameSettings.Instance.isShowingDebris;
                Uihud.Instance.SetDebrisCanvasVisibility(GameSettings.Instance.isShowingDebris);
            }
        }

        private void ShowHideBananaTrees() {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha2) || UnityEngine.Input.GetKeyDown(KeyCode.JoystickButton3)) {
                GameSettings.Instance.isShowingBananaTrees = !GameSettings.Instance.isShowingBananaTrees;
                Uihud.Instance.SetBananaTreeCanvasVisibility(GameSettings.Instance.isShowingBananaTrees);
            }
        }

        private void Close() {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Escape) || UnityEngine.Input.GetKeyDown(KeyCode.I) || UnityEngine.Input.GetAxis("DpadHorizontal") > 0) {
                UIManager.Instance.Show_Hide_interface();
            }
        }
    }
}
