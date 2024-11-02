using System;
using System.Collections.Generic;
using UnityEngine;

namespace InGame.Dialogues {
    [Serializable]
    public struct StringList {
        public List<string> stringList;
    }
    
    [CreateAssetMenu (fileName = "Properties", menuName = "ScriptableObjects/DialogueSetScriptableObject", order = 4)]
    public class DialogueSetScriptableObject : ScriptableObject {
        public List<StringList> sentencesSequences;

        public GenericDictionary<StringList, DialogueSetScriptableObject> dialogueSetScriptableObjectsByResponsesChoices;
    }
}
