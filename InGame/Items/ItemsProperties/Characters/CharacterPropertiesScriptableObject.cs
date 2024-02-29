using UnityEngine;

namespace InGame.Items.ItemsProperties.Characters {
    [CreateAssetMenu (fileName = "Properties", menuName = "ScriptableObjects/CharacterPropertiesScriptableObject", order = 2)]
    public class CharacterPropertiesScriptableObject : ItemScriptableObject {
        public Color[] clothColors;
    }
}