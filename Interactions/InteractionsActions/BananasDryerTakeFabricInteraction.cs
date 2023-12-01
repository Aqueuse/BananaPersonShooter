using Gestion.Buildables;
using UnityEngine;

namespace Interactions.InteractionsActions {
    public class BananasDryerTakeFabricInteraction : Interact {
        public override void Activate(GameObject interactedGameObject) {
            interactedGameObject.GetComponent<BananasDryer>().GiveFabric();
        }
    }
}
