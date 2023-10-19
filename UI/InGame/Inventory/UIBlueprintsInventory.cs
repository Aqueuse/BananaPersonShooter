using System.Collections.Generic;
using Enums;
using UnityEngine;

namespace UI.InGame.Inventory {
    public class UIBlueprintsInventory : MonoBehaviour {
        [SerializeField] private Transform inventoryContentTransform; 

        public GenericDictionary<BuildableType, UInventorySlot> inventorySlotsByBuildableType;
        
        private Dictionary<BuildableType, int> _itemsIndexByType;
        
        public void RefreshUInventory() {
            var inventory = ObjectsReference.Instance.blueprintsInventory.blueprintsInventory;

            foreach (var inventoryItem in inventory) {
                if (inventoryItem.Value > 0) {
                    inventorySlotsByBuildableType[inventoryItem.Key].gameObject.SetActive(true);
                }

                else inventorySlotsByBuildableType[inventoryItem.Key].gameObject.SetActive(false);
            }
        }
        
        public void HideAllBlueprints() {
            foreach (var blueprintSlot in inventorySlotsByBuildableType) {
                blueprintSlot.Value.gameObject.SetActive(false);
            }
        }
        
        public void UnselectAllSlots() {
            foreach (var inventoryItem in inventorySlotsByBuildableType) {
                inventoryItem.Value.UnselectInventorySlot();
            }
        }

        public void SelectFirstSlot() {
            UnselectAllSlots();

            if (inventoryContentTransform.childCount == 0) return;
            
            foreach (var slot in inventoryContentTransform.GetComponentsInChildren<UInventorySlot>()) {
                if (slot.gameObject.activeInHierarchy) {
                    slot.GetComponent<UInventorySlot>().SelectInventorySlot();
                    break;
                }
            }
        }
    }
}
