using System;
using System.IO;
using System.Linq;
using InGame.Player;
using Newtonsoft.Json;
using Save.Templates;
using UnityEngine;

namespace Save {
    public class PlayerSave : MonoBehaviour {
        private BananaMan bananaMan;
        private Transform bananaManTransform;
        
        private GenericDictionary<BananaType, int> bananasInventory;
        private GenericDictionary<RawMaterialType, int> rawMaterialsInventory;
        private GenericDictionary<ManufacturedItemsType, int> manufacturedItemsInventory;
        private GenericDictionary<IngredientsType, int> ingredientsInventory;

        private BananaManSavedData bananaManSavedData;
        
        private void Start() {
            bananaMan = ObjectsReference.Instance.bananaMan;
            bananaManTransform = bananaMan.transform;
        }

        public void LoadPlayer(string saveUuid) {
            bananaManSavedData = LoadPlayerByUuid(saveUuid);
            
            LoadBananasInventory();
            LoadRawMaterialsInventory();
            LoadIngredientsInventory();
            LoadManufacturedItemsInventory();
            
            LoadSelectedBanana();
            LoadBananaManVitals();
            LoadPositionAndRotation();

            LoadBananaGunMode();
            LoadActiveBanana();
            LoadActiveBuildable();
            LoadBitkongQuantity();
            CheckTutorialFinished();
            
            ObjectsReference.Instance.uInventoriesManager.RefreshInventories();
        }
        
        private BananaManSavedData LoadPlayerByUuid(string saveUuid) {
            var savePath = Path.Combine(ObjectsReference.Instance.gameSave._savesPath, saveUuid);
            
            var savefilePath = Path.Combine(savePath, "player.json");

            var playerString = File.ReadAllText(savefilePath);

            return JsonConvert.DeserializeObject<BananaManSavedData>(playerString);
        }
        
        private void LoadBananasInventory() {
            bananasInventory = bananaMan.inventories.bananasInventory;

            foreach (var bananaSlot in bananasInventory.ToList()) {
                if (bananaSlot.Key == BananaType.EMPTY) continue;

                bananasInventory[bananaSlot.Key] = bananaManSavedData.bananaInventory[bananaSlot.Key.ToString()];
            }
        }

        private void LoadRawMaterialsInventory() {
            rawMaterialsInventory = bananaMan.inventories.rawMaterialsInventory;

            foreach (var rawMaterialSlot in rawMaterialsInventory.ToList()) {
                if (rawMaterialSlot.Key == RawMaterialType.EMPTY) continue;

                rawMaterialsInventory[rawMaterialSlot.Key] = bananaManSavedData.rawMaterialsInventory[rawMaterialSlot.Key.ToString()];
            }
        }

        private void LoadIngredientsInventory() {
            ingredientsInventory = bananaMan.inventories.ingredientsInventory;

            foreach (var ingredientsSlot in ingredientsInventory.ToList()) {
                if (ingredientsSlot.Key == IngredientsType.EMPTY) continue;
                
                ingredientsInventory[ingredientsSlot.Key] = bananaManSavedData.ingredientsInventory[ingredientsSlot.Key.ToString()];
            }
        }

        private void LoadManufacturedItemsInventory() {
            manufacturedItemsInventory = bananaMan.inventories.manufacturedItemsInventory;

            foreach (var manufacturedItemSlot in manufacturedItemsInventory.ToList()) {
                if (manufacturedItemSlot.Key == ManufacturedItemsType.EMPTY) continue;
                
                manufacturedItemsInventory[manufacturedItemSlot.Key] = bananaManSavedData.manufacturedInventory[manufacturedItemSlot.Key.ToString()];
            }
        }

        private void LoadBananaGunMode() {
            var bananagunMode = Enum.Parse<BananaGunMode>(bananaManSavedData.bananaGunMode);
            bananaMan.bananaGunMode = bananagunMode;
            ObjectsReference.Instance.bananaGunActionsSwitch.SwitchToBananaGunMode(bananaMan.bananaGunMode);
        }
        
        private void LoadSelectedBanana() {
            var bananaSlotType = Enum.Parse<BananaType>(bananaManSavedData.bananaSlot);
            var bananaScriptableObject = ObjectsReference.Instance.meshReferenceScriptableObject.bananasPropertiesScriptableObjects[bananaSlotType];
            bananaMan.activeBanana = bananaScriptableObject;
            ObjectsReference.Instance.uiFlippers.SetBananaQuantity(bananaMan.inventories.bananasInventory[bananaScriptableObject.bananaType]);
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
            bananaMan.activeBanana = ObjectsReference.Instance.meshReferenceScriptableObject.bananasPropertiesScriptableObjects[activeBananaType];

            ObjectsReference.Instance.uiFlippers.SetBananaQuantity(ObjectsReference.Instance.bananasInventory.GetQuantity(activeBananaType));
        }
        
        private void LoadActiveBuildable() {
            var activeBuildableType = bananaManSavedData.activeBuildable;
            
            bananaMan.activeBuildable = ObjectsReference.Instance.meshReferenceScriptableObject.buildablePropertiesScriptableObjects[activeBuildableType];
            ObjectsReference.Instance.uiFlippers.SetBuildablePlacementAvailability(ObjectsReference.Instance.rawMaterialsInventory.HasCraftingIngredients(activeBuildableType));
        }
        
        private void LoadBitkongQuantity() {
            bananaMan.inventories.bitKongQuantity = bananaManSavedData.bitKongQuantity;
        }
        
        private void CheckTutorialFinished() {
            bananaMan.tutorialFinished = bananaManSavedData.hasFinishedTutorial;

            if (bananaMan.tutorialFinished) {
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
            
            foreach (var inventorySlot in bananaMan.inventories.bananasInventory) {
                bananaManSavedData.bananaInventory[inventorySlot.Key.ToString()] = inventorySlot.Value;
            }
            foreach (var inventorySlot in bananaMan.inventories.rawMaterialsInventory) {
                bananaManSavedData.rawMaterialsInventory[inventorySlot.Key.ToString()] = inventorySlot.Value;
            }
            foreach (var inventorySlot in bananaMan.inventories.ingredientsInventory) {
                bananaManSavedData.ingredientsInventory[inventorySlot.Key.ToString()] = inventorySlot.Value;
            }
            foreach (var inventorySlot in bananaMan.inventories.manufacturedItemsInventory) {
                bananaManSavedData.manufacturedInventory[inventorySlot.Key.ToString()] = inventorySlot.Value;
            }

            bananaManSavedData.bananaGunMode = bananaMan.bananaGunMode.ToString();
            bananaManSavedData.bananaSlot = bananaMan.activeBanana.bananaType.ToString();
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

            bananaManSavedData.activeBanana = bananaMan.activeBanana.bananaType;
            bananaManSavedData.activeBuildable = bananaMan.activeBuildable.buildableType;

            bananaManSavedData.bitKongQuantity = bananaMan.inventories.bitKongQuantity;
        
            var jsonbananaManSaved = JsonConvert.SerializeObject(bananaManSavedData);
        
            var savefilePath = Path.Combine(savePath, "player.json");
            File.WriteAllText(savefilePath, jsonbananaManSaved);
        }
    }
}
