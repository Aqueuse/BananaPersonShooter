using Gestion.Buildables;
using Gestion.BuildablesBehaviours;
using UnityEngine;

namespace Interactions.InteractionsActions {
    public class BananasDryerAddPeelInteraction : Interact {
        public override void Activate(GameObject interactedGameObject) {
            interactedGameObject.GetComponent<BananasDryerBehaviour>().AddBananaPeel();
        }
    }
}
