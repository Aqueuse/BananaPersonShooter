using UnityEngine;

namespace Interactions.InteractionsActions {
    public class BubbleInteraction : Interact {
        public override void Activate(GameObject interactedObject) {
            interactedObject.GetComponent<BubbleDialogue>().PlayDialogue();
        }
    }
}
