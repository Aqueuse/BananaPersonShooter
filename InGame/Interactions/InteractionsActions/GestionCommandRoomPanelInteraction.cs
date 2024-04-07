using UnityEngine;

namespace InGame.Interactions.InteractionsActions {
    public class GestionCommandRoomPanelInteraction : Interaction {
        public override void Activate(GameObject interactedGameObject) {
            ObjectsReference.Instance.commandRoomControlPanelsManager.FocusPanel(CommandRoomPanelType.GESTION);
        }
    }
}
