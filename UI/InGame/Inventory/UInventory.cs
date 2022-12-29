using Enums;
using TMPro;
using UnityEngine;

namespace UI.InGame.Inventory {
    public class UInventory : MonoSingleton<UInventory> {
        public TextMeshProUGUI itemDescription;

        [SerializeField] private GenericDictionary<ItemThrowableType, GameObject> inventorySlots;
        [SerializeField] private GameObject firstInventoryItem;
        
        public GameObject lastselectedInventoryItem;

        private void Start() {
            lastselectedInventoryItem = firstInventoryItem;
        }

        public void RefreshUInventory() {
            var inventory = global::Inventory.Instance.bananaManInventory;

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
