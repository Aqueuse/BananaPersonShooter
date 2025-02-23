using System.Collections.Generic;
using Cinemachine;
using Tags;
using UnityEngine;

namespace InGame {
    public class GameManager : MonoBehaviour {
        [SerializeField] private Color penumbraAmbientLightColor;
        
        [SerializeField] public Camera cameraMain;
        [SerializeField] private CinemachineFreeLook playerCamera;

        public GameObject loadingScreen;
        
        [SerializeField] private GameObject startAnimations;
        [SerializeField] private List<GameObject> inGameGameObjects;
        [SerializeField] private GameObject initialInteractables; // banana gun pieces

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
            
            _bananaManTransform.position = spawnPointsBySpawnType[SpawnPoint.NEW_GAME].position;
            _bananaManRotation = spawnPointsBySpawnType[SpawnPoint.NEW_GAME].rotation.eulerAngles;

            _bananaManTransform.rotation = Quaternion.Euler(_bananaManRotation);
            
            ObjectsReference.Instance.gameSave.buildablesSave.SpawnInitialBuildables();
            ObjectsReference.Instance.gameSave.spaceshipDebrisSave.SpawnInitialSpaceshipDebris();
            
            Destroy(TagsManager.GetAllGameObjectsWithTag(GAME_OBJECT_TAG.INITAL_INTERACTABLES)[0].gameObject); 
            Instantiate(initialInteractables);

            loadingScreen.SetActive(false);

            ObjectsReference.Instance.commandRoomControlPanelsManager.Init();
            ObjectsReference.Instance.commandRoomControlPanelsManager.assembler.Init();

            ObjectsReference.Instance.bananaGun.UngrabBananaGun();

            ObjectsReference.Instance.uiManager.canvasGroupsByUICanvasType[UICanvasGroupType.CROSSHAIRS].alpha = 1f;
            ObjectsReference.Instance.uiCrosshairs.SetCrosshair(false);

            ObjectsReference.Instance.uInventoriesManager.HideUIHelpers();
            ObjectsReference.Instance.uiManager.SetActive(UICanvasGroupType.HUD_BANANAMAN, false);

            ObjectsReference.Instance.uiManager.SetActive(UICanvasGroupType.BANANAGUN_HELPER_KEYBOARD, false);
            ObjectsReference.Instance.uiManager.SetActive(UICanvasGroupType.BANANAGUN_HELPER_GAMEPAD, false);
            ObjectsReference.Instance.bananaGunActionsSwitch.enabled = false;
            
            ObjectsReference.Instance.gameReset.ResetGameData();

            ObjectsReference.Instance.inputManager.SwitchContext(InputContext.UI);

            ObjectsReference.Instance.cinematiques.Play(CinematiqueType.NEW_GAME);
            
            ObjectsReference.Instance.bananaMan.tutorialFinished = false;
            
            foreach (var accessManagedGameObject in TagsManager.GetAllGameObjectsWithTag(GAME_OBJECT_TAG.ACCESS_MANAGED)) {
                accessManagedGameObject.GetComponent<ManageAccess>().ForbidUsage();
            }

            ObjectsReference.Instance.commandRoomControlPanelsManager.chimployeeCommandRoom.SetTutorialChimployeeConfiguration();

            RenderSettings.ambientLight = penumbraAmbientLightColor;
            
            Invoke(nameof(SaveInitialState), 20);
        }
        
        private void SwitchToInGameSettings() {
            startAnimations.SetActive(false);
            foreach (var inGameGameObject in inGameGameObjects) {
                inGameGameObject.SetActive(true);
            }
            
            ObjectsReference.Instance.uiManager.SetActive(UICanvasGroupType.HUD_BANANAMAN, true);
            ObjectsReference.Instance.uiManager.canvasGroupsByUICanvasType[UICanvasGroupType.CROSSHAIRS].alpha = 1f;
            ObjectsReference.Instance.uiCrosshairs.SetCrosshair(false);

            ObjectsReference.Instance.uInventoriesManager.ShowCurrentUIHelper();

            ObjectsReference.Instance.inputManager.SwitchContext(InputContext.GAME);
            gameManager.gameContext = GameContext.IN_GAME;

            ObjectsReference.Instance.uiManager.HideGameMenu();
            ObjectsReference.Instance.uiManager.HideHomeMenu();

            ObjectsReference.Instance.cameraPlayer.Return_back_To_Player();

            ObjectsReference.Instance.audioManager.SetMusiqueAndAmbianceByRegion(RegionType.COROLLE);
            
            ObjectsReference.Instance.commandRoomControlPanelsManager.Init();
            ObjectsReference.Instance.commandRoomControlPanelsManager.assembler.Init();

            ObjectsReference.Instance.bananaGun.GrabBananaGun();
            
            loadingScreen.SetActive(false);
        }
        
        private void SaveInitialState() {
            ObjectsReference.Instance.uiSave.CreateNewSave();
        }
    }
}