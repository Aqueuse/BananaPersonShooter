using System;
using Building;
using Cinemachine;
using Enums;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace Settings {
    public class GameSettings : MonoBehaviour {
        [SerializeField] private Slider musicLevelSlider;
        [SerializeField] private Slider ambianceLevelSlider;
        [SerializeField] private Slider effectsLevelSlider;
        [SerializeField] private Slider voicesLevelSlider;

        [SerializeField] private Toggle fullScreenToggle;
        [SerializeField] private Toggle vsyncToggle;

        [SerializeField] private TMP_Dropdown resolutionDropDown;
        [SerializeField] private KeymapRebinding[] keymapRebindings;
        [SerializeField] private TMP_Dropdown languageDropDown;

        [SerializeField] private CinemachineFreeLook playerCamera;
        [SerializeField] private Slider lookSensibilitySlider;

        [SerializeField] private Toggle horizontalCameraInversionToggle;
        [SerializeField] private Toggle verticalCameraInversionToggle;

        [SerializeField] private Slider debrisFallSlider;
        
        [SerializeField] private Toggle debrisVisibilityToggle;
        [SerializeField] private Toggle bananaTreesVisibilityToggle;
        [SerializeField] private Toggle monkeysVisibilityToggle;

        [SerializeField] private Slider saveDelaySlider;
        [SerializeField] private TextMeshProUGUI saveDelayText;

        private FullScreenMode _fullScreenMode;

        private string _isFullscreen;
        private int _resolution;
        private string _isVsync;
        public bool isKeyRebinding;

        private string _keymapBinding;
        public int languageIndexSelected;
        public float lookSensibility;

        private bool _isCameraVerticallyInverted;
        private bool _isCameraHorizontallyInverted;

        public bool areDebrisFallingOnTheTrees;
        
        public bool isShowingDebris;
        public bool isShowingBananaTrees;
        public bool isShowingMonkeys;

        public int saveDelay;
        
        public JsonPlayerPrefs prefs;
        
        private void Start() {
            Application.targetFrameRate = 60; // fix the framerate to prevent crash on some GPU

            // 1 = match monitor refresh rate. 0 = Don't use vsync: use targetFrameRate instead.
            QualitySettings.vSyncCount = 0;
            
            GraphicsSettings.useScriptableRenderPipelineBatching = true;
        }

        public void LoadSettings() {
            prefs = new JsonPlayerPrefs(ObjectsReference.Instance.saveData.gamePath + "/preferences.json");
            
            ObjectsReference.Instance.audioManager.musicLevel = prefs.GetFloat("musicLevel", 0.5f);
            ObjectsReference.Instance.audioManager.voicesLevel = prefs.GetFloat("voicesLevel", 0.5f);
            ObjectsReference.Instance.audioManager.effectsLevel = prefs.GetFloat("effectsLevel", 0.5f);
            ObjectsReference.Instance.audioManager.ambianceLevel = prefs.GetFloat("ambianceLevel", 0.5f);

            _isFullscreen = prefs.GetString("isFullscreen", "False");
            _isVsync = prefs.GetString("isVSync", "True");
            _resolution = prefs.GetInt("resolution", 3);
            
            languageIndexSelected = prefs.GetInt("language", 1);
            _keymapBinding = prefs.GetString("keymap_binding", null);

            lookSensibility = prefs.GetFloat("LookSensibility", 0.6f);

            _isCameraVerticallyInverted = prefs.GetString("isCameraVerticalAxisInverted", "False").Equals("True");
            _isCameraHorizontallyInverted = prefs.GetString("isCameraHorizontalAxisInverted", "False").Equals("True");

            saveDelay = prefs.GetInt("saveDelay", 300);

            SetMusicVolume(ObjectsReference.Instance.audioManager.musicLevel);
            SetAmbianceVolume(ObjectsReference.Instance.audioManager.ambianceLevel);
            SetEffectVolume(ObjectsReference.Instance.audioManager.effectsLevel);
            SetVoicesVolume(ObjectsReference.Instance.audioManager.voicesLevel);

            InverseCameraVerticalAxis(_isCameraVerticallyInverted);
            InverseCameraHorizontalAxis(_isCameraHorizontallyInverted);
            SetLookSensibility(lookSensibility);

            ToggleFullscreen(_isFullscreen.Equals("True"));
            ToggleVSync(_isVsync.Equals("True"));
            SetResolution(_resolution);
            
            areDebrisFallingOnTheTrees = prefs.GetString("areDebrisFallingOnTheTrees", "True").Equals("True");

            isShowingDebris = prefs.GetString("areDebrisVisible", "True").Equals("True");
            isShowingBananaTrees = prefs.GetString("areBananaTreesVisible", "True").Equals("True");
            isShowingMonkeys = prefs.GetString("areMonkeysVisible", "True").Equals("True");

            // reflects values on UI 
            musicLevelSlider.value = ObjectsReference.Instance.audioManager.musicLevel;
            ambianceLevelSlider.value = ObjectsReference.Instance.audioManager.ambianceLevel;
            effectsLevelSlider.value = ObjectsReference.Instance.audioManager.effectsLevel*10;
            voicesLevelSlider.value = ObjectsReference.Instance.audioManager.voicesLevel;

            fullScreenToggle.isOn = _isFullscreen.Equals("True");
            vsyncToggle.isOn = _isVsync.Equals("True");
            resolutionDropDown.value = _resolution;

            lookSensibilitySlider.value = lookSensibility;

            horizontalCameraInversionToggle.isOn = _isCameraHorizontallyInverted;
            verticalCameraInversionToggle.isOn = _isCameraVerticallyInverted;
    
            languageDropDown.value = languageIndexSelected;

            debrisFallSlider.value = Convert.ToInt32(areDebrisFallingOnTheTrees);
            
            debrisVisibilityToggle.isOn = isShowingDebris;
            bananaTreesVisibilityToggle.isOn = isShowingBananaTrees;
            monkeysVisibilityToggle.isOn = isShowingMonkeys;

            saveDelaySlider.value = Mathf.Abs(saveDelay/60);
        }

        public void ResetOptions() {
            prefs.DeleteAll();
            prefs.Save();
            
            LoadSettings();
        }

        public void SetMusicVolume(float level) {
            ObjectsReference.Instance.audioManager.SetVolume(AudioSourcesType.MUSIC, level);
            ObjectsReference.Instance.cinematiques.SetCinematiqueVolume();
            
            prefs.SetFloat("musicLevel", level);
            prefs.Save();
        }

        public void SetVoicesVolume(float level) {
            ObjectsReference.Instance.audioManager.SetVolume(AudioSourcesType.VOICE, level);

            prefs.SetFloat("voicesLevel", level);
            prefs.Save();
        }

        public void SetEffectLevelFromUISlider(float level) {
            SetEffectVolume(level/10);
        }

        private void SetEffectVolume(float level) {
            ObjectsReference.Instance.audioManager.SetVolume(AudioSourcesType.EFFECT, level);
        
            prefs.SetFloat("effectsLevel", level);
            prefs.Save();
        }

        public void SetAmbianceVolume(float level) {
            ObjectsReference.Instance.audioManager.SetVolume(AudioSourcesType.AMBIANCE, level);
            
            prefs.SetFloat("ambianceLevel", level);
            prefs.Save();
        }

        public void ToggleFullscreen(bool isGameFullscreen) {
            Screen.fullScreenMode = isGameFullscreen ? FullScreenMode.ExclusiveFullScreen : FullScreenMode.Windowed;
            _fullScreenMode = Screen.fullScreenMode;

            prefs.SetString("isFullscreen", isGameFullscreen ? "True" : "False");
            prefs.Save();
        }

        public void ToggleVSync(bool isGameVsync) {
            QualitySettings.vSyncCount = isGameVsync ? 1 : 0;

            prefs.SetString("isVSync", isGameVsync ? "True" : "False");
            
            prefs.Save();
        }

        public void SetResolution(int gameResolution) {
            switch (gameResolution) {
                case 0:
                    Screen.SetResolution(1024, 768, _fullScreenMode, Screen.currentResolution.refreshRate);
                    break;
                case 1:
                    Screen.SetResolution(1280, 720, _fullScreenMode, Screen.currentResolution.refreshRate);
                    break;
                case 2:
                    Screen.SetResolution(1920, 1080, _fullScreenMode, Screen.currentResolution.refreshRate);
                    break;
            }
            prefs.SetInt("resolution", gameResolution);
            
            prefs.Save();
        }

        public void SetLookSensibility(float sensibility) {
            playerCamera.m_YAxis.m_MaxSpeed = sensibility;
            playerCamera.m_XAxis.m_MaxSpeed = sensibility * 400;

            lookSensibility = sensibility;
            prefs.SetFloat("LookSensibility", lookSensibility);
        }

        public void InverseCameraVerticalAxis(bool isCameraInverted) {
            playerCamera.m_YAxis.m_InvertInput = !isCameraInverted; // Cinemachine is naturally inverted
            prefs.SetString("isCameraVerticalAxisInverted", isCameraInverted.ToString());

            prefs.Save();
        }

        public void InverseCameraHorizontalAxis(bool isCameraInverted) {
            playerCamera.m_XAxis.m_InvertInput = isCameraInverted; // Cinemachine is naturally inverted
            prefs.SetString("isCameraHorizontalAxisInverted", isCameraInverted.ToString());

            prefs.Save();
        }

        // public void ResetAllBindings() {
        //     foreach (var keymapRebinding in keymapRebindings) {
        //         keymapRebinding.ResetBinding();
        //     }
        //
        //     foreach (var keymapWasdRebinding in keymapWasdRebindings) {
        //         keymapWasdRebinding.ResetBinding();
        //     }
        //     
        // }

        public void SetDebrisFall(float level) {
            areDebrisFallingOnTheTrees = Convert.ToBoolean((int)level);
            prefs.SetString("areDebrisFallingOnTheTrees", areDebrisFallingOnTheTrees ? "True" : "False");
            
            prefs.Save();
        }

        public void SaveDebrisCanvasVisibility(bool isVisible) {
            isShowingDebris = isVisible;
            prefs.SetString("areDebrisVisible", isVisible ? "True" : "False");
            if (ObjectsReference.Instance.gameManager.gameContext == GameContext.IN_GAME) MapItems.Instance.uiCanvasItemsHiddableManager.SetDebrisSpriteRendererVisibility(isVisible);
            
            prefs.Save();
        }
        
        public void SaveBananaTreeCanvasVisibility(bool isVisible) {
            isShowingBananaTrees = isVisible;
            prefs.SetString("areBananaTreesVisible", isVisible ? "True" : "False");
            if (ObjectsReference.Instance.gameManager.gameContext == GameContext.IN_GAME) MapItems.Instance.uiCanvasItemsHiddableManager.SetBananaTreeVisibility(isVisible); 

            prefs.Save();
        }

        public void SaveMonkeysVisibility(bool isVisible) {
            isShowingMonkeys = isVisible;

            prefs.SetString("areMonkeysVisible", isVisible ? "True" : "False");
            if (ObjectsReference.Instance.gameManager.gameContext == GameContext.IN_GAME && ObjectsReference.Instance.mapsManager.currentMap.activeMonkeyType != MonkeyType.NONE) MapItems.Instance.uiCanvasItemsHiddableManager.SetMonkeysVisibility(isVisible);

            prefs.Save();
        }

        public void SetSaveDelay(float delay) {
            saveDelay = (int)delay*60;
            prefs.SetInt("saveDelay", (int)delay*60);
            if (delay == 0) saveDelayText.text = LocalizationSettings.StringDatabase.GetLocalizedString("UI", "never");
            else {
                saveDelayText.text = delay + "m";
            }

            prefs.Save();
        }
    }
}
