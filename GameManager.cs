using System.Collections;
using Audio;
using Cameras;
using Enums;
using Input;
using Items;
using Player;
using Settings;
using UI;
using UI.InGame;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class GameManager : MonoSingleton<GameManager> {
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private Transform homeSpawnTransform;
    
    private Camera _cameraMain;
    private GameObject _bananaSplashVideo;


    public bool isInGame;
    public bool isGamePlaying;
    public bool isInCorolle;
    public bool isFigthing;
    
    private void Start() {
        _cameraMain = Camera.main;
        if (_cameraMain != null) _bananaSplashVideo = _cameraMain.GetComponent<MainCamera>().bananaSplashVideo;
        BananaMan.Instance.GetComponent<RagDoll>().SetRagDoll(false);
        
        GameSettings.Instance.LoadSettings();
        GameSettings.Instance.SetMusicVolume(PlayerPrefs.GetFloat("musicLevel", 0.2f));
        AudioManager.Instance.PlayMusic(MusicType.HOME, false);
    }


    public void Play() {
        //PlayerPrefs.DeleteAll(); // temporaly reset the player prefs on launch while in the building of the beta

        GameSave.Instance.LoadPlayerGameState();
        
        // hide menu lancement
        UIManager.Instance.Hide_home_menu();
        
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        // Switch scene to last scene
        loadingScreen.SetActive(true);
        StartCoroutine(LoadScene(GameSave.Instance.lastMap, GameSave.Instance.lastPositionOnMap));
        
        _cameraMain.clearFlags = CameraClearFlags.Skybox;
        BananaMan.Instance.transform.position = GameSave.Instance.lastPositionOnMap;
    }


    public void PauseGame(bool pause) {
        if (pause) {
            Time.timeScale = 0;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            Set_Playing_State(false);
        }

        if (!pause) {
            Time.timeScale = 1;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            Set_Playing_State(true);
        }
    }

    
    public void ReturnHome() {
        isInGame = false;

        _cameraMain.clearFlags = CameraClearFlags.SolidColor;
        GameSave.Instance.SavePlayerGameState();
        
        SwitchScene("Home", homeSpawnTransform.position);
        UIManager.Instance.Show_home_menu();
        UIManager.Instance.Hide_options_menu();
        UIManager.Instance.Hide_HUD();

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        
        Time.timeScale = 1; // reset the time scale to play animations in Home scene

        // reset banana man state
        BananaMan.Instance.GetComponent<BananaMan>().ResetToPlayable();
        
        Set_Playing_State(false);
    }

    void Set_Playing_State(bool isPlaying) {
        _cameraMain.GetComponent<ThirdPersonOrbitCamBasic>().enabled = isPlaying;

        InputManager.Instance.SwitchContext(isPlaying ? GameContext.GAME : GameContext.UI);

        isGamePlaying = isPlaying;
    }

    public void Quit() {
        Application.Quit();
    }
    
    public void Death() {
        if (isGamePlaying) {
            isGamePlaying = false;
            
            if (SceneManager.GetActiveScene().name.Equals("BossRoom")) {
                isFigthing = false;
            }
        
            _bananaSplashVideo.GetComponent<MeshRenderer>().enabled = true;
            _bananaSplashVideo.GetComponent<VideoPlayer>().Play();

            _cameraMain.GetComponent<ThirdPersonOrbitCamBasic>().enabled = false;
            BananaMan.Instance.GetComponent<BananaMan>().Die();
            UIFace.Instance.Die(true);
            AudioManager.Instance.PlayEffect(EffectType.BANANASPLASH);
            AudioManager.Instance.PlayMusic(MusicType.DEATH, false);
        
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            UIManager.Instance.Show_death_Panel();
            InputManager.Instance.SwitchContext(GameContext.UI);
        }
    }

    public void ReturnToGameAfterDeath() {  // onclick
        isFigthing = false;
        isGamePlaying = true;
        Set_Playing_State(false);
    
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        
        // Hide death UI
        _cameraMain.GetComponent<MainCamera>().bananaSplashVideo.GetComponent<MeshRenderer>().enabled = false;
        UIManager.Instance.GetComponent<UIManager>().Hide_death_Panel();

        // reset banana man state
        BananaMan.Instance.GetComponent<BananaMan>().ResetToPlayable();

        // Switch scene to last scene
        GameSave.Instance.LoadPlayerGameState();
        loadingScreen.SetActive(true);
        StartCoroutine(LoadScene(GameSave.Instance.lastMap, GameSave.Instance.lastPositionOnMap));
    }
    
    /// SCENES SWITCH ///
    
    IEnumerator LoadScene(string sceneName, Vector3 spawnPoint) {
        AsyncOperation load = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        isInCorolle = sceneName.Equals("Corolle"); 
        
        // Wait until the asynchronous scene fully loads
        while (!load.isDone) {
            yield return null;
        }

        if (load.isDone) {
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
            BananaMan.Instance.GetComponent<CharacterController>().enabled = false;
            BananaMan.Instance.transform.position = spawnPoint;
            ItemsManager.Instance.lootMessage.SetActive(false);
            
            loadingScreen.SetActive(false);

            if (sceneName != "Home") {
                UIManager.Instance.Show_HUD();
                isInGame = true;
                isGamePlaying = true;
                isInCorolle = false;
                BananaMan.Instance.GetComponent<CharacterController>().enabled = true;
            }

            Set_Playing_State(sceneName != "Home");
            
            // Ambiance and music sounds
            switch (sceneName) {
                case "Home":
                    AudioManager.Instance.StopAudioSource(AudioSourcesType.AMBIANCE);
                    AudioManager.Instance.PlayMusic(MusicType.HOME, false);
                    break;
                
                case "Map01":
                    AudioManager.Instance.PlayAmbiance(AmbianceType.MAP01);
                    AudioManager.Instance.PlayMusic(MusicType.MAP01, false);
                    break;
                    
                case "Corolle":
                    AudioManager.Instance.PlayAmbiance(AmbianceType.MAP01);
                    AudioManager.Instance.PlayMusic(MusicType.MAP01, false);
                    break;
                
                case "BossRoom":
                    AudioManager.Instance.StopAudioSource(AudioSourcesType.AMBIANCE);
                    AudioManager.Instance.StopAudioSource(AudioSourcesType.MUSIC);
                    break;
            }  // 7 secondes 955  / 39 secondes 775
        }
    }

    public void SwitchScene(string sceneName, Vector3 spawnPoint) {
        Set_Playing_State(false);
        UIManager.Instance.Hide_HUD();
        ItemsManager.Instance.lootMessage.SetActive(false);
        
        loadingScreen.SetActive(true);
        StartCoroutine(LoadScene(sceneName, spawnPoint));

        GameSave.Instance.lastMap = sceneName;
    }
}
