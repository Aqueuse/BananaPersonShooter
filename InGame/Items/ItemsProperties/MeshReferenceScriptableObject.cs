using InGame.Items.ItemsProperties.Bananas;
using InGame.Items.ItemsProperties.Buildables;
using InGame.Items.ItemsProperties.Characters;
using UnityEngine;

namespace InGame.Items.ItemsProperties {
    [CreateAssetMenu (fileName = "Properties", menuName = "ScriptableObjects/MeshReferenceScriptableObject", order = 4)]
    public class MeshReferenceScriptableObject : ScriptableObject {
        public GenericDictionary<BuildableType, GameObject> buildablePrefabByBuildableType;

        public GenericDictionary<CharacterType, GameObject[]> debrisPrefabsBycharacterType;

        public GenericDictionary<BuildableType, BuildablePropertiesScriptableObject> buildablePropertiesScriptableObjects;
        public GenericDictionary<BananaType, BananasPropertiesScriptableObject> bananasPropertiesScriptableObjects;
        
        public GameObject[] visitorsPrefab;
        
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

        public GenericDictionary<MerchantType, CharacterPropertiesScriptableObject> merchantsScriptableObjectByMerchantType;

        public GameObject pirateSpaceshipPrefab;
        public GameObject visitorSpaceshipPrefab;
        public GameObject merchantSpaceshipPrefab;
    }
}
