using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace UI.InGame {
    public class UInventory : MonoSingleton<UInventory> {
        public TextMeshProUGUI itemDescription;
        public TextMeshProUGUI itemHealth;
        public TextMeshProUGUI itemDamage;
        public TextMeshProUGUI itemResistance;

        [SerializeField] private List<GameObject> inventorySlots;
        [SerializeField] private GameObject firstInventoryItem;
        
        public GameObject lastselectedInventoryItem;

        private void Start() {
            lastselectedInventoryItem = firstInventoryItem;
        }

        public void RefreshUInventory() {
            var inventory = Inventory.Instance.bananaManInventory;

            foreach (var bananaType in inventory) {
                if (bananaType.Value > 0) {
                    inventorySlots[BananasTypeReference.reference[bananaType.Key]].SetActive(true);
                    inventorySlots[BananasTypeReference.reference[bananaType.Key]].GetComponent<UInventorySlot>().SetQuantity(bananaType.Value);
                }

                else inventorySlots[BananasTypeReference.reference[bananaType.Key]].SetActive(false);
            }
        }
    }
}
