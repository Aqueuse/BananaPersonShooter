using UnityEngine;

namespace Player {
    public class Killer : MonoBehaviour {
        private void OnTriggerEnter(Collider other) {
            if (other.tag.Equals("Player")) {
                ObjectsReference.Instance.death.Die();
            }
        }
    }
}
