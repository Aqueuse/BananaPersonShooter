using UnityEngine;

namespace Input.UIActions {
    public class UIChimployeeActions : MonoBehaviour {
        
        private float _counter;
        private float _slowDownValue;

        private void Update() {
            Hide_Interface();
            
            Switch_To_Left();
            Switch_To_Right();
            
            Teleport();

            if (!ObjectsReference.Instance.uiChimployee.dialogueShown) {
                NextDialogue();
                SkipDialogue();
            }
        }

        private static void Hide_Interface() {
            if (ObjectsReference.Instance.uiChimployee.dialogueShown) {
                if (UnityEngine.Input.GetKeyDown(KeyCode.Escape) || UnityEngine.Input.GetKeyDown(KeyCode.I) || UnityEngine.Input.GetAxis("DpadHorizontal") > 0) {
                    ObjectsReference.Instance.uiManager.Show_Hide_interface();
                }
            }
        }
        
        private static void Switch_To_Left() {
            if (UnityEngine.Input.GetKeyDown(KeyCode.JoystickButton4)) {
                ObjectsReference.Instance.uihud.Switch_To_Left_Tab();
            }
        }

        private static void Switch_To_Right() {
            if (UnityEngine.Input.GetKeyDown(KeyCode.JoystickButton5)) {
                ObjectsReference.Instance.uihud.Switch_To_Right_Tab();
            }
        }
        
        private static void Teleport() {
            if (ObjectsReference.Instance.uiChimployee.TpButton.activeInHierarchy) {
                if (UnityEngine.Input.GetKeyDown(KeyCode.T) || UnityEngine.Input.GetKeyDown(KeyCode.JoystickButton3)) {
                    ObjectsReference.Instance.scenesSwitch.TeleportToCommandRoom();
                }
            }
        }

        private static void NextDialogue() {
            if (UnityEngine.Input.GetKeyDown(KeyCode.E) || UnityEngine.Input.GetKeyDown(KeyCode.JoystickButton2)) {
                ObjectsReference.Instance.uiChimployee.Next();
            }
        }

        private static void SkipDialogue() {
            if (UnityEngine.Input.GetKeyDown(KeyCode.S) || UnityEngine.Input.GetKeyDown(KeyCode.JoystickButton1)) {
                ObjectsReference.Instance.uiChimployee.FinishDialogue();
            }
        }
    }
}
