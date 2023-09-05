using System.Collections.Generic;
using Enums;
using UnityEngine;

namespace UI.InGame.Inventory {
    public class UIBlueprintsInventory : MonoBehaviour {
        public GenericDictionary<BuildableType, UInventorySlot> inventorySlotsByBuildableType;
        
        private Dictionary<BuildableType, int> _itemsIndexByType;
        
        public void RefreshUInventory() {
            var inventory = ObjectsReference.Instance.blueprintsInventory.blueprintsInventory;

            foreach (var inventoryItem in inventory) {
                if (inventoryItem.Value > 0) {
                    inventorySlotsByBuildableType[inventoryItem.Key].gameObject.SetActive(true);
                    inventorySlotsByBuildableType[inventoryItem.Key].GetComponent<UInventorySlot>().SetQuantity(inventoryItem.Value);
                }

                else inventorySlotsByBuildableType[inventoryItem.Key].gameObject.SetActive(false);
            }
        }
        
        public void HideAllBlueprints() {
            foreach (var blueprintSlot in inventorySlotsByBuildableType) {
                blueprintSlot.Value.gameObject.SetActive(false);
            }
        }
    }
}
