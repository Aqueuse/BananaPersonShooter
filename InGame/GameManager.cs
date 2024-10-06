using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

namespace InGame {
    public class GameManager : MonoBehaviour {
        [SerializeField] public Camera cameraMain;
        [SerializeField] private CinemachineFreeLook playerCamera;

        public GameObject loadingScreen;
        
        [SerializeField] private GameObject startAnimations;
        [SerializeField] private List<GameObject> inGameGameObjects;
        
        public GameContext gameContext;
        
        public GenericDictionary<SpawnPoint, Transform> spawnPointsBySpawnType;
        
        private Vector3 _bananaManRotation;
        private Transform _bananaManTransform;

        private GameManager gameManager;
        
        private void Start() {
            gameContext = GameContext.IN_HOME;
            RenderSettings.ambientLight = Color.white;

            _bananaManTransform = ObjectsReference.Instance.bananaMan.transform;
            gameManager = ObjectsReference.Instance.gameManager;
        }

        public void Prepare_New_Game() {
            Play(null, true);
        }

        public void Play(string saveUuid, bool isNewGame) {
            ObjectsReference.Instance.uiManager.HideHomeMenu();
            ObjectsReference.Instance.uiManager.SetActive(UICanvasGroupType.HUD_BANANAMAN, false);

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            loadingScreen.SetActive(true);

            cameraMain.clearFlags = CameraClearFlags.Skybox;
            playerCamera.Priority = 10;
            
            ObjectsReference.Instance.uiManager.HideHomeMenu();

            // prevent banana man to fall while loading scene
            ObjectsReference.Instance.playerController.StopPlayer();
            
            if (isNewGame) {
                ObjectsReference.Instance.bananaMan.tutorialFinished = false;
                SwitchToNewGameSettings();
            }

            else {
                ObjectsReference.Instance.gameSave.LoadSave(saveUuid);

                ObjectsReference.Instance.bananaMan.tutorialFinished = true;
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
            
            if (gameContext == GameContext.IN_GAME) {
                foreach (var monkey in ObjectsReference.Instance.worldData.monkeys) {
                    monkey.PauseMonkey();
                }
            }

            gameContext = GameContext.IN_GAME_MENU;
        }

        public void UnpauseGame() {
            ObjectsReference.Instance.cameraPlayer.SetNormalSensibility();
            ObjectsReference.Instance.playerController.canMove = true;

            ObjectsReference.Instance.bananaGunActionsSwitch.SwitchToBananaGunMode(ObjectsReference.Instance.bananaMan.bananaGunMode);
            
            if (gameContext == GameContext.IN_GAME_MENU) {
                foreach (var monkey in ObjectsReference.Instance.worldData.monkeys) {
                    monkey.UnpauseMonkey();
                }
            }

            gameContext = GameContext.IN_GAME;
        }

        public void Quit() {
            Application.Quit();
        }
        
        public void ReturnHome() {
            ObjectsReference.Instance.inputManager.SwitchContext(InputContext.UI);
            ObjectsReference.Instance.bananaGunActionsSwitch.DesactiveBananaGun();

            // prevent banana man to fall while loading scene
            ObjectsReference.Instance.playerController.StopPlayer();
            
            SwitchToHomeSettings();

            gameManager.loadingScreen.SetActive(false);
        }

        private void SwitchToHomeSettings() {
            startAnimations.SetActive(true);
            
            foreach (var inGameGameObject in inGameGameObjects) {
                inGameGameObject.SetActive(false);
            }
            
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            gameManager.gameContext = GameContext.IN_HOME;

            ObjectsReference.Instance.uiManager.HideGameMenu();
            ObjectsReference.Instance.uiManager.ShowHomeMenu();

            ObjectsReference.Instance.uiManager.SetActive(UICanvasGroupType.HUD_BANANAMAN, false);
            ObjectsReference.Instance.uiManager.SetActive(UICanvasGroupType.CROSSHAIRS, false);

            ObjectsReference.Instance.cameraPlayer.Set0Sensibility();

            RenderSettings.ambientLight = Color.white;

            ObjectsReference.Instance.playerController.canMove = false;
            ObjectsReference.Instance.bananaMan.SetBananaSkinHealth();

            ObjectsReference.Instance.gameSave.currentSaveUuid = null;

            ObjectsReference.Instance.bananaMan.transform.position = spawnPointsBySpawnType[SpawnPoint.HOME].position;

            ObjectsReference.Instance.gameSave.CancelAutoSave();

            ObjectsReference.Instance.inputManager.SwitchContext(InputContext.HOME);
            ObjectsReference.Instance.keyboardUiActions.enabled = true;
        }

        private void SwitchToNewGameSettings() {
            startAnimations.SetActive(false);
            
            foreach (var inGameGameObject in inGameGameObjects) {
                inGameGameObject.SetActive(true);
            }

            ObjectsReference.Instance.gameSave.CancelAutoSave();

            ObjectsReference.Instance.teleportation.TeleportDown();

            _bananaManTransform.position = spawnPointsBySpawnType[SpawnPoint.NEW_GAME].position;
            _bananaManRotation = spawnPointsBySpawnType[SpawnPoint.NEW_GAME].rotation.eulerAngles;

            _bananaManTransform.rotation = Quaternion.Euler(_bananaManRotation);
            
            ObjectsReference.Instance.gameSave.buildablesSave.SpawnInitialBuildables();
            ObjectsReference.Instance.gameSave.spaceshipDebrisSave.SpawnInitialSpaceshipDebris();

            loadingScreen.SetActive(false);
            Init();

            ObjectsReference.Instance.tutorial.StartTutorial();
        }
        
        private void SwitchToInGameSettings() {
            startAnimations.SetActive(false);
            foreach (var inGameGameObject in inGameGameObjects) {
                inGameGameObject.SetActive(true);
            }
            
            ObjectsReference.Instance.uiManager.SetActive(UICanvasGroupType.HUD_BANANAMAN, true);
            ObjectsReference.Instance.uiManager.canvasGroupsByUICanvasType[UICanvasGroupType.CROSSHAIRS].alpha = 1f;
            ObjectsReference.Instance.uiCrosshairs.SetCrosshair(BananaType.EMPTY);

            ObjectsReference.Instance.uInventoriesManager.ShowCurrentUIHelper();

            ObjectsReference.Instance.inputManager.SwitchContext(InputContext.GAME);
            gameManager.gameContext = GameContext.IN_GAME;

            ObjectsReference.Instance.uiManager.HideGameMenu();
            ObjectsReference.Instance.uiManager.HideHomeMenu();

            ObjectsReference.Instance.cameraPlayer.Return_back_To_Player();

            ObjectsReference.Instance.audioManager.SetMusiqueAndAmbianceByRegion(RegionType.COROLLE);
            
            Init();
            loadingScreen.SetActive(false);
        }

        private void Init() {
            ObjectsReference.Instance.commandRoomControlPanelsManager.Init();
            ObjectsReference.Instance.commandRoomControlPanelsManager.assembler.Init();
        }
    }
}