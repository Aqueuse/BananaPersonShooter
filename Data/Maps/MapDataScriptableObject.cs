using Data.Monkeys;
using UnityEngine;

namespace Data.Maps {
    [CreateAssetMenu (fileName = "Data", menuName = "ScriptableObjects/MapDataScriptableObject", order = 2)]
    public class MapDataScriptableObject : ItemScriptableObject {
        public string sceneName;
        
        public GenericDictionary<string, MonkeyDataScriptableObject> monkeyDataScriptableObjectsByMonkeyId;

        public int visitorsQuantity;
        public float cleanliness;
    }
}