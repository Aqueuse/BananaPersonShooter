using UnityEngine;

namespace Data.Buildables {
    [CreateAssetMenu (fileName = "Data", menuName = "ScriptableObjects/buildableDataScriptableObject", order = 2)]
    public class BuildableDataScriptableObject : ItemScriptableObject {
        [Header("craft")] 
        public GenericDictionary<ItemType, int> rawMaterialsWithQuantity;
        public GameObject buildablePrefab;

        public bool mustSnap;
        public BuildableGridSize buildableGridSize;
    }
}