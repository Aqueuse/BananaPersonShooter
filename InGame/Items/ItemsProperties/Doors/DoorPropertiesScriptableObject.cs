using InGame.Items.ItemsProperties.Maps;
using UnityEngine;

namespace InGame.Items.ItemsProperties.Doors {
    [CreateAssetMenu (fileName = "Properties", menuName = "ScriptableObjects/DoorPropertiesScriptableObject", order = 2)]
    public class DoorPropertiesScriptableObject : ItemScriptableObject {
        public MapPropertiesScriptableObject associatedMapPropertiesScriptableObject;
    }
}
