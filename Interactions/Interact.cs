using UnityEngine;

namespace Interactions {
    public abstract class Interact : MonoBehaviour {
        public abstract void Activate(GameObject interactedGameObject);
    }
}