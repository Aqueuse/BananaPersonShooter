using Game.CommandRoomPanelControls;
using UnityEngine;

namespace Interactions.InteractionsActions {
    public class BananaGunInteraction : Interaction {
        public override void Activate(GameObject interactedGameObject) {
            CommandRoomControlPanelsManager.Instance.assembler.HideBananaGunInteractableGameObject();
            ObjectsReference.Instance.tutorial.FinishTutorial();
            
            CommandRoomControlPanelsManager.Instance.miniChimp.bubbleDialogue.SetBubbleDialogue(dialogueSet.PRO_TIP);
            CommandRoomControlPanelsManager.Instance.miniChimp.bubbleDialogue.PlayDialogue();
        }
    }
}
