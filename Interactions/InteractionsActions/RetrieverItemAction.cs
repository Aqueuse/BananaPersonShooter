using System.Linq;
using Gestion;
using Gestion.Buildables.Plateforms;
using Enums;
using Items;
using UnityEngine;

namespace Interactions.InteractionsActions {
    public class RetrieverInteraction : MonoBehaviour {
        public static void Activate(GameObject interactedGameObject) {
            var plateforms = MapItems.Instance.aspirablesContainer.GetComponentsInChildren<Plateform>().ToList();
            var plateformsCount = plateforms.Count;

            if (plateformsCount > 0) {
                var craftingMaterials = ObjectsReference.Instance.scriptableObjectManager.GetBuildableCraftingIngredients(BuildableType.PLATEFORM);

                 foreach (var craftingMaterial in craftingMaterials) {
                     ObjectsReference.Instance.rawMaterialsInventory.AddQuantity(craftingMaterial.Key, craftingMaterial.Value*plateformsCount);
                 }

                foreach (var plateform in plateforms) {
                    DestroyImmediate(plateform.gameObject);
                }

                ObjectsReference.Instance.mapsManager.currentMap.RefreshItemsDataMap();

                if (interactedGameObject.GetComponent<RotateTransform>() == null) interactedGameObject.AddComponent<RotateTransform>();
            }

            else {
                ObjectsReference.Instance.uiQueuedMessages.AddNothingToRetrieveMessage();
            }

        }
    }
}
