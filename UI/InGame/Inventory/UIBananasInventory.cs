using System.Collections.Generic;
using Enums;
using UnityEngine;

namespace UI.InGame.Inventory {
    public class UIBananasInventory : MonoBehaviour {
        [SerializeField] private Transform inventoryContentTransform; 

        public GenericDictionary<BananaType, UInventorySlot> inventorySlotsByBananaType;
        
        private Dictionary<BananaType, int> _itemsIndexByType;
        
        public void RefreshUInventory() {
            var inventory = ObjectsReference.Instance.bananasInventory.bananasInventory;

            foreach (var inventoryItem in inventory) {
                if (inventoryItem.Value > 0) {
                    inventorySlotsByBananaType[inventoryItem.Key].gameObject.SetActive(true);
                    inventorySlotsByBananaType[inventoryItem.Key].GetComponent<UInventorySlot>().SetQuantity(inventoryItem.Value);
                }

                else inventorySlotsByBananaType[inventoryItem.Key].gameObject.SetActive(false);
            }
        }

        public void UnselectAllSlots() {
            foreach (var inventoryItem in inventorySlotsByBananaType) {
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
