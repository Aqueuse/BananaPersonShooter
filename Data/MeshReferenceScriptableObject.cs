using Data.Bananas;
using Data.Buildables;
using UnityEngine;

namespace Data {
    [CreateAssetMenu (fileName = "Data", menuName = "ScriptableObjects/MeshReferenceScriptableObject", order = 4)]
    public class MeshReferenceScriptableObject : ScriptableObject {
        public GenericDictionary<BuildableType, GameObject> buildablePrefabByBuildableType;

        public GameObject[] piratesDebrisPrefab;
        public GameObject[] visitorsDebrisPrefab;
        
        public GenericDictionary<BuildableType, BuildableDataScriptableObject> buildableDataScriptableObjects;
        public GenericDictionary<BananaType, BananasDataScriptableObject> bananasDataScriptableObjects;
        
        public GameObject GetRandomDebrisByCharacterType(CharacterType characterType) {
            switch (characterType) {
                case CharacterType.PIRATE:
                    return piratesDebrisPrefab[Random.Range(0, piratesDebrisPrefab.Length - 1)];
                case CharacterType.VISITOR:
                    return visitorsDebrisPrefab[Random.Range(0, visitorsDebrisPrefab.Length - 1)];
            }

            return null;
        }
    }
}
