using UnityEngine;

namespace InGame.Interactions.InteractionsActions {
    public class ManagementWorkstationInteraction : Interaction {
        public override void Activate(GameObject interactedGameObject) {
            ObjectsReference.Instance.mainCamera.SwitchToGestionView();
        }
    }
}
