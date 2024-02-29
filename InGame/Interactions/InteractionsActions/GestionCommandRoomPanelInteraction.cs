using InGame.CommandRoomPanelControls;
using UnityEngine;

namespace InGame.Interactions.InteractionsActions {
    public class GestionCommandRoomPanelInteraction : Interaction {
        public override void Activate(GameObject interactedGameObject) {
            CommandRoomControlPanelsManager.Instance.gestionPanel.SwitchToGestionPanel();
        }
    }
}
