using Interactions;
using UnityEngine;

namespace Monkeys.MiniChimps {
    public class MiniChimp : MonoBehaviour {
        [SerializeField] private Interaction interaction;
        [SerializeField] private BoxCollider interactionCollider;
        
        public BubbleDialogue bubbleDialogue;

        private void Awake() {
            if (ObjectsReference.Instance.bananaMan.tutorialFinished) {
                bubbleDialogue.SetBubbleDialogue(dialogueSet.EAT_BANANAS);
            }

            else {
                bubbleDialogue.SetBubbleDialogue(dialogueSet.REPAIR_BANANA_GUN);
            }
        }

        private void OnTriggerEnter(Collider other) {
            bubbleDialogue.ShowBubble();
            interactionCollider.enabled = true;
        }

        private void OnTriggerExit(Collider other) {
            bubbleDialogue.HideBubble();
            interaction.HideUI();
            interactionCollider.enabled = false;
        }
    }
}
