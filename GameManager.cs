using System.Collections;
using System.Linq;
using Audio;
using Building;
using Cameras;
using Data;
using Enums;
using Items;
using Player;
using Settings;
using UI;
using UI.InGame;
using UI.InGame.Inventory;
using UI.Menus;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class GameManager : MonoSingleton<GameManager> {
    public Transform initialSpawnTransform;
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private Transform homeSpawnTransform;
    
    private Camera _cameraMain;
    private GameObject _bananaSplashVideo;

    // saving
    private string _lastMap = "Map01";
    private Vector3 _lastPositionOnMap;

    public bool isInGame;
    public bool isGamePlaying;
    public bool isFigthing;
    
    private void Start() {
        _cameraMain = Camera.main;
        if (_cameraMain != null) _bananaSplashVideo = _cameraMain.GetComponent<MainCamera>().bananaSplashVideo;
        BananaMan.Instance.GetComponent<RagDoll>().SetRagDoll(false);
        
        GameSettings.Instance.LoadSettings();
        GameSettings.Instance.SetMusicVolume(PlayerPrefs.GetFloat("musicLevel", 0.2f));
        AudioManager.Instance.PlayMusic(MusicType.HOME, false);
    }

    private void LoadPlayerGameState() {
        // position
        var initialSpawnPosition = initialSpawnTransform.position;
        
        _lastPositionOnMap = new Vector3(
            PlayerPrefs.GetFloat("PlayerXPosition", initialSpawnPosition.x),
            PlayerPrefs.GetFloat("PlayerYPosition", initialSpawnPosition.y),
            PlayerPrefs.GetFloat("PlayerZPosition", initialSpawnPosition.z)
        );

        _lastMap = PlayerPrefs.GetString("Last Map", "Map01");
        
        // inventory
        foreach (var bananaSlot in Inventory.Instance.bananaManInventory.ToList()) {
            Inventory.Instance.bananaManInventory[bananaSlot.Key] = PlayerPrefs.GetInt(bananaSlot.Key.ToString(), 0);
        }
        
        // Has Mover ?
        BananaMan.Instance.hasMover = PlayerPrefs.GetString("HasMover").Equals("true");

        // slots
        UInventory.Instance.ActivateAllInventory();  // activate temporally all the inventory to find the index of slots
        
        foreach (var slot in UISlotsManager.Instance.slotsMappingToInventory.ToList()) {
            var itemType = UInventory.Instance.GetItemThrowableTypeByIndex(PlayerPrefs.GetInt("inventorySlot"+slot.Key));
            var itemCategory = UInventory.Instance.GetItemThrowableCategoryByIndex(PlayerPrefs.GetInt("inventorySlot"+slot.Key));
            
            UISlotsManager.Instance.slotsMappingToInventory[slot.Key] = PlayerPrefs.GetInt("inventorySlot"+slot.Key);
            UISlotsManager.Instance.uiSlotsScripts[slot.Key].SetSlot(itemType, itemCategory);
            UISlotsManager.Instance.uiSlotsScripts[slot.Key].SetSprite(UInventory.Instance.GetItemSprite(itemType));
        }
        
        // active item type, category, banana
        var activeItemType = UInventory.Instance.GetItemThrowableTypeByIndex(PlayerPrefs.GetInt("activeItem"));
        var activeItemCategory = UInventory.Instance.GetItemThrowableCategoryByIndex(PlayerPrefs.GetInt("activeItem"));

        if (activeItemCategory == ItemThrowableCategory.BANANA) {
            BananaMan.Instance.activeItem = ScriptableObjectManager.Instance.GetBananaScriptableObject(activeItemType);
        }

        BananaMan.Instance.activeItemThrowableType = activeItemType;
        BananaMan.Instance.activeItemThrowableCategory = activeItemCategory;

        // health and resistance
        BananaMan.Instance.health = PlayerPrefs.GetFloat("health", 100);
        BananaMan.Instance.resistance = PlayerPrefs.GetFloat("resistance", 100);

        // refects values in UI
        UIVitals.Instance.Set_Health(BananaMan.Instance.health);
        UIVitals.Instance.Set_Resistance(BananaMan.Instance.resistance);
        
        Mover.Instance.SetRocketsQuantity(Inventory.Instance.bananaManInventory[ItemThrowableType.ROCKET]);
    }

    public void Play() {
        //PlayerPrefs.DeleteAll(); // temporaly reset the player prefs on launch while in the building of the beta

        LoadPlayerGameState();
        
        // hide menu lancement
        UIManager.Instance.Hide_home_menu();
        
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        // Switch scene to last scene
        loadingScreen.SetActive(true);
        StartCoroutine(LoadScene(_lastMap, _lastPositionOnMap));
        
        _cameraMain.clearFlags = CameraClearFlags.Skybox;
        BananaMan.Instance.transform.position = _lastPositionOnMap;
        
        isInGame = true;
        isGamePlaying = true;
    }

    public void ResetPlayerState() {
        UIOptionsMenu.Instance.HideConfirmationMessage();
        
        // position
        var initialSpawnPosition = initialSpawnTransform.position;

        _lastPositionOnMap = initialSpawnPosition;
        _lastMap =  "Map01";
        
        // inventory
        foreach (var bananaSlot in Inventory.Instance.bananaManInventory.ToList()) {
            Inventory.Instance.bananaManInventory[bananaSlot.Key] = 0;
        }
        
        BananaMan.Instance.hasMover = false;
        PlayerPrefs.SetString("HasMover", "false");
        
        // active itemType and Category
        BananaMan.Instance.activeItemThrowableType = ItemThrowableType.ROCKET;
        BananaMan.Instance.activeItemThrowableCategory = ItemThrowableCategory.ROCKET;
        
        // active item
        PlayerPrefs.SetInt("activeItem", UInventory.Instance.GetSlotIndex(ItemThrowableType.ROCKET));

        // slots
        foreach (var slot in UISlotsManager.Instance.slotsMappingToInventory) {
            PlayerPrefs.SetInt("inventorySlot"+slot.Key, 0);
        }
        foreach (var instanceUISlotsScript in UISlotsManager.Instance.uiSlotsScripts) {
            instanceUISlotsScript.SetSlot(ItemThrowableType.ROCKET, ItemThrowableCategory.ROCKET);
        }

        // health and resistance
        BananaMan.Instance.health = 100;
        BananaMan.Instance.resistance = 100;

        UIVitals.Instance.Set_Health(BananaMan.Instance.health);
        UIVitals.Instance.Set_Resistance(BananaMan.Instance.resistance);
        
        UIOptionsMenu.Instance.EmptyDateAndHour();

        BananaMan.Instance.transform.position = initialSpawnPosition;
        
        SavePlayerGameState();
        
        ReturnHome();

        // lock maps
        // reinit mini chimps quests
        // reinit spaceship state
        // reinit central workstation state
        // reinit assets positions on maps
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

    public void SavePlayerGameState() {
        // position and map
        PlayerPrefs.SetString("Last Map", _lastMap);

        Vector3 playerPosition = BananaMan.Instance.gameObject.transform.position; 

        PlayerPrefs.SetFloat("PlayerXPosition", playerPosition.x);
        PlayerPrefs.SetFloat("PlayerYPosition", playerPosition.y);
        PlayerPrefs.SetFloat("PlayerZPosition", playerPosition.z);

        // bananas in inventory
        foreach (var bananaSlot in Inventory.Instance.bananaManInventory) {
            PlayerPrefs.SetInt(bananaSlot.Key.ToString(), bananaSlot.Value);
        }
        
        //has mover ?
        PlayerPrefs.SetString("HasMover", BananaMan.Instance.hasMover.ToString());

        // slots state
        foreach (var slot in UISlotsManager.Instance.slotsMappingToInventory) {
            PlayerPrefs.SetInt("inventorySlot"+slot.Key, slot.Value);
        }
        
        // active item
        UInventory.Instance.ActivateAllInventory();
        PlayerPrefs.SetInt("activeItem", UInventory.Instance.GetSlotIndex(BananaMan.Instance.activeItemThrowableType));

        // health and resistance
        PlayerPrefs.SetFloat("health", BananaMan.Instance.health);
        PlayerPrefs.SetFloat("resistance", BananaMan.Instance.resistance);
        
        
        // update the text in option with the actual date and hour
        UIOptionsMenu.Instance.SetActualDateAndHour(System.DateTime.Now.Date.ToString("MM/dd/yyyy h:mm:ss"));
    }
    
    public void ReturnHome() {
        isInGame = false;

        _cameraMain.clearFlags = CameraClearFlags.SolidColor;
        SavePlayerGameState();
        
        SwitchScene("Home", homeSpawnTransform.position);
        UIManager.Instance.Show_home_menu();
        UIManager.Instance.Hide_options_menu();
        UIManager.Instance.Hide_HUD();

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        
        Time.timeScale = 1; // reset the time scale to play animations in Home scene

        // reset banana man state
        BananaMan.Instance.GetComponent<PlayerController>().ResetToPlayable();
        
        Set_Playing_State(false);
    }

    void Set_Playing_State(bool isPlaying) {
        _cameraMain.GetComponent<ThirdPersonOrbitCamBasic>().enabled = isPlaying;
        BananaMan.Instance.GetComponent<PlayerInput>().SwitchCurrentActionMap(isPlaying ? "Player" : "UI");
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
            BananaMan.Instance.GetComponent<PlayerController>().Die();
            UIFace.Instance.Die(true);
            AudioManager.Instance.PlayEffect(EffectType.BANANASPLASH);
            AudioManager.Instance.PlayMusic(MusicType.DEATH, false);
        
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            UIManager.Instance.Show_death_Panel();
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
        BananaMan.Instance.GetComponent<PlayerController>().ResetToPlayable();

        // Switch scene to last scene
        LoadPlayerGameState();
        loadingScreen.SetActive(true);
        StartCoroutine(LoadScene(_lastMap, _lastPositionOnMap));
    }
    
    /// SCENES SWITCH ///
    
    IEnumerator LoadScene(string sceneName, Vector3 spawnPoint) {
        AsyncOperation load = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        if (sceneName.Equals("BossRoom")) {
            SavePlayerGameState(); // implicit save to load back player state if he die
        }
    
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

        _lastMap = sceneName;
    }
}
