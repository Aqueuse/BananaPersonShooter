using UnityEngine;

namespace InGame.Interactions.InteractionsActions {
    public class TPCorolleInteraction : Interaction {
        private Transform TPCorolleTransform;

        private void Start() {
            TPCorolleTransform = ObjectsReference.Instance.gameManager.spawnPointsBySpawnType[SpawnPoint.COROLLE_TOP_OF_HANGARS];
        }

        public override void Activate(GameObject interactedGameObject) {
            ObjectsReference.Instance.bananaMan.transform.position = TPCorolleTransform.position;
            ObjectsReference.Instance.bananaMan.transform.rotation = Quaternion.Euler(TPCorolleTransform.rotation.eulerAngles);
        }
    }
}
