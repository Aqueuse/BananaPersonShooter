using UnityEngine;

namespace Items {
    public class BlueprintsData : MonoBehaviour {
        private static GameObject blueprintDataGameObject;

        private void Start() {
            blueprintDataGameObject = gameObject;
        }
        
        public static void Activate() {
            var buildablesManager = ObjectsReference.Instance.buildablesManager;
            var playerBlueprints = ObjectsReference.Instance.buildablesManager.playerBlueprints; 
            var buildablesToGive = buildablesManager.buildablesToGive;
            
            foreach (var blueprint in playerBlueprints.ToArray()) {
                if (!buildablesToGive.Contains(blueprint)) playerBlueprints.Remove(blueprint);
            }

            ObjectsReference.Instance.buildablesManager.playerBlueprints = playerBlueprints;
            
            foreach (var buildableType in buildablesToGive) {
                if (!playerBlueprints.Contains(buildableType)) {
                    buildablesManager.playerBlueprints.Add(buildableType);
                    ObjectsReference.Instance.uiBlueprints.SetVisible(buildableType);
                    ObjectsReference.Instance.uiQueuedMessages.UnlockBlueprint(buildableType);
                }
            }

            blueprintDataGameObject.SetActive(false);
        }
    }
}
