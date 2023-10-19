using System.Collections.Generic;
using Enums;
using UnityEngine;

namespace UI.InGame.Inventory {
    public class UIRawMaterialsInventory : MonoBehaviour {
        [SerializeField] private Transform inventoryContentTransform; 

        public GenericDictionary<RawMaterialType, UInventorySlot> inventorySlotsByRawMaterialType;
        private Dictionary<BananaType, int> _itemsIndexByType;
        
        public void RefreshUInventory() {
            var inventory = ObjectsReference.Instance.rawMaterialsInventory.rawMaterialsInventory;

            foreach (var inventoryItem in inventory) {
                if (inventoryItem.Value > 0) {
                    inventorySlotsByRawMaterialType[inventoryItem.Key].gameObject.SetActive(true);
                    inventorySlotsByRawMaterialType[inventoryItem.Key].GetComponent<UInventorySlot>().SetQuantity(inventoryItem.Value);
                }

                else inventorySlotsByRawMaterialType[inventoryItem.Key].gameObject.SetActive(false);
            }
        }
        
        public void UnselectAllSlots() {
            foreach (var inventoryItem in inventorySlotsByRawMaterialType) {
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
