using Enums;
using Game;
using UI;
using UnityEngine;

namespace Input.UIActions {
    public class UICinematiquesActions : MonoBehaviour {
        [SerializeField] private UICinematique uiCinematique;
        
        void Update() {
            Skip();
            PauseGame();
        }

        private void Skip() {
            if (UnityEngine.Input.GetKey(KeyCode.E) || UnityEngine.Input.GetKey(KeyCode.JoystickButton2)) {
                uiCinematique.AddToSlider();
            }
        }
        
        private void PauseGame() {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Escape) || UnityEngine.Input.GetKeyDown(KeyCode.JoystickButton7)) {
                InputManager.Instance.uiSchemaContext = UISchemaSwitchType.HOME_MENU;
                GameManager.Instance.PauseGame(true);
                Cinematiques.Instance.Pause();
                UIManager.Instance.Show_home_menu();
            }
        }
    }
}
