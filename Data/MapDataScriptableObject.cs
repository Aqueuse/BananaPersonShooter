using Enums;
using UnityEngine;

namespace Data {
    [CreateAssetMenu (fileName = "Data", menuName = "ScriptableObjects/mapDataScriptableObject", order = 3)]
    public class MapDataScriptableObject : ScriptableObject {
        public int debrisQuantity;
        
        public Vector3[] debrisPosition;
        public Quaternion[] debrisRotation;
        public int[] debrisIndex;

        public MonkeyType MonkeyType;
        public float monkeySasiety;

        public bool hasDebris;
    }
}
