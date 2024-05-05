using InGame.Items.ItemsProperties.Bananas;
using InGame.Items.ItemsProperties.Buildables;
using InGame.Items.ItemsProperties.Characters;
using UnityEngine;

namespace InGame {
    [CreateAssetMenu (fileName = "Properties", menuName = "ScriptableObjects/MeshReferenceScriptableObject", order = 4)]
    public class MeshReferenceScriptableObject : ScriptableObject {
        public GenericDictionary<BuildableType, GameObject> buildablePrefabByBuildableType;

        public GenericDictionary<CharacterType, GameObject[]> debrisPrefabsByCharacterType;

        public GenericDictionary<BuildableType, BuildablePropertiesScriptableObject> buildablePropertiesScriptableObjects;
        public GenericDictionary<BananaType, BananasPropertiesScriptableObject> bananasPropertiesScriptableObjects;
        
        public GameObject[] touristsPrefab;
        public GameObject piratePrefab;
        
        public GenericDictionary<RawMaterialType, GameObject> prefabByRawMaterialType;

        public GenericDictionary<MerchantType, CharacterPropertiesScriptableObject> merchantsScriptableObjectByMerchantType;

        public GenericDictionary<CharacterType, GameObject> spaceshipByCharacterType;
    }
}
