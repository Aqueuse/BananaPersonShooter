using Enums;
using UI;
using UnityEngine;

namespace Input.UIActions {
    public class UICinematiquesActions : MonoBehaviour {
        [SerializeField] private UICinematique uiCinematique;

        private void Update() {
            Skip();
            PauseCinematique();
        }

        private void Skip() {
            if (UnityEngine.Input.GetKey(KeyCode.E) || UnityEngine.Input.GetKey(KeyCode.JoystickButton2)) {
                uiCinematique.AddToSlider();
            }
        }
        
        private static void PauseCinematique() {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Escape) || UnityEngine.Input.GetKeyDown(KeyCode.JoystickButton7)) {
                ObjectsReference.Instance.inputManager.uiSchemaContext = UISchemaSwitchType.GAME_MENU;
                
                ObjectsReference.Instance.gameManager.PauseGame(true);
                ObjectsReference.Instance.cinematiques.Pause();
                ObjectsReference.Instance.uiManager.Show_game_menu();
            }
        }
    }
}
