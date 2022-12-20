using TMPro;
using UnityEngine;

namespace UI {
    public class UIOptionsMenu : MonoSingleton<UIOptionsMenu> {
        [SerializeField] private GameObject saveMessage;
        [SerializeField] private TextMeshProUGUI lastSaveTimestamp;
        
        [SerializeField] private GameObject deleteSaveButton;
        [SerializeField] private GameObject yesButton;
        [SerializeField] private GameObject noButton;

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
