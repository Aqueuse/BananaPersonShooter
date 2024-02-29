using InGame.CommandRoomPanelControls;
using UnityEngine;

namespace InGame.Interactions.InteractionsActions {
    public class CommandRoomPanelInteraction : Interaction {
        public override void Activate(GameObject interactedObject) {
            CommandRoomControlPanelsManager.Instance.ShowHidePanel(interactedObject.GetComponent<CommandRoomPanel>().commandRoomPanelType);
        }
    }
}
