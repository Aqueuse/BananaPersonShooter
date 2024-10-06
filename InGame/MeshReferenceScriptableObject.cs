using InGame.Items.ItemsProperties.Bananas;
using InGame.Items.ItemsProperties.Buildables;
using InGame.Items.ItemsProperties.Characters;
using UnityEngine;
using UnityEngine.Localization;

namespace InGame {
    [CreateAssetMenu (fileName = "Properties", menuName = "ScriptableObjects/MeshReferenceScriptableObject", order = 4)]
    public class MeshReferenceScriptableObject : ScriptableObject {
        public GenericDictionary<BuildableType, GameObject> buildablePrefabByBuildableType;

        public GenericDictionary<CharacterType, GameObject[]> spaceshipDebrisPrefabsByCharacterType;

        public GenericDictionary<DroppedType, GameObject> droppedPrefabByDroppedType;
        
        public GenericDictionary<BuildableType, BuildablePropertiesScriptableObject> buildablePropertiesScriptableObjects;
        public GenericDictionary<BananaType, BananasPropertiesScriptableObject> bananasPropertiesScriptableObjects;
        
        public GameObject[] touristsPrefab;
        public GameObject piratePrefab;
        
        public GenericDictionary<DroppedType, GameObject> prefabByRawMaterialType;
        
        public GenericDictionary<CharacterType, GameObject> spaceshipByCharacterType;

        public GenericDictionary<CharacterType, Color[]> colorSetByCharacterType;
        
        public ColorSet[] merchantsColorsSets;
        public ColorSet[] visitorsColorsSets;

        // since pirate are now mutable in visitor, merchant, or cultivator, they need to switch their item description
        public GenericDictionary<CharacterType, LocalizedString> genericNameByCharacterType;
        public GenericDictionary<CharacterType, LocalizedString> genericDescriptionByCharacterType;

        public GenericDictionary<CharacterType, Sprite> itemPreviewByCharacterType;
    }
}
