using UnityEngine;

namespace InGame.Interactions.InteractionsActions {
    public class SpaceTrafficControlInteraction : Interaction {
        public override void Activate(GameObject interactedGameObject) {
            ObjectsReference.Instance.commandRoomControlPanelsManager.FocusPanel((int)CommandRoomPanelType.SPACE_TRAFFIC_CONTROL);
        }
    }
}
