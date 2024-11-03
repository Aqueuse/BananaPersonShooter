using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.PropertyVariants.TrackedProperties;

namespace UI.InGame.CommandRoomControlPanels {
    public class UIJournalPanel : MonoBehaviour {
        [SerializeField] private List<LocalizedStringProperty> journalEntries;
        [SerializeField] private TextMeshProUGUI journalText;
    
        public int currentJournalIndex;

        private void Start() {
            ShowLastEntry();
        }

        public void ShowPreviousEntry() {
            currentJournalIndex -= 1;
            if (currentJournalIndex < 0)
                currentJournalIndex = journalEntries.Count-1;

            journalText.text = journalEntries[currentJournalIndex].LocalizedString.GetLocalizedString();
        }
    
        public void ShowNextEntry() {
            currentJournalIndex += 1;
            if (currentJournalIndex > journalEntries.Count-1)
                currentJournalIndex = 0;

            journalText.text = journalEntries[currentJournalIndex].LocalizedString.GetLocalizedString();
        }

        public void ShowLastEntry() {
            currentJournalIndex = journalEntries.Count-1;
        
            journalText.text = journalEntries[currentJournalIndex].LocalizedString.GetLocalizedString();
        }
    }
}
