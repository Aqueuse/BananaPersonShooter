using System.Collections.Generic;
using Data.Buildables;
using Enums;
using Gestion;
using UnityEngine;

namespace UI.InGame.Inventory {
    public class UIBlueprintsInventory : MonoBehaviour {
        public GenericDictionary<BuildableType, UIBlueprintSlot> inventorySlotsByBuildableType;
        
        private BuildableDataScriptableObject buildableDataScriptableObject;
        
        private Dictionary<BuildableType, int> _itemsIndexByType;
        
        public void RefreshUInventory() {
            var blueprintInventory = ObjectsReference.Instance.blueprintsInventory.blueprintsInventory;

            foreach (var blueprintItem in blueprintInventory) {
                var uiBlueprintSlot = inventorySlotsByBuildableType[blueprintItem.Key]; 
                
                uiBlueprintSlot.gameObject.SetActive(blueprintItem.Value > 0);

                buildableDataScriptableObject = uiBlueprintSlot.itemScriptableObject;
                
                if (!ObjectsReference.Instance.rawMaterialsInventory.HasCraftingIngredients(buildableDataScriptableObject)) {
                    uiBlueprintSlot.SetColor(ObjectsReference.Instance.ghostsReference.GetColorByGhostState(GhostState.NOT_ENOUGH_MATERIALS));
                }
            }
        }
        
        public void HideAllBlueprints() {
            foreach (var blueprintSlot in inventorySlotsByBuildableType) {
                blueprintSlot.Value.gameObject.SetActive(false);
            }
        }
    }
}
