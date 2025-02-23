using UnityEngine;

namespace InGame.Interactions.InteractionsActions {
    public class MessageInteraction : Interaction {
        public override void Activate(GameObject interactedObject) {
            interactedObject.GetComponent<Message>().ShowHideMessage();
        }
    }
}
