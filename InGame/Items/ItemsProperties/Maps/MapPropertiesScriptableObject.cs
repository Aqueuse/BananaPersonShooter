using UnityEngine;

namespace InGame.Items.ItemsProperties.Maps {
    [CreateAssetMenu (fileName = "Properties", menuName = "ScriptableObjects/MapPropertiesScriptableObject", order = 2)]
    public class MapPropertiesScriptableObject : ItemScriptableObject {
        public RegionType regionName;
    }
}