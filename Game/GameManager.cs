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

        public GameObject loadingScreen;
        private CinemachineFreeLook _playerCamera;
    
        public bool isGamePlaying;
        
        public GameContext gameContext;
        
        private void Start() {
            if (cameraMain != null) {
                _playerCamera = cameraMain.GetComponentInChildren<CinemachineFreeLook>();
            }

            gameContext = GameContext.IN_HOME;

            BananaMan.Instance.GetComponent<RagDoll>().SetRagDoll(false);
        
            GameSettings.Instance.LoadSettings(); 
            AudioManager.Instance.PlayMusic(MusicType.HOME, false);

            InputManager.Instance.uiSchemaContext = UISchemaSwitchType.HOME_MENU;
            InputManager.Instance.SwitchContext(InputContext.UI);

            // isInGame = false;
            // isGamePlaying = false;
            // isOnDialogue = false;
            // isFigthing = false;
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
            UIManager.Instance.Hide_HUD();
            
            Play(GameData.Instance.currentSaveUuid, true);
        }
        
        public void Play(string saveUuid, bool newGame) {
            GameLoad.Instance.LoadGameData(saveUuid);
        
            UIManager.Instance.Hide_home_menu();
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            loadingScreen.SetActive(true);
            
            cameraMain.clearFlags = CameraClearFlags.Skybox;
            _playerCamera.Priority = 10;
            
            ScenesSwitch.Instance.SwitchScene(GameData.Instance.bananaManSavedData.lastMap, GameData.Instance.lastPositionOnMap, newGame);
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
