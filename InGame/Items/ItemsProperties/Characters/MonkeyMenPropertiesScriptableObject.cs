using UnityEngine;

namespace InGame.Items.ItemsProperties.Characters {
    [CreateAssetMenu (fileName = "Properties", menuName = "ScriptableObjects/MonkeyMenPropertiesScriptableObject", order = 2)]
    public class MonkeyMenPropertiesScriptableObject : ItemScriptableObject {
        public Color[] colorSets;
        public int prefabIndex;

        public CharacterType characterType;
        
        public GameObject headAccessoryPrefab;
    }
}