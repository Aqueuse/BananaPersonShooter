using Enums;
using Player;
using UI.InGame.QuickSlots;

namespace Game {
    public class Inventory : MonoSingleton<Inventory> {
        public GenericDictionary<ItemThrowableType, int> bananaManInventory;
    
        private void Start() {
            bananaManInventory = new GenericDictionary<ItemThrowableType, int> {
                { ItemThrowableType.EMPTY, 0},
                {ItemThrowableType.DEBRIS, 0},
                {ItemThrowableType.INGOT, 0},
                {ItemThrowableType.BARANGAN, 0},
                {ItemThrowableType.BLUE_JAVA, 0},
                {ItemThrowableType.BURRO, 0},
                {ItemThrowableType.CAVENDISH, 0},
                {ItemThrowableType.GOLD_FINGER, 0},
                {ItemThrowableType.GROS_MICHEL, 0},
                {ItemThrowableType.LADY_FINGER, 0},
                {ItemThrowableType.MANZANO, 0},
                {ItemThrowableType.MATOKE, 0},
                {ItemThrowableType.MUSA_VELUTINA, 0},
                {ItemThrowableType.NANJANGUD, 0},
                {ItemThrowableType.PISANG_RAJA, 0},
                {ItemThrowableType.PLANTAIN, 0},
                {ItemThrowableType.PRAYING_HANDS, 0},
                {ItemThrowableType.RED, 0},
                {ItemThrowableType.RINO_HORN, 0},
                {ItemThrowableType.TINDOK, 0},
                {ItemThrowableType.PLATEFORM, 0}
            };
        }

        public void AddQuantity(ItemThrowableType itemThrowableType, ItemThrowableCategory itemThrowableCategory, int quantity) {
            bananaManInventory[itemThrowableType] += quantity;
            UISlotsManager.Instance.RefreshQuantityInQuickSlot(itemThrowableType);
        
            UISlotsManager.Instance.TryToPutOnSlot(itemThrowableType, itemThrowableCategory);
        }

        public int GetQuantity(ItemThrowableType itemThrowableType) {
            return bananaManInventory[itemThrowableType];
        }

        public void RemoveQuantity(ItemThrowableType itemThrowableType, int quantity) {
            if (bananaManInventory[itemThrowableType] > quantity) bananaManInventory[itemThrowableType] -= quantity;
            else {
                bananaManInventory[itemThrowableType] = 0;
                BananaMan.Instance.activeItemThrowableType = ItemThrowableType.EMPTY;
                BananaMan.Instance.activeItemThrowableCategory = ItemThrowableCategory.EMPTY;
            }
            UISlotsManager.Instance.RefreshQuantityInQuickSlot(itemThrowableType);
        }
    }
}