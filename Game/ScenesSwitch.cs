using System.Collections;
using Enums;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game {
    public class ScenesSwitch : MonoBehaviour {
        public GenericDictionary<SpawnPoint, Transform> spawnPointsBySpawnType;
        private Vector3 _bananaManRotation;

        private Transform _bananaManTransform;

        public Vector3 teleportDestination;
        public Quaternion teleportRotation;

        private void Start() {
            _bananaManTransform = ObjectsReference.Instance.bananaMan.transform;
        }

        private IEnumerator LoadScene(string sceneName, SpawnPoint spawnPoint, bool isTeleporting, bool isNewGame) {
            var load = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);

            // Wait until the asynchronous scene fully loads
            while (!load.isDone) {
                yield return null;
            }

            if (load.isDone) {
                SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
                
                if (spawnPoint == SpawnPoint.HOME) {
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;

                    ObjectsReference.Instance.inputManager.uiSchemaContext = UISchemaSwitchType.HOME_MENU;
                    ObjectsReference.Instance.inputManager.SwitchContext(InputContext.UI);

                    ObjectsReference.Instance.gameManager.isGamePlaying = false;
                    ObjectsReference.Instance.gameManager.gameContext = GameContext.IN_HOME;

                    ObjectsReference.Instance.uiManager.Hide_Game_Menu();
                    ObjectsReference.Instance.uiManager.Show_Home_Menu();

                    ObjectsReference.Instance.uiManager.canvasGroupsByUICanvasType[UICanvasGroupType.HUD].alpha = 0f;
                    ObjectsReference.Instance.uiManager.canvasGroupsByUICanvasType[UICanvasGroupType.CROSSHAIRS].alpha = 0f;
                    
                    ObjectsReference.Instance.mainCamera.Set0Sensibility();

                    RenderSettings.ambientLight = Color.white; 

                    ObjectsReference.Instance.playerController.canMove = false;
                    ObjectsReference.Instance.bananaMan.SetBananaSkinHealth();

                    ObjectsReference.Instance.gameData.currentSaveUuid = null;

                    ObjectsReference.Instance.bananaMan.transform.position = ObjectsReference.Instance.scenesSwitch.spawnPointsBySpawnType[SpawnPoint.HOME].position;
                    
                    ObjectsReference.Instance.gameSave.CancelAutoSave();
                    
                    
                }

                else {
                    //// spawning banana man
                    if (isTeleporting && !isNewGame) {
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

                    ObjectsReference.Instance.gameData.bananaManSavedData.lastMap = sceneName;
                    
                    ObjectsReference.Instance.gameLoad.RespawnAspirablesOnMap();
                    ObjectsReference.Instance.mapsManager.currentMap.Init();
                    
                    Cursor.visible = false;
                    Cursor.lockState = CursorLockMode.Locked;
                    ObjectsReference.Instance.uiManager.canvasGroupsByUICanvasType[UICanvasGroupType.CROSSHAIRS].alpha = 1f;
                    ObjectsReference.Instance.uiCrosshairs.SetCrosshair(ObjectsReference.Instance.bananaMan.activeItemCategory, ObjectsReference.Instance.bananaMan.activeBananaType);
                    
                    ObjectsReference.Instance.inputManager.SwitchContext(InputContext.GAME);
                    ObjectsReference.Instance.gameManager.gameContext = GameContext.IN_GAME;
                    ObjectsReference.Instance.gameManager.isGamePlaying = true;
                    
                    ObjectsReference.Instance.uiManager.Hide_Game_Menu();
                    ObjectsReference.Instance.uiManager.Hide_home_menu();
                    
                    ObjectsReference.Instance.mainCamera.Return_back_To_Player();
                    ObjectsReference.Instance.mainCamera.SetNormalSensibility();
                    
                    ObjectsReference.Instance.playerController.canMove = true;
                    ObjectsReference.Instance.bananaMan.GetComponent<Rigidbody>().isKinematic = false;
                }

                ObjectsReference.Instance.gameManager.loadingScreen.SetActive(false);

                if (!ObjectsReference.Instance.bananaMan.tutorialFinished) {
                    ObjectsReference.Instance.tutorial.StartTutorial();
                }

                else {
                    ObjectsReference.Instance.audioManager.SetMusiqueAndAmbianceBySceneName(sceneName);
                }
            }
        }

        public void SwitchScene(string sceneName, SpawnPoint spawnPoint, bool isTeleporting, bool isNewGame) {
            ObjectsReference.Instance.inputManager.uiSchemaContext = UISchemaSwitchType.LOAD;
            ObjectsReference.Instance.inputManager.SwitchContext(InputContext.UI);

            ObjectsReference.Instance.gameManager.loadingScreen.SetActive(true);
            ObjectsReference.Instance.uiManager.Hide_home_menu();
            
            // prevent banana man to fall while loading scene
            ObjectsReference.Instance.playerController.StopPlayer();
            
            ObjectsReference.Instance.mapsManager.currentMap = ObjectsReference.Instance.mapsManager.mapBySceneName[sceneName];
            
            StartCoroutine(LoadScene(sceneName, spawnPoint, isTeleporting, isNewGame));
        }

        public void ReturnHome() {
            SwitchScene("HOME", SpawnPoint.HOME, false, false);
        }
    }
}
