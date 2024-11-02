﻿using UnityEngine;

namespace InGame.Items.ItemsProperties.Bananas {
    [CreateAssetMenu (fileName = "Properties", menuName = "ScriptableObjects/bananasPropertiesScriptableObject", order = 1)]
    public class BananasPropertiesScriptableObject : ItemScriptableObject {
        public float sasiety;

        public float healthBonus;
        public float resistanceBonus;

        public BananaEffect[] bananaEffects;

        public Color bananaColor;
    }
}