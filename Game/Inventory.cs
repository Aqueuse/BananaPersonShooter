using Enums;
using UnityEngine;

namespace Game {
    public class Inventory : MonoBehaviour {
        public GenericDictionary<ItemType, int> bananaManInventory;
    
        private void Start() {
            bananaManInventory = new GenericDictionary<ItemType, int> {
                {ItemType.EMPTY, 0},
                {ItemType.BARANGAN, 0},
                {ItemType.BLUE_JAVA, 0},
                {ItemType.BURRO, 0},
                {ItemType.CAVENDISH, 0},
                {ItemType.GOLD_FINGER, 0},
                {ItemType.GROS_MICHEL, 0},
                {ItemType.LADY_FINGER, 0},
                {ItemType.MANZANO, 0},
                {ItemType.MATOKE, 0},
                {ItemType.MUSA_VELUTINA, 0},
                {ItemType.NANJANGUD, 0},
                {ItemType.PISANG_RAJA, 0},
                {ItemType.PLANTAIN, 0},
                {ItemType.PRAYING_HANDS, 0},
                {ItemType.RED, 0},
                {ItemType.RINO_HORN, 0},
                {ItemType.TINDOK, 0},
                {ItemType.METAL, 0},
                {ItemType.BATTERY, 0},
                {ItemType.ELECTRONIC, 0},
                {ItemType.TISSU, 0},
                {ItemType.BANANA_SKIN, 0}
            };
        }

        public void AddQuantity(ItemCategory itemCategory, ItemType itemType, int quantity) {
            bananaManInventory[itemType] += quantity;
            ObjectsReference.Instance.uiSlotsManager.RefreshQuantityInQuickSlot(itemCategory, itemType);
        
            ObjectsReference.Instance.uiSlotsManager.TryToPutOnSlot(itemCategory, itemType);
        }

        public int GetQuantity(ItemType itemType) {
            return bananaManInventory[itemType];
        }

        public void RemoveQuantity(ItemCategory itemCategory, ItemType itemType, int quantity) {
            if (bananaManInventory[itemType] > quantity) bananaManInventory[itemType] -= quantity;
            else {
                bananaManInventory[itemType] = 0;
                ObjectsReference.Instance.bananaMan.activeItemType = ItemType.EMPTY;
                ObjectsReference.Instance.bananaMan.activeItemCategory = ItemCategory.EMPTY;
            }
            ObjectsReference.Instance.uiSlotsManager.RefreshQuantityInQuickSlot(itemCategory, itemType);
        }

        public bool HasCraftingIngredients(GenericDictionary<ItemType, int> craftingIngredients) {
            foreach (var craftingIngredient in craftingIngredients) {
                if (bananaManInventory[craftingIngredient.Key] < craftingIngredient.Value) return false;
            }

            return true;
        }
    }
}