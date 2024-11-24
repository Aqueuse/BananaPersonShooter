using UnityEngine;

namespace InGame.Interactions.InteractionsActions {
    public class JournalCommandRoomPanelInteraction : Interaction {
        public override void Activate(GameObject interactedGameObject) {
            ObjectsReference.Instance.commandRoomControlPanelsManager.FocusPanel((int)CommandRoomPanelType.JOURNAL);
        }
    }
}