using Enums;
using UnityEngine;

namespace UI.Menus {
    public class UIOptionsMenu : MonoBehaviour {
        [SerializeField] private GenericDictionary<UISchemaSwitchType, UIOptionsTab> tabs;

        public Color tabButtonActivatedColor;
        public Color tabButtonUnactivatedColor;

        public Color buttonActivatedColor;
        public Color buttonUnactivatedColor;
        
        private UISchemaSwitchType _selectedTab;

        private void Start() {
            _selectedTab = UISchemaSwitchType.AUDIOVIDEO_TAB;
        }

        public void Switch_to_Tab(UISchemaSwitchType uiSchemaSwitchType) {
            ObjectsReference.Instance.inputManager.uiSchemaSwitcher.SwitchUISchema(uiSchemaSwitchType);
            
            foreach (var tab in tabs) {
                tabs[tab.Key].Disable();
            }

            tabs[uiSchemaSwitchType].Enable();
            _selectedTab = uiSchemaSwitchType;
        }

        public void Switch_to_Left_Tab() {
            switch (_selectedTab) {
                case UISchemaSwitchType.AUDIOVIDEO_TAB:
                    Switch_to_Tab(UISchemaSwitchType.LANGUAGES_TAB);
                    break;

                case UISchemaSwitchType.KEYBOARD_TAB:
                    Switch_to_Tab(UISchemaSwitchType.AUDIOVIDEO_TAB);
                    break;

                case UISchemaSwitchType.GAMEPAD_TAB:
                    Switch_to_Tab(UISchemaSwitchType.KEYBOARD_TAB);
                    break;

                case UISchemaSwitchType.ACCESSIBILITY_TAB:
                    Switch_to_Tab(UISchemaSwitchType.GAMEPAD_TAB);
                    break;

                case UISchemaSwitchType.LANGUAGES_TAB:
                    Switch_to_Tab(UISchemaSwitchType.ACCESSIBILITY_TAB);
                    break;
            }
        }
        
        public void Switch_to_Right_Tab() {
            switch (_selectedTab) {
                case UISchemaSwitchType.AUDIOVIDEO_TAB:
                    Switch_to_Tab(UISchemaSwitchType.KEYBOARD_TAB);
                    break;
                
                case UISchemaSwitchType.KEYBOARD_TAB:
                    Switch_to_Tab(UISchemaSwitchType.GAMEPAD_TAB);
                    break;

                case UISchemaSwitchType.GAMEPAD_TAB:
                    Switch_to_Tab(UISchemaSwitchType.ACCESSIBILITY_TAB);
                    break;
                
                case UISchemaSwitchType.ACCESSIBILITY_TAB:
                    Switch_to_Tab(UISchemaSwitchType.LANGUAGES_TAB);
                    break;
                
                case UISchemaSwitchType.LANGUAGES_TAB:
                    Switch_to_Tab(UISchemaSwitchType.AUDIOVIDEO_TAB);
                    break;
            }
        }

        public void Switch_To_Audio_Video_Tab() {
            Switch_to_Tab(UISchemaSwitchType.AUDIOVIDEO_TAB);
        }
        
        public void Switch_To_Keyboard_Tab() {
            Switch_to_Tab(UISchemaSwitchType.KEYBOARD_TAB);
        }

        public void Switch_To_Gamepad_Tab() {
            Switch_to_Tab(UISchemaSwitchType.GAMEPAD_TAB);
        }

        public void Switch_To_Accessibility_Tab() {
            Switch_to_Tab(UISchemaSwitchType.ACCESSIBILITY_TAB);
        }

        public void Switch_To_languages_Tab() {
            Switch_to_Tab(UISchemaSwitchType.LANGUAGES_TAB);
        }
    }
}
