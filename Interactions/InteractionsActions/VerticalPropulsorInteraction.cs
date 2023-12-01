using UnityEngine;

namespace Interactions.InteractionsActions {
    public class VerticalPropulsorInteraction : Interact {
        [SerializeField] private float propulsionForce;
        
        public override void Activate(GameObject interactedGameObject) {
            ObjectsReference.Instance.bananaMan.GetComponent<Rigidbody>().AddForce(Vector3.up * propulsionForce, ForceMode.Acceleration);
        }
    }
}
