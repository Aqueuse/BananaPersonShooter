using ItemsProperties.Bananas;
using ItemsProperties.Buildables;
using UnityEngine;

namespace ItemsProperties {
    [CreateAssetMenu (fileName = "Properties", menuName = "ScriptableObjects/MeshReferenceScriptableObject", order = 4)]
    public class MeshReferenceScriptableObject : ScriptableObject {
        public GenericDictionary<BuildableType, GameObject> buildablePrefabByBuildableType;

        public GenericDictionary<CharacterType, GameObject[]> debrisPrefabsBycharacterType;

        public GenericDictionary<BuildableType, BuildablePropertiesScriptableObject> buildablePropertiesScriptableObjects;
        public GenericDictionary<BananaType, BananasPropertiesScriptableObject> bananasPropertiesScriptableObjects;
        
        public GenericDictionary<VisitorType, GameObject> visitorPrefabByPrefabIndex;
        
        public GameObject GetRandomDebrisByCharacterType(CharacterType characterType) {
            switch (characterType) {
                case CharacterType.PIRATE:
                    return debrisPrefabsBycharacterType[CharacterType.PIRATE][Random.Range(0, debrisPrefabsBycharacterType[CharacterType.PIRATE].Length - 1)];
                case CharacterType.VISITOR:
                    return debrisPrefabsBycharacterType[CharacterType.VISITOR][Random.Range(0, debrisPrefabsBycharacterType[CharacterType.VISITOR].Length - 1)];
            }

            return null;
        }

        public GenericDictionary<RawMaterialType, GameObject> prefabByRawMaterialType;
    }
}
