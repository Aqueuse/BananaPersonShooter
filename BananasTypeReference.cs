using System.Collections.Generic;
using Enums;
using UnityEngine;

public class BananasTypeReference : MonoBehaviour {
    public static Dictionary<BananaType, int> reference;

    private void Start() {
        reference = new Dictionary<BananaType, int> {
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
            {BananaType.MUSA_VELUTINA, 10},
            {BananaType.NANJANGUD, 11},
            {BananaType.PISANG_RAJA, 12},
            {BananaType.PLANTAIN, 13},
            {BananaType.PRAYING_HANDS, 14},
            {BananaType.RED, 15},
            {BananaType.RINO_HORN, 16},
            {BananaType.TINDOK, 17}
        };
    }
}
