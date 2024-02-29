using InGame.Items.ItemsBehaviours.BuildablesBehaviours;
using UnityEngine;

namespace InGame.Interactions.InteractionsActions {
    public class PlateformInteraction : Interaction {
        public override void Activate(GameObject interactedGameObject) {
            interactedGameObject.GetComponentInParent<PlateformBehaviour>().ShowHideWorkbench();
        }
    }
}
