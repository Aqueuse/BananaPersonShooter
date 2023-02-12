﻿using System;
using System.Collections;
using System.Globalization;
using Audio;
using Cameras;
using Cinemachine;
using Enums;
using Input;
using Items;
using Player;
using Save;
using Settings;
using UI;
using UI.InGame;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class GameManager : MonoSingleton<GameManager> {
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private Transform homeSpawnTransform;
    [SerializeField] private GenericDictionary<string, Transform> teleportSpawnPointBySceneName;

    private Camera _cameraMain;
    private GameObject _bananaSplashVideo;
    private CinemachineFreeLook playerCamera;
    
    public bool isInGame;
    public bool isGamePlaying;
    public bool isInCorolle;
    public bool isFigthing;
    
    private void Start() {
        _cameraMain = Camera.main;
        if (_cameraMain != null) {
            _bananaSplashVideo = _cameraMain.GetComponent<MainCamera>().bananaSplashVideo;
            playerCamera = _cameraMain.GetComponent<CinemachineFreeLook>();
        }

        BananaMan.Instance.GetComponent<RagDoll>().SetRagDoll(false);
        
        GameSettings.Instance.LoadSettings();
        AudioManager.Instance.PlayMusic(MusicType.HOME, false);
    }

    public void Play(string saveUuid) {
        GameLoad.Instance.LoadGameData(saveUuid);
        
        // hide menu lancement
        UIManager.Instance.Hide_home_menu();
        
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        // Switch scene to last scene
        loadingScreen.SetActive(true);
        StartCoroutine(LoadScene(GameData.Instance.bananaManSavedData.last_map, GameData.Instance.lastPositionOnMap));
        
        _cameraMain.clearFlags = CameraClearFlags.Skybox;
    }

    public void PauseGame(bool pause) {
        if (pause) {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            Set_Playing_State(false);
        }

        if (!pause) {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            Set_Playing_State(true);
        }
    }
    
    public void ReturnHome() {
        isInGame = false;

        _cameraMain.clearFlags = CameraClearFlags.SolidColor;
        var date = DateTime.ParseExact(DateTime.Now.ToString("U"), "U", CultureInfo.CurrentCulture).ToString(CultureInfo.CurrentCulture);

        GameSave.Instance.SaveGameData("auto_save", date);
        
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
        InputManager.Instance.SwitchContext(isPlaying ? GameContext.GAME : GameContext.UI);
        playerCamera.enabled = isPlaying;
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

    public void ReturnToGameAfterDeath() {
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
        GameLoad.Instance.LoadGameData("auto_save");
        loadingScreen.SetActive(true);
        StartCoroutine(LoadScene(GameData.Instance.bananaManSavedData.last_map, GameData.Instance.lastPositionOnMap));
    }

    public void Teleport(string sceneName) {
        // show TP VFX on banana man
        UIManager.Instance.Show_Hide_inventory();
        SwitchScene(sceneName, teleportSpawnPointBySceneName[sceneName].position);
    }

    /// SCENES SWITCH ///
    
    IEnumerator LoadScene(string sceneName, Vector3 spawnPoint) {
        AsyncOperation load = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        //if (LoadData.Instance.HasMapData()) LoadData.Instance.LoadMapDataByUuid();
        
        isInCorolle = sceneName.Equals("COROLLE"); 
        
        // Wait until the asynchronous scene fully loads
        while (!load.isDone) {
            yield return null;
        }

        if (load.isDone) {
            GameData.Instance.bananaManSavedData.last_map = sceneName;
            
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
            BananaMan.Instance.GetComponent<CharacterController>().enabled = false;
            BananaMan.Instance.transform.position = spawnPoint;
            ItemsManager.Instance.lootMessage.SetActive(false);
            
            loadingScreen.SetActive(false);
            

            if (sceneName != "Home") {
                UIManager.Instance.Show_HUD();
                isInGame = true;
                isGamePlaying = true;
                BananaMan.Instance.GetComponent<CharacterController>().enabled = true;
            }
            
            Set_Playing_State(sceneName != "Home");
            
            // Ambiance and music sounds
            switch (sceneName) {
                case "Home":
                    AudioManager.Instance.StopAudioSource(AudioSourcesType.AMBIANCE);
                    AudioManager.Instance.PlayMusic(MusicType.HOME, false);
                    break;
                
                case "MAP01":
                    AudioManager.Instance.PlayAmbiance(AmbianceType.MAP01);
                    AudioManager.Instance.PlayMusic(MusicType.MAP01, false);
                    break;
                    
                case "COROLLE":
                    AudioManager.Instance.PlayAmbiance(AmbianceType.MAP01);
                    AudioManager.Instance.PlayMusic(MusicType.MAP01, false);
                    break;
                
                case "COMMANDROOM":
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

        GameData.Instance.bananaManSavedData.last_map = sceneName;
    }
}
