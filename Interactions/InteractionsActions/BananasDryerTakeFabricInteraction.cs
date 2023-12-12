using Gestion.Buildables;
using Gestion.BuildablesBehaviours;
using UnityEngine;

namespace Interactions.InteractionsActions {
    public class BananasDryerTakeFabricInteraction : Interact {
        public override void Activate(GameObject interactedGameObject) {
            interactedGameObject.GetComponent<BananasDryerBehaviour>().GiveFabric();
        }
    }
}
