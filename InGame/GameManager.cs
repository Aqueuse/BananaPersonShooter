using Cinemachine;
using InGame.Items.ItemsData;
using UnityEngine;

namespace InGame {
    public class GameManager : MonoBehaviour {
        [SerializeField] public Camera cameraMain;
        [SerializeField] private CinemachineFreeLook playerCamera;

        public GameObject loadingScreen;
        [SerializeField] private GameObject startAnimations;

        public bool isGamePlaying;

        public GameContext gameContext;


        public GenericDictionary<SpawnPoint, Transform> spawnPointsBySpawnType;
        
        private Vector3 _bananaManRotation;
        private Transform _bananaManTransform;

        private GameManager gameManager;
        
        private void Start() {
            isGamePlaying = false;
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
            ObjectsReference.Instance.uiManager.SetActive(UICanvasGroupType.HUD, false);

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            loadingScreen.SetActive(true);

            cameraMain.clearFlags = CameraClearFlags.Skybox;
            playerCamera.Priority = 10;
            
            ObjectsReference.Instance.uiManager.HideHomeMenu();

            // prevent banana man to fall while loading scene
            ObjectsReference.Instance.playerController.StopPlayer();

            if (gameManager.gameContext != GameContext.IN_HOME) {
                World.Instance.SaveAspirablesOnWorld();
                World.Instance.SaveMapMonkeysData();
            }

            ObjectsReference.Instance.gameData.worldData = ObjectsReference.Instance.gameData.worldData;
            
            if (isNewGame) {
                ObjectsReference.Instance.bananaMan.tutorialFinished = false;
                SwitchToNewGameSettings();
            }

            else {
                ObjectsReference.Instance.gameLoad.LoadGameData(saveUuid);

                ObjectsReference.Instance.bananaMan.tutorialFinished = true;
                SwitchToInGameSettings();
            }
        }

        public void PauseGame() {
            ObjectsReference.Instance.cameraPlayer.Set0Sensibility();
            ObjectsReference.Instance.playerController.canMove = false;

            if (gameContext == GameContext.IN_GAME && World.Instance.monkeysInMap != null) {
                foreach (var monkey in World.Instance.monkeysInMap) {
                    monkey.PauseMonkey();
                }
            }

            gameContext = GameContext.IN_GAME_MENU;
            isGamePlaying = false;
        }

        public void UnpauseGame() {
            ObjectsReference.Instance.cameraPlayer.SetNormalSensibility();
            ObjectsReference.Instance.playerController.canMove = true;

            if (gameContext == GameContext.IN_GAME_MENU && World.Instance.monkeysInMap != null) {
                foreach (var monkey in World.Instance.monkeysInMap) {
                    monkey.UnpauseMonkey();
                }
            }

            gameContext = GameContext.IN_GAME;
            isGamePlaying = true;
        }

        public void Quit() {
            Application.Quit();
        }
        
        public void ReturnHome() {
            ObjectsReference.Instance.inputManager.SwitchContext(InputContext.UI);
            
            // prevent banana man to fall while loading scene
            ObjectsReference.Instance.playerController.StopPlayer();
            
            // TODO : remove world player objects
            // TODO : activate banana man in hamac
            // TODO : activate gorilla in corolle

            SwitchToHomeSettings();

            gameManager.loadingScreen.SetActive(false);
        }

        private void SwitchToHomeSettings() {
            startAnimations.SetActive(true);

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            gameManager.isGamePlaying = false;
            gameManager.gameContext = GameContext.IN_HOME;

            ObjectsReference.Instance.uiManager.HideGameMenu();
            ObjectsReference.Instance.uiManager.ShowHomeMenu();

            ObjectsReference.Instance.uiManager.SetActive(UICanvasGroupType.HUD, false);
            ObjectsReference.Instance.uiManager.SetActive(UICanvasGroupType.CROSSHAIRS, false);

            ObjectsReference.Instance.cameraPlayer.Set0Sensibility();

            RenderSettings.ambientLight = Color.white;

            ObjectsReference.Instance.playerController.canMove = false;
            ObjectsReference.Instance.bananaMan.SetBananaSkinHealth();

            ObjectsReference.Instance.gameData.currentSaveUuid = null;

            ObjectsReference.Instance.bananaMan.transform.position = spawnPointsBySpawnType[SpawnPoint.HOME].position;

            ObjectsReference.Instance.gameSave.CancelAutoSave();

            ObjectsReference.Instance.inputManager.SwitchContext(InputContext.HOME);
            ObjectsReference.Instance.uiActions.enabled = true;
        }

        private void SwitchToNewGameSettings() {
            startAnimations.SetActive(false);

            ObjectsReference.Instance.gameSave.CancelAutoSave();

            ObjectsReference.Instance.teleportation.TeleportDown();

            _bananaManTransform.position = spawnPointsBySpawnType[SpawnPoint.NEW_GAME].position;
            _bananaManRotation = spawnPointsBySpawnType[SpawnPoint.NEW_GAME].rotation.eulerAngles;

            _bananaManTransform.rotation = Quaternion.Euler(_bananaManRotation);
            
            World.Instance.SpawnInitialBuidablesAndDebrisOnWorld();

            loadingScreen.SetActive(false);
            
            ObjectsReference.Instance.tutorial.StartTutorial();
        }
        
        private void SwitchToInGameSettings() {
            startAnimations.SetActive(false);

            _bananaManTransform.position = ObjectsReference.Instance.gameData.lastPositionOnMap;
            _bananaManRotation = ObjectsReference.Instance.gameData.lastRotationOnMap;

            _bananaManTransform.rotation = Quaternion.Euler(_bananaManRotation);
            
            World.Instance.RespawnBuildablesOnWorld();
            World.Instance.RespawnDebrisOnWorld();
            
            ObjectsReference.Instance.uiManager.SetActive(UICanvasGroupType.HUD, true);
            ObjectsReference.Instance.uiManager.canvasGroupsByUICanvasType[UICanvasGroupType.CROSSHAIRS].alpha = 1f;
            ObjectsReference.Instance.uInventoriesManager.ShowCurrentUIHelper();

            ObjectsReference.Instance.inputManager.SwitchContext(InputContext.GAME);
            gameManager.gameContext = GameContext.IN_GAME;
            gameManager.isGamePlaying = true;

            ObjectsReference.Instance.uiManager.HideGameMenu();
            ObjectsReference.Instance.uiManager.HideHomeMenu();

            ObjectsReference.Instance.cameraPlayer.Return_back_To_Player();

            ObjectsReference.Instance.audioManager.SetMusiqueAndAmbianceByRegion(RegionType.COROLLE);
            
            loadingScreen.SetActive(false);
            
            ObjectsReference.Instance.spaceTrafficControlMiniGameManager.Init();
        }
    }
}