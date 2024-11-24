using UnityEngine;

namespace InGame.Items.ItemsProperties.Dropped.ManufacturedItems {
    [CreateAssetMenu (fileName = "Properties", menuName = "ScriptableObjects/ManufacturedItemPropertiesScriptableObject", order = 5)]
    public class ManufacturedItemPropertiesScriptableObject : ItemScriptableObject {
        [Header("craft")]
        public GameObject prefab;
    }
}
