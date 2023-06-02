using System;
using Building;
using Cinemachine;
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
            ObjectsReference.Instance.audioManager.PlayMusic(MusicType.HOME);

            ObjectsReference.Instance.inputManager.uiSchemaContext = UISchemaSwitchType.HOME_MENU;
            ObjectsReference.Instance.inputManager.SwitchContext(InputContext.UI);
        }
        
        public void New_Game() {
            ObjectsReference.Instance.uiManager.Hide_home_menu();
            GameObject.FindWithTag("startAnimations").GetComponent<StartAnimations>().enabled = false;
            
            ObjectsReference.Instance.cinematiques.Play(CinematiqueType.NEW_GAME);
        }

        public void Start_New_Game() {
            ObjectsReference.Instance.gameData.currentSaveUuid = DateTime.Now.ToString("yyyyMMddHHmmss");

            ObjectsReference.Instance.gameReset.ResetGameData();
            
            ObjectsReference.Instance.gameSave.SaveGameData(ObjectsReference.Instance.gameData.currentSaveUuid);
            ObjectsReference.Instance.uiSave.CreateNewSave(ObjectsReference.Instance.gameData.currentSaveUuid);
         
            ObjectsReference.Instance.bananaGun.bananaGunInBack.SetActive(false);
            ObjectsReference.Instance.uiManager.Set_active(UICanvasGroupType.HUD, false);
            
            Play(ObjectsReference.Instance.gameData.currentSaveUuid, true);
        }
        
        public void Play(string saveUuid, bool newGame) {
            ObjectsReference.Instance.gameLoad.LoadGameData(saveUuid);
        
            ObjectsReference.Instance.uiManager.Hide_home_menu();
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            loadingScreen.SetActive(true);
            
            cameraMain.clearFlags = CameraClearFlags.Skybox;
            playerCamera.Priority = 10;
            
            if (newGame) {
                ObjectsReference.Instance.scenesSwitch.SwitchScene(
                    "COMMANDROOM", 
                    SpawnPoint.COMMAND_ROOM_TELEPORTATION,
                    true);
            }
            else {
                ObjectsReference.Instance.scenesSwitch.SwitchScene(
                    ObjectsReference.Instance.gameData.bananaManSavedData.lastMap, 
                    SpawnPoint.LAST_MAP,
                    false);
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
