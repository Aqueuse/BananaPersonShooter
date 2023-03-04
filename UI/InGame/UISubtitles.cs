using TMPro;
using UnityEngine;

namespace UI.InGame {
    public class UISubtitles : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI dialogueText;
    
        public void show_dialogue(string message) {
            dialogueText.text = message;
        }
    }
}
