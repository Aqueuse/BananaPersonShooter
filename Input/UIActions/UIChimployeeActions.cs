using UnityEngine;

namespace Input.UIActions {
    public class UIChimployeeActions : MonoBehaviour {
        
        private float _counter;
        private float _slowDownValue;

        private void Update() {
            Hide_Interface();
            
            Switch_To_Left();
            Switch_To_Right();
            
            Switch_To_Inventory();
            Switch_To_Blueprints();
            
            Teleport();

            if (!ObjectsReference.Instance.chimployee.dialogueShown) {
                SkipDialogue();
            }
        }

        private static void Hide_Interface() {
            if (ObjectsReference.Instance.chimployee.dialogueShown) {
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
        
        private void Switch_To_Inventory() {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha1)) {
                ObjectsReference.Instance.uihud.Switch_To_Inventory();
            }
        }

        private void Switch_To_Blueprints() {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha2)) {
                ObjectsReference.Instance.uihud.Switch_To_Blueprints();
            }
        }
        
        private static void Teleport() {
            if (ObjectsReference.Instance.chimployee.TpButton.activeInHierarchy) {
                if (UnityEngine.Input.GetKeyDown(KeyCode.JoystickButton3)) {
                    ObjectsReference.Instance.scenesSwitch.TeleportToCommandRoom();
                }
            }
        }

        private static void SkipDialogue() {
            if (UnityEngine.Input.GetKeyDown(KeyCode.JoystickButton1)) {
                ObjectsReference.Instance.chimployee.FinishDialogue();
            }
        }
    }
}
