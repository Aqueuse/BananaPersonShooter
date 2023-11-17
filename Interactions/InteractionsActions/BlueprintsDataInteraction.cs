using UnityEngine;

namespace Interactions.InteractionsActions {
    public class BlueprintsDataInteraction : MonoBehaviour {
        [SerializeField] private GameObject interactionGameObject;
        
        public void Activate() {
            var scriptableObjectManager = ObjectsReference.Instance.scriptableObjectManager;
            var buildablesToGive = scriptableObjectManager.buildablesToGive;

            foreach (var buildable in buildablesToGive) {
                ObjectsReference.Instance.uiBlueprintsInventory.inventorySlotsByBuildableType[buildable].gameObject.SetActive(true);
                ObjectsReference.Instance.blueprintsInventory.blueprintsInventory[buildable] = 1;
                ObjectsReference.Instance.uiQueuedMessages.UnlockBlueprint(buildable);
            }
            
            HideBlueprintsData();
        }

        public void ShowBlueprintDataIfAvailable() {
            if (AreNewBlueprintsAvailable()) ShowBlueprintsData();
            else {
                HideBlueprintsData();
            }
        }

        public bool AreNewBlueprintsAvailable() {
            foreach (var buildableType in ObjectsReference.Instance.scriptableObjectManager.buildablesToGive) {
                if (ObjectsReference.Instance.blueprintsInventory.blueprintsInventory[buildableType] == 0) return true;
            }

            return false;
        }

        private void ShowBlueprintsData() {
            GetComponent<MeshRenderer>().enabled = true;
            GetComponent<MeshRenderer>().gameObject.layer = 8;
            interactionGameObject.SetActive(true);
        }

        public void HideBlueprintsData() {
            GetComponent<MeshRenderer>().enabled = false;
            GetComponent<MeshRenderer>().gameObject.layer = 0;
            interactionGameObject.SetActive(false);
        }
    }
}
