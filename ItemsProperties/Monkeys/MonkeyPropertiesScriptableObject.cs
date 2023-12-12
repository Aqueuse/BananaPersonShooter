using UnityEngine;

namespace ItemsProperties.Monkeys {
    [CreateAssetMenu (fileName = "Properties", menuName = "ScriptableObjects/MonkeyPropertiesScriptableObject", order = 3)]
    public class MonkeyPropertiesScriptableObject : ItemScriptableObject {
        public MonkeyType monkeyType;

        private float _maxHappiness;
        public float happiness;
        
        public float sasiety;
    }
}
