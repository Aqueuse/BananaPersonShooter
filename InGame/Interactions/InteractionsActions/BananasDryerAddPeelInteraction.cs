using InGame.Items.ItemsBehaviours.BuildablesBehaviours;
using UnityEngine;

namespace InGame.Interactions.InteractionsActions {
    public class BananasDryerAddPeelInteraction : Interaction {
        public override void Activate(GameObject interactedGameObject) {
            interactedGameObject.GetComponentInParent<BananaDryerSlot>().AddBananaPeel();
        }
    }
}
