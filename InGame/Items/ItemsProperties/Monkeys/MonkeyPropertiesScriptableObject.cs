using UnityEngine;

namespace InGame.Items.ItemsProperties.Monkeys {
    [CreateAssetMenu (fileName = "Properties", menuName = "ScriptableObjects/MonkeyPropertiesScriptableObject", order = 3)]
    public class MonkeyPropertiesScriptableObject : ItemScriptableObject {
        public MonkeyType monkeyType;
        public string monkeyId;

        private float _maxHappiness;
        public float happiness;
        
        public float sasietyTimer;
        public Vector3 lastPosition;
        public Vector3 initialPosition;
    }
}
