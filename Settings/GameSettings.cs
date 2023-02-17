using Audio;
using Cinemachine;
using Enums;
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
        
        private FullScreenMode _fullScreenMode;

        private string _isFullscreen;
        private int _resolution;
        private string _isVsync;
        public bool isKeyRebinding;

        private string _keymapBinding;
        public int languageIndexSelected;
        public float lookSensibility;
        
        private void Start() {
            Application.targetFrameRate = 60; // fix the framerate to prevent crash on some GPU

            // 1 = match monitor refresh rate. 0 = Don't use vsync: use targetFrameRate instead.
            QualitySettings.vSyncCount = 0;
            
            GraphicsSettings.useScriptableRenderPipelineBatching = true;
        }

        public void LoadSettings() {
            AudioManager.Instance.musicLevel = PlayerPrefs.GetFloat("musicLevel", 0.1f);
            AudioManager.Instance.ambianceLevel = PlayerPrefs.GetFloat("ambianceLevel", 0.1f);
            AudioManager.Instance.effectsLevel = PlayerPrefs.GetFloat("effectsLevel", 0.1f);

            _isFullscreen = PlayerPrefs.GetString("isFullscreen", "True");
            _isVsync = PlayerPrefs.GetString("isVSync", "True");
            _resolution = PlayerPrefs.GetInt("resolution", 3);

            languageIndexSelected = PlayerPrefs.GetInt("language", 1);
            _keymapBinding = PlayerPrefs.GetString("keymap_binding", null);

            lookSensibility = PlayerPrefs.GetFloat("LookSensibility", 0.6f);
            
            SetMusicVolume(AudioManager.Instance.musicLevel);
            SetAmbianceVolume(AudioManager.Instance.ambianceLevel);
            SetEffectVolume(AudioManager.Instance.effectsLevel);
            
            InverseCameraVerticalAxis(PlayerPrefs.GetString("isCameraVerticalAxisInverted", "False").Equals("True"));
            SetLookSensibility(lookSensibility);
            
            Invoke(nameof(SetLanguage), 0.2f);
        
            // reflects values on UI 
            ToggleFullscreen(_isFullscreen.Equals("True"));
            ToggleVSync(_isVsync.Equals("True"));
            SetResolution(_resolution);

            musicLevelSlider.value = AudioManager.Instance.musicLevel;
            ambianceLevelSlider.value = AudioManager.Instance.ambianceLevel;
            effectsLevelSlider.value = AudioManager.Instance.effectsLevel;

            fullScreenToggle.isOn = _isFullscreen.Equals("True");
            vsyncToggle.isOn = _isVsync.Equals("True");
            resolutionDropDown.value = _resolution;

            lookSensibilitySlider.value = lookSensibility;
    
            languageDropDown.value = languageIndexSelected;
        }

        public void ResetOptions() {
            PlayerPrefs.DeleteAll(); // temporaly reset the player prefs on launch while in the building of the beta
            
            LoadSettings();
        }

        void SetLanguage() {
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[languageIndexSelected];
        }

        public void SetEffectVolume(float level) {
            AudioManager.Instance.SetVolume(AudioSourcesType.EFFECT, level);
            AudioManager.Instance.SetVolume(AudioSourcesType.VOICE, level);
        
            PlayerPrefs.SetFloat("effectsLevel", level);
        }

        public void SetMusicVolume(float level) {
            AudioManager.Instance.SetVolume(AudioSourcesType.MUSIC, level);

            PlayerPrefs.SetFloat("musicLevel", level);
        }

        public void SetAmbianceVolume(float level) {
            AudioManager.Instance.SetVolume(AudioSourcesType.AMBIANCE, level);
            
            PlayerPrefs.SetFloat("ambianceLevel", level);
        }

        public void ToggleFullscreen(bool isGameFullscreen) {
            Screen.fullScreenMode = isGameFullscreen ? FullScreenMode.ExclusiveFullScreen : FullScreenMode.Windowed;
            _fullScreenMode = Screen.fullScreenMode;

            PlayerPrefs.SetString("isFullscreen", isGameFullscreen ? "True" : "False");
        }

        public void ToggleVSync(bool isGameVsync) {
            QualitySettings.vSyncCount = isGameVsync ? 1 : 0;

            PlayerPrefs.SetString("isVSync", isGameVsync ? "True" : "False");
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
            PlayerPrefs.SetInt("resolution", gameResolution);
        }

        public void SetLookSensibility(float sensibility) {
            playerCamera.m_YAxis.m_MaxSpeed = sensibility;
            playerCamera.m_XAxis.m_MaxSpeed = sensibility * 400;

            lookSensibility = sensibility;
            PlayerPrefs.SetFloat("LookSensibility", lookSensibility);
        }

        public void InverseCameraVerticalAxis(bool isCameraInverted) {
            playerCamera.m_YAxis.m_InvertInput = isCameraInverted;
            PlayerPrefs.SetString("isCameraVerticalAxisInverted", isCameraInverted.ToString());            
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
