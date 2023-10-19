using UnityEngine;

namespace Interactions.InteractionsActions {
    public class VerticalPropulsorInteraction : MonoBehaviour {
        [SerializeField] private float propulsionForce;
        
        public void Activate() {
            ObjectsReference.Instance.bananaMan.GetComponent<Rigidbody>().AddForce(Vector3.up * propulsionForce, ForceMode.Acceleration);
        }
    }
}
