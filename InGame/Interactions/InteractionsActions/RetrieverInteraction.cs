using System.Collections.Generic;
using InGame.Items.ItemsBehaviours.BuildablesBehaviours;
using UnityEngine;

namespace InGame.Interactions.InteractionsActions {
    public class RetrieverInteraction : Interaction {
        public override void Activate(GameObject interactedGameObject) {
            var plateforms = GetAllBuildablesByTypeInAspirableContainer(BuildableType.BUMPER);
            var plateformsCount = plateforms.Count;
            
            if (plateformsCount > 0) {
                var craftingMaterials = ObjectsReference.Instance.meshReferenceScriptableObject.buildablePropertiesScriptableObjects[BuildableType.BUMPER].rawMaterialsWithQuantity;

                 foreach (var craftingMaterial in craftingMaterials) {
                     ObjectsReference.Instance.bananaManRawMaterialInventory.AddQuantity(craftingMaterial.Key, craftingMaterial.Value*plateformsCount);
                 }
                 
                foreach (var plateform in plateforms) {
                    DestroyImmediate(plateform.gameObject);
                }

                interactedGameObject.transform.GetComponentInChildren<RotateTransform>().enabled = true;
            }

            else {
                ObjectsReference.Instance.uiQueuedMessages.AddNothingToRetrieveMessage();
            }
        }
        
        private List<GameObject> GetAllBuildablesByTypeInAspirableContainer(BuildableType buildableType) {
            var buildablesBehaviours = FindObjectsByType<BuildableBehaviour>(FindObjectsSortMode.None);

            var gameObjectsWithBuildableType = new List<GameObject>();

            foreach (var buildable in buildablesBehaviours) {
                if (buildable.buildableType == buildableType) {
                    gameObjectsWithBuildableType.Add(buildable.gameObject);
                }
            }

            return gameObjectsWithBuildableType;
        }
    }
}
