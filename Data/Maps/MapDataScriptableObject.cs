using Data.Monkeys;
using UnityEngine;

namespace Data.Maps {
    [CreateAssetMenu (fileName = "Data", menuName = "ScriptableObjects/MapDataScriptableObject", order = 2)]
    public class MapDataScriptableObject : ItemScriptableObject {
        public string sceneName;
        
        public int monkeysQuantity;
        public MonkeyType monkeyType;
        public MonkeyDataScriptableObject monkeyDataScriptableObject;

        public int visitorsQuantity;
        public float cleanliness;
    }
}