using System.Collections.Generic;
using Enums;

public class Inventory : MonoSingleton<Inventory> {
    public Dictionary<ItemThrowableType, int> bananaManInventory;
    
    private void Start() {
        bananaManInventory = new Dictionary<ItemThrowableType, int> {
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
            {ItemThrowableType.ROCKET, 0},
            {ItemThrowableType.PLATEFORM_CAVENDISH, 0}
        };
    }

    public void AddQuantity(ItemThrowableType itemThrowableType, int quantity) {
        bananaManInventory[itemThrowableType] += quantity;
    }

    public int GetQuantity(ItemThrowableType itemThrowableType) {
        return bananaManInventory[itemThrowableType];
    }

    public void RemoveQuantity(ItemThrowableType itemThrowableType, int quantity) {
        if (bananaManInventory[itemThrowableType] > quantity) bananaManInventory[itemThrowableType] -= quantity;
        else {
            bananaManInventory[itemThrowableType] = 0;
        }
    }
}