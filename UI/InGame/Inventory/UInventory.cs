using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace UI.InGame.Inventory {
    public class UInventory : MonoBehaviour {
        public TextMeshProUGUI itemDescription;

        [SerializeField] private GenericDictionary<ItemType, GameObject> inventorySlots;
        [SerializeField] private GameObject firstInventoryItem;

        private Dictionary<ItemType, int> _itemsIndexByType;

        public GameObject lastselectedInventoryItem;

        private void Start() {
            lastselectedInventoryItem = firstInventoryItem;
        }

        public void RefreshUInventory() {
            var inventory = ObjectsReference.Instance.inventory.bananaManInventory;

            foreach (var inventoryItem in inventory) {
                if (inventoryItem.Value > 0) {
                    inventorySlots[inventoryItem.Key].SetActive(true);
                    inventorySlots[inventoryItem.Key].GetComponent<UInventorySlot>().SetQuantity(inventoryItem.Value);
                }

                else inventorySlots[inventoryItem.Key].SetActive(false);
            }
        }
    }
}