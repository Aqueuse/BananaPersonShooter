using UnityEngine;

namespace Dialogues {
    [CreateAssetMenu (fileName = "Data", menuName = "ScriptableObjects/dialogueDataScriptableObject", order = 1)]
    public class DialogueDataScriptableObject : ScriptableObject {
        [Multiline] public string[] dialogue;
    }
}