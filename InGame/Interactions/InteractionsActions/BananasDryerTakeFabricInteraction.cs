using InGame.Items.ItemsBehaviours.BuildablesBehaviours;
using UnityEngine;

namespace InGame.Interactions.InteractionsActions {
    public class BananasDryerTakeFabricInteraction : Interaction {
        public override void Activate(GameObject interactedGameObject) {
            interactedGameObject.GetComponentInParent<MeshRenderer>().enabled = false;
            interactedGameObject.GetComponent<BoxCollider>().enabled = false;

            ObjectsReference.Instance.rawMaterialInventory.AddQuantity(RawMaterialType.FABRIC, 1);
            interactedGameObject.GetComponentInParent<BananaDryerSlot>().bananasDryerBehaviour.fabricQuantity -= 1;
            interactedGameObject.GetComponentInParent<BananaDryerSlot>().addBananaPeelBoxCollider.enabled = true;
        }
    }
}
