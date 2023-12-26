using Game.CommandRoomPanelControls;
using UnityEngine;

namespace Interactions.InteractionsActions {
    public class CommandRoomPanelInteraction : Interaction {
        public override void Activate(GameObject interactedObject) {
            CommandRoomControlPanelsManager.Instance.ShowHidePanel(interactedObject.GetComponent<CommandRoomPanel>().commandRoomPanelType);
        }
    }
}
