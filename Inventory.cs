using System.Collections.Generic;

public class Inventory : MonoSingleton<Inventory> {
    public Dictionary<BananaType, int> BananaManInventory;
    
    private void Start() {
        BananaManInventory = new Dictionary<BananaType, int> {
            {BananaType.RED, 60},
            {BananaType.BURRO, 60},
            {BananaType.MATOKE, 60},
            {BananaType.TINDOK, 60},
            {BananaType.MANZANO, 60},
            {BananaType.BARANGAN, 60},
            {BananaType.BLUE_JAVA, 60},
            {BananaType.PLANTAIN, 60},
            {BananaType.RINO_HORN, 60},
            {BananaType.CAVENDISH, 60},
            {BananaType.GROS_MICHEL, 60},
            {BananaType.LADY_FINGER, 60},
            {BananaType.PISANG_RAJA, 60},
            {BananaType.PRAYING_HANDS, 60},
            {BananaType.GOLD_FINGER, 60},
            {BananaType.NANJANGUD, 60},
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