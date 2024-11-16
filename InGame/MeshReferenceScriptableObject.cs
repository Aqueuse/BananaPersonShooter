using System;
using System.Collections.Generic;
using InGame.Items.ItemsProperties.Bananas;
using InGame.Items.ItemsProperties.Buildables;
using InGame.Items.ItemsProperties.Characters;
using UnityEngine;
using UnityEngine.Localization;
using Random = UnityEngine.Random;

namespace InGame {
    [CreateAssetMenu (fileName = "Properties", menuName = "ScriptableObjects/MeshReferenceScriptableObject", order = 4)]
    public class MeshReferenceScriptableObject : ScriptableObject {
        public GenericDictionary<BuildableType, GameObject> buildablePrefabByBuildableType;
        public GenericDictionary<DroppedType, GameObject> droppedPrefabByDroppedType;
        public GenericDictionary<MeteoriteType, GameObject> meteoritePrefabByMeteoriteType;
        
        public GenericDictionary<BuildableType, BuildablePropertiesScriptableObject> buildablePropertiesScriptableObjects;
        public GenericDictionary<BananaType, BananasPropertiesScriptableObject> bananasPropertiesScriptableObjects;
        
        public GenericDictionary<MonkeyMenType, GameObject> monkeyMenPrefabByMonkeyMenType;
        public GenericDictionary<SpaceshipType, GameObject> spaceshipPrefabBySpaceshipType;

        public GenericDictionary<SpaceshipType, List<GameObject>> spaceshipDebrisBySpaceshipType;
        
        public GameObject GetRandomChimpmen() {
            var chimpmenTypes = Enum.GetValues(typeof(MonkeyMenType));

            var randomChimpmenType = chimpmenTypes.GetValue(Random.Range(0, chimpmenTypes.Length - 1));
            
            return monkeyMenPrefabByMonkeyMenType[(MonkeyMenType)randomChimpmenType];
        }

        public static SpaceshipType GetRandomSpaceshipType() {
            var spaceshipTypes = Enum.GetValues(typeof(SpaceshipType));

            return (SpaceshipType)spaceshipTypes.GetValue(Random.Range(0, spaceshipTypes.Length - 1));
        }
        
        public GenericDictionary<BananaEffect, Color> bananaGoopColorByEffectType;
        
        public GenericDictionary<DroppedType, GameObject> prefabByRawMaterialType;

        public List<MonkeyMenPropertiesScriptableObject> chimpMensAppearanceScriptableObjects;
        
        // since pirate are now mutable in visitor, merchant, or cultivator, they need to switch their item description
        public GenericDictionary<CharacterType, LocalizedString> genericNameByCharacterType;
        public GenericDictionary<CharacterType, LocalizedString> genericDescriptionByCharacterType;

        public GenericDictionary<CharacterType, Sprite> itemPreviewByCharacterType;

        public GameObject teleportDownVFXPrefab;
    }
}
