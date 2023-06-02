using UnityEngine;

namespace Data.Dialogues {
    [CreateAssetMenu (fileName = "Data", menuName = "ScriptableObjects/dialogueDataScriptableObject", order = 1)]
    public class DialogueDataScriptableObject : ScriptableObject {
        public GenericDictionary<int, string[]> dialogue;
    }
}