using ItemsProperties.Buildables;
using ItemsProperties.Buildables.VisitorsBuildable;
using Gestion;
using UnityEngine;

namespace UI.InGame.Inventory {
    public class UIBlueprintsInventory : MonoBehaviour {
        public GenericDictionary<BuildableType, UIBlueprintSlot> inventorySlotsByBuildableType;

        private BuildablePropertiesScriptableObject buildableDataScriptableObject;

        public void RefreshUInventory() {
            foreach (var blueprintItem in inventorySlotsByBuildableType) {
                var uiBlueprintSlot = inventorySlotsByBuildableType[blueprintItem.Key];
                buildableDataScriptableObject = blueprintItem.Value.itemScriptableObject;

                if (buildableDataScriptableObject.buildableType == BuildableType.EMPTY) continue;

                uiBlueprintSlot.SetColor(
                    !ObjectsReference.Instance.rawMaterialsInventory.HasCraftingIngredients(buildableDataScriptableObject)
                        ? ObjectsReference.Instance.ghostsReference.GetColorByGhostState(GhostState.NOT_ENOUGH_MATERIALS)
                        : ObjectsReference.Instance.ghostsReference.GetColorByGhostState(GhostState.VALID));
            }
        }
    }
}