using System.Collections;
using Audio;
using Cameras;
using Enums;
using Input;
using Player;
using Save;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using VFX;

namespace Game {
    public class ScenesSwitch : MonoSingleton<ScenesSwitch> {
        [SerializeField] private Transform homeSpawnTransform;
        [SerializeField] private GameObject teleportationGameObject;
        
        public GenericDictionary<string, Transform> teleportSpawnPointBySceneName;

        private IEnumerator LoadScene(string sceneName, Vector3 spawnPoint, bool isTeleporting) {
            AsyncOperation load = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        
            // Wait until the asynchronous scene fully loads
            while (!load.isDone) {
                yield return null;
            }

            if (load.isDone) {
                GameData.Instance.BananaManSavedData.last_map = sceneName;
                SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
                
                BananaMan.Instance.transform.position = spawnPoint;

                GameManager.Instance.loadingScreen.SetActive(false);

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

//                    StartScreen.Instance.enabled = true;

                    BananaMan.Instance.GetComponent<CharacterController>().enabled = false;
                    BananaMan.Instance.GetComponent<PlayerController>().canMove = false;
                    BananaMan.Instance.GetComponent<BananaMan>().ResetToPlayable();
                }

                else {
                    MapsManager.Instance.currentMap = MapsManager.Instance.mapBySceneName[sceneName];
                    MapsManager.Instance.currentMap.isDiscovered = true;

                    if (MapsManager.Instance.currentMap.activeMonkeyType != MonkeyType.NONE) {
                        MapsManager.Instance.currentMap.SpawnMonkey();
                        MapsManager.Instance.currentMap.RecalculateHapiness();
                    }
                    
                    if (MapsManager.Instance.currentMap.hasDebris) GameLoad.Instance.RespawnDebrisOnMap();

                    Cursor.visible = false;
                    Cursor.lockState = CursorLockMode.Locked;
                    
                    InputManager.Instance.SwitchContext(InputContext.GAME);
                    GameManager.Instance.gameContext = GameContext.IN_GAME;
                    GameManager.Instance.isGamePlaying = true;
                    
                    UIManager.Instance.Hide_Game_Menu();
                    UIManager.Instance.Hide_home_menu();

                    if (GameData.Instance.BananaManSavedData.advancementState == AdvancementState.NEW_GAME) {
                        GameManager.Instance.isGamePlaying = false;
                    }
                    else {
                        UIManager.Instance.Show_HUD();
                    }
                    
                    MainCamera.Instance.Return_back_To_Player();
                    MainCamera.Instance.SetNormalSensibility();
                    
                    BananaMan.Instance.GetComponent<CharacterController>().enabled = true;
                    BananaMan.Instance.GetComponent<PlayerController>().canMove = true;

                    if (isTeleporting) Teleportation.Instance.TeleportDown();
                }

                AudioManager.Instance.SetMusiqueBySceneName(sceneName);
            }
        }
    
        public void SwitchScene(string sceneName, Vector3 spawnPoint, bool isTeleporting) {
            if (!SceneManager.GetActiveScene().name.Equals("INITIALHOME") && 
                !SceneManager.GetActiveScene().name.Equals("HOME")) {
                
                MapsManager.Instance.currentMap.SaveDataOnMap();

                if (sceneName.ToUpper() != "HOME") MapsManager.Instance.currentMap = MapsManager.Instance.mapBySceneName[sceneName];
            }

            UIManager.Instance.Hide_HUD();
            UIManager.Instance.Hide_home_menu();
            
            // prevent banana man to fall while loading scene
            BananaMan.Instance.GetComponent<PlayerController>().canMove = false;
            BananaMan.Instance.GetComponent<CharacterController>().enabled = false;
        
            GameManager.Instance.loadingScreen.SetActive(true);
            StartCoroutine(LoadScene(sceneName, spawnPoint, isTeleporting));
        }

        
        public void ReturnHome() {
            GameSave.Instance.SaveGameData(GameData.Instance.currentSaveUuid);
        
            SwitchScene("HOME", homeSpawnTransform.position, false);
        }

        public void Teleport(string sceneName) {
            // show TP VFX on banana man
            teleportationGameObject.SetActive(true);            

            Teleportation.Instance.TeleportUp();
            UIManager.Instance.Show_Hide_interface();
            SwitchScene(sceneName.ToUpper(), teleportSpawnPointBySceneName[sceneName].position, true);
        }
    }
}
