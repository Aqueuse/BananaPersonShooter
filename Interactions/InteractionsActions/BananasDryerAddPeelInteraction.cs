using Gestion.Buildables;
using UnityEngine;

namespace Interactions.InteractionsActions {
    public class BananasDryerAddPeelInteraction : Interact {
        public override void Activate(GameObject interactedGameObject) {
            interactedGameObject.GetComponent<BananasDryer>().AddBananaPeel();
        }
    }
}
