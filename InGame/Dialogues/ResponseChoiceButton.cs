using TMPro;
using UnityEngine;

namespace InGame.Dialogues {
    public class ResponseChoiceButton : MonoBehaviour {
        public DialogueSetScriptableObject associatedDialogueSet;
        public TextMeshProUGUI responseText;

        public void ResponseSelect() {
            ObjectsReference.Instance.miniChimpDialoguesManager.ChooseResponse(associatedDialogueSet);
        }
    }
}
