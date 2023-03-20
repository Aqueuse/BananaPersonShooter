using Audio;
using Cinemachine;
using Enums;
using Save;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace Settings {
    public class GameSettings : MonoSingleton<GameSettings> {
        [SerializeField] private Slider musicLevelSlider;
        [SerializeField] private Slider ambianceLevelSlider;
        [SerializeField] private Slider effectsLevelSlider;

        [SerializeField] private Toggle fullScreenToggle;
        [SerializeField] private Toggle vsyncToggle;

        [SerializeField] private TMPro.TMP_Dropdown resolutionDropDown;
        [SerializeField] private KeymapRebinding[] keymapRebindings;
        [SerializeField] private TMPro.TMP_Dropdown languageDropDown;

        [SerializeField] private CinemachineFreeLook playerCamera;
        [SerializeField] private Slider lookSensibilitySlider;

        [SerializeField] private Toggle horizontalCameraInversionToggle;
        [SerializeField] private Toggle verticalCameraInversionToggle;
        
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

        public JsonPlayerPrefs prefs;
        
        private void Start() {
            Application.targetFrameRate = 60; // fix the framerate to prevent crash on some GPU

            // 1 = match monitor refresh rate. 0 = Don't use vsync: use targetFrameRate instead.
            QualitySettings.vSyncCount = 0;
            
            GraphicsSettings.useScriptableRenderPipelineBatching = true;
        }

        public void LoadSettings() {
            prefs = new JsonPlayerPrefs(SaveData.Instance.gamePath + "/preferences.json");
            
            AudioManager.Instance.musicLevel = prefs.GetFloat("musicLevel", 0.1f);
            AudioManager.Instance.voicesLevel = prefs.GetFloat("voicesLevel", 0.1f);
            AudioManager.Instance.effectsLevel = prefs.GetFloat("effectsLevel", 0.1f);
            AudioManager.Instance.ambianceLevel = prefs.GetFloat("ambianceLevel", 0.1f);

            _isFullscreen = prefs.GetString("isFullscreen", "False");
            _isVsync = prefs.GetString("isVSync", "True");
            _resolution = prefs.GetInt("resolution", 3);
            
            languageIndexSelected = prefs.GetInt("language", 1);
            _keymapBinding = prefs.GetString("keymap_binding", null);

            lookSensibility = prefs.GetFloat("LookSensibility", 0.6f);

            _isCameraVerticallyInverted = prefs.GetString("isCameraVerticalAxisInverted", "False").Equals("True");
            _isCameraHorizontallyInverted = prefs.GetString("isCameraHorizontalAxisInverted", "False").Equals("True"); 
            
            SetMusicVolume(AudioManager.Instance.musicLevel);
            SetAmbianceVolume(AudioManager.Instance.ambianceLevel);
            SetEffectVolume(AudioManager.Instance.effectsLevel);
            
            InverseCameraVerticalAxis(_isCameraVerticallyInverted);
            InverseCameraHorizontalAxis(_isCameraHorizontallyInverted);
            SetLookSensibility(lookSensibility);
            
            Invoke(nameof(SetLanguage), 0.2f);
        
            ToggleFullscreen(_isFullscreen.Equals("True"));
            ToggleVSync(_isVsync.Equals("True"));
            SetResolution(_resolution);

            // reflects values on UI 
            musicLevelSlider.value = AudioManager.Instance.musicLevel;
            ambianceLevelSlider.value = AudioManager.Instance.ambianceLevel;
            effectsLevelSlider.value = AudioManager.Instance.effectsLevel;

            fullScreenToggle.isOn = _isFullscreen.Equals("True");
            vsyncToggle.isOn = _isVsync.Equals("True");
            resolutionDropDown.value = _resolution;

            lookSensibilitySlider.value = lookSensibility;

            horizontalCameraInversionToggle.isOn = _isCameraHorizontallyInverted;
            verticalCameraInversionToggle.isOn = _isCameraVerticallyInverted;
    
            languageDropDown.value = languageIndexSelected;
            
            prefs.Save();
        }

        public void ResetOptions() {
            prefs.DeleteAll(); // temporaly reset the player prefs on launch while in the building of the beta
            
            LoadSettings();
        }

        void SetLanguage() {
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[languageIndexSelected];
            prefs.SetInt("language", languageIndexSelected);
        }

        public void SetMusicVolume(float level) {
            AudioManager.Instance.SetVolume(AudioSourcesType.MUSIC, level);

            prefs.SetFloat("musicLevel", level);
        }

        public void SetVoicesVolume(float level) {
            AudioManager.Instance.SetVolume(AudioSourcesType.VOICE, level);

            prefs.SetFloat("voiceLevel", level);
        }

        public void SetEffectVolume(float level) {
            AudioManager.Instance.SetVolume(AudioSourcesType.EFFECT, level);
        
            prefs.SetFloat("effectsLevel", level);
        }

        public void SetAmbianceVolume(float level) {
            AudioManager.Instance.SetVolume(AudioSourcesType.AMBIANCE, level);
            
            prefs.SetFloat("ambianceLevel", level);
        }

        public void ToggleFullscreen(bool isGameFullscreen) {
            Screen.fullScreenMode = isGameFullscreen ? FullScreenMode.ExclusiveFullScreen : FullScreenMode.Windowed;
            _fullScreenMode = Screen.fullScreenMode;

            prefs.SetString("isFullscreen", isGameFullscreen ? "True" : "False");
        }

        public void ToggleVSync(bool isGameVsync) {
            QualitySettings.vSyncCount = isGameVsync ? 1 : 0;

            prefs.SetString("isVSync", isGameVsync ? "True" : "False");
        }

        public void SetResolution(int gameResolution) {
            switch (gameResolution) {
                case 0:
                    Screen.SetResolution(1024, 768, _fullScreenMode, Screen.currentResolution.refreshRateRatio);
                    break;
                case 1:
                    Screen.SetResolution(1280, 720, _fullScreenMode, Screen.currentResolution.refreshRateRatio);
                    break;
                case 2:
                    Screen.SetResolution(1920, 1080, _fullScreenMode, Screen.currentResolution.refreshRateRatio);
                    break;
            }
            prefs.SetInt("resolution", gameResolution);
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
        }

        public void InverseCameraHorizontalAxis(bool isCameraInverted) {
            playerCamera.m_XAxis.m_InvertInput = isCameraInverted; // Cinemachine is naturally inverted
            prefs.SetString("isCameraHorizontalAxisInverted", isCameraInverted.ToString());
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
    }
}
