using Game.CommandRoomPanelControls;
using UnityEngine;

namespace Items.ItemsActions {
    public class CommandRoomPanelItemAction : MonoBehaviour {
        public static void Activate(GameObject interactedObject) {
            CommandRoomControlPanelsManager.Instance.ShowHidePanel(interactedObject.GetComponent<CommandRoomPanel>().commandRoomPanelType);
        }
    }
}
