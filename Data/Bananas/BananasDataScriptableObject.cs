using Enums;
using UnityEngine;

namespace Data.Bananas {
    [CreateAssetMenu (fileName = "Data", menuName = "ScriptableObjects/bananasDataScriptableObject", order = 1)]
    public class BananasDataScriptableObject : ItemScriptableObject {
        [Multiline] public string[] effects;

        public int regimeQuantity;

        public float sasiety;
        public float damageTime;

        public float healthBonus;
        public float resistanceBonus;

        public BananaEffect bananaEffect;

        public Material bananaMaterial;
    }
}