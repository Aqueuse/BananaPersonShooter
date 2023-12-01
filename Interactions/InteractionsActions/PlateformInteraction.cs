using Gestion.Buildables.Plateforms;
using UnityEngine;

namespace Interactions.InteractionsActions {
    public class PlateformInteraction : Interact {
        public override void Activate(GameObject interactedGameObject) {
            interactedGameObject.GetComponentInParent<Plateform>().ShowHideWorkbench();
        }
    }
}
