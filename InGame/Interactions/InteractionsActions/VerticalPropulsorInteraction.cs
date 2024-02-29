using UnityEngine;

namespace InGame.Interactions.InteractionsActions {
    public class VerticalPropulsorInteraction : Interaction {
        [SerializeField] private float propulsionForce;
        
        public override void Activate(GameObject interactedGameObject) {
            ObjectsReference.Instance.bananaMan.GetComponent<Rigidbody>().AddForce(Vector3.up * propulsionForce, ForceMode.Acceleration);
        }
    }
}
