using Items;
using UnityEngine;

namespace Interactions.InteractionsActions {
    public class MiniChimpInteraction : MonoBehaviour {
        public static void Activate(GameObject interactedObject) {
            interactedObject.GetComponent<BubbleDialogue>().PlayDialogue();
        }
    }
}
