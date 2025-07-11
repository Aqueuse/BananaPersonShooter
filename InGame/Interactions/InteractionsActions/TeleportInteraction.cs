using UI.InGame;
using UI.InGame.Interactions;
using UnityEngine;

namespace InGame.Interactions.InteractionsActions {
    public class TeleportInteraction : Interaction {
        
        public override void Activate(GameObject interactedGameObject) {
            var spawnPoint = interactedGameObject.GetComponent<UTeleportInteraction>().spawnPoint;
            var teleportTransform = ObjectsReference.Instance.gameManager.spawnPointsBySpawnType[spawnPoint];
            
            ObjectsReference.Instance.bananaMan.transform.position = teleportTransform.position;
            ObjectsReference.Instance.bananaMan.transform.rotation = Quaternion.Euler(teleportTransform.rotation.eulerAngles);
        }
    }
}
