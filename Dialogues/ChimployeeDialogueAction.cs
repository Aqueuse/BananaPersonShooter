using UI.InGame.Chimployee;
using UnityEngine;

namespace Dialogues {
    public class ChimployeeDialogueAction : MonoBehaviour {
        [SerializeField] private ChimployeeDialogue _chimployeeDialogue;
    
        void Start() {
            switch (_chimployeeDialogue) {
                case ChimployeeDialogue.chimployee_first_interaction:
                    UIChimployee.AuthorizeDoorsAccess();
                    break;
            }
        }
    }
}
