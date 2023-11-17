using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Menus {
    public enum OPTION_TAB {
        AUDIO_VIDEO,
        KEYBOARD,
        GAMEPAD,
        GAMEPLAY,
        LANGUAGES
    }
    
    public class UIOptionsMenu : MonoBehaviour {
        public Color tabButtonActivatedColor;
        public Color tabButtonUnactivatedColor;
        
        public Color buttonActivatedColor;
        public Color buttonUnactivatedColor;
        
        [SerializeField] private GenericDictionary<OPTION_TAB, Image> optionsButtons;
        [SerializeField] private GenericDictionary<OPTION_TAB, UIOptionsTab> tabs;

        private OPTION_TAB _selectedTab;

        private void Start() {
            _selectedTab = OPTION_TAB.AUDIO_VIDEO;
        }

        public void SetActivatedButton(Image buttonImage) {
            foreach (var image in optionsButtons) {
                image.Value.color = tabButtonActivatedColor;
                image.Value.GetComponentInChildren<TextMeshProUGUI>().color = tabButtonActivatedColor;
            }

            buttonImage.color = tabButtonActivatedColor;
            buttonImage.GetComponentInChildren<TextMeshProUGUI>().color = tabButtonActivatedColor;
        }

        private void Switch_to_Tab(OPTION_TAB optionTab) {
            foreach (var tab in tabs) {
                tabs[tab.Key].Disable();
            }

            tabs[optionTab].Enable();
            
            _selectedTab = optionTab;
        }

        public void Switch_to_Left_Tab() {
            switch (_selectedTab) {
                case OPTION_TAB.AUDIO_VIDEO:
                    Switch_to_Tab(OPTION_TAB.LANGUAGES);
                    break;

                case OPTION_TAB.KEYBOARD:
                    Switch_to_Tab(OPTION_TAB.AUDIO_VIDEO);
                    break;

                case OPTION_TAB.GAMEPAD:
                    Switch_to_Tab(OPTION_TAB.KEYBOARD);
                    break;

                case OPTION_TAB.GAMEPLAY:
                    Switch_to_Tab(OPTION_TAB.GAMEPAD);
                    break;

                case OPTION_TAB.LANGUAGES:
                    Switch_to_Tab(OPTION_TAB.GAMEPLAY);
                    break;
            }
        }
        
        public void Switch_to_Right_Tab() {
            switch (_selectedTab) {
                case OPTION_TAB.AUDIO_VIDEO:
                    Switch_to_Tab(OPTION_TAB.KEYBOARD);
                    break;
                
                case OPTION_TAB.KEYBOARD:
                    Switch_to_Tab(OPTION_TAB.GAMEPAD);
                    break;

                case OPTION_TAB.GAMEPAD:
                    Switch_to_Tab(OPTION_TAB.GAMEPLAY);
                    break;
                
                case OPTION_TAB.GAMEPLAY:
                    Switch_to_Tab(OPTION_TAB.LANGUAGES);
                    break;
                
                case OPTION_TAB.LANGUAGES:
                    Switch_to_Tab(OPTION_TAB.AUDIO_VIDEO);
                    break;
            }
        }

        public void SwitchToAudioVideoTab() {
            Switch_to_Tab(OPTION_TAB.AUDIO_VIDEO);
        }
        
        public void Switch_To_Keyboard_Tab() {
            Switch_to_Tab(OPTION_TAB.KEYBOARD);
        }

        public void Switch_To_Gamepad_Tab() {
            Switch_to_Tab(OPTION_TAB.GAMEPAD);
        }

        public void Switch_To_Gameplay_Tab() {
            Switch_to_Tab(OPTION_TAB.GAMEPLAY);
        }

        public void Switch_To_languages_Tab() {
            Switch_to_Tab(OPTION_TAB.LANGUAGES);
        }
    }
}
