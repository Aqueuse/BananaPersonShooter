using System.Collections.Generic;

public class Inventory : MonoSingleton<Inventory> {
    public Dictionary<BananaType, int> BananaManInventory;
    
    private void Start() {
        BananaManInventory = new Dictionary<BananaType, int> {
            {BananaType.RED, 1},
            {BananaType.BURRO, 0},
            {BananaType.MATOKE, 0},
            {BananaType.TINDOK, 0},
            {BananaType.MANZANO, 0},
            {BananaType.BARANGAN, 0},
            {BananaType.BLUE_JAVA, 0},
            {BananaType.PLANTAIN, 0},
            {BananaType.RINO_HORN, 0},
            {BananaType.CAVENDISH, 0},
            {BananaType.GROS_MICHEL, 0},
            {BananaType.LADY_FINGER, 0},
            {BananaType.PISANG_RAJA, 0},
            {BananaType.PRAYING_HANDS, 0},
            {BananaType.GOLD_FINGER, 0},
            {BananaType.NANJANGUD, 0},
            {BananaType.EMPTY_HAND, 1}
        };
    }

    public void AddQuantity(BananaType bananaType, int quantity) {
        BananaManInventory[bananaType] += quantity;
    }

    public int GetQuantity(BananaType bananaType) {
        return BananaManInventory[bananaType];
    }

    public void RemoveQuantity(BananaType bananaType, int quantity) {
        if (BananaManInventory[bananaType] > quantity) BananaManInventory[bananaType] -= quantity;
        else {
            BananaManInventory[bananaType] = 0;
        }
    }
}