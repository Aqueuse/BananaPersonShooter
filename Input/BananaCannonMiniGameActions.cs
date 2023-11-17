using Game.BananaCannonMiniGame;
using UnityEngine;

namespace Input {
    public class BananaCannonMiniGameActions : MonoBehaviour {
        private static void StartGame() {
            if (ObjectsReference.Instance.uIbananaCannonMiniGame.startMenuCanvasGroup.alpha > 0) {
                if (UnityEngine.Input.GetKeyDown(KeyCode.E) || UnityEngine.Input.GetKeyDown(KeyCode.JoystickButton0) ) {
                    BananaCannonMiniGameManager.Instance.PlayMiniGame();
                }
            }
        }

        private static void Teleport() {
            if (ObjectsReference.Instance.uIbananaCannonMiniGame.startMenuCanvasGroup.alpha > 0) {
                if (UnityEngine.Input.GetKeyDown(KeyCode.JoystickButton2) ) {
                    BananaCannonMiniGameManager.Instance.Teleport();
                }
            }
        }

        private static void UnpauseGame() {
            if (ObjectsReference.Instance.uIbananaCannonMiniGame.pauseMenuCanvasGroup.alpha > 0) {
                if (UnityEngine.Input.GetKeyDown(KeyCode.JoystickButton1) ) {
                    BananaCannonMiniGameManager.UnpauseMiniGame();
                }
            }
        }

        private static void QuitGame() {
            if (ObjectsReference.Instance.uIbananaCannonMiniGame.startMenuCanvasGroup.alpha > 0) {
                if (UnityEngine.Input.GetKeyDown(KeyCode.JoystickButton1) ) {
                    BananaCannonMiniGameManager.Instance.QuitMiniGame();
                }
            }
        }
        
        ////////////////////////////

        private static void PauseGame() {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Escape) || UnityEngine.Input.GetKeyDown(KeyCode.JoystickButton7) ) {
                BananaCannonMiniGameManager.PauseMiniGame();
            }
        }

        private static void RotateCannon() {
            BananaCannonMiniGameManager.Instance.MoveTarget(UnityEngine.Input.GetAxis("Horizontal"), UnityEngine.Input.GetAxis("Vertical"));
            BananaCannonMiniGameManager.Instance.MoveTarget(UnityEngine.Input.GetAxis("Mouse X"), UnityEngine.Input.GetAxis("Mouse Y"));
        }

        private void Shoot() {
            if (UnityEngine.Input.GetMouseButtonDown(0) || UnityEngine.Input.GetKeyDown(KeyCode.JoystickButton0)) {
                BananaCannonMiniGameManager.Instance.Shoot();
            }
        }
    }
}