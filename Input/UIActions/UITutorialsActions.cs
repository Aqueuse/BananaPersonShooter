using UnityEngine;

namespace Input.UIActions {
    public class UITutorialsActions : MonoBehaviour {
        private void Update() {
            Close();
        }
        
        private static void Close() {
            if (
                UnityEngine.Input.GetKeyDown(KeyCode.Escape) || 
                UnityEngine.Input.GetKeyDown(KeyCode.H) || 
                UnityEngine.Input.GetKeyDown(KeyCode.JoystickButton1) ||
                UnityEngine.Input.GetKeyDown(KeyCode.JoystickButton6)) {
                ObjectsReference.Instance.gameManager.PauseGame(false);
                ObjectsReference.Instance.tutorialsManager.Hide_Help();
            }
        }
    }
}
