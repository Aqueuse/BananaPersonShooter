using Game;
using UnityEngine;

namespace Player {
    public class Killer : MonoBehaviour {
        private void OnTriggerEnter(Collider other) {
            if (other.tag.Equals("Player")) {
                Death.Instance.Die();
            }
        }
    }
}
