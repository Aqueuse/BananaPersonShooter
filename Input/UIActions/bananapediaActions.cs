using UnityEngine;

namespace Input.UIActions {
    public class bananapediaActions : MonoBehaviour {
        private void Update() {
            Escape();
        }

        private void Escape() {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Escape) || UnityEngine.Input.GetKeyDown(KeyCode.JoystickButton1)) {
                ObjectsReference.Instance.uiManager.Hide_menus();
            }
        }
    }
}
