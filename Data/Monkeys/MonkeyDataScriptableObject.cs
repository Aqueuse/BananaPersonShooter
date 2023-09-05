using UnityEngine;

namespace Data.Monkeys {
    [CreateAssetMenu (fileName = "Data", menuName = "ScriptableObjects/MonkeyDataScriptableObject", order = 3)]
    public class MonkeyDataScriptableObject : ItemScriptableObject {
        private float _maxHappiness;
        public float happiness;
        
        public float sasiety;
    }
}
