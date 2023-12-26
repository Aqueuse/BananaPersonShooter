using System.Collections;
using Data;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game {
    public class ScenesSwitch : MonoBehaviour {
        public GenericDictionary<SpawnPoint, Transform> spawnPointsBySpawnType;
        private Vector3 _bananaManRotation;

        private Transform _bananaManTransform;

        private GameManager gameManager;

        public Vector3 teleportDestination;
        public Quaternion teleportRotation;

        private void Start() {
            _bananaManTransform = ObjectsReference.Instance.bananaMan.transform;
            gameManager = ObjectsReference.Instance.gameManager;
        }

        private IEnumerator LoadScene(SceneType sceneName, SpawnPoint spawnPoint, bool isNewGame) {
            var load = SceneManager.LoadSceneAsync(sceneName.ToString(), LoadSceneMode.Single);

            // Wait until the asynchronous scene fully loads
            while (!load.isDone) {
                yield return null;
            }

            if (load.isDone) {
                SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName.ToString()));
                
                if (spawnPoint == SpawnPoint.HOME) {
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

                    ObjectsReference.Instance.bananaMan.transform.position = ObjectsReference.Instance.scenesSwitch.spawnPointsBySpawnType[SpawnPoint.HOME].position;
                    
                    ObjectsReference.Instance.gameSave.CancelAutoSave();
                    
                    ObjectsReference.Instance.inputManager.SwitchContext(InputContext.HOME);
                }

                else {
                    //// spawning banana man
                    if (isNewGame) {
                        ObjectsReference.Instance.teleportation.TeleportDown();
                        _bananaManTransform.position = teleportDestination;
                        _bananaManTransform.rotation = teleportRotation;
                    }
                    
                    if (spawnPoint == SpawnPoint.LAST_MAP) {
                        _bananaManTransform.position = ObjectsReference.Instance.gameData.lastPositionOnMap;
                        _bananaManRotation = ObjectsReference.Instance.gameData.lastRotationOnMap;
                        
                        _bananaManTransform.rotation = Quaternion.Euler(_bananaManRotation);
                    }

                    if (spawnPoint != SpawnPoint.LAST_MAP) {
                        _bananaManTransform.position = spawnPointsBySpawnType[spawnPoint].position;
                        _bananaManRotation = spawnPointsBySpawnType[spawnPoint].rotation.eulerAngles;

                        _bananaManTransform.rotation = Quaternion.Euler(_bananaManRotation);
                    }

                    ObjectsReference.Instance.gameData.bananaManSaved.lastMap = sceneName;
                    
                    Map.Instance.RespawnAspirablesOnMap();
                    Map.Instance.SpawnNewDebris();
                    
                    ObjectsReference.Instance.uiManager.SetActive(UICanvasGroupType.HUD, true);
                    ObjectsReference.Instance.uiManager.canvasGroupsByUICanvasType[UICanvasGroupType.CROSSHAIRS].alpha = 1f;
                    ObjectsReference.Instance.uInventoriesManager.ShowCurrentUIHelper();

                    ObjectsReference.Instance.inputManager.SwitchContext(InputContext.GAME);
                    gameManager.gameContext = GameContext.IN_GAME;
                    gameManager.isGamePlaying = true;

                    ObjectsReference.Instance.uiManager.HideGameMenu();
                    ObjectsReference.Instance.uiManager.HideHomeMenu();

                    ObjectsReference.Instance.cameraPlayer.Return_back_To_Player();

                    ObjectsReference.Instance.audioManager.SetMusiqueAndAmbianceBySceneName(sceneName);

                    if (!ObjectsReference.Instance.bananaMan.tutorialFinished) {
                        ObjectsReference.Instance.tutorial.StartTutorial();
                    }
                }

                gameManager.loadingScreen.SetActive(false);
            }
        }

        public void SwitchScene(SceneType sceneName, SpawnPoint spawnPoint, bool isNewGame) {
            ObjectsReference.Instance.inputManager.SwitchContext(InputContext.UI);

            gameManager.loadingScreen.SetActive(true);
            ObjectsReference.Instance.uiManager.HideHomeMenu();
            
            // prevent banana man to fall while loading scene
            ObjectsReference.Instance.playerController.StopPlayer();

            if (gameManager.gameContext != GameContext.IN_HOME) {
                Map.Instance.SaveAspirablesOnMap();
                Map.Instance.SaveMapMonkeysData();
            }
            
            ObjectsReference.Instance.gameData.currentMapData = ObjectsReference.Instance.gameData.mapBySceneName[sceneName];
            
            StartCoroutine(LoadScene(sceneName, spawnPoint, isNewGame));
        }

        public void ReturnHome() {
            SwitchScene(SceneType.HOME, SpawnPoint.HOME, false);
        }
    }
}
