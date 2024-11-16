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
        private Transform bananaManTransform;
        
        private Dictionary<BananaType, int> bananasInventory;
        private Dictionary<DroppedType, int> droppedInventory;
        private Dictionary<ManufacturedItemsType, int> manufacturedItemsInventory;
        private Dictionary<IngredientsType, int> ingredientsInventory;

        private BananaManSavedData bananaManSavedData;
        
        private void Start() {
            bananaMan = ObjectsReference.Instance.bananaMan;
            bananaManTransform = bananaMan.transform;
        }

        public void LoadPlayer(string saveUuid) {
            bananaManSavedData = LoadPlayerByUuid(saveUuid);
            
            LoadBananasInventory();
            LoadDroppedInventory();
            LoadIngredientsInventory();
            LoadManufacturedItemsInventory();
            
            LoadSelectedBanana();
            LoadBananaManVitals();
            LoadPositionAndRotation();

            LoadBananaGun();
            LoadActiveBanana();
            LoadActiveBuildable();
            LoadBitkongQuantity();
            CheckTutorialFinished();
            
            ObjectsReference.Instance.uInventoriesManager.RefreshInventories();
        }
        
        private static BananaManSavedData LoadPlayerByUuid(string saveUuid) {
            var savePath = Path.Combine(ObjectsReference.Instance.gameSave._savesPath, saveUuid);
            
            var savefilePath = Path.Combine(savePath, "player.json");

            var playerString = File.ReadAllText(savefilePath);

            return JsonConvert.DeserializeObject<BananaManSavedData>(playerString);
        }
        
        private void LoadBananasInventory() {
            bananasInventory = bananaMan.bananaManData.bananasInventory;

            foreach (var banana in bananaManSavedData.bananaInventory) {
                bananasInventory[(BananaType)Enum.Parse(typeof(BananaType), banana.Key)] = banana.Value;
            }
        }

        private void LoadDroppedInventory() {
            droppedInventory = bananaMan.bananaManData.droppedInventory;

            foreach (var dropped in bananaManSavedData.droppedInventory) {
                droppedInventory[(DroppedType)Enum.Parse(typeof(DroppedType), dropped.Key)] = dropped.Value;
            }
        }

        private void LoadIngredientsInventory() {
            ingredientsInventory = bananaMan.bananaManData.ingredientsInventory;
            ObjectsReference.Instance.bananaManUiIngredientsInventory.ingredientsInventory = ingredientsInventory;

            foreach (var ingredient in bananaManSavedData.ingredientsInventory) {
                ingredientsInventory[(IngredientsType)Enum.Parse(typeof(IngredientsType), ingredient.Key)] = ingredient.Value;
            }
        }

        private void LoadManufacturedItemsInventory() {
            manufacturedItemsInventory = bananaMan.bananaManData.manufacturedItemsInventory;
            ObjectsReference.Instance.bananaManUiManufacturedItemsInventory.manufacturedItemsInventory = manufacturedItemsInventory;

            foreach (var manufacturedItem in bananaManSavedData.manufacturedInventory) {
                manufacturedItemsInventory[(ManufacturedItemsType)Enum.Parse(typeof(ManufacturedItemsType), manufacturedItem.Key)] = manufacturedItem.Value;
            }
        }

        private void LoadBananaGun() {
            var bananagunMode = Enum.Parse<BananaGunMode>(bananaManSavedData.bananaGunMode);
            bananaMan.bananaGunMode = bananagunMode;
            ObjectsReference.Instance.bananaGunActionsSwitch.SwitchToBananaGunMode(bananaMan.bananaGunMode);
        }
        
        private void LoadSelectedBanana() {
            var bananaSlotType = Enum.Parse<BananaType>(bananaManSavedData.bananaSlot);
            var bananaScriptableObject = ObjectsReference.Instance.meshReferenceScriptableObject.bananasPropertiesScriptableObjects[bananaSlotType];
            bananaMan.bananaManData.activeBanana = bananaScriptableObject;
            ObjectsReference.Instance.uiFlippers.SetBananaQuantity(bananaMan.bananaManData.bananasInventory[bananaScriptableObject.bananaType]);
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
        
        private void LoadActiveBanana() {
            var activeBananaType = bananaManSavedData.activeBanana;
            bananaMan.bananaManData.activeBanana = ObjectsReference.Instance.meshReferenceScriptableObject.bananasPropertiesScriptableObjects[activeBananaType];

            ObjectsReference.Instance.uiFlippers.SetBananaType(bananaMan.bananaManData.activeBanana);
            ObjectsReference.Instance.uiFlippers.SetBananaQuantity(ObjectsReference.Instance.bananasInventory.GetQuantity(activeBananaType));
        }
        
        private void LoadActiveBuildable() {
            var activeBuildableType = bananaManSavedData.activeBuildable;
            
            bananaMan.bananaManData.activeBuildable = ObjectsReference.Instance.meshReferenceScriptableObject.buildablePropertiesScriptableObjects[activeBuildableType];
            ObjectsReference.Instance.uiFlippers.SetBuildable(bananaMan.bananaManData.activeBuildable.blueprintSprite);
            ObjectsReference.Instance.uiFlippers.SetBuildablePlacementAvailability(ObjectsReference.Instance.droppedInventory.HasCraftingIngredients(activeBuildableType));
        }
        
        private void LoadBitkongQuantity() {
            bananaMan.bananaManData.bitKongQuantity = bananaManSavedData.bitKongQuantity;
        }
        
        private void CheckTutorialFinished() {
            bananaMan.tutorialFinished = bananaManSavedData.hasFinishedTutorial;
            
            if (bananaManSavedData.hasFinishedTutorial) {
                ObjectsReference.Instance.bananaGun.bananaGunInBack.SetActive(true);
                ObjectsReference.Instance.uiManager.SetActive(UICanvasGroupType.HUD_BANANAMAN, true);
            }

            else {
                ObjectsReference.Instance.bananaGun.bananaGunInBack.SetActive(false);
                ObjectsReference.Instance.uiManager.SetActive(UICanvasGroupType.HUD_BANANAMAN, false);
                ObjectsReference.Instance.uiCrosshairs.SetCrosshair(BananaType.EMPTY);
            }
        }

        public void SavePlayerByUuid(string saveUuid) {
            bananaManSavedData ??= new BananaManSavedData();
            
            var savePath = Path.Combine(ObjectsReference.Instance.gameSave._savesPath, saveUuid);
            
            foreach (var inventorySlot in bananaMan.bananaManData.bananasInventory) {
                bananaManSavedData.bananaInventory[inventorySlot.Key.ToString()] = inventorySlot.Value;
            }
            foreach (var inventorySlot in bananaMan.bananaManData.droppedInventory) {
                bananaManSavedData.droppedInventory[inventorySlot.Key.ToString()] = inventorySlot.Value;
            }
            foreach (var inventorySlot in bananaMan.bananaManData.ingredientsInventory) {
                bananaManSavedData.ingredientsInventory[inventorySlot.Key.ToString()] = inventorySlot.Value;
            }
            foreach (var inventorySlot in bananaMan.bananaManData.manufacturedItemsInventory) {
                bananaManSavedData.manufacturedInventory[inventorySlot.Key.ToString()] = inventorySlot.Value;
            }

            bananaManSavedData.bananaGunMode = bananaMan.bananaGunMode.ToString();
            bananaManSavedData.bananaSlot = bananaMan.bananaManData.activeBanana.bananaType.ToString();
            bananaManSavedData.health = bananaMan.health; 
            bananaManSavedData.resistance = bananaMan.resistance;

            var bananaManPosition = bananaManTransform.position;
            
            bananaManSavedData.xWorldPosition = bananaManPosition.x;
            bananaManSavedData.yWorldPosition = bananaManPosition.y;
            bananaManSavedData.zworldPosition = bananaManPosition.z;

            var bananaManRotation = bananaMan.transform.rotation.eulerAngles;
            
            bananaManSavedData.xWorldRotation = bananaManRotation.x;
            bananaManSavedData.yWorldRotation = bananaManRotation.y;
            bananaManSavedData.zWorldRotation = bananaManRotation.z;

            bananaManSavedData.hasFinishedTutorial = bananaMan.tutorialFinished;

            if (bananaMan.bananaManData.activeBanana != null)
                bananaManSavedData.activeBanana = bananaMan.bananaManData.activeBanana.bananaType;
            
            if (bananaMan.bananaManData.activeBuildable != null)
                bananaManSavedData.activeBuildable = bananaMan.bananaManData.activeBuildable.buildableType;

            bananaManSavedData.bitKongQuantity = bananaMan.bananaManData.bitKongQuantity;
        
            var jsonbananaManSaved = JsonConvert.SerializeObject(bananaManSavedData);
        
            var savefilePath = Path.Combine(savePath, "player.json");
            File.WriteAllText(savefilePath, jsonbananaManSaved);
        }
    }
}
