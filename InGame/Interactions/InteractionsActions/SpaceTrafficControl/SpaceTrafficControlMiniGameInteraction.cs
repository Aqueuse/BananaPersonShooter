using InGame.CommandRoomPanelControls;
using UnityEngine;

namespace InGame.Interactions.InteractionsActions.SpaceTrafficControl {
    public class SpaceTrafficControlMiniGameInteraction : Interaction {
        public override void Activate(GameObject interactedGameObject) {
            if (ObjectsReference.Instance.bananaMan.tutorialFinished) {
                CommandRoomControlPanelsManager.Instance.ShowPanel(CommandRoomPanelType.SPACE_TRAFFIC_CONTROL);
            }
        }
    }
}
