using Game.BananaCannonMiniGame;
using UnityEngine;

namespace Input.UIActions {
    public class BananaCannonMiniGameActions : MonoBehaviour {
        public bool leftTriggerActivated;

        private void Start() {
            leftTriggerActivated = false;
        }

        private void Update() {
            StartGame();
            Teleport();
            UnpauseGame();
            QuitGame();

            // in game only methods
            if (ObjectsReference.Instance.uIbananaCannonMiniGame.startMenuCanvasGroup.alpha > 0 || ObjectsReference.Instance.uIbananaCannonMiniGame.pauseMenuCanvasGroup.alpha > 0) return;
            PauseGame();
            RotateCannon();
            Shoot();
        }

        private static void StartGame() {
            if (ObjectsReference.Instance.uIbananaCannonMiniGame.startMenuCanvasGroup.alpha > 0) {
                if (UnityEngine.Input.GetKeyDown(KeyCode.JoystickButton0) ) {
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
            
            if (ObjectsReference.Instance.uIbananaCannonMiniGame.pauseMenuCanvasGroup.alpha > 0) {
                if (UnityEngine.Input.GetKeyDown(KeyCode.JoystickButton2) ) {
                    BananaCannonMiniGameManager.Instance.QuitMiniGame();
                }
            }
        }
        
        ////////////////////////////

        private static void PauseGame() {
            if (UnityEngine.Input.GetKeyDown(KeyCode.JoystickButton7) ) {
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
            if (UnityEngine.Input.GetAxis("LeftTrigger") != 0 && !leftTriggerActivated) {
                leftTriggerActivated = true;
                BananaCannonMiniGameManager.Instance.Shoot();
            }

            if (UnityEngine.Input.GetAxis("LeftTrigger") == 0 && leftTriggerActivated)  {
                leftTriggerActivated = false;
            }
        }
    }
}