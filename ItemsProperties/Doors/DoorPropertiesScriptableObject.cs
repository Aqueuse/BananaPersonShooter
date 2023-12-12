using ItemsProperties.Maps;
using UnityEngine;

namespace ItemsProperties.Doors {
    [CreateAssetMenu (fileName = "Properties", menuName = "ScriptableObjects/DoorPropertiesScriptableObject", order = 2)]
    public class DoorPropertiesScriptableObject : ItemScriptableObject {
        public MapPropertiesScriptableObject associatedMapPropertiesScriptableObject;
    }
}
