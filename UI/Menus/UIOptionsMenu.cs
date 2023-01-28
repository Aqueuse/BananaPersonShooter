using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Menus {
    public class UIOptionsMenu : MonoSingleton<UIOptionsMenu> {
        [SerializeField] private GameObject saveMessage;
        [SerializeField] private TextMeshProUGUI lastSaveTimestamp;
        
        [SerializeField] private GameObject deleteSaveButton;
        [SerializeField] private GameObject yesButton;
        [SerializeField] private GameObject noButton;

        [SerializeField] private GameObject[] tabs;
        [SerializeField] private Image[] tabsButtons;

        [SerializeField] Color activatedColor;
        [SerializeField] Color unactivatedColor;

        private int selectedTab;

        private void Start() {
            selectedTab = 0;
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

            selectedTab = index;
        }

        public void Switch_to_Left_Tab() {
            if (selectedTab == 0) {
                selectedTab = 4;
                Switch_to_Tab(4);
            }

            else {
                selectedTab--;
                Switch_to_Tab(selectedTab);
            }
        }
        
        public void Switch_to_Right_Tab() {
            if (selectedTab == 4) {
                selectedTab = 0;
                Switch_to_Tab(0);
            }

            else {
                selectedTab++;
                Switch_to_Tab(selectedTab);
            }
        }

        public GameObject GetFirstTab() {
            return tabs[0];
        }

        public void SetActualDateAndHour(string timestanp) {
            saveMessage.SetActive(true);
            lastSaveTimestamp.text = timestanp;
        }

        public void EmptyDateAndHour() {
            saveMessage.SetActive(false);
            lastSaveTimestamp.text = "";
        }

        public void ResetGameState() {
            deleteSaveButton.SetActive(false);
            yesButton.SetActive(true);
            noButton.SetActive(true);
        }

        public void HideConfirmationMessage() {
            deleteSaveButton.SetActive(true);
            yesButton.SetActive(false);
            noButton.SetActive(false);
        }
    }
}
