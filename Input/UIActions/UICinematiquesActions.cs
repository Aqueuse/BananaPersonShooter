using Enums;
using UI;
using UnityEngine;

namespace Input.UIActions {
    public class UICinematiquesActions : MonoBehaviour {
        [SerializeField] private UICinematique uiCinematique;

        private void Update() {
            Skip();
            PauseGame();
        }

        private void Skip() {
            if (UnityEngine.Input.GetKey(KeyCode.E) || UnityEngine.Input.GetKey(KeyCode.JoystickButton2)) {
                uiCinematique.AddToSlider();
            }
        }
        
        private static void PauseGame() {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Escape) || UnityEngine.Input.GetKeyDown(KeyCode.JoystickButton7)) {
                ObjectsReference.Instance.inputManager.uiSchemaContext = UISchemaSwitchType.HOME_MENU;
                ObjectsReference.Instance.gameManager.PauseGame(true);
                ObjectsReference.Instance.cinematiques.Pause();
                ObjectsReference.Instance.uiManager.Show_home_menu();
            }
        }
    }
}
