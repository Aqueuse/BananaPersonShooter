using Items;
using UI.InGame;
using UnityEngine;

namespace Monkeys.MiniChimps {
    public class MiniChimp : MonoBehaviour {
        [SerializeField] private ItemInteraction itemInteraction;
        [SerializeField] private BoxCollider interactionCollider;

        public BubbleDialogue bubbleDialogue;

        private void OnTriggerEnter(Collider other) {
            bubbleDialogue.ShowBubble();
            interactionCollider.enabled = true;
        }

        private void OnTriggerExit(Collider other) {
            bubbleDialogue.HideBubble();
            itemInteraction.HideUI();
            interactionCollider.enabled = false;
        }
    }
}
