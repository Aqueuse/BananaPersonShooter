using UnityEngine;

namespace InGame.Interactions.InteractionsActions {
    public class BubbleInteraction : Interaction {
        public override void Activate(GameObject interactedObject) {
            interactedObject.GetComponent<BubbleDialogue>().PlayDialogue();
        }
    }
}
