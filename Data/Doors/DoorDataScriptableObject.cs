using Data.Maps;
using UnityEngine;

namespace Data.Door {
    [CreateAssetMenu (fileName = "Data", menuName = "ScriptableObjects/DoorDataScriptableObject", order = 2)]
    public class DoorDataScriptableObject : ItemScriptableObject {
        public MapDataScriptableObject associatedMapDataScriptableObject;
    }
}
