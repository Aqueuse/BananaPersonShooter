using ItemsProperties.Monkeys;
using UnityEngine;

namespace ItemsProperties.Maps {
    [CreateAssetMenu (fileName = "Properties", menuName = "ScriptableObjects/MapPropertiesScriptableObject", order = 2)]
    public class MapPropertiesScriptableObject : ItemScriptableObject {
        public SceneType sceneName;

        public GenericDictionary<string, MonkeyPropertiesScriptableObject> monkeyPropertiesScriptableObjectsByMonkeyId;

        public float piratesDebris;
        public float visitorsDebris;

        public int piratesQuantity;
        public int visitorsQuantity;
        public int chimployeesQuantity;
    }
}