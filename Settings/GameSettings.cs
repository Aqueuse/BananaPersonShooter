using System.Collections;
using Cinemachine;
using UI;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Localization.Settings;
using UnityEngine.Rendering;

namespace Settings {
    public class GameSettings : MonoBehaviour {
        public InputActionAsset inputActionAsset;
        [SerializeField] private CinemachineFreeLook playerCamera;

        public string isFullscreen;
        public int resolution;
        public string isVsync;

        public int languageIndexSelected;
        public float lookSensibility;

        public bool isCameraVerticallyInverted;
        public bool isCameraHorizontallyInverted;

        public int saveDelayMinute;
        public JsonPlayerPrefs prefs;
        private UISettings uiSettings;
        private FullScreenMode _fullScreenMode;
        public Resolution[] resolutions;

        private delegate void OnSettingsLoadedCompleted();
        private event OnSettingsLoadedCompleted onSettingsLoadedCompleted;
        
        private IEnumerator Start() {
            uiSettings = ObjectsReference.Instance.uiSettings;
            onSettingsLoadedCompleted += uiSettings.ReflectAllSettingsOnUI;
            
            Application.targetFrameRate = 60; // fix the framerate to prevent crash on some GPU

            resolutions = Screen.resolutions;
            
            // 1 = match monitor refresh rate. 0 = Don't use vsync: use targetFrameRate instead.
            QualitySettings.vSyncCount = 0;
            
            GraphicsSettings.useScriptableRenderPipelineBatching = true;
            
            yield return LocalizationSettings.InitializationOperation;
            LoadSettings();
        }
        
        public void LoadSettings() {
            prefs = new JsonPlayerPrefs(ObjectsReference.Instance.gameSave.gamePath + "/preferences.json");
            
            ObjectsReference.Instance.audioManager.musicLevel = prefs.GetFloat("musicLevel", 0.5f);
            ObjectsReference.Instance.audioManager.voicesLevel = prefs.GetFloat("voicesLevel", 0.5f);
            ObjectsReference.Instance.audioManager.effectsLevel = prefs.GetFloat("effectsLevel", 0.5f);
            ObjectsReference.Instance.audioManager.ambianceLevel = prefs.GetFloat("ambianceLevel", 0.5f);
            
            isFullscreen = prefs.GetString("isFullscreen", "False");
            isVsync = prefs.GetString("isVSync", "True");
            resolution = prefs.GetInt("resolution");
            
            if (resolution > Screen.resolutions.Length) resolution = 0;
            
            if (prefs.HasKey("inputBindings")) {
                inputActionAsset.LoadBindingOverridesFromJson(prefs.GetString("inputBindings"));
            }
            
            lookSensibility = prefs.GetFloat("LookSensibility", 0.6f);
            
            isCameraVerticallyInverted = prefs.GetString("isCameraVerticalAxisInverted", "False").Equals("True");
            isCameraHorizontallyInverted = prefs.GetString("isCameraHorizontalAxisInverted", "False").Equals("True");
            
            saveDelayMinute = prefs.GetInt("saveDelay", 300);
            
            languageIndexSelected = prefs.GetInt("language", 1);
            ObjectsReference.Instance.cinematiques.SetCinematiqueVolume();
            ObjectsReference.Instance.audioManager.SetVolume(AudioSourcesType.VOICE, ObjectsReference.Instance.audioManager.voicesLevel);
            ObjectsReference.Instance.audioManager.SetVolume(AudioSourcesType.MUSIC, ObjectsReference.Instance.audioManager.musicLevel);
            ObjectsReference.Instance.audioManager.SetVolume(AudioSourcesType.AMBIANCE, ObjectsReference.Instance.audioManager.ambianceLevel);
            ObjectsReference.Instance.audioManager.SetVolume(AudioSourcesType.EFFECT, ObjectsReference.Instance.audioManager.effectsLevel);
            
            playerCamera.m_YAxis.m_InvertInput = !isCameraVerticallyInverted; // Cinemachine is naturally inverted
            playerCamera.m_XAxis.m_InvertInput = isCameraHorizontallyInverted; // Cinemachine is naturally inverted
            
            playerCamera.m_YAxis.m_MaxSpeed = lookSensibility;
            playerCamera.m_XAxis.m_MaxSpeed = lookSensibility * 400;
            
            Screen.fullScreenMode = isFullscreen.Equals("True") ? FullScreenMode.ExclusiveFullScreen : FullScreenMode.Windowed;
            _fullScreenMode = Screen.fullScreenMode;
            QualitySettings.vSyncCount = isVsync.Equals("True") ? 1 : 0;
            
            Screen.SetResolution(
                resolutions[resolution].width,
                resolutions[resolution].height,
                _fullScreenMode,
                Screen.currentResolution.refreshRate);
            
            var languageIndexInt = prefs.GetInt("language");
            languageIndexSelected = languageIndexInt;
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[languageIndexInt];
            
            onSettingsLoadedCompleted?.Invoke();
        }
        
        public void ResetOptions() {
            if (ObjectsReference.Instance.uiManager.canvasGroupsByUICanvasType[UICanvasGroupType.OPTIONS].alpha < 1) return;

            prefs.DeleteAll();
            prefs.Save();
            
            LoadSettings();
        }

        public void ResetKeyboardBinding() {
            if (ObjectsReference.Instance.uiManager.canvasGroupsByUICanvasType[UICanvasGroupType.OPTIONS].alpha < 1) return;
            
            foreach (var actionMap in inputActionAsset.actionMaps) {
                foreach (var action in actionMap.actions) {
                    for (var idBinding = 0;
                         idBinding < action.bindings.Count; idBinding++)
                    {
                        Debug.Log($" groups: {action.bindings[idBinding].groups}");
                        Debug.Log($" path : {action.bindings[idBinding].path}");
                        Debug.Log($" action : {action.bindings[idBinding].action}");

                        var mask = new InputBinding
                        {
                            path = "<Keyboard>/*",
                            action = action.bindings[idBinding].action
                        };

                        if (action.bindings[idBinding].Matches(mask)) {
                            action.RemoveBindingOverride(action.bindings[idBinding]);
                        }
                    }
                    
                    action.RemoveBindingOverride(new InputBinding());
                }
            }
            
            prefs.SetString("inputBindings", inputActionAsset.SaveBindingOverridesAsJson());
            prefs.Save();

            uiSettings.RefreshRebinds();
        }

        public void ResetGamepadBinding() {
            if (ObjectsReference.Instance.uiManager.canvasGroupsByUICanvasType[UICanvasGroupType.OPTIONS].alpha < 1) return;
            
            if (ObjectsReference.Instance.uiManager.canvasGroupsByUICanvasType[UICanvasGroupType.OPTIONS].alpha < 1) return;
            
            foreach (var actionMap in inputActionAsset.actionMaps) {
                foreach (var action in actionMap.actions) {
                    var index = action.GetBindingIndex(group: "Gamepad");
                    if (index >= 0) action.RemoveBindingOverride(index);
                }
            }
            
            prefs.SetString("inputBindings", inputActionAsset.SaveBindingOverridesAsJson());
            prefs.Save();
        }

        public void SetMusicVolume(float level) {
            if (ObjectsReference.Instance.uiManager.canvasGroupsByUICanvasType[UICanvasGroupType.OPTIONS].alpha < 1) return;

            ObjectsReference.Instance.audioManager.SetVolume(AudioSourcesType.MUSIC, level);
            ObjectsReference.Instance.cinematiques.SetCinematiqueVolume();
            
            prefs.SetFloat("musicLevel", level);
            prefs.Save();
        }
        
        public void SetVoicesVolume(float level) {
            if (ObjectsReference.Instance.uiManager.canvasGroupsByUICanvasType[UICanvasGroupType.OPTIONS].alpha < 1) return;

            ObjectsReference.Instance.audioManager.SetVolume(AudioSourcesType.VOICE, level);
        
            prefs.SetFloat("voicesLevel", level);
            prefs.Save();
        }
        
        public void SetEffectLevelFromUISlider(float level) {
            if (ObjectsReference.Instance.uiManager.canvasGroupsByUICanvasType[UICanvasGroupType.OPTIONS].alpha < 1) return;

            SetEffectVolume(level/10);
        }
        
        private void SetEffectVolume(float level) {
            if (ObjectsReference.Instance.uiManager.canvasGroupsByUICanvasType[UICanvasGroupType.OPTIONS].alpha < 1) return;

            ObjectsReference.Instance.audioManager.SetVolume(AudioSourcesType.EFFECT, level);
        
            prefs.SetFloat("effectsLevel", level);
            prefs.Save();
        }
        
        public void SetAmbianceVolume(float level) {
            if (ObjectsReference.Instance.uiManager.canvasGroupsByUICanvasType[UICanvasGroupType.OPTIONS].alpha < 1) return;

            ObjectsReference.Instance.audioManager.SetVolume(AudioSourcesType.AMBIANCE, level);
            
            prefs.SetFloat("ambianceLevel", level);
            prefs.Save();
        }
        
        public void ToggleFullscreen(bool isGameFullscreen) {
            if (ObjectsReference.Instance.uiManager.canvasGroupsByUICanvasType[UICanvasGroupType.OPTIONS].alpha < 1) return;

            Screen.fullScreenMode = isGameFullscreen ? FullScreenMode.ExclusiveFullScreen : FullScreenMode.Windowed;
            _fullScreenMode = Screen.fullScreenMode;
        
            prefs.SetString("isFullscreen", isGameFullscreen ? "True" : "False");
            prefs.Save();
        }
        
        public void ToggleVSync(bool isGameVsync) {
            if (ObjectsReference.Instance.uiManager.canvasGroupsByUICanvasType[UICanvasGroupType.OPTIONS].alpha < 1) return;

            QualitySettings.vSyncCount = isGameVsync ? 1 : 0;
        
            prefs.SetString("isVSync", isGameVsync ? "True" : "False");
            
            prefs.Save();
        }
        
        public void SetResolution(int gameResolution) {
            if (ObjectsReference.Instance.uiManager.canvasGroupsByUICanvasType[UICanvasGroupType.OPTIONS].alpha < 1) return;
            
            Screen.SetResolution(
                resolutions[gameResolution].width,
                resolutions[gameResolution].height,
                _fullScreenMode,
                Screen.currentResolution.refreshRate);
            
            prefs.SetInt("resolution", gameResolution);
            
            prefs.Save();
            
            uiSettings.ReflectResolutionSettingOnUI(gameResolution);
        }
        
        public void SetLowerResolution() {
            if (ObjectsReference.Instance.uiManager.canvasGroupsByUICanvasType[UICanvasGroupType.OPTIONS].alpha < 1) return;

            if (resolution > 0) resolution--;
            SetResolution(resolution);
        }
        
        public void SetHigherResolution() {
            if (ObjectsReference.Instance.uiManager.canvasGroupsByUICanvasType[UICanvasGroupType.OPTIONS].alpha < 1) return;

            if (resolution < LocalizationSettings.AvailableLocales.Locales.Count-1) resolution++;
            SetResolution(resolution);
        }

        public void SetKeysBinding() {
            if (ObjectsReference.Instance.uiManager.canvasGroupsByUICanvasType[UICanvasGroupType.OPTIONS].alpha < 1) return;
            
            prefs.SetString("inputBindings", inputActionAsset.SaveBindingOverridesAsJson());
            prefs.Save();
        }
        
        public void SetLookSensibility(float sensibility) {
            if (ObjectsReference.Instance.uiManager.canvasGroupsByUICanvasType[UICanvasGroupType.OPTIONS].alpha < 1) return;

            playerCamera.m_YAxis.m_MaxSpeed = sensibility;
            playerCamera.m_XAxis.m_MaxSpeed = sensibility * 400;

            lookSensibility = sensibility;
            prefs.SetFloat("LookSensibility", lookSensibility);
        }
        
        public void InverseCameraVerticalAxis(bool isCameraInverted) {
            if (ObjectsReference.Instance.uiManager.canvasGroupsByUICanvasType[UICanvasGroupType.OPTIONS].alpha < 1) return;

            playerCamera.m_YAxis.m_InvertInput = !isCameraInverted; // Cinemachine is naturally inverted
            prefs.SetString("isCameraVerticalAxisInverted", isCameraInverted.ToString());
        
            prefs.Save();
        }
        
        public void InverseCameraHorizontalAxis(bool isCameraInverted) {
            if (ObjectsReference.Instance.uiManager.canvasGroupsByUICanvasType[UICanvasGroupType.OPTIONS].alpha < 1) return;

            playerCamera.m_XAxis.m_InvertInput = isCameraInverted; // Cinemachine is naturally inverted
            prefs.SetString("isCameraHorizontalAxisInverted", isCameraInverted.ToString());
        
            prefs.Save();
        }
        
        public void SetSaveDelay(float delay) {
            if (ObjectsReference.Instance.uiManager.canvasGroupsByUICanvasType[UICanvasGroupType.OPTIONS].alpha < 1) return;

            prefs.SetInt("saveDelay", (int)delay*60);
        
            prefs.Save();
            
            uiSettings.ReflectSaveDelayOnUI((int)delay);
        }

        public void Setlanguage(float languageIndex) {
            if (ObjectsReference.Instance.uiManager.canvasGroupsByUICanvasType[UICanvasGroupType.OPTIONS].alpha < 1) return;
            
            var languageIndexInt = (int)languageIndex;
            
            languageIndexSelected = languageIndexInt;
            
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[languageIndexInt];
            
            prefs.SetInt("language", languageIndexInt);
            prefs.Save();
            
            uiSettings.ReflectLanguageSettingsOnUI(languageIndexInt);
        }

        public void SetLowerLanguage() {
            if (ObjectsReference.Instance.uiManager.canvasGroupsByUICanvasType[UICanvasGroupType.OPTIONS].alpha < 1) return;
            
            if (languageIndexSelected > 0) languageIndexSelected--;
            Setlanguage(languageIndexSelected);
        }
        
        public void SetHigherLanguage() {
            if (ObjectsReference.Instance.uiManager.canvasGroupsByUICanvasType[UICanvasGroupType.OPTIONS].alpha < 1) return;
        
            if (languageIndexSelected < Screen.resolutions.Length-1) languageIndexSelected++;
            Setlanguage(languageIndexSelected);
        }

        public Resolution GetCurrentResolution() {
            return resolutions[resolution];
        }
    }
}
