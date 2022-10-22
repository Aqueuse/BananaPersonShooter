using System.Collections.Generic;
using UnityEngine;

public class BananasTypeReference : MonoBehaviour {
    public static Dictionary<BananaType, int> Reference;

    private void Start() {
        Reference = new Dictionary<BananaType, int> {
            {BananaType.BARANGAN, 0},
            {BananaType.BLUE_JAVA, 1},
            {BananaType.BURRO, 2},
            {BananaType.CAVENDISH, 3},
            {BananaType.EMPTY_HAND, 4},
            {BananaType.GOLD_FINGER, 5},
            {BananaType.GROS_MICHEL, 6},
            {BananaType.LADY_FINGER, 7},
            {BananaType.MANZANO, 8},
            {BananaType.MATOKE, 9},
            {BananaType.NANJANGUD, 10},
            {BananaType.PISANG_RAJA, 11},
            {BananaType.PLANTAIN, 12},
            {BananaType.PRAYING_HANDS, 13},
            {BananaType.RED, 14},
            {BananaType.RINO_HORN, 15},
            {BananaType.TINDOK, 16}
        };
    }
}
