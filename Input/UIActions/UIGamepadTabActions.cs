using UnityEngine;

namespace Input.UIActions {
    public class UIGamepadTabActions : MonoBehaviour {
        private void Update() {
            Scroll_Left_Options_Tab();
            Scroll_Right_Options_Tab();
            
            Escape();
        }

        private static void Scroll_Left_Options_Tab() {
            if (UnityEngine.Input.GetKeyDown(KeyCode.JoystickButton4)) {
                ObjectsReference.Instance.uiOptionsMenu.Switch_to_Left_Tab();
            }
        }
        
        private static void Scroll_Right_Options_Tab() {
            if (UnityEngine.Input.GetKeyDown(KeyCode.JoystickButton5)) {
                ObjectsReference.Instance.uiOptionsMenu.Switch_to_Right_Tab();
            }
        }
        
        private static void Escape() {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Escape) || UnityEngine.Input.GetKeyDown(KeyCode.JoystickButton1)) {
                ObjectsReference.Instance.uiManager.Hide_menus();
            }
        }
    }
}
