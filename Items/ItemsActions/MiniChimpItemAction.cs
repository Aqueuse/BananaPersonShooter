using Monkeys.MiniChimps;
using UnityEngine;

namespace Items.ItemsActions {
    public class MiniChimpItemAction : MonoBehaviour {
        public static void Activate(GameObject interactedObject) {
            interactedObject.GetComponent<MiniChimpDialogue>().Play();
        }
    }
}
