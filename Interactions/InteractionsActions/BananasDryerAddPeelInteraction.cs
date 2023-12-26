using Gestion.BuildablesBehaviours;
using UnityEngine;

namespace Interactions.InteractionsActions {
    public class BananasDryerAddPeelInteraction : Interaction {
        public override void Activate(GameObject interactedGameObject) {
            interactedGameObject.GetComponentInParent<BananaDryerSlot>().AddBananaPeel();
        }
    }
}
