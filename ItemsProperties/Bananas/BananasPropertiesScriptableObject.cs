using UnityEngine;

namespace ItemsProperties.Bananas {
    [CreateAssetMenu (fileName = "Properties", menuName = "ScriptableObjects/bananasPropertiesScriptableObject", order = 1)]
    public class BananasPropertiesScriptableObject : ItemScriptableObject {
        [Multiline] public string[] effects;
        
        public float sasiety;
        public float damageTime;

        public float healthBonus;
        public float resistanceBonus;

        public BananaEffect bananaEffect;

        public Color bananaColor;
    }
}