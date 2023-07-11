using System.Collections;
using System.Collections.Generic;
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
            var selected = 0;
            for (var i = 0; i < LocalizationSettings.AvailableLocales.Locales.Count; ++i) {
                var locale = LocalizationSettings.AvailableLocales.Locales[i];
                if (LocalizationSettings.SelectedLocale == locale)
                    selected = i;
                options.Add(new TMP_Dropdown.OptionData(locale.name));
            }

            dropdown.options = options;

            dropdown.value = selected;
            dropdown.onValueChanged.AddListener(LocaleSelected);
            
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[ObjectsReference.Instance.gameSettings.languageIndexSelected];
            dropdown.value = ObjectsReference.Instance.gameSettings.languageIndexSelected;
        }

        static void LocaleSelected(int index) {
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[index];
            ObjectsReference.Instance.gameSettings.languageIndexSelected = index;
            ObjectsReference.Instance.gameSettings.prefs.SetInt("language", index);
            ObjectsReference.Instance.gameSettings.prefs.Save();
        }
    }
}