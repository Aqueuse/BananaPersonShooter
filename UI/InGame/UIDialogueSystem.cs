using TMPro;
using UnityEngine;

namespace UI.InGame {
    public class UIDialogueSystem : MonoBehaviour {
        [SerializeField] private CanvasGroup dialogueTextCanvasGroup;
        [SerializeField] private TextMeshProUGUI dialogueText;
    
        public void show_dialogue(string message) {
            dialogueText.text = message;
            UIManager.Instance.Set_active(dialogueTextCanvasGroup, true);
        }

        public void hide_dialogue() {
            dialogueText.text = "";
            UIManager.Instance.Set_active(dialogueTextCanvasGroup, false);
        }
    }
}
