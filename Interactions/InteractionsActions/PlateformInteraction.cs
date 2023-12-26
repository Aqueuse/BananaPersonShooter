using Gestion.BuildablesBehaviours;
using UnityEngine;

namespace Interactions.InteractionsActions {
    public class PlateformInteraction : Interaction {
        public override void Activate(GameObject interactedGameObject) {
            interactedGameObject.GetComponentInParent<PlateformBehaviour>().ShowHideWorkbench();
        }
    }
}
