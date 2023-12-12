using Game.CommandRoomPanelControls;
using UnityEngine;

namespace Interactions.InteractionsActions {
    public class GestionCommandRoomPanelInteraction : Interact {
        public override void Activate(GameObject interactedGameObject) {
            CommandRoomControlPanelsManager.Instance.gestionPanel.SwitchToGestionPanel();
        }
    }
}
