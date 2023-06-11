using System;
using System.Collections;
using Enums;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game {
    public class ScenesSwitch : MonoBehaviour {
        [SerializeField] private GameObject teleportationGameObject;

        public GenericDictionary<SpawnPoint, Transform> spawnPointsBySpawnType;
        public GenericDictionary<SpawnPoint, string> sceneNameBySpawnPoint;
        private Vector3 _bananaManRotation;

        private Transform _bananaManTransform;
        
        private void Start() {
            _bananaManTransform = ObjectsReference.Instance.bananaMan.transform;
        }

        private IEnumerator LoadScene(string sceneName, SpawnPoint spawnPoint, bool isTeleporting, bool isNewGame) {
            AsyncOperation load = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);

            // Wait until the asynchronous scene fully loads
            while (!load.isDone) {
                yield return null;
            }

            if (load.isDone) {
                if (isNewGame) {
                    ObjectsReference.Instance.gameReset.ResetGameData();
                    ObjectsReference.Instance.uiSave.CreateNewSave();
                }

                
                SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
                
                if (spawnPoint == SpawnPoint.HOME) {
                    ObjectsReference.Instance.gameManager.cameraMain.clearFlags = CameraClearFlags.SolidColor;
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;

                    ObjectsReference.Instance.inputManager.uiSchemaContext = UISchemaSwitchType.HOME_MENU;
                    ObjectsReference.Instance.inputManager.SwitchContext(InputContext.UI);

                    ObjectsReference.Instance.gameManager.isGamePlaying = false;
                    ObjectsReference.Instance.gameManager.gameContext = GameContext.IN_HOME;

                    ObjectsReference.Instance.uiManager.Hide_Game_Menu();
                    ObjectsReference.Instance.uiManager.Show_home_menu();

                    ObjectsReference.Instance.mainCamera.Set0Sensibility();

                    ObjectsReference.Instance.playerController.canMove = false;

                    ObjectsReference.Instance.gameData.currentSaveUuid = null;

                    ObjectsReference.Instance.bananaMan.transform.position = ObjectsReference.Instance.scenesSwitch.spawnPointsBySpawnType[SpawnPoint.HOME].position;
                }

                else {
                    //// spawning banana man
                    if (isTeleporting) ObjectsReference.Instance.teleportation.TeleportDown();

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

                ObjectsReference.Instance.audioManager.SetMusiqueBySceneName(sceneName);
                
                ObjectsReference.Instance.gameLoad.LoadAdvancements();
            }
        }

        public void SwitchScene(string sceneName, SpawnPoint spawnPoint, bool isTeleporting, bool isNewGame) {
            ObjectsReference.Instance.inputManager.uiSchemaContext = UISchemaSwitchType.LOAD;
            ObjectsReference.Instance.inputManager.SwitchContext(InputContext.UI);

            ObjectsReference.Instance.gameManager.loadingScreen.SetActive(true);
            ObjectsReference.Instance.uiManager.Set_active(UICanvasGroupType.HUD, false);
            ObjectsReference.Instance.uiManager.Hide_home_menu();
            
            // prevent banana man to fall while loading scene
            ObjectsReference.Instance.playerController.canMove = false;

            if (!isNewGame && ObjectsReference.Instance.gameManager.gameContext == GameContext.IN_GAME && ObjectsReference.Instance.mapsManager.currentMap.isDiscovered) {
                ObjectsReference.Instance.mapsManager.currentMap.RefreshAspirablesDataMap();
            }
            
            ObjectsReference.Instance.mapsManager.currentMap = ObjectsReference.Instance.mapsManager.mapBySceneName[sceneName];
            
            StartCoroutine(LoadScene(sceneName, spawnPoint, isTeleporting, isNewGame));
        }

        public void ReturnHome() {
            if (ObjectsReference.Instance.gameData.currentSaveUuid != null) ObjectsReference.Instance.gameSave.SaveGame(ObjectsReference.Instance.gameData.currentSaveUuid);
        
            SwitchScene("HOME", SpawnPoint.HOME, false, false);
        }

        public void Teleport(SpawnPoint spawnPoint) {
            teleportationGameObject.SetActive(true);

            ObjectsReference.Instance.teleportation.TeleportUp();

            SwitchScene(sceneNameBySpawnPoint[spawnPoint].ToUpper(), spawnPoint, true, false);
        }

        public void TeleportToCommandRoom() {
            ObjectsReference.Instance.uiManager.HideInterface();
            Teleport(SpawnPoint.COMMAND_ROOM_TELEPORTATION);
        }
    }
}
