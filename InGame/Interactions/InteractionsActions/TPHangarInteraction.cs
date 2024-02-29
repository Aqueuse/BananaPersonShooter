using UnityEngine;

namespace InGame.Interactions.InteractionsActions {
    public class TPHangarInteraction : Interaction {
        private Transform TPHangarTransform;

        private void Start() {
            TPHangarTransform = ObjectsReference.Instance.gameManager.spawnPointsBySpawnType[SpawnPoint.HANGARS];
        }

        public override void Activate(GameObject interactedGameObject) {
            ObjectsReference.Instance.bananaMan.transform.position = TPHangarTransform.position;
            ObjectsReference.Instance.bananaMan.transform.rotation = Quaternion.Euler(TPHangarTransform.rotation.eulerAngles);
        }
    }
}
