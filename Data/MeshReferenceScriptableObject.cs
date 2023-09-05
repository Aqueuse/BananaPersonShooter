using System.Collections.Generic;
using Data.Bananas;
using Data.Buildables;
using Enums;
using Tags;
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

        public GenericDictionary<GAME_OBJECT_TAG, ItemScriptableObject> gameObjectDataScriptableObjectsByTag;
        
        public GenericDictionary<BuildableType, BuildableDataScriptableObject> buildableDataScriptableObjects;
        public GenericDictionary<BananaType, BananasDataScriptableObject> bananasDataScriptableObjects;
    }
}
