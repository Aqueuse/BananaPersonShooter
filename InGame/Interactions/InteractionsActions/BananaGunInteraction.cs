using UnityEngine;

namespace InGame.Interactions.InteractionsActions {
    public class BananaGunInteraction : Interaction {
        public override void Activate(GameObject interactedGameObject) {
            ObjectsReference.Instance.commandRoomControlPanelsManager.assembler.HideBananaGunInteractableGameObject();
            ObjectsReference.Instance.tutorial.FinishTutorial();
            
            ObjectsReference.Instance.commandRoomControlPanelsManager.miniChimp.bubbleDialogue.SetBubbleDialogue(dialogueSet.PRO_TIP);
            ObjectsReference.Instance.commandRoomControlPanelsManager.miniChimp.bubbleDialogue.PlayDialogue();
        }
    }
}
