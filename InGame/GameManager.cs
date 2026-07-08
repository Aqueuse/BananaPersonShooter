using System.Collections.Generic;
using Cameras;
using UnityEngine;

namespace InGame {
    public class GameManager : MonoBehaviour {
        [SerializeField] private Color penumbraAmbientLightColor;

        public GameObject loadingScreen;
        
        [SerializeField] private GameObject startAnimations;
        [SerializeField] private List<GameObject> inGameGameObjects;
        [SerializeField] private GameObject bananaGun;

        public GameContext gameContext;

        public GenericDictionary<SpawnPoint, Transform> spawnPointsBySpawnType;
        
        private Vector3 _bananaManRotation;
        private Transform _bananaManTransform;
        
        private void Start() {
            gameContext = GameContext.HOME;
            RenderSettings.ambientLight = Color.white;

            _bananaManTransform = ObjectsReference.Instance.bananaMan.transform;
        }

        public void Prepare_New_Game() {
            Play(null, true);
        }

        public void Play(string saveUuid, bool isNewGame) {
            ObjectsReference.Instance.uIGlobal.HideHomeMenu();
            ObjectsReference.Instance.uiManager.SetActive(UICanvasGroupType.HUD_BANANAMAN, false);

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            loadingScreen.SetActive(true);
            
            ObjectsReference.Instance.uIGlobal.HideHomeMenu();

            // prevent banana man to fall while loading scene
            ObjectsReference.Instance.playerController.StopPlayer();
            
            if (isNewGame) {
                SwitchToNewGameSettings();
            }

            else {
                ObjectsReference.Instance.gameSave.LoadSave(saveUuid);
                SwitchToInGameSettings();
            }
            
            foreach (var inGameGameObject in inGameGameObjects) {
                inGameGameObject.SetActive(true);
            }
        }

        public void PauseGame() {
            ObjectsReference.Instance.cameraPlayer.Set0Sensibility();
            ObjectsReference.Instance.playerController.canMove = false;
            
            ObjectsReference.Instance.bananaGunActionsSwitch.DesactiveBananaGun();
            
            if (gameContext == GameContext.BANANAMAN_CONTROL) {
                foreach (var monkey in ObjectsReference.Instance.worldData.monkeys) {
                    monkey.PauseMonkey();
                }
            }

            gameContext = GameContext.GAME_MENU;
        }

        public void UnpauseGame() {
            ObjectsReference.Instance.cameraPlayer.SetNormalSensibility();
            ObjectsReference.Instance.playerController.canMove = true;

            ObjectsReference.Instance.bananaGunActionsSwitch.SwitchToBananaGunMode(ObjectsReference.Instance.bananaMan.bananaGunMode);
            
            if (gameContext == GameContext.GAME_MENU) {
                foreach (var monkey in ObjectsReference.Instance.worldData.monkeys) {
                    monkey.UnpauseMonkey();
                }
            }

            gameContext = GameContext.BANANAMAN_CONTROL;
        }

        public void Quit() {
            Application.Quit();
        }
        
        public void ReturnHome() {
            ObjectsReference.Instance.inputManager.SwitchContext(InputContext.UI);
            
            // prevent banana man to fall while loading scene
            ObjectsReference.Instance.playerController.StopPlayer();

            SwitchToHomeSettings();

            loadingScreen.SetActive(false);
        }

        private void SwitchToHomeSettings() {
            ObjectsReference.Instance.bananaMan.SetToNotPlayable();

            startAnimations.SetActive(true);
            
            foreach (var inGameGameObject in inGameGameObjects) {
                inGameGameObject.SetActive(false);
            }
            
            gameContext = GameContext.HOME;

            ObjectsReference.Instance.uIGameMenu.HideGameMenu();
            ObjectsReference.Instance.uIGlobal.ShowHomeMenu();

            ObjectsReference.Instance.uiManager.SetActive(UICanvasGroupType.HUD_BANANAMAN, false);
            ObjectsReference.Instance.uICrosshairs.HideCrosshairs();
            
            RenderSettings.ambientLight = Color.white;

            ObjectsReference.Instance.bananaMan.SetBananaSkinHealth();
            
            ObjectsReference.Instance.bananaMan.transform.position = spawnPointsBySpawnType[SpawnPoint.HOME].position;

            ObjectsReference.Instance.gameSave.CancelAutoSave();

            ObjectsReference.Instance.inputManager.SwitchContext(InputContext.HOME);
            
            ObjectsReference.Instance.camerasManager.Switch_To_Camera_View(CameraModeType.HOME_VIEW);
        }

        private void SwitchToNewGameSettings() {
            startAnimations.SetActive(false);
            
            foreach (var inGameGameObject in inGameGameObjects) {
                inGameGameObject.SetActive(true);
            }

            ObjectsReference.Instance.gameSave.CancelAutoSave();
            
            _bananaManTransform.position = spawnPointsBySpawnType[SpawnPoint.NEW_GAME].position;
            _bananaManRotation = spawnPointsBySpawnType[SpawnPoint.NEW_GAME].rotation.eulerAngles;

            _bananaManTransform.rotation = Quaternion.Euler(_bananaManRotation);
            
            ObjectsReference.Instance.gameSave.spaceshipDebrisSave.SpawnInitialSpaceshipDebris();
            
            loadingScreen.SetActive(false);

            ObjectsReference.Instance.bananaGun.UngrabBananaGun();

            ObjectsReference.Instance.uICrosshairs.ShowCrosshairs();

            ObjectsReference.Instance.uInventoriesManager.HideUIHelpers();
            ObjectsReference.Instance.uiManager.SetActive(UICanvasGroupType.HUD_BANANAMAN, false);

            ObjectsReference.Instance.uiManager.SetActive(UICanvasGroupType.BANANAGUN_HELPER_KEYBOARD, false);
            ObjectsReference.Instance.uiManager.SetActive(UICanvasGroupType.BANANAGUN_HELPER_GAMEPAD, false);
            ObjectsReference.Instance.bananaGunActionsSwitch.enabled = false;
            
            ObjectsReference.Instance.gameReset.ResetGameData();

            ObjectsReference.Instance.inputManager.SwitchContext(InputContext.UI);

            ObjectsReference.Instance.cinematiques.Play(CinematiqueType.NEW_GAME);
            
            ObjectsReference.Instance.commandRoomControlPanelsManager.chimployeeCommandRoom.SetTutorialChimployeeConfiguration();

            bananaGun.SetActive(true);
            
            SwitchToNightLightSetting();
            
            ObjectsReference.Instance.camerasManager.Switch_To_Camera_View(CameraModeType.PLAYER_VIEW);
        }
        
        private void SwitchToInGameSettings() {
            startAnimations.SetActive(false);
            foreach (var inGameGameObject in inGameGameObjects) {
                inGameGameObject.SetActive(true);
            }
            
            ObjectsReference.Instance.uiManager.SetActive(UICanvasGroupType.HUD_BANANAMAN, true);
            ObjectsReference.Instance.uICrosshairs.ShowCrosshairs();

            ObjectsReference.Instance.uInventoriesManager.ShowCurrentUIHelper();

            ObjectsReference.Instance.inputManager.SwitchContext(InputContext.GAME);
            gameContext = GameContext.BANANAMAN_CONTROL;

            ObjectsReference.Instance.uIGameMenu.HideGameMenu();
            ObjectsReference.Instance.uIGlobal.HideHomeMenu();
            
            ObjectsReference.Instance.audioManager.SetMusiqueAndAmbianceByRegion(RegionType.COROLLE);
            
            ObjectsReference.Instance.bananaGun.GrabBananaGun();
            
            ObjectsReference.Instance.uiGestionPanel.SwitchLight(ObjectsReference.Instance.worldData.stationLightSetting == 1 ? 1 : 0);

            ObjectsReference.Instance.camerasManager.Switch_To_Camera_View(CameraModeType.PLAYER_VIEW);

            loadingScreen.SetActive(false);
        }
        
        public void SwitchToNightLightSetting() {
            RenderSettings.ambientLight = penumbraAmbientLightColor;
        }
    }
}