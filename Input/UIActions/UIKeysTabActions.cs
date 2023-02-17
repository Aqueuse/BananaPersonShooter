using UI;
using UI.Menus;
using UnityEngine;

namespace Input.UIActions {
    public class UIKeysTabActions : MonoBehaviour {
        private void Update() {
            Scroll_Left_Options_Tab();
            Scroll_Right_Options_Tab();
        }

        private void Scroll_Left_Options_Tab() {
            if (UnityEngine.Input.GetKeyDown(KeyCode.JoystickButton4)) {
                if (UIManager.Instance.optionsMenuCanvasGroup.alpha > 0) {
                    UIOptionsMenu.Instance.Switch_to_Left_Tab();
                }
            }
        }
        
        private void Scroll_Right_Options_Tab() {
            if (UnityEngine.Input.GetKeyDown(KeyCode.JoystickButton5)) {
                if (UIManager.Instance.optionsMenuCanvasGroup.alpha > 0) {
                    UIOptionsMenu.Instance.Switch_to_Right_Tab();
                }
            }
        }
    }
}