using UnityEngine;

namespace Input.UIActions {
    public class UIAudioVideoTabActions : MonoBehaviour {
        private void Update() {
            Scroll_Left_Options_Tab();
            Scroll_Right_Options_Tab();
        }

        private void Scroll_Left_Options_Tab() {
            if (UnityEngine.Input.GetKeyDown(KeyCode.JoystickButton4)) {
                if (ObjectsReference.Instance.uiManager.canvasGroupsByUICanvasType[UICanvasGroupType.OPTIONS].alpha > 0) {
                    ObjectsReference.Instance.uiOptionsMenu.Switch_to_Left_Tab();
                }
            }
        }
        
        private void Scroll_Right_Options_Tab() {
            if (UnityEngine.Input.GetKeyDown(KeyCode.JoystickButton5)) {
                if (ObjectsReference.Instance.uiManager.canvasGroupsByUICanvasType[UICanvasGroupType.OPTIONS].alpha > 0) {
                    ObjectsReference.Instance.uiOptionsMenu.Switch_to_Right_Tab();
                }
            }
        }
    }
}
