using Data.Bananas;
using Data.Buildables;
using Data.RawMaterial;
using UnityEngine;

namespace Data {
    [CreateAssetMenu (fileName = "Data", menuName = "ScriptableObjects/MeshReferenceScriptableObject", order = 4)]
    public class MeshReferenceScriptableObject : ScriptableObject {
        public GenericDictionary<int, GameObject> buildablePrefabByPrefabIndex;
        public GenericDictionary<int, ItemType> plateformeTypeByPrefabIndex;

        public GenericDictionary<Mesh, BuildableType> buildableTypeByMesh;

        public GenericDictionary<Mesh, int> debrisPrefabIndexByMesh;

        public GenericDictionary<ItemType, BananasDataScriptableObject> bananasDataScriptableObject;
        public GenericDictionary<BuildableType, BuildableDataScriptableObject> buildablesDataScriptableObject;
        public GenericDictionary<ItemType, RawMaterialDataScriptableObject> rawMaterialDataScriptableObjects;
        
        public Mesh matureBananaTree;
    }
}
