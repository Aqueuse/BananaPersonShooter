using System;
using System.Collections.Generic;
using InGame.Items.ItemsProperties;
using InGame.Items.ItemsProperties.Buildables;
using InGame.Items.ItemsProperties.Characters;
using InGame.Items.ItemsProperties.Dropped.Bananas;
using UnityEngine;
using UnityEngine.Localization;
using Random = UnityEngine.Random;

namespace InGame {
    [CreateAssetMenu (fileName = "Properties", menuName = "ScriptableObjects/MeshReferenceScriptableObject", order = 4)]
    public class MeshReferenceScriptableObject : ScriptableObject {
        public GenericDictionary<BuildableType, GameObject> buildablePrefabByBuildableType;
        public GenericDictionary<BananaType, GameObject> bananaPrefabByBananaType;
        public GenericDictionary<RawMaterialType, GameObject> rawMaterialPrefabByRawMaterialType;
        public GenericDictionary<BananaEffect, GameObject> DyePrefabByBananaEffect;
        public GenericDictionary<IngredientsType, GameObject> ingredientPrefabByIngredientType;
        public GenericDictionary<ManufacturedItemsType, GameObject> manufacturedItemPrefabByManufacturedItemType;
        public GenericDictionary<FoodType, GameObject> foodPrefabByFoodType;
        public GenericDictionary<MeteoriteType, GameObject> meteoritePrefabByMeteoriteType;
        [Space]
        public GenericDictionary<BuildableType, BuildablePropertiesScriptableObject> buildablePropertiesScriptableObjects;
        public GenericDictionary<BananaType, BananasPropertiesScriptableObject> bananasPropertiesScriptableObjects;
        public GenericDictionary<RawMaterialType, ItemScriptableObject> rawMaterialPropertiesScriptableObjects;
        public GenericDictionary<ManufacturedItemsType, ItemScriptableObject> manufacturedItemsPropertiesScriptableObjects;
        public GenericDictionary<IngredientsType, ItemScriptableObject> ingredientsPropertiesScriptableObjects;
        public GenericDictionary<FoodType, ItemScriptableObject> foodPropertiesScriptableObjects;
        public GenericDictionary<BananaEffect, ItemScriptableObject> rawMaterialPropertiesByBananaEffect;
        [Space]
        public GenericDictionary<RawMaterialType, BuildableType[]> unlockedBuildablesByRawMaterialType;
        [Space]
        public GenericDictionary<MonkeyMenType, GameObject> monkeyMenPrefabByMonkeyMenType;
        public GenericDictionary<SpaceshipType, GameObject> spaceshipPrefabBySpaceshipType;
        [Space]
        public GenericDictionary<SpaceshipType, List<GameObject>> spaceshipDebrisBySpaceshipType;
        public GenericDictionary<BananaEffect, Color> bananaGoopColorByEffectType;
        public GenericDictionary<RawMaterialType, GameObject> prefabByRawMaterialType;
        [Space]
        public List<MonkeyMenPropertiesScriptableObject> chimpMensAppearanceScriptableObjects;
        
        // since pirate are now mutable in visitor, merchant, or cultivator, they need to switch their item description
        public GenericDictionary<CharacterType, LocalizedString> genericNameByCharacterType;
        public GenericDictionary<CharacterType, LocalizedString> genericDescriptionByCharacterType;
        [Space]
        public GenericDictionary<CharacterType, Sprite> itemPreviewByCharacterType;
        [Space]
        public GameObject teleportDownVFXPrefab;

        public GameObject GetRandomChimpmen() {
            var chimpmenTypes = Enum.GetValues(typeof(MonkeyMenType));

            var randomChimpmenType = chimpmenTypes.GetValue(Random.Range(0, chimpmenTypes.Length - 1));
            
            return monkeyMenPrefabByMonkeyMenType[(MonkeyMenType)randomChimpmenType];
        }

        public static SpaceshipType GetRandomSpaceshipType() {
            var spaceshipTypes = Enum.GetValues(typeof(SpaceshipType));

            return (SpaceshipType)spaceshipTypes.GetValue(Random.Range(0, spaceshipTypes.Length - 1));
        }

        public GameObject GetActiveDroppablePrefab() {
            switch (ObjectsReference.Instance.bananaMan.bananaManData.activeDropped) {
                case DroppedType.BANANA:
                    return bananaPrefabByBananaType[ObjectsReference.Instance.bananaMan.bananaManData.activeBanana];
                case DroppedType.INGREDIENTS:
                    return ingredientPrefabByIngredientType[ObjectsReference.Instance.bananaMan.bananaManData.activeIngredient];
                case DroppedType.RAW_MATERIAL:
                    return rawMaterialPrefabByRawMaterialType[ObjectsReference.Instance.bananaMan.bananaManData.activeRawMaterial];
                case DroppedType.MANUFACTURED_ITEMS:
                    return manufacturedItemPrefabByManufacturedItemType[ObjectsReference.Instance.bananaMan.bananaManData.activeManufacturedItem];
                case DroppedType.FOOD:
                    return foodPrefabByFoodType[ObjectsReference.Instance.bananaMan.bananaManData.activeFood];
            }

            return bananaPrefabByBananaType[BananaType.CAVENDISH];
        }
    }
}
