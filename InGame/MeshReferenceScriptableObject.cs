using System;
using System.Collections.Generic;
using InGame.Items.ItemsProperties;
using InGame.Items.ItemsProperties.Buildables;
using InGame.Items.ItemsProperties.Dropped.Bananas;
using InGame.Monkeys.Chimpvisitors;
using InGame.WorldMaps;
using UnityEngine;
using UnityEngine.Localization;
using Random = UnityEngine.Random;

namespace InGame {
    [CreateAssetMenu (fileName = "Properties", menuName = "ScriptableObjects/MeshReferenceScriptableObject", order = 4)]
    public class MeshReferenceScriptableObject : ScriptableObject {
        public GenericDictionary<BuildableType, GameObject> buildablePrefabByBuildableType;
        public GenericDictionary<BananaType, GameObject> bananaPrefabByBananaType;
        public GenericDictionary<RawMaterialType, GameObject> rawMaterialPrefabByRawMaterialType;
        public GenericDictionary<BananaColor, GameObject> DyePrefabByBananaColor;
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
        public GenericDictionary<BananaColor, ItemScriptableObject> rawMaterialPropertiesByBananaColor;
        [Space] 
        public List<WorldMap> worldMaps;
        public List<GameObject> monkeyMenPrefabs;
        public GameObject merchimpPrefab;
        [SerializeField] private VisitorColorPresetScriptableObject[] chimpvisitorsColorPresets;
        
        public GenericDictionary<SpaceshipType, GameObject> spaceshipPrefabBySpaceshipType;
        [Space]
        public GenericDictionary<SpaceshipType, List<GameObject>> spaceshipDebrisBySpaceshipType;
        public GenericDictionary<RawMaterialType, int> rawMaterialSpawnProbability;
        public GenericDictionary<RawMaterialType, int> rawMaterialSpawnMaxQuantity;
        
        public Material blueprintBuildableMaterial;
        public Material completedBuildableMaterial;
        
        // since pirate are now mutable in visitor, merchant, or cultivator, they need to switch their item description
        public List<string> genericNameByCharacterType;
        public GenericDictionary<CharacterType,LocalizedString> genericDescriptionByCharacterType;
        [Space]
        public GenericDictionary<CharacterType, Sprite> itemPreviewByCharacterType;
        
        [Space]
        public GameObject teleportDownVFXPrefab;
        
        private ItemScriptableObject activeItem;
        
        public static SpaceshipType GetRandomSpaceshipType() {
            var spaceshipTypes = Enum.GetValues(typeof(SpaceshipType));

            return (SpaceshipType)spaceshipTypes.GetValue(Random.Range(0, spaceshipTypes.Length - 1));
        }

        public GameObject GetActiveDroppablePrefab() {
            activeItem = ObjectsReference.Instance.bottomSlotsManager.GetSelectedSlot(); 
            
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

        public string GetRandomChimpmenName() {
            return genericNameByCharacterType[Random.Range(0, genericNameByCharacterType.Count - 1)];
        }

        public Color[] GetRandomChimpenColorsPreset() {
            return chimpvisitorsColorPresets[0].colors;
        }
    }
}
