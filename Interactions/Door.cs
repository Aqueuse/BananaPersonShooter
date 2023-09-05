using Data.Door;
using UnityEngine;

namespace Interactions {
    public class Door : MonoBehaviour {
        public string destinationMap;
        public SpawnPoint spawnPoint;
        public DoorDataScriptableObject doorDataScriptableObject;

        [SerializeField] private Interaction interaction;

        private void Start() {
            if (ObjectsReference.Instance.bananaMan.tutorialFinished) {
                GetComponent<Collider>().enabled = true;
            }
            else {
                interaction.Desactivate();
            }
        }
    }
}
