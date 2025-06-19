using System.Collections.Generic;
using InGame.Items.ItemsBehaviours.DroppedBehaviours;
using UnityEngine;

namespace InGame.Items.ItemsBehaviours.BuildablesBehaviours {
    public class DistributorBehaviour : VisitorBuildableBehaviour {
        [SerializeField] private List<ItemStack> itemStacksInDistributor;
        
        private void OnCollisionEnter(Collision other) {
            if (buildableData.buildableState == BuildableState.BLUEPRINT) return;
            
            if (other.gameObject.TryGetComponent(out DroppedBehaviour droppedBehaviour)) {
                foreach (var itemStack in itemStacksInDistributor) {
                    if (itemStack.itemScriptableObject == droppedBehaviour.itemScriptableObject) {
                        itemStack.AddOne();
                        Destroy(other.gameObject);
                        break;
                    }
                }
            }
        }
    }
}
