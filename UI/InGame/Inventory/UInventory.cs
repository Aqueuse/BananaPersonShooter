using System.Collections.Generic;
using System.Linq;
using Enums;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.InGame.Inventory {
    public class UInventory : MonoSingleton<UInventory> {
        public TextMeshProUGUI itemDescription;

        [SerializeField] private GenericDictionary<ItemThrowableType, GameObject> inventorySlots;
        [SerializeField] private GameObject firstInventoryItem;

        private Dictionary<ItemThrowableType, int> itemsIndexByType;

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

        public void ActivateAllInventory() {
            foreach (var inventoryItem in inventorySlots) {
                inventoryItem.Value.SetActive(true);
            }
        }

        public int GetSlotIndex(ItemThrowableType itemThrowableType) {
            foreach (UInventorySlot inventorySlot in transform.GetComponentsInChildren<UInventorySlot>()) {
                if (inventorySlot.itemThrowableType == itemThrowableType) {
                    return inventorySlot.GetComponent<RectTransform>().GetSiblingIndex();
                }
            }

            return 0;
        }

        public ItemThrowableType GetItemThrowableTypeByIndex(int index) {
            return transform.GetComponentsInChildren<UInventorySlot>().ToList()[index].itemThrowableType;
        }
        
        public ItemThrowableCategory GetItemThrowableCategoryByIndex(int index) {
            return transform.GetComponentsInChildren<UInventorySlot>().ToList()[index].itemThrowableCategory;
        }

        public Sprite GetItemSprite(ItemThrowableType itemThrowableType) {
            foreach (UInventorySlot inventorySlot in transform.GetComponentsInChildren<UInventorySlot>()) {
                if (inventorySlot.itemThrowableType == itemThrowableType) {
                    return inventorySlot.GetComponent<RectTransform>().GetChild(0).GetComponentInChildren<Image>().sprite;
                }
            }

            return null;
        }
    }
}
