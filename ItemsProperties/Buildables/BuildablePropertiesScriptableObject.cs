using UnityEngine;

namespace ItemsProperties.Buildables {
    [CreateAssetMenu (fileName = "Properties", menuName = "ScriptableObjects/buildablePropertiesScriptableObject", order = 2)]
    public class BuildablePropertiesScriptableObject : ItemScriptableObject {
        [Header("craft")]
        public GenericDictionary<RawMaterialType, int> rawMaterialsWithQuantity;
        public GameObject buildablePrefab;
    }
}