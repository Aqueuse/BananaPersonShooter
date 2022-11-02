using System.Collections;
using Cameras;
using Items;
using Player;
using UI;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoSingleton<GameManager> {
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private Transform initialSpawnTransform;
    [SerializeField] private Transform homeSpawnTransform;

    private Camera _cameraMain;
    private static GameObject _bananaMan;

    private string _lastMap = "Map01";
    private Vector3 _lastPositionOnMap;

    public bool isPlaying;
    public bool isBossFigthing = true;
    
    private void Start() {
        _bananaMan = BananaMan.Instance.gameObject;
        _cameraMain = Camera.main;
    }

    void LoadPlayerPrefs() {
        foreach (var bananaSlot in BananasTypeReference.Reference) {
             Inventory.Instance.BananaManInventory[bananaSlot.Key] = PlayerPrefs.GetInt(bananaSlot.Key.ToString(), 0);
        }
        Inventory.Instance.BananaManInventory[BananaType.EMPTY_HAND] = 1;
        
        var initialSpawnPosition = initialSpawnTransform.position;
        
        _lastPositionOnMap = new Vector3(
            PlayerPrefs.GetFloat("PlayerXPosition", initialSpawnPosition.x),
            PlayerPrefs.GetFloat("PlayerYPosition", initialSpawnPosition.y),
            PlayerPrefs.GetFloat("PlayerZPosition", initialSpawnPosition.z)
        );

        _lastMap = PlayerPrefs.GetString("Last Map", "Map01");
    }

    void InitGame() {
        _cameraMain.clearFlags = CameraClearFlags.Skybox;
        _bananaMan.transform.position = _lastPositionOnMap;
        isPlaying = true;
    }
    
    public void Play() {
        LoadPlayerPrefs();
        
        // hide menu lancement
        UIManager.Instance.Hide_home_menu();
        
        // Switch scene to last scene
        loadingScreen.SetActive(true);
        StartCoroutine(LoadScene(_lastMap, _lastPositionOnMap));
        
        InitGame();
    }

    public void PauseGame(bool pause) {
        if (pause) {
            Time.timeScale = 0;
            Set_Playing_State(false);
        }

        if (!pause) {
            Time.timeScale = 1;
            Set_Playing_State(true);
        }
    }

    void Save() {
        PlayerPrefs.SetString("Last Map", _lastMap);

        Vector3 playerPosition = _bananaMan.gameObject.transform.position; 

        PlayerPrefs.SetFloat("PlayerXPosition", playerPosition.x);
        PlayerPrefs.SetFloat("PlayerYPosition", playerPosition.y);
        PlayerPrefs.SetFloat("PlayerZPosition", playerPosition.z);
        
        foreach (var bananaSlot in Inventory.Instance.BananaManInventory) {
            PlayerPrefs.SetInt(bananaSlot.Key.ToString(), bananaSlot.Value);
        }
    }

    public void ReturnHome() {
        _cameraMain.clearFlags = CameraClearFlags.SolidColor;
        Save();
        
        SwitchScene("Home", homeSpawnTransform.position);
        UIManager.Instance.Show_home_menu();
        UIManager.Instance.Hide_HUD();
        
        Time.timeScale = 1; // reset the time scale to play animations in Home scene

        isPlaying = false;
        Set_Playing_State(false);
    }

    public void Quit() {
        Application.Quit();
    }

    IEnumerator LoadScene(string sceneName, Vector3 spawnPoint) {
        AsyncOperation load = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        if (sceneName.Equals("BossRoom")) isBossFigthing = false;

        // Wait until the asynchronous scene fully loads
        while (!load.isDone) {
            yield return null;
        }

        if (load.isDone) {
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
            BananaMan.Instance.transform.position = spawnPoint;
            ItemsManager.Instance.lootMessage.SetActive(false);
            
            loadingScreen.SetActive(false);

            if (sceneName != "Home") {
                UIManager.Instance.Show_HUD();
            }

            Set_Playing_State(sceneName != "Home");
        }
    }

    void Set_Playing_State(bool isGamePlaying) {
        _cameraMain.GetComponent<ThirdPersonOrbitCamBasic>().enabled = isGamePlaying;
        _bananaMan.GetComponent<PlayerInput>().SwitchCurrentActionMap(isGamePlaying ? "Player" : "UI");
    }

    public void SwitchScene(string sceneName, Vector3 spawnPoint) {
        Set_Playing_State(false);
        UIManager.Instance.Hide_HUD();
        ItemsManager.Instance.lootMessage.SetActive(false);
        
        loadingScreen.SetActive(true);
        StartCoroutine(LoadScene(sceneName, spawnPoint));

        _lastMap = sceneName;
    }
}
