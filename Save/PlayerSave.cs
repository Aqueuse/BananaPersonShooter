using System;
using System.Collections.Generic;
using System.IO;
using InGame.Player;
using Newtonsoft.Json;
using Save.Templates;
using UnityEngine;

namespace Save {
    public class PlayerSave : MonoBehaviour {
        private BananaMan bananaMan;
        
        private readonly Dictionary<BananaType, int> bananasInventory = new();
        private readonly Dictionary<RawMaterialType, int> rawMaterialsInventory = new();
        private readonly Dictionary<ManufacturedItemsType, int> manufacturedItemsInventory = new();
        private readonly Dictionary<IngredientsType, int> ingredientsInventory = new();
        private readonly Dictionary<FoodType, int> foodInventory = new();

        private BananaManSavedData bananaManSavedData;
        
        private void Start() {
            bananaMan = ObjectsReference.Instance.bananaMan;
        }

        public void LoadPlayer(string saveUuid) {
            bananaManSavedData = LoadPlayerByUuid(saveUuid);
            
            LoadBananasInventory();
            LoadRawMaterialsInventory();
            LoadIngredientsInventory();
            LoadManufacturedItemsInventory();
            LoadFoodInventory();
            
            LoadBananaManVitals();
            LoadPositionAndRotation();

            LoadSlots();
            LoadActiveSlotIndex();
            
            LoadBitkongQuantity();
            LoadDiscoveredMaterials();
        }

        private static BananaManSavedData LoadPlayerByUuid(string saveUuid) {
            var savePath = Path.Combine(ObjectsReference.Instance.gameSave._savesPath, saveUuid);
            
            var savefilePath = Path.Combine(savePath, "player.json");

            var playerString = File.ReadAllText(savefilePath);

            return JsonConvert.DeserializeObject<BananaManSavedData>(playerString);
        }
        
        private void LoadBananasInventory() {
            bananasInventory.Clear();
            
            foreach (var banana in bananaManSavedData.bananaInventory) {
                bananasInventory.Add((BananaType)Enum.Parse(typeof(BananaType), banana.Key), banana.Value);
            }

            ObjectsReference.Instance.BananaManBananasInventory.bananasInventory = bananasInventory;
        }

        private void LoadRawMaterialsInventory() {
            rawMaterialsInventory.Clear();
            
            foreach (var rawMaterial in bananaManSavedData.rawMaterialsInventory) {
                rawMaterialsInventory[(RawMaterialType)Enum.Parse(typeof(RawMaterialType), rawMaterial.Key)] = rawMaterial.Value;
            }
            
            ObjectsReference.Instance.bananaManRawMaterialInventory.rawMaterialsInventory = rawMaterialsInventory;
        }

        private void LoadIngredientsInventory() {
            ingredientsInventory.Clear();
            
            foreach (var ingredient in bananaManSavedData.ingredientsInventory) {
                ingredientsInventory[(IngredientsType)Enum.Parse(typeof(IngredientsType), ingredient.Key)] = ingredient.Value;
            }

            ObjectsReference.Instance.bananaManIngredientsInventory.ingredientsInventory = ingredientsInventory;
        }

        private void LoadManufacturedItemsInventory() {
            manufacturedItemsInventory.Clear();
            
            foreach (var manufacturedItem in bananaManSavedData.manufacturedInventory) {
                manufacturedItemsInventory[(ManufacturedItemsType)Enum.Parse(typeof(ManufacturedItemsType), manufacturedItem.Key)] = manufacturedItem.Value;
            }

            ObjectsReference.Instance.bananaManManufacturedItemsInventory.manufacturedItemsInventory = manufacturedItemsInventory;
        }

        private void LoadFoodInventory() {
            foodInventory.Clear();
            
            foreach (var food in bananaManSavedData.foodInventory) {
                foodInventory[(FoodType)Enum.Parse(typeof(FoodType), food.Key)] = food.Value;
            }

            ObjectsReference.Instance.bananaManFoodInventory.foodInventory = foodInventory;
        }
        
        private void LoadBananaManVitals() {
            bananaMan.health = bananaManSavedData.health;
            bananaMan.resistance = bananaManSavedData.resistance;

            bananaMan.SetBananaSkinHealth();
        }

        private void LoadPositionAndRotation() {
            bananaMan.transform.position = new Vector3(bananaManSavedData.xWorldPosition, bananaManSavedData.yWorldPosition, bananaManSavedData.zworldPosition);
            
            var bananaManRotation = new Vector3(bananaManSavedData.xWorldRotation, bananaManSavedData.yWorldRotation, bananaManSavedData.zWorldRotation);
            bananaMan.transform.rotation = Quaternion.Euler(bananaManRotation);
        }
        
        // [0] ITEM_CATEGORY
        // [1] BUILDABLE_TYPE
        // [2] BANANA_TYPE
        // [3] RAW_MATERIAL_TYPE
        // [4] INGREDIENT_TYPE
        // [5] FOOD_TYPE
        // [6] MANUFACTURED_ITEM_TYPE
        // [7] SlotIndex
        
        private void LoadSlots() {
            foreach (var slot in bananaManSavedData.slots) {
                var rawItemString = slot.Split("-");

                ItemCategory itemCategory = Enum.Parse<ItemCategory>(rawItemString[0]);
                int slotIndex = int.Parse(rawItemString[7]);
                
                switch (itemCategory) {
                    case ItemCategory.BUILDABLE:
                        var buildableType = Enum.Parse<BuildableType>(rawItemString[1]);
                        
                        ObjectsReference.Instance.bottomSlots.SetSlotByIndex(
                            ObjectsReference.Instance.meshReferenceScriptableObject.buildablePropertiesScriptableObjects[buildableType],
                            slotIndex
                        );
                        break;
                    case ItemCategory.BANANA:
                        var bananaType = Enum.Parse<BananaType>(rawItemString[2]);
                        
                        ObjectsReference.Instance.bottomSlots.SetSlotByIndex(
                            ObjectsReference.Instance.meshReferenceScriptableObject.bananasPropertiesScriptableObjects[bananaType],
                            slotIndex
                        );
                        break;
                    case ItemCategory.RAW_MATERIAL:
                        var rawMaterialType = Enum.Parse<RawMaterialType>(rawItemString[3]);
                        
                        ObjectsReference.Instance.bottomSlots.SetSlotByIndex(
                            ObjectsReference.Instance.meshReferenceScriptableObject.rawMaterialPropertiesScriptableObjects[rawMaterialType],
                            slotIndex
                        );
                        break;
                    case ItemCategory.INGREDIENT:
                        var ingredientsType = Enum.Parse<IngredientsType>(rawItemString[4]);
                        
                        ObjectsReference.Instance.bottomSlots.SetSlotByIndex(
                            ObjectsReference.Instance.meshReferenceScriptableObject.ingredientsPropertiesScriptableObjects[ingredientsType],
                            slotIndex
                        );
                        break;
                    case ItemCategory.FOOD:
                        var foodType = Enum.Parse<FoodType>(rawItemString[5]);
                        
                        ObjectsReference.Instance.bottomSlots.SetSlotByIndex(
                            ObjectsReference.Instance.meshReferenceScriptableObject.foodPropertiesScriptableObjects[foodType],
                            slotIndex
                        );
                        break;
                    case ItemCategory.MANUFACTURED_ITEM:
                        var manufacturedItemsType = Enum.Parse<ManufacturedItemsType>(rawItemString[6]);
                        
                        ObjectsReference.Instance.bottomSlots.SetSlotByIndex(
                            ObjectsReference.Instance.meshReferenceScriptableObject.manufacturedItemsPropertiesScriptableObjects[manufacturedItemsType],
                            slotIndex
                        );
                        break;
                }
            }
        }
        
        private void LoadActiveSlotIndex() {
            ObjectsReference.Instance.bottomSlots.activeSlotIndex = bananaManSavedData.activeSlotIndex;
            ObjectsReference.Instance.bottomSlots.ActivateSlot(bananaManSavedData.activeSlotIndex);
        }
        
        private void LoadBitkongQuantity() {
            bananaMan.bananaManData.bitKongQuantity = bananaManSavedData.bitKongQuantity;
        }
        
        private void LoadDiscoveredMaterials() {
            bananaMan.bananaManData.discoveredRawMaterials.Clear();
            
            foreach (var discoveredRawMaterialString in bananaManSavedData.discoveredRawMaterials) {
                RawMaterialType rawMaterialType = Enum.Parse<RawMaterialType>(discoveredRawMaterialString);
                bananaMan.bananaManData.discoveredRawMaterials.Add(rawMaterialType);

                var buildablesToUnlock =
                    ObjectsReference.Instance.meshReferenceScriptableObject.unlockedBuildablesByRawMaterialType[
                        rawMaterialType];
                
                ObjectsReference.Instance.bananaManBuildablesInventory.UnlockBuildablesTier(buildablesToUnlock);    
            }
        }
        
        public void SavePlayerByUuid(string saveUuid) {
            bananaManSavedData ??= new BananaManSavedData();
            
            var savePath = Path.Combine(ObjectsReference.Instance.gameSave._savesPath, saveUuid);
            
            foreach (var inventorySlot in ObjectsReference.Instance.BananaManBananasInventory.bananasInventory) {
                bananaManSavedData.bananaInventory[inventorySlot.Key.ToString()] = inventorySlot.Value;
            }
            foreach (var inventorySlot in ObjectsReference.Instance.bananaManRawMaterialInventory.rawMaterialsInventory) {
                bananaManSavedData.rawMaterialsInventory[inventorySlot.Key.ToString()] = inventorySlot.Value;
            }
            foreach (var inventorySlot in ObjectsReference.Instance.bananaManIngredientsInventory.ingredientsInventory) {
                bananaManSavedData.ingredientsInventory[inventorySlot.Key.ToString()] = inventorySlot.Value;
            }
            foreach (var inventorySlot in ObjectsReference.Instance.bananaManManufacturedItemsInventory.manufacturedItemsInventory) {
                bananaManSavedData.manufacturedInventory[inventorySlot.Key.ToString()] = inventorySlot.Value;
            }
            foreach (var inventorySlot in ObjectsReference.Instance.bananaManFoodInventory.foodInventory) {
                bananaManSavedData.foodInventory[inventorySlot.Key.ToString()] = inventorySlot.Value;
            }

            bananaManSavedData.health = bananaMan.health; 
            bananaManSavedData.resistance = bananaMan.resistance;

            var bananaManPosition = bananaMan.transform.position;
            
            bananaManSavedData.xWorldPosition = bananaManPosition.x;
            bananaManSavedData.yWorldPosition = bananaManPosition.y;
            bananaManSavedData.zworldPosition = bananaManPosition.z;

            var bananaManRotation = bananaMan.transform.rotation.eulerAngles;
            
            bananaManSavedData.xWorldRotation = bananaManRotation.x;
            bananaManSavedData.yWorldRotation = bananaManRotation.y;
            bananaManSavedData.zWorldRotation = bananaManRotation.z;
            
            foreach (var discoveredRawMaterial in bananaMan.bananaManData.discoveredRawMaterials) {
                bananaManSavedData.discoveredRawMaterials.Add(discoveredRawMaterial.ToString());
            }
            
            SaveSlots();
            bananaManSavedData.activeSlotIndex = ObjectsReference.Instance.bottomSlots.activeSlotIndex;
            
            bananaManSavedData.bitKongQuantity = bananaMan.bananaManData.bitKongQuantity;

            var jsonbananaManSaved = JsonConvert.SerializeObject(bananaManSavedData);
        
            var savefilePath = Path.Combine(savePath, "player.json");
            File.WriteAllText(savefilePath, jsonbananaManSaved);
        }

        private void SaveSlots() {
            bananaManSavedData.slots.Clear();
            
            foreach (var bottomSlot in ObjectsReference.Instance.bottomSlots.uiBottomSlots) {
                if (bottomSlot.slotType is SlotType.DROP or SlotType.BUILD) {
                    bananaManSavedData.slots.Add(bottomSlot.GenerateSavedData());
                }
            }
        }
    }
}
