using Gestion.BuildablesBehaviours;
using UnityEngine;

namespace Interactions.InteractionsActions {
    public class PlateformInteraction : Interact {
        public override void Activate(GameObject interactedGameObject) {
            interactedGameObject.GetComponentInParent<PlateformBehaviour>().ShowHideWorkbench();
        }
    }
}
