using Enums;
using UnityEngine;

namespace Items {
    public class Door : MonoBehaviour {
        public string destinationMap;
        public SpawnPoint spawnPoint;

        private void Start() {
            if (ObjectsReference.Instance.bananaMan.tutorialFinished) {
                GetComponent<BoxCollider>().enabled = true;
            }
            else {
                GetComponent<ItemStatic>().Desactivate();
            }
        }
    }
}
