using Building;
using Cinemachine;
using Enums;
using UnityEngine;

namespace Game {
    public class GameManager : MonoBehaviour {
        [SerializeField] public Camera cameraMain;
        [SerializeField] private CinemachineFreeLook playerCamera;

        public GameObject loadingScreen;
    
        public bool isGamePlaying;
        
        public GameContext gameContext;
        
        private void Start() {
            isGamePlaying = false;
            gameContext = GameContext.IN_HOME;
            
            ObjectsReference.Instance.gameSettings.LoadSettings(); 
            ObjectsReference.Instance.audioManager.PlayMusic(MusicType.HOME, 0);

            ObjectsReference.Instance.inputManager.uiSchemaContext = UISchemaSwitchType.HOME_MENU;
            ObjectsReference.Instance.inputManager.SwitchContext(InputContext.UI);
        }
        
        public static void Prepare_New_Game() {
            ObjectsReference.Instance.gameManager.Play(null, true);
        }

        public void Play(string saveUuid, bool isNewGame) {
            ObjectsReference.Instance.uiManager.Hide_home_menu();
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            loadingScreen.SetActive(true);
            
            cameraMain.clearFlags = CameraClearFlags.Skybox;
            playerCamera.Priority = 10;
            
            if (isNewGame) {
                ObjectsReference.Instance.scenesSwitch.SwitchScene(
                    "COMMANDROOM", 
                    SpawnPoint.COMMAND_ROOM_TELEPORTATION,
                    true, true);
            }
            else {
                ObjectsReference.Instance.gameLoad.LoadGameData(saveUuid);

                ObjectsReference.Instance.scenesSwitch.SwitchScene(
                    ObjectsReference.Instance.gameData.bananaManSavedData.lastMap, 
                    SpawnPoint.LAST_MAP,
                    false, false);
            }
        }

        public void PauseGame(bool pause) {
            if (pause) {
                ObjectsReference.Instance.inputManager.SwitchContext(InputContext.UI);

                ObjectsReference.Instance.mainCamera.Set0Sensibility();

                if (gameContext == GameContext.IN_GAME && ObjectsReference.Instance.mapsManager.currentMap.activeMonkeyType != MonkeyType.NONE) {
                    foreach (var monkey in MapItems.Instance.monkeys) {
                        monkey.PauseMonkey();
                    }
                }
                
                isGamePlaying = false;
            }

            if (!pause) {
                ObjectsReference.Instance.inputManager.SwitchContext(InputContext.GAME);
                
                ObjectsReference.Instance.mainCamera.SetNormalSensibility();

                if (ObjectsReference.Instance.mapsManager.currentMap.activeMonkeyType != MonkeyType.NONE) {
                    foreach (var monkey in MapItems.Instance.monkeys) {
                        monkey.UnpauseMonkey();
                    }
                }

                isGamePlaying = true;
            }
        }

        public void Quit() {
            ObjectsReference.Instance.gameSettings.prefs.Save();
            Application.Quit();
        }
    }
}
