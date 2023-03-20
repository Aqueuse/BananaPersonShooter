using System.Collections.Generic;
using Enums;
using Items;
using TMPro;
using UnityEngine;

namespace UI.InGame.Inventory {
    public class UInventory : MonoSingleton<UInventory> {
        public TextMeshProUGUI itemDescription;

        [SerializeField] private GenericDictionary<ItemThrowableType, GameObject> inventorySlots;
        [SerializeField] private GameObject firstInventoryItem;

        private Dictionary<ItemThrowableType, int> _itemsIndexByType;

        public GameObject lastselectedInventoryItem;

        private void Start() {
            lastselectedInventoryItem = firstInventoryItem;
        }

        public void RefreshUInventory() {
            var inventory = global::Game.Inventory.Instance.bananaManInventory;

            foreach (var inventoryItem in inventory) {
                if (inventoryItem.Value > 0) {
                    inventorySlots[inventoryItem.Key].SetActive(true);
                    inventorySlots[inventoryItem.Key].GetComponent<UInventorySlot>().SetQuantity(inventoryItem.Value);
                }

                else inventorySlots[inventoryItem.Key].SetActive(false);
            }
        }
        
        public Sprite GetItemSprite(ItemThrowableType itemThrowableType) {
            return ItemsManager.Instance.itemsDataScriptableObject.itemSpriteByItemType[itemThrowableType];
        }
    }
}