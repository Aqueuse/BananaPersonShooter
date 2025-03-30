using InGame.Items.ItemsBehaviours;
using UnityEngine;

namespace InGame.Interactions.InteractionsActions {
    public class BlueprinterInteraction : Interaction {
        public override void Activate(GameObject interactedGameObject) {
            foreach (var buildable in interactedGameObject.GetComponent<BlueprintBehaviour>().associatedBuildables) {
                ObjectsReference.Instance.bananaManBuildablesInventory.AllowBuildable(buildable);    
            }
        }
    }
}