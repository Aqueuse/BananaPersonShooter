using UnityEngine;

namespace Building {
    public class AutoDestruction : MonoBehaviour {
        public void AutoDestruct() {
            Destroy(gameObject);
        }
    }
}
