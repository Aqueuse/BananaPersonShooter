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
            
            LoadBananaGun();
            LoadActiveDroppable();
            LoadActiveBuildable();
            LoadBitkongQuantity();
            CheckTutorialFinished();
        }
        
        private static BananaManSavedData LoadPlayerByUuid(string saveUuid) {
            var savePath = Path.Combine(ObjectsReference.Instance.gameSave._savesPath, saveUuid);
            
            var savefilePath = Path.Combine(savePath, "player.json");

            var playerString = File.ReadAllText(savefilePath);

            return JsonConvert.DeserializeObject<BananaManSavedData>(playerString);
        }
        
        private void LoadBananasInventory() {
            bananaMan.bananaManData.activeBanana = bananaManSavedData.activeBanana;

            foreach (var banana in bananaManSavedData.bananaInventory) {
                bananasInventory.Add((BananaType)Enum.Parse(typeof(BananaType), banana.Key), banana.Value);
            }

            ObjectsReference.Instance.BananaManBananasInventory.bananasInventory = bananasInventory;
        }

        private void LoadRawMaterialsInventory() {
            bananaMan.bananaManData.activeRawMaterial = bananaManSavedData.activeRawMaterial;
            
            foreach (var rawMaterial in bananaManSavedData.rawMaterialsInventory) {
                rawMaterialsInventory[(RawMaterialType)Enum.Parse(typeof(RawMaterialType), rawMaterial.Key)] = rawMaterial.Value;
            }
            
            ObjectsReference.Instance.bananaManRawMaterialInventory.rawMaterialsInventory = rawMaterialsInventory;
        }

        private void LoadIngredientsInventory() {
            bananaMan.bananaManData.activeIngredient = bananaManSavedData.activeIngredient;

            foreach (var ingredient in bananaManSavedData.ingredientsInventory) {
                ingredientsInventory[(IngredientsType)Enum.Parse(typeof(IngredientsType), ingredient.Key)] = ingredient.Value;
            }

            ObjectsReference.Instance.bananaManIngredientsInventory.ingredientsInventory = ingredientsInventory;
        }

        private void LoadManufacturedItemsInventory() {
            bananaMan.bananaManData.activeManufacturedItem = bananaManSavedData.activeManufacturedItem;
            
            foreach (var manufacturedItem in bananaManSavedData.manufacturedInventory) {
                manufacturedItemsInventory[(ManufacturedItemsType)Enum.Parse(typeof(ManufacturedItemsType), manufacturedItem.Key)] = manufacturedItem.Value;
            }

            ObjectsReference.Instance.bananaManManufacturedItemsInventory.manufacturedItemsInventory = manufacturedItemsInventory;
        }

        private void LoadFoodInventory() {
            bananaMan.bananaManData.activeFood = bananaManSavedData.activeFood;

            foreach (var food in bananaManSavedData.foodInventory) {
                foodInventory[(FoodType)Enum.Parse(typeof(FoodType), food.Key)] = food.Value;
            }

            ObjectsReference.Instance.bananaManFoodInventory.foodInventory = foodInventory;
        }

        private void LoadBananaGun() {
            var bananagunMode = Enum.Parse<BananaGunMode>(bananaManSavedData.bananaGunMode);
            bananaMan.bananaGunMode = bananagunMode;
            ObjectsReference.Instance.bananaGunActionsSwitch.SwitchToBananaGunMode(bananaMan.bananaGunMode);
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

        private void LoadActiveDroppable() {
            switch (bananaManSavedData.activeDroppable) {
                case DroppedType.BANANA:
                    ObjectsReference.Instance.bananaMan.bananaManData.activeDroppableItem =
                        ObjectsReference.Instance.meshReferenceScriptableObject.bananasPropertiesScriptableObjects[
                            bananaManSavedData.activeBanana];
                    break;
                case DroppedType.RAW_MATERIAL:
                    ObjectsReference.Instance.bananaMan.bananaManData.activeDroppableItem =
                        ObjectsReference.Instance.meshReferenceScriptableObject.rawMaterialPropertiesScriptableObjects[
                            bananaManSavedData.activeRawMaterial];
                    break;
                case DroppedType.INGREDIENTS:
                    ObjectsReference.Instance.bananaMan.bananaManData.activeDroppableItem =
                        ObjectsReference.Instance.meshReferenceScriptableObject.ingredientsPropertiesScriptableObjects[
                            bananaManSavedData.activeIngredient];
                    break;
                case DroppedType.MANUFACTURED_ITEMS:
                    ObjectsReference.Instance.bananaMan.bananaManData.activeDroppableItem =
                        ObjectsReference.Instance.meshReferenceScriptableObject.manufacturedItemsPropertiesScriptableObjects[
                            bananaManSavedData.activeManufacturedItem];
                    break;
                case DroppedType.FOOD:
                    ObjectsReference.Instance.bananaMan.bananaManData.activeDroppableItem =
                        ObjectsReference.Instance.meshReferenceScriptableObject.foodPropertiesScriptableObjects[
                            bananaManSavedData.activeFood];
                    break;
            }
        }
        
        private void LoadActiveBuildable() {
            bananaMan.bananaManData.activeBuildable = bananaManSavedData.activeBuildable;
            ObjectsReference.Instance.uiFlippers.RefreshActiveBuildableAvailability();
        }
        
        private void LoadBitkongQuantity() {
            bananaMan.bananaManData.bitKongQuantity = bananaManSavedData.bitKongQuantity;
        }
        
        private void CheckTutorialFinished() {
            bananaMan.tutorialFinished = bananaManSavedData.hasFinishedTutorial;
            
            if (bananaManSavedData.hasFinishedTutorial) {
                ObjectsReference.Instance.bananaGun.GrabBananaGun();
                ObjectsReference.Instance.uiManager.SetActive(UICanvasGroupType.HUD_BANANAMAN, true);
            }

            else {
                ObjectsReference.Instance.bananaGun.UngrabBananaGun();
                ObjectsReference.Instance.uiManager.SetActive(UICanvasGroupType.HUD_BANANAMAN, false);
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

            bananaManSavedData.bananaGunMode = bananaMan.bananaGunMode.ToString();
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

            bananaManSavedData.hasFinishedTutorial = bananaMan.tutorialFinished;

            bananaManSavedData.activeDroppable = bananaMan.bananaManData.activeDropped;
            bananaManSavedData.activeBanana = bananaMan.bananaManData.activeBanana;
            bananaManSavedData.activeBuildable = bananaMan.bananaManData.activeBuildable;
            bananaManSavedData.activeIngredient = bananaMan.bananaManData.activeIngredient;
            bananaManSavedData.activeManufacturedItem = bananaMan.bananaManData.activeManufacturedItem;
            bananaManSavedData.activeFood = bananaMan.bananaManData.activeFood;

            bananaManSavedData.bitKongQuantity = bananaMan.bananaManData.bitKongQuantity;

            var jsonbananaManSaved = JsonConvert.SerializeObject(bananaManSavedData);
        
            var savefilePath = Path.Combine(savePath, "player.json");
            File.WriteAllText(savefilePath, jsonbananaManSaved);
        }
    }
}
