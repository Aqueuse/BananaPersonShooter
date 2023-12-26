using Interactions;
using UI.InGame;
using UnityEngine;

namespace Game.Monkeys.MiniChimps {
    public class MiniChimp : MonoBehaviour {
        [SerializeField] private UInteraction uInteraction;
        [SerializeField] private BoxCollider interactionCollider;
        
        public BubbleDialogue bubbleDialogue;

        private void Awake() {
            if (ObjectsReference.Instance.bananaMan.tutorialFinished) {
                bubbleDialogue.SetBubbleDialogue(dialogueSet.PRO_TIP);
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
            uInteraction.HideUI();
            interactionCollider.enabled = false;
        }
    }
}
