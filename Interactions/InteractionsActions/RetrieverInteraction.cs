using Gestion;
using Items;
using Tags;
using UnityEngine;

namespace Interactions.InteractionsActions {
    public class RetrieverInteraction : Interact {
        public override void Activate(GameObject interactedGameObject) {
            var plateforms = Map.Instance.GetAllBuildablesByTypeInAspirableContainer(BuildableType.PLATEFORM);
            var plateformsCount = plateforms.Count;

            if (plateformsCount > 0) {
                var craftingMaterials = ObjectsReference.Instance.scriptableObjectManager.GetBuildableCraftingIngredients(BuildableType.PLATEFORM);

                 foreach (var craftingMaterial in craftingMaterials) {
                     ObjectsReference.Instance.rawMaterialsInventory.AddQuantity(craftingMaterial.Key, craftingMaterial.Value*plateformsCount);
                 }
                 
                foreach (var plateform in plateforms) {
                    DestroyImmediate(plateform.gameObject);
                }

                var logo = TagsManager.Instance.GetFirstGameObjectWithTagInGameObject(
                    interactedGameObject.transform.parent.gameObject,
                    GAME_OBJECT_TAG.RETRIEVER_ROTATING_LOGO
                );

                if (logo != null && logo.GetComponent<RotateTransform>() == null)
                    logo.AddComponent<RotateTransform>();
            }

            else {
                ObjectsReference.Instance.uiQueuedMessages.AddNothingToRetrieveMessage();
            }

        }
    }
}
