using System.Linq;
using Building;
using Enums;
using UnityEngine;

namespace Items.ItemsActions {
    public class RetrieverItemAction : MonoBehaviour {
        public static void Activate(GameObject interactedGameObject) {
            var plateforms = MapItems.Instance.aspirablesContainer.GetComponentsInChildren<ItemThrowable>().Where(b => b.buildableType == BuildableType.PLATEFORM).ToList();
            var plateformsCount = plateforms.Count;

            if (plateformsCount > 0) {
                var craftingMaterials = ObjectsReference.Instance.buildablesManager.GetBuildableCraftingIngredients(BuildableType.PLATEFORM);

                foreach (var craftingMaterial in craftingMaterials) {
                    ObjectsReference.Instance.inventory.AddQuantity(ItemCategory.RAW_MATERIAL, craftingMaterial.Key, craftingMaterial.Value*plateformsCount);
                }

                foreach (var plateform in plateforms) {
                    DestroyImmediate(plateform.gameObject);
                }

                ObjectsReference.Instance.mapsManager.currentMap.RefreshAspirablesItemsDataMap();

                if (interactedGameObject.GetComponent<RotateTransform>() == null) interactedGameObject.AddComponent<RotateTransform>();
            }

            else {
                ObjectsReference.Instance.uiQueuedMessages.AddNothingToRetrieveMessage();
            }

        }
    }
}
