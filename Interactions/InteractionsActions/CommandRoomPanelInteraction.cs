using Game.CommandRoomPanelControls;
using UnityEngine;

namespace Interactions.InteractionsActions {
    public class CommandRoomPanelInteraction : MonoBehaviour {
        public static void Activate(GameObject interactedObject) {
            CommandRoomControlPanelsManager.Instance.ShowHidePanel(interactedObject.GetComponent<CommandRoomPanel>().commandRoomPanelType);
        }
    }
}
