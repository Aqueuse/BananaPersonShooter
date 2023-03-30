using Building;
using UI;
using UnityEngine;

namespace Input.UIActions {
    public class UIBuildStationActions : MonoSingleton<UIBuildStationActions> {
        public BuildStation activeBuildStation;
        
        void Update() {
            Print();
            Close();
        }

        private void Print() {
            if (UnityEngine.Input.GetKeyDown(KeyCode.E) || UnityEngine.Input.GetKeyDown(KeyCode.JoystickButton2)) {
                activeBuildStation.Print();
            }
        }

        private void Close() {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Escape) || UnityEngine.Input.GetKeyDown(KeyCode.JoystickButton1)) {
                UIManager.Instance.uiBuildStation.HideBuildStationInterface();
            }
        }

    }
}
