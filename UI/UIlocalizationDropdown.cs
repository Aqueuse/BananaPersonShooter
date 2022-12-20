using System.Collections;
using System.Collections.Generic;
using Settings;
using UnityEngine;
using UnityEngine.Localization.Settings;
using TMP_Dropdown = TMPro.TMP_Dropdown;

namespace UI {
    public class UIlocalizationDropdown : MonoBehaviour {
        [SerializeField] private TMP_Dropdown dropdown;

        IEnumerator Start() {
            // Wait for the localization system to initialize, loading Locales, preloading etc.
            yield return LocalizationSettings.InitializationOperation;

            // Generate list of available Locales
            var options = new List<TMP_Dropdown.OptionData>();
            int selected = 0;
            for (int i = 0; i < LocalizationSettings.AvailableLocales.Locales.Count; ++i) {
                var locale = LocalizationSettings.AvailableLocales.Locales[i];
                if (LocalizationSettings.SelectedLocale == locale)
                    selected = i;
                options.Add(new TMP_Dropdown.OptionData(locale.name));
            }

            dropdown.options = options;

            dropdown.value = selected;
            dropdown.onValueChanged.AddListener(LocaleSelected);
        }

        static void LocaleSelected(int index) {
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[index];
            GameSettings.Instance.languageIndexSelected = index;
            PlayerPrefs.SetInt("language", index);
        }
    }
}