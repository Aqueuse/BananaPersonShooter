using System;
using Settings;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

namespace UI {
    public class UISettings : MonoBehaviour {
        [SerializeField] private Slider musicLevelSlider;
        [SerializeField] private Slider ambianceLevelSlider;
        [SerializeField] private Slider effectsLevelSlider;
        [SerializeField] private Slider voicesLevelSlider;

        [SerializeField] private Toggle fullScreenToggle;
        [SerializeField] private Toggle vsyncToggle;

        [SerializeField] private InputRebind[] inputRebinds;
        
        [SerializeField] private Slider resolutionSlider;
        [SerializeField] private TextMeshProUGUI resolutionText;
        public Slider languageSlider;
        [SerializeField] private TextMeshProUGUI languageText;

        [SerializeField] private Slider lookSensibilitySlider;

        [SerializeField] private Toggle horizontalCameraInversionToggle;
        [SerializeField] private Toggle verticalCameraInversionToggle;

        [SerializeField] private Slider droppedFallSlider;
        
        [SerializeField] private Slider saveDelaySlider;
        [SerializeField] private TextMeshProUGUI saveDelayText;

        private string[] _languagesValues;

        private GameSettings gameSettings;

        private void Start() {
            gameSettings = ObjectsReference.Instance.gameSettings;
            
            _languagesValues = new[] { "english", "français", "spanish" };
        }

        public void ReflectAllSettingsOnUI() {
            musicLevelSlider.value = ObjectsReference.Instance.audioManager.musicLevel;
            ambianceLevelSlider.value = ObjectsReference.Instance.audioManager.ambianceLevel;
            effectsLevelSlider.value = ObjectsReference.Instance.audioManager.effectsLevel*10;
            voicesLevelSlider.value = ObjectsReference.Instance.audioManager.voicesLevel;

            fullScreenToggle.isOn = gameSettings.isFullscreen.Equals("True");
            vsyncToggle.isOn = gameSettings.isVsync.Equals("True");
            
            ReflectResolutionSettingOnUI(gameSettings.resolution);

            lookSensibilitySlider.value = gameSettings.lookSensibility;

            horizontalCameraInversionToggle.isOn = gameSettings.isCameraHorizontallyInverted;
            verticalCameraInversionToggle.isOn = gameSettings.isCameraVerticallyInverted;
    
            ReflectLanguageSettingsOnUI(gameSettings.languageIndexSelected);

            droppedFallSlider.value = Convert.ToInt32(gameSettings.areDroppedInSpaceFallingOnTheTrees);
            
            ReflectSaveDelayOnUI(Mathf.Abs(gameSettings.saveDelayMinute/60));
            
            foreach (var inputRebind in inputRebinds) {
                inputRebind.Init();
            }
            
            ObjectsReference.Instance.audioManager.PlayMusic(MusicType.HOME, 0);
        }

        public void ReflectLanguageSettingsOnUI(int languageIndexInt) {
            languageSlider.value = gameSettings.languageIndexSelected;
            languageText.text = _languagesValues[languageIndexInt];
        }

        public void ReflectResolutionSettingOnUI(int gameResolution) {
            resolutionSlider.maxValue = Screen.resolutions.Length;
            
            resolutionSlider.value = gameResolution;
            
            resolutionText.text = gameSettings.resolutions[gameResolution].ToString();
        }

        public void ReflectSaveDelayOnUI(int delay) {
            var saveDelayMinute = delay*60;

            if (delay == 0) saveDelayText.text = LocalizationSettings.StringDatabase.GetLocalizedString("UI", "never");
            else {
                saveDelayText.text = saveDelayMinute + "m";
            }

            saveDelaySlider.value = saveDelayMinute;
        }

        public void RefreshRebinds() {
            foreach (var inputRebind in inputRebinds) {
                inputRebind.Init();
            }
        }

        public void CancelAllRebinds() {
            foreach (var inputRebind in inputRebinds) {
                inputRebind.current_rebind?.Cancel();
            }
        }
    }
}
