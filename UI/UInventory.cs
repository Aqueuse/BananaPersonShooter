using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace UI {
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
            var inventory = Inventory.Instance.BananaManInventory;

            foreach (var bananaType in inventory) {
                if (bananaType.Value > 0) {
                    inventorySlots[BananasTypeReference.Reference[bananaType.Key]].SetActive(true);
                    inventorySlots[BananasTypeReference.Reference[bananaType.Key]].GetComponent<UInventorySlot>().SetQuantity(bananaType.Value);
                }

                else inventorySlots[BananasTypeReference.Reference[bananaType.Key]].SetActive(false);
            }
        }
    }
}
