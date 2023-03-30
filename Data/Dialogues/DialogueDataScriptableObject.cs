using UnityEngine;

namespace Data {
    [CreateAssetMenu (fileName = "Data", menuName = "ScriptableObjects/dialogueDataScriptableObject", order = 1)]
    public class DialogueDataScriptableObject : ScriptableObject {
        public GenericDictionary<int, string[]> dialogue;
    }
}