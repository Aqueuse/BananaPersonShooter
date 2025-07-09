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
        public List<GameObject> monkeyMenPrefabs;
        public GameObject merchimpPrefab;
        public GenericDictionary<SpaceshipType, GameObject> spaceshipPrefabBySpaceshipType;
        [Space]
        public GenericDictionary<SpaceshipType, List<GameObject>> spaceshipDebrisBySpaceshipType;
        public GenericDictionary<BananaEffect, Color> bananaGoopColorByEffectType;
        public Material blueprintBuildableMaterial;
        public Material completedBuildableMaterial;
        [Space]
        public List<MonkeyMenPropertiesScriptableObject> visitorsAppearanceScriptableObjects;
        public List<MonkeyMenPropertiesScriptableObject> merchimpsAppearanceScriptableObjects;
        
        // since pirate are now mutable in visitor, merchant, or cultivator, they need to switch their item description
        public GenericDictionary<CharacterType, LocalizedString> genericNameByCharacterType;
        public GenericDictionary<CharacterType, LocalizedString> genericDescriptionByCharacterType;
        [Space]
        public GenericDictionary<CharacterType, Sprite> itemPreviewByCharacterType;
        public GameObject groupPrefab;
        
        public GroupScriptableObject[] visitorsGroupsScriptableObjects;
        public GroupScriptableObject[] merchimpsGroupsScriptableObjects;
        
        [Space]
        public GameObject teleportDownVFXPrefab;

        private ItemScriptableObject activeItem;
        
        public GroupScriptableObject GetNextVisitorGroup() {
            ObjectsReference.Instance.worldData.lastVisitorGroup += 1;

            if (ObjectsReference.Instance.worldData.lastVisitorGroup > visitorsGroupsScriptableObjects.Length-1) {
                ObjectsReference.Instance.worldData.lastVisitorGroup = 0;
            }

            return visitorsGroupsScriptableObjects[ObjectsReference.Instance.worldData.lastVisitorGroup];
        }
        
        public GroupScriptableObject GetNextMerchimpGroup() {
            ObjectsReference.Instance.worldData.lastMerchimpGroup += 1;

            if (ObjectsReference.Instance.worldData.lastMerchimpGroup > merchimpsGroupsScriptableObjects.Length-1) {
                ObjectsReference.Instance.worldData.lastMerchimpGroup = 0;
            }

            return merchimpsGroupsScriptableObjects[ObjectsReference.Instance.worldData.lastMerchimpGroup];
        }
        
        public static SpaceshipType GetRandomSpaceshipType() {
            var spaceshipTypes = Enum.GetValues(typeof(SpaceshipType));

            return (SpaceshipType)spaceshipTypes.GetValue(Random.Range(0, spaceshipTypes.Length - 1));
        }

        public GameObject GetActiveDroppablePrefab() {
            activeItem = ObjectsReference.Instance.bottomSlots.GetSelectedSlot(); 
            
            switch (activeItem.droppedType) {
                case DroppedType.BANANA:
                    return bananaPrefabByBananaType[activeItem.bananaType];
                case DroppedType.INGREDIENTS:
                    return ingredientPrefabByIngredientType[activeItem.ingredientsType];
                case DroppedType.RAW_MATERIAL:
                    return rawMaterialPrefabByRawMaterialType[activeItem.rawMaterialType];
                case DroppedType.MANUFACTURED_ITEMS:
                    return manufacturedItemPrefabByManufacturedItemType[activeItem.manufacturedItemsType];
                case DroppedType.FOOD:
                    return foodPrefabByFoodType[activeItem.foodType];
            }

            return bananaPrefabByBananaType[BananaType.CAVENDISH];
        }

        public int GetRandomMerchimpPropertiesIndex() {
            return Random.Range(0, merchimpsAppearanceScriptableObjects.Count - 1);
        }
    }
}
