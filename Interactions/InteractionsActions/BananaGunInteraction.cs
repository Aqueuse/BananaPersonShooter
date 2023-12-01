using Game.CommandRoomPanelControls;
using UnityEngine;

namespace Interactions.InteractionsActions {
    public class BananaGunInteraction : Interact {
        public override void Activate(GameObject interactedGameObject) {
            CommandRoomControlPanelsManager.Instance.assembler.HideBananaGunInteractableGameObject();
            ObjectsReference.Instance.tutorial.FinishTutorial();
            
            CommandRoomControlPanelsManager.Instance.miniChimp.bubbleDialogue.SetBubbleDialogue(dialogueSet.EAT_BANANAS);
            CommandRoomControlPanelsManager.Instance.miniChimp.bubbleDialogue.PlayDialogue();
        }
    }
}
