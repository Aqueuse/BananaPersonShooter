using UnityEngine;

namespace InGame.Interactions.InteractionsActions {
    public class MarketingCommandRoomPanelInteraction : Interaction {
        public override void Activate(GameObject interactedGameObject) {
            ObjectsReference.Instance.commandRoomControlPanelsManager.FocusPanel((int)CommandRoomPanelType.MARKETING);
        }
    }
}