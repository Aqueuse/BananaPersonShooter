using UnityEngine;

namespace InGame.Interactions.InteractionsActions {
    public class TPCommandRoomInteraction : Interaction {
        private Transform TPCommandRoomTransform;

        private void Start() {
            TPCommandRoomTransform = ObjectsReference.Instance.gameManager.spawnPointsBySpawnType[SpawnPoint.TP_COMMAND_ROOM];
        }

        public override void Activate(GameObject interactedGameObject) {
            ObjectsReference.Instance.bananaMan.transform.position = TPCommandRoomTransform.position;
            ObjectsReference.Instance.bananaMan.transform.rotation = Quaternion.Euler(TPCommandRoomTransform.rotation.eulerAngles);
        }
    }
}
