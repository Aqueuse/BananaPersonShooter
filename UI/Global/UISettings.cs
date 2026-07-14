using Settings;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

namespace UI.Global {
    public class UISettings : MonoBehaviour {
        [SerializeField] private Slider musicLevelSlider;
        [SerializeField] private Slider ambianceLevelSlider;
        [SerializeField] private Slider effectsLevelSlider;
        [SerializeField] private Slider voicesLevelSlider;

        [SerializeField] private TMP_Dropdown windowStyleDropdown;
        [SerializeField] private Toggle vsyncToggle;

        [SerializeField] private InputRebind[] inputRebinds;
        
        public Slider languageSlider;
        [SerializeField] private TextMeshProUGUI languageText;

        [SerializeField] private Slider horizontalLookSensibilitySlider;
        [SerializeField] private Slider verticalLookSensibilitySlider;
        
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

            vsyncToggle.isOn = gameSettings.isVsync.Equals("True");
            
            ReflectResolutionSettingOnUI(gameSettings.windowStyle);

            horizontalLookSensibilitySlider.value = gameSettings.horizontalLookSensibility;
            verticalLookSensibilitySlider.value = gameSettings.verticalLookSensibility;
            
            ReflectLanguageSettingsOnUI(gameSettings.languageIndexSelected);
            
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

        private void ReflectResolutionSettingOnUI(int windowStyleIndex) {
            windowStyleDropdown.SetValueWithoutNotify(windowStyleIndex);
        }

        public void ReflectSaveDelayOnUI(int delay) {
            if (delay == 0) saveDelayText.text = LocalizationSettings.StringDatabase.GetLocalizedString("UI", "never");
            else {
                saveDelayText.text = delay + "mn";
            }

            saveDelaySlider.value = delay;
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
