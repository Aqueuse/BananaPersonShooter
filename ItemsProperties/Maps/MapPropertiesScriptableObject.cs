using UnityEngine;

namespace ItemsProperties.Maps {
    [CreateAssetMenu (fileName = "Properties", menuName = "ScriptableObjects/MapPropertiesScriptableObject", order = 2)]
    public class MapPropertiesScriptableObject : ItemScriptableObject {
        public SceneType sceneName;
    }
}