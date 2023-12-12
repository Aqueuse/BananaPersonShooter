﻿using Gestion;
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
            RenderSettings.ambientLight = Color.white;

            ObjectsReference.Instance.inputManager.SwitchContext(InputContext.HOME);
        }
        
        public static void Prepare_New_Game() {
            ObjectsReference.Instance.gameManager.Play(null, true);
        }

        public void Play(string saveUuid, bool isNewGame) {
            ObjectsReference.Instance.uiManager.HideHomeMenu();
            
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            loadingScreen.SetActive(true);
            
            cameraMain.clearFlags = CameraClearFlags.Skybox;
            playerCamera.Priority = 10;
            
            if (isNewGame) {
                ObjectsReference.Instance.bananaMan.tutorialFinished = false;

                ObjectsReference.Instance.scenesSwitch.SwitchScene(
                    SceneType.COROLLE,
                    SpawnPoint.NEW_GAME,
                    true);
            }
            else {
                ObjectsReference.Instance.gameLoad.LoadGameData(saveUuid);

                ObjectsReference.Instance.scenesSwitch.SwitchScene(
                    ObjectsReference.Instance.gameData.bananaManSaved.lastMap, 
                    SpawnPoint.LAST_MAP, 
                    false);
            }
        }

        public void PauseGame() {
            ObjectsReference.Instance.cameraPlayer.Set0Sensibility();
            
            if (gameContext == GameContext.IN_GAME && ObjectsReference.Instance.gameData.currentMapData.mapPropertiesScriptableObject.monkeyPropertiesScriptableObjectsByMonkeyId.Count > 0) {
                foreach (var monkey in Map.Instance.monkeys) {
                    monkey.PauseMonkey();
                }
            }

            isGamePlaying = false;
        }

        public void UnpauseGame() {
            ObjectsReference.Instance.cameraPlayer.SetNormalSensibility();

            if (ObjectsReference.Instance.gameData.currentMapData.mapPropertiesScriptableObject.monkeyPropertiesScriptableObjectsByMonkeyId.Count > 0) {
                foreach (var monkey in Map.Instance.monkeys) {
                    monkey.UnpauseMonkey();
                }
            }

            isGamePlaying = true;
        }

        public void Quit() {
            Application.Quit();
        }
    }
}
