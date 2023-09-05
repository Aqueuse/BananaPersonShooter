using Data.Characters;
using Interactions;
using Items;
using UI.InGame;
using UnityEngine;

namespace Monkeys.MiniChimps {
    public class MiniChimp : MonoBehaviour {
        [SerializeField] private Interaction interaction;
        [SerializeField] private BoxCollider interactionCollider;
        public MiniChimpDataScriptableObject miniChimpDataScriptableObject;
        
        public BubbleDialogue bubbleDialogue;

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
