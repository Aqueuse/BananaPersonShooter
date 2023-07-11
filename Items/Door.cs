using Enums;
using UnityEngine;

namespace Items {
    public class Door : MonoBehaviour {
        public string destinationMap;
        public SpawnPoint spawnPoint;

        private void Start() {
            if (ObjectsReference.Instance.gameData.bananaManSavedData.playerAdvancements.Contains(AdvancementState.ASPIRE_SOMETHING)) {
                GetComponent<BoxCollider>().enabled = true;
            }
            else {
                GetComponent<ItemStatic>().Desactivate();
            }
        }
    }
}
