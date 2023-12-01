using Game.CommandRoomPanelControls;
using UnityEngine;

namespace Interactions.InteractionsActions {
    public class CommandRoomPanelInteraction : Interact {
        public override void Activate(GameObject interactedObject) {
            CommandRoomControlPanelsManager.Instance.ShowHidePanel(interactedObject.GetComponent<CommandRoomPanel>().commandRoomPanelType);
        }
    }
}
