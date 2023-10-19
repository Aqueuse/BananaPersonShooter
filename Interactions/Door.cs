using Data.Door;
using Enums;
using UnityEngine;

namespace Interactions {
    public class Door : MonoBehaviour {
        public string destinationMap;
        public SpawnPoint spawnPoint;
        public DoorDataScriptableObject doorDataScriptableObject;

        [SerializeField] private Interaction interaction;

        private void Start() {
            if (spawnPoint == SpawnPoint.OUTER_SPACE) return;

            if (ObjectsReference.Instance.bananaMan.tutorialFinished) {
                GetComponent<Collider>().enabled = true;
            }
            else {
                interaction.Desactivate();
            }
        }
    }
}
