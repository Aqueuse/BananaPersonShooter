using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Menus {
    public class UIOptionsMenu : MonoBehaviour {
        [SerializeField] private GameObject[] tabs;
        [SerializeField] private Image[] tabsButtons;

        [SerializeField] Color activatedColor;
        [SerializeField] Color unactivatedColor;

        private int _selectedTab;

        private void Start() {
            _selectedTab = 0;
        }

        public void Switch_to_Tab(int index) {
            for (var i=0; i<tabs.Length; i++) {
                tabs[i].SetActive(false);
                tabsButtons[i].color = unactivatedColor;
                tabsButtons[i].GetComponentInChildren<TextMeshProUGUI>().color = activatedColor;
            }
        
            tabs[index].SetActive(true);
            tabsButtons[index].color = activatedColor;
            tabsButtons[index].GetComponentInChildren<TextMeshProUGUI>().color = Color.black;

            _selectedTab = index;
        }

        public void Switch_to_Left_Tab() {
            if (_selectedTab == 0) {
                _selectedTab = 4;
                Switch_to_Tab(4);
            }

            else {
                _selectedTab--;
                Switch_to_Tab(_selectedTab);
            }
        }
        
        public void Switch_to_Right_Tab() {
            if (_selectedTab == 4) {
                _selectedTab = 0;
                Switch_to_Tab(0);
            }

            else {
                _selectedTab++;
                Switch_to_Tab(_selectedTab);
            }
        }
    }
}
