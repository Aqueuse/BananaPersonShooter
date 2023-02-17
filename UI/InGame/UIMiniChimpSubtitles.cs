using TMPro;
using UnityEngine;

namespace UI.InGame {
    public class UIMiniChimpSubtitles : MonoBehaviour {
        [SerializeField] private CanvasGroup subtitlesTextCanvasGroup;
        [SerializeField] private TextMeshProUGUI dialogueText;
    
        public void show_dialogue(string message) {
            dialogueText.text = message;
            UIManager.Instance.Set_active(subtitlesTextCanvasGroup, true);
        }

        public void hide_dialogue() {
            dialogueText.text = "";
            UIManager.Instance.Set_active(subtitlesTextCanvasGroup, false);
        }
    }
}
