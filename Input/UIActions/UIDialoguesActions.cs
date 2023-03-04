using Dialogues;
using Enums;
using Game;
using UI;
using UnityEngine;

namespace Input.UIActions {
    public class UIDialoguesActions : MonoBehaviour {
        private MiniChimpType miniChimpType;
        
        void Update() {
            Validate();
            PauseGame();
        }

        private void Validate() {
            if (UnityEngine.Input.GetKeyDown(KeyCode.E) || UnityEngine.Input.GetKeyDown(KeyCode.JoystickButton2)) {
                miniChimpType = DialoguesManager.Instance.GetActiveMiniChimpType();
                
                switch (miniChimpType) {
                    case MiniChimpType.START_GAME:
                        StartInteraction.Instance.Validate();
                        break;
                    
                    default:
                        SimpleInteraction.Instance.Validate();
                        break;
                }
            }
        }
        
        private void PauseGame() {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Escape) || UnityEngine.Input.GetKeyDown(KeyCode.JoystickButton7)) {
                InputManager.Instance.uiSchemaContext = UISchemaSwitchType.GAME_MENU;
                GameManager.Instance.PauseGame(true);
                UIManager.Instance.Show_game_menu();
            }
        }

    }
}
