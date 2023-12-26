using UnityEngine;

namespace Interactions {
    public abstract class Interaction : MonoBehaviour {
        public abstract void Activate(GameObject interactedGameObject);
    }
}