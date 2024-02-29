using System;
using System.Globalization;
using InGame.Player;
using Save.Templates;
using UnityEngine;

namespace Save {
    public class GameSave : MonoBehaviour {
        [SerializeField] private GameObject autoSaveBanana;
        private BananaManSavedData bananaManSavedData;
        private GameData gameData;
        private BananaMan bananaMan;

        private void Start() {
            bananaManSavedData = ObjectsReference.Instance.gameData.bananaManSaved;
            gameData = ObjectsReference.Instance.gameData;
            bananaMan = ObjectsReference.Instance.bananaMan;
        }

        public void SaveGame(string saveUuid) {
            var date = DateTime.ParseExact(DateTime.Now.ToString("U"), "U", CultureInfo.CurrentCulture).ToString(CultureInfo.CurrentCulture);
            
            SaveBananasInventory();
            SaveRawMaterialsInventory();
            SaveIngredientsInventory();
            SaveManufacturedItemsInventory();
            
            SaveBananaSlot();
            SaveActiveItem();
            SaveBitkongQuantity();
            
            SaveBananaManVitals();
            SavePositionAndRotation();

            SaveTutorialState();
            
            ObjectsReference.Instance.saveData.Save(saveUuid, date);
        }

        private void SaveBananasInventory() {
            foreach (var inventorySlot in bananaMan.inventories.bananasInventory) {
                bananaManSavedData.bananaInventory[inventorySlot.Key.ToString()] = inventorySlot.Value;
            }
        }

        private void SaveRawMaterialsInventory() {
            foreach (var inventorySlot in bananaMan.inventories.rawMaterialsInventory) {
                bananaManSavedData.rawMaterialsInventory[inventorySlot.Key.ToString()] = inventorySlot.Value;
            }
        }

        private void SaveIngredientsInventory() {
            foreach (var inventorySlot in bananaMan.inventories.ingredientsInventory) {
                bananaManSavedData.ingredientsInventory[inventorySlot.Key.ToString()] = inventorySlot.Value;
            }
        }
        
        private void SaveManufacturedItemsInventory() {
            foreach (var inventorySlot in bananaMan.inventories.manufacturedItemsInventory) {
                bananaManSavedData.manufacturedInventory[inventorySlot.Key.ToString()] = inventorySlot.Value;
            }
        }
        
        private void SaveBananaSlot() {
            bananaManSavedData.bananaSlot = bananaMan.activeItem.bananaType.ToString();
        }

        private void SaveBananaManVitals() {
            bananaManSavedData.health = bananaMan.health; 
            bananaManSavedData.resistance = bananaMan.resistance; 
        }

        private void SavePositionAndRotation() {
            var bananaManTransform = bananaMan.transform;
            gameData.lastPositionOnMap = bananaManTransform.position;
            gameData.lastRotationOnMap = bananaManTransform.rotation.eulerAngles;
            
            bananaManSavedData.xWorldPosition = gameData.lastPositionOnMap.x;
            bananaManSavedData.yWorldPosition = gameData.lastPositionOnMap.y;
            bananaManSavedData.zworldPosition = gameData.lastPositionOnMap.z;

            bananaManSavedData.xWorldRotation = gameData.lastRotationOnMap.x;
            bananaManSavedData.yWorldRotation = gameData.lastRotationOnMap.y;
            bananaManSavedData.zWorldRotation = gameData.lastRotationOnMap.z;
        }

        private void SaveTutorialState() {
            bananaManSavedData.hasFinishedTutorial = bananaMan.tutorialFinished;
        }

        private void SaveActiveItem() {
            bananaManSavedData.activeBanana = bananaMan.activeItem.bananaType;
        }

        private void SaveBitkongQuantity() {
            bananaManSavedData.bitKongQuantity = bananaMan.inventories.bitKongQuantity;
        }
        
        public void StartAutoSave() {
            if (ObjectsReference.Instance.gameSettings.saveDelayMinute > 0) InvokeRepeating(nameof(AutoSave), ObjectsReference.Instance.gameSettings.saveDelayMinute, ObjectsReference.Instance.gameSettings.saveDelayMinute);
        }
        
        public void CancelAutoSave() {
            CancelInvoke();
        }
        
        public void AutoSave() {
            autoSaveBanana.SetActive(true);
            SaveGame(gameData.currentSaveUuid);
            Invoke(nameof(HideAutoSaveBanana), 5);
        }

        private void HideAutoSaveBanana() {
            autoSaveBanana.SetActive(false);
        }
    }
}
