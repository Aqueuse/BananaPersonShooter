using System.Collections.Generic;
using InGame.Items.ItemsBehaviours.DroppedBehaviours;
using UnityEngine;

namespace InGame.Items.ItemsBehaviours.BuildablesBehaviours {
    public class DistributorBehaviour : BuildableBehaviour {
        [SerializeField] private List<ItemStack> itemStacksInDistributor;
        
        private void OnCollisionEnter(Collision other) {
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
