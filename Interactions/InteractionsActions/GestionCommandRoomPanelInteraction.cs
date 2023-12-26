using Game.CommandRoomPanelControls;
using UnityEngine;

namespace Interactions.InteractionsActions {
    public class GestionCommandRoomPanelInteraction : Interaction {
        public override void Activate(GameObject interactedGameObject) {
            CommandRoomControlPanelsManager.Instance.gestionPanel.SwitchToGestionPanel();
        }
    }
}
