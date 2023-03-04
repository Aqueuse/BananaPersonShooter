using Building;
using Enums;
using UnityEngine;

namespace Input.UIActions {
    public class UIBuildStationActions : MonoBehaviour {
        private MiniChimpType miniChimpType;
        
        void Update() {
            Print();
            Close();
        }

        private void Print() {
            if (UnityEngine.Input.GetKeyDown(KeyCode.E) || UnityEngine.Input.GetKeyDown(KeyCode.JoystickButton2)) {
                BuildStation.Instance.Print();
            }
        }

        private void Close() {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Escape) || UnityEngine.Input.GetKeyDown(KeyCode.JoystickButton1)) {
                BuildStation.Instance.HideBuildStationInterface();
            }
        }

    }
}
