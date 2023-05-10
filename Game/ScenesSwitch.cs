using System.Collections;
using Building;
using Enums;
using Player;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game {
    public class ScenesSwitch : MonoBehaviour {
        [SerializeField] private GameObject teleportationGameObject;
        
        public GenericDictionary<SpawnPoint, Transform> spawnPointsBySpawnType;
        public GenericDictionary<SpawnPoint, string> sceneNameBySpawnPoint;

        private Transform _bananaManTransform;
        private Vector3 _bananaManRotation;

        private void Start() {
            _bananaManTransform = ObjectsReference.Instance.bananaMan.transform;
        }

        private IEnumerator LoadScene(string sceneName, SpawnPoint spawnPoint, bool isTeleporting) {
            AsyncOperation load = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);

            _bananaManRotation = _bananaManTransform.rotation.eulerAngles;

            // Wait until the asynchronous scene fully loads
            while (!load.isDone) {
                yield return null;
            }

            if (load.isDone) {
                SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
                
                //// spawning banana man
                if (isTeleporting) {
                    ObjectsReference.Instance.teleportation.TeleportDown();
                    _bananaManTransform.position = spawnPointsBySpawnType[spawnPoint].position;
                    
                    _bananaManRotation = _bananaManTransform.rotation.eulerAngles;
                    _bananaManRotation.y = spawnPointsBySpawnType[spawnPoint].rotation.y;

                    _bananaManTransform.rotation = Quaternion.Euler(_bananaManRotation);
                }

                if (spawnPoint == SpawnPoint.LAST_MAP) {
                    _bananaManTransform.position = ObjectsReference.Instance.gameData.lastPositionOnMap;
                    _bananaManRotation = ObjectsReference.Instance.gameData.lastRotationOnMap;
                    
                    _bananaManTransform.rotation = Quaternion.Euler(_bananaManRotation);
                }

                else {
                    _bananaManTransform.position = spawnPointsBySpawnType[spawnPoint].position;
                    _bananaManRotation = spawnPointsBySpawnType[spawnPoint].rotation.eulerAngles;
                    
                    _bananaManTransform.rotation = Quaternion.Euler(_bananaManRotation);
                }
                
                if (sceneName.ToUpper() == "HOME") {
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

                    ObjectsReference.Instance.bananaMan.GetComponent<PlayerController>().canMove = false;

                    ObjectsReference.Instance.gameData.currentSaveUuid = null;
                    
                    _bananaManTransform.position = spawnPointsBySpawnType[spawnPoint].position;
                    _bananaManTransform.rotation = Quaternion.Euler(_bananaManRotation);
                }

                else {
                    ObjectsReference.Instance.gameData.bananaManSavedData.lastMap = sceneName;

                    if (ObjectsReference.Instance.mapsManager.currentMap.activeMonkeyType != MonkeyType.NONE) {
                        ObjectsReference.Instance.mapsManager.currentMap.RecalculateHappiness();
                        MapItems.Instance.uiCanvasItemsHiddableManager.SetMonkeysVisibility(ObjectsReference.Instance.gameSettings.isShowingMonkeys);
                    }

                    if (ObjectsReference.Instance.mapsManager.currentMap.hasDebris) {
                        if (ObjectsReference.Instance.mapsManager.currentMap.isDiscovered) ObjectsReference.Instance.gameLoad.RespawnDebrisOnMap();
                        MapItems.Instance.uiCanvasItemsHiddableManager.SetDebrisCanvasVisibility(ObjectsReference.Instance.gameSettings.isShowingDebris);
                    }

                    if (ObjectsReference.Instance.mapsManager.currentMap.hasBananaTree) {
                        MapItems.Instance.uiCanvasItemsHiddableManager.SetBananaTreeVisibility(ObjectsReference.Instance.gameSettings.isShowingBananaTrees); 
                    }
                    
                    ObjectsReference.Instance.gameLoad.RespawnPlateformsOnMap();
                    
                    Cursor.visible = false;
                    Cursor.lockState = CursorLockMode.Locked;
                    
                    ObjectsReference.Instance.inputManager.SwitchContext(InputContext.GAME);
                    ObjectsReference.Instance.gameManager.gameContext = GameContext.IN_GAME;
                    ObjectsReference.Instance.gameManager.isGamePlaying = true;
                    
                    ObjectsReference.Instance.uiManager.Hide_Game_Menu();
                    ObjectsReference.Instance.uiManager.Hide_home_menu();
                    
                    if (ObjectsReference.Instance.gameData.bananaManSavedData.playerAdvancements.Contains(AdvancementState.GET_BANANAGUN)) {
                        ObjectsReference.Instance.uiManager.Set_active(UICanvasGroupType.HUD, true);
                        ObjectsReference.Instance.bananaGun.bananaGunInBack.SetActive(true);
                    }

                    else {
                        ObjectsReference.Instance.uiManager.Set_active(UICanvasGroupType.HUD, false);
                        ObjectsReference.Instance.bananaGun.bananaGunInBack.SetActive(false);
                    }
                    
                    ObjectsReference.Instance.mainCamera.Return_back_To_Player();
                    ObjectsReference.Instance.mainCamera.SetNormalSensibility();
                    
                    ObjectsReference.Instance.bananaMan.GetComponent<PlayerController>().canMove = true;
                }
                
                ObjectsReference.Instance.gameManager.loadingScreen.SetActive(false);

                ObjectsReference.Instance.audioManager.SetMusiqueBySceneName(sceneName);
            }
        }
    
        public void SwitchScene(string sceneName, SpawnPoint spawnPoint, bool isTeleporting) {
            ObjectsReference.Instance.gameManager.loadingScreen.SetActive(true);

            if (ObjectsReference.Instance.gameManager.gameContext == GameContext.IN_GAME) {
                ObjectsReference.Instance.mapsManager.currentMap.RefreshPlateformsDataMap();
                ObjectsReference.Instance.mapsManager.currentMap.RefreshDebrisDataMap();
            }
            
            ObjectsReference.Instance.inputManager.uiSchemaContext = UISchemaSwitchType.LOAD;
            ObjectsReference.Instance.inputManager.SwitchContext(InputContext.UI);

            ObjectsReference.Instance.mapsManager.currentMap = ObjectsReference.Instance.mapsManager.mapBySceneName[sceneName];

            ObjectsReference.Instance.uiManager.Set_active(UICanvasGroupType.HUD, false);
            ObjectsReference.Instance.uiManager.Hide_home_menu();
            
            // prevent banana man to fall while loading scene
            ObjectsReference.Instance.bananaMan.GetComponent<PlayerController>().canMove = false;
        
            StartCoroutine(LoadScene(sceneName, spawnPoint, isTeleporting));
        }

        public void ReturnHome() {
            if (ObjectsReference.Instance.gameData.currentSaveUuid != null) ObjectsReference.Instance.gameSave.SaveGameData(ObjectsReference.Instance.gameData.currentSaveUuid);
        
            SwitchScene("HOME", SpawnPoint.HOME, false);
        }

        public void Teleport(SpawnPoint spawnPoint) {
            teleportationGameObject.SetActive(true);

            ObjectsReference.Instance.teleportation.TeleportUp();
            ObjectsReference.Instance.uiManager.HideInterface();

            SwitchScene(sceneNameBySpawnPoint[spawnPoint].ToUpper(), spawnPoint, true);
        }

        public void TeleportToCommandRoom() {
            Teleport(SpawnPoint.COMMAND_ROOM_TELEPORTATION);
        }
    }
}
