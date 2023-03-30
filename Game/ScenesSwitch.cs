using System.Collections;
using Audio;
using Cameras;
using Enums;
using Input;
using Player;
using Save;
using Settings;
using UI;
using UI.InGame;
using UnityEngine;
using UnityEngine.SceneManagement;
using VFX;

namespace Game {
    public class ScenesSwitch : MonoSingleton<ScenesSwitch> {
        [SerializeField] private GameObject teleportationGameObject;
        
        public GenericDictionary<SpawnPoint, Transform> spawnPointsBySpawnType;
        public GenericDictionary<SpawnPoint, string> sceneNameBySpawnPoint;

        private Transform bananaManTransform;
        private Vector3 bananaManRotation;

        private void Start() {
            bananaManTransform = BananaMan.Instance.transform;
        }

        private IEnumerator LoadScene(string sceneName, SpawnPoint spawnPoint, bool isTeleporting) {
            AsyncOperation load = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);

            bananaManRotation = bananaManTransform.rotation.eulerAngles;

            // Wait until the asynchronous scene fully loads
            while (!load.isDone) {
                yield return null;
            }

            if (load.isDone) {
                SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
                
                //// spawning banana man
                if (isTeleporting) {
                    Teleportation.Instance.TeleportDown();
                    bananaManTransform.position = spawnPointsBySpawnType[spawnPoint].position;
                    
                    bananaManRotation = bananaManTransform.rotation.eulerAngles;
                    bananaManRotation.y = spawnPointsBySpawnType[spawnPoint].rotation.y;

                    bananaManTransform.rotation = Quaternion.Euler(bananaManRotation);
                }

                if (spawnPoint == SpawnPoint.LAST_MAP) {
                    Teleportation.Instance.ShowBananaManMaterials();
                    bananaManTransform.position = GameData.Instance.lastPositionOnMap;
                    bananaManRotation = GameData.Instance.lastRotationOnMap;
                    
                    bananaManTransform.rotation = Quaternion.Euler(bananaManRotation);
                }

                else {
                    Teleportation.Instance.ShowBananaManMaterials();
                    bananaManTransform.position = spawnPointsBySpawnType[spawnPoint].position;
                    bananaManRotation = spawnPointsBySpawnType[spawnPoint].rotation.eulerAngles;
                    
                    bananaManTransform.rotation = Quaternion.Euler(bananaManRotation);
                }
                
                if (sceneName.ToUpper() == "HOME") {
                    GameManager.Instance.cameraMain.clearFlags = CameraClearFlags.SolidColor;
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;
                    
                    InputManager.Instance.uiSchemaContext = UISchemaSwitchType.HOME_MENU;
                    InputManager.Instance.SwitchContext(InputContext.UI);

                    GameManager.Instance.isGamePlaying = false;
                    GameManager.Instance.gameContext = GameContext.IN_HOME;

                    UIManager.Instance.Hide_Game_Menu();
                    UIManager.Instance.Show_home_menu();

                    MainCamera.Instance.Set0Sensibility();

                    BananaMan.Instance.GetComponent<CharacterController>().enabled = false;
                    BananaMan.Instance.GetComponent<PlayerController>().canMove = false;
                    BananaMan.Instance.GetComponent<BananaMan>().ResetToPlayable();

                    GameData.Instance.currentSaveUuid = null;
                    
                    bananaManTransform.position = spawnPointsBySpawnType[spawnPoint].position;
                    bananaManTransform.rotation = Quaternion.Euler(bananaManRotation);
                }

                else {
                    GameData.Instance.bananaManSavedData.lastMap = sceneName;

                    if (MapsManager.Instance.currentMap.activeMonkeyType != MonkeyType.NONE) {
                        MapsManager.Instance.currentMap.SpawnMonkey();
                        MapsManager.Instance.currentMap.RecalculateHapiness();
                        MapsManager.Instance.currentMap.RefreshMonkeyDataMap();
                    }

                    if (MapsManager.Instance.currentMap.hasDebris) {
                        if (MapsManager.Instance.currentMap.isDiscovered) GameLoad.Instance.RespawnDebrisOnMap();
                        Uihud.Instance.SetDebrisCanvasVisibility(GameSettings.Instance.isShowingDebris);
                    }

                    if (MapsManager.Instance.currentMap.hasBananaTree) {
                        Uihud.Instance.SetBananaTreeCanvasVisibility(GameSettings.Instance.isShowingBananaTrees); 
                    }
                    
                    GameLoad.Instance.RespawnPlateformsOnMap();
                    
                    Cursor.visible = false;
                    Cursor.lockState = CursorLockMode.Locked;
                    
                    InputManager.Instance.SwitchContext(InputContext.GAME);
                    GameManager.Instance.gameContext = GameContext.IN_GAME;
                    GameManager.Instance.isGamePlaying = true;
                    
                    UIManager.Instance.Hide_Game_Menu();
                    UIManager.Instance.Hide_home_menu();

                    if (GameData.Instance.bananaManSavedData.advancementState != AdvancementState.NEW_GAME) {
                        UIManager.Instance.Set_active(UICanvasGroupType.HUD, true);
                    }
                    
                    MainCamera.Instance.Return_back_To_Player();
                    MainCamera.Instance.SetNormalSensibility();
                    
                    BananaMan.Instance.GetComponent<CharacterController>().enabled = true;
                    BananaMan.Instance.GetComponent<PlayerController>().canMove = true;
                }
                
                GameManager.Instance.loadingScreen.SetActive(false);

                AudioManager.Instance.SetMusiqueBySceneName(sceneName);
            }
        }
    
        public void SwitchScene(string sceneName, SpawnPoint spawnPoint, bool isTeleporting) {
            GameManager.Instance.loadingScreen.SetActive(true);

            MapsManager.Instance.currentMap = MapsManager.Instance.mapBySceneName[sceneName];

            UIManager.Instance.Set_active(UICanvasGroupType.HUD, false);
            UIManager.Instance.Hide_home_menu();
            
            // prevent banana man to fall while loading scene
            BananaMan.Instance.GetComponent<PlayerController>().canMove = false;
            BananaMan.Instance.GetComponent<CharacterController>().enabled = false;
        
            StartCoroutine(LoadScene(sceneName, spawnPoint, isTeleporting));
        }

        
        public void ReturnHome() {
            if (GameData.Instance.currentSaveUuid != null) GameSave.Instance.SaveGameData(GameData.Instance.currentSaveUuid);
        
            SwitchScene("HOME", SpawnPoint.HOME, false);
        }

        public void Teleport(SpawnPoint spawnPoint) {
            teleportationGameObject.SetActive(true);            

            Teleportation.Instance.TeleportUp();
            UIManager.Instance.HideInterface();
            
            SwitchScene(sceneNameBySpawnPoint[spawnPoint].ToUpper(), spawnPoint, true);
        }

        public void TeleportToCommandRoom() {
            Teleport(SpawnPoint.COMMAND_ROOM_TELEPORTATION);
        }
    }
}
