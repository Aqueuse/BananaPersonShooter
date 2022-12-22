using Audio;
using Cameras;
using Enums;
using Player;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Localization.Settings;
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
        [SerializeField] private KeymapWasdRebinding[] keymapWasdRebindings;
        [SerializeField] private TMPro.TMP_Dropdown languageDropDown;

        [SerializeField] private ThirdPersonOrbitCamBasic thirdPersonOrbitCamBasic;
        
        private FullScreenMode _fullScreenMode;

        private string _isFullscreen;
        private int _resolution;
        private string _isVsync;

        public float horizontalCameraSensibility = 0.1f;
        public float verticalCameraSensibility = 0.1f;
        
        private InputActionRebindingExtensions.RebindingOperation _rebindingOperation;
        private InputAction _inputAction;
        
        private string _keymapBinding;
        public int languageIndexSelected;

        private void Start() {
            Application.targetFrameRate = 60; // fix the framerate to prevent crash on some GPU

            // 1 = match monitor refresh rate. 0 = Don't use vsync: use targetFrameRate instead.
            QualitySettings.vSyncCount = 0;
        }

        public void LoadSettings() {
            AudioManager.Instance.musicLevel = PlayerPrefs.GetFloat("musicLevel", 0.1f);
            AudioManager.Instance.ambianceLevel = PlayerPrefs.GetFloat("ambianceLevel", 0.1f);
            AudioManager.Instance.effectsLevel = PlayerPrefs.GetFloat("effectsLevel", 0.1f);

            _isFullscreen = PlayerPrefs.GetString("isFullscreen", "false");
            _isVsync = PlayerPrefs.GetString("isVSync", "false");
            _resolution = PlayerPrefs.GetInt("resolution", 3);

            languageIndexSelected = PlayerPrefs.GetInt("language", 1);
            _keymapBinding = PlayerPrefs.GetString("keymap_binding", null);

            horizontalCameraSensibility = PlayerPrefs.GetFloat("LookSensibility", 0.1f);
            verticalCameraSensibility = horizontalCameraSensibility;

            SetMusicVolume(AudioManager.Instance.musicLevel);
            SetAmbianceVolume(AudioManager.Instance.ambianceLevel);
            SetEffectVolume(AudioManager.Instance.effectsLevel);
            
            BananaMan.Instance.GetComponent<PlayerInput>().actions.LoadBindingOverridesFromJson(_keymapBinding);
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[languageIndexSelected];
        
            // reflects values on UI 
            ToggleFullscreen(_isFullscreen.Equals("true"));
            ToggleVSync(_isVsync.Equals("true"));
            SetResolution(_resolution);

            musicLevelSlider.value = AudioManager.Instance.musicLevel;
            ambianceLevelSlider.value = AudioManager.Instance.ambianceLevel;
            effectsLevelSlider.value = AudioManager.Instance.effectsLevel;

            fullScreenToggle.isOn = _isFullscreen.Equals("true");
            vsyncToggle.isOn = _isVsync.Equals("true");
            resolutionDropDown.value = _resolution;
        
            languageDropDown.value = languageIndexSelected;
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

            PlayerPrefs.SetString("isFullscreen", isGameFullscreen ? "true" : "false");
        }

        public void ToggleVSync(bool isGameVsync) {
            QualitySettings.vSyncCount = isGameVsync ? 1 : 0;

            PlayerPrefs.SetString("isVSync", isGameVsync ? "true" : "false");
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
            horizontalCameraSensibility = sensibility;
            verticalCameraSensibility = sensibility;
            
            PlayerPrefs.SetFloat("LookSensibility", sensibility);

            if (thirdPersonOrbitCamBasic.isActiveAndEnabled) {
                thirdPersonOrbitCamBasic.horizontalSensibility = sensibility / 5;
                thirdPersonOrbitCamBasic.verticalSensibility = sensibility / 5;
            }
        }
        
        public void ResetAllBindings() {
            foreach (var keymapRebinding in keymapRebindings) {
                keymapRebinding.ResetBinding();
            }

            foreach (var keymapWasdRebinding in keymapWasdRebindings) {
                keymapWasdRebinding.ResetBinding();
            }
            
            PlayerPrefs.SetString("keymap_binding", BananaMan.Instance.GetComponent<PlayerInput>().actions.SaveBindingOverridesAsJson());
        }
    }
}
