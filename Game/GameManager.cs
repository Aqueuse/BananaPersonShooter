using System;
using Audio;
using Building;
using Cameras;
using Cinemachine;
using Enums;
using Input;
using Player;
using Save;
using Settings;
using UI;
using UI.Save;
using UnityEngine;

namespace Game {
    public class GameManager : MonoSingleton<GameManager> {
        [SerializeField] public Camera cameraMain;
        [SerializeField] private CinemachineFreeLook playerCamera;

        public GameObject loadingScreen;
    
        public bool isGamePlaying;
        
        public GameContext gameContext;
        
        private void Start() {
            isGamePlaying = false;
            gameContext = GameContext.IN_HOME;

            BananaMan.Instance.GetComponent<RagDoll>().SetRagDoll(false);
        
            GameSettings.Instance.LoadSettings(); 
            AudioManager.Instance.PlayMusic(MusicType.HOME);

            InputManager.Instance.uiSchemaContext = UISchemaSwitchType.HOME_MENU;
            InputManager.Instance.SwitchContext(InputContext.UI);
        }
        
        public void New_Game() {
            UIManager.Instance.Hide_home_menu();
            StartScreen.Instance.enabled = false;
            
            Cinematiques.Instance.Play(CinematiqueType.NEW_GAME);
        }

        public void Start_New_Game() {
            GameData.Instance.currentSaveUuid = DateTime.Now.ToString("yyyyMMddHHmmss");

            GameReset.Instance.ResetGameData();
            
            GameSave.Instance.SaveGameData(GameData.Instance.currentSaveUuid);
            UISave.Instance.CreateNewSave(GameData.Instance.currentSaveUuid);
         
            BananaGun.Instance.bananaGunInBack.SetActive(false);
            UIManager.Instance.Set_active(UICanvasGroupType.HUD, false);
            
            Play(GameData.Instance.currentSaveUuid, true);
        }
        
        public void Play(string saveUuid, bool newGame) {
            GameLoad.Instance.LoadGameData(saveUuid);
        
            UIManager.Instance.Hide_home_menu();
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            loadingScreen.SetActive(true);
            
            cameraMain.clearFlags = CameraClearFlags.Skybox;
            playerCamera.Priority = 10;
            MainCamera.Instance.SwitchToFreeLookCamera();
            
            if (newGame) {
                ScenesSwitch.Instance.SwitchScene(
                    "COMMANDROOM", 
                    SpawnPoint.COMMAND_ROOM_TELEPORTATION,
                    true);
            }
            else {
                ScenesSwitch.Instance.SwitchScene(
                    GameData.Instance.bananaManSavedData.lastMap, 
                    SpawnPoint.LAST_MAP,
                    false);
            }
        }

        public void PauseGame(bool pause) {
            if (pause) {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                InputManager.Instance.SwitchContext(InputContext.UI);

                MainCamera.Instance.Set0Sensibility();
                
                if (MapsManager.Instance.currentMap.activeMonkeyType != MonkeyType.NONE) MapsManager.Instance.currentMap.activeMonkey.PauseMonkey();
                
                isGamePlaying = false;
            }

            if (!pause) {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                InputManager.Instance.SwitchContext(InputContext.GAME);
                
                MainCamera.Instance.SetNormalSensibility();
                
                if (MapsManager.Instance.currentMap.activeMonkeyType != MonkeyType.NONE) MapsManager.Instance.currentMap.activeMonkey.UnpauseMonkey();

                isGamePlaying = true;
            }
        }

        public void Quit() {
            GameSettings.Instance.prefs.Save();
            Application.Quit();
        }
    }
}
