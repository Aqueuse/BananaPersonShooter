using System.Collections.Generic;
using Data.Bananas;
using Data.Buildables;
using Data.RawMaterial;
using Enums;
using UnityEngine;

namespace Data {
    [CreateAssetMenu (fileName = "Data", menuName = "ScriptableObjects/MeshReferenceScriptableObject", order = 4)]
    public class MeshReferenceScriptableObject : ScriptableObject {
        public GenericDictionary<Mesh, BuildableType> buildableTypeByMesh;
        public GenericDictionary<BuildableType, GameObject> buildablePrefabByBuildableType;

        public GameObject[] debrisPrefab;
        public GameObject[] ruinesPrefab;
        
        public List<Mesh> debrisMeshes;
        public List<Mesh> ruinesMeshes;
        
        public GameObject chimployeePrefab;
        
        public GenericDictionary<ItemType, BananasDataScriptableObject> bananasDataScriptableObject;
        public GenericDictionary<BuildableType, BuildableDataScriptableObject> buildablesDataScriptableObject;
        public GenericDictionary<ItemType, RawMaterialDataScriptableObject> rawMaterialDataScriptableObjects;
    }
}
