using UnityEngine;

namespace Interactions.InteractionsActions {
    public class ManagementWorkstationInteraction : Interact {
        public override void Activate(GameObject interactedGameObject) {
            ObjectsReference.Instance.gestionMode.SwitchToGestionMode();
        }
    }
}
