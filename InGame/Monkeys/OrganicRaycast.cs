using UnityEngine;
using UnityEngine.Events;

namespace InGame.Monkeys {
    public class OrganicRaycast : MonoBehaviour {
        public UnityEvent<GameObject> onDetected;
        
        private void OnTriggerEnter(Collider other) {
            onDetected.Invoke(other.gameObject);
        }
    }
}