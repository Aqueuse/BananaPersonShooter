using UnityEngine;

namespace Input.UIActions {
    public class UIStatisticsActions : MonoBehaviour {
        void Update() {
            Close();
        }

        // TODO add teleportation method and keymap
        
        private void Close() {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Escape) || UnityEngine.Input.GetKeyDown(KeyCode.I) || UnityEngine.Input.GetAxis("DpadHorizontal") > 0) {
                ObjectsReference.Instance.uiManager.Show_Hide_interface();
            }
        }
    }
}
