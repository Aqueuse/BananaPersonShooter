using System;
using System.Globalization;
using UnityEngine;

namespace Save {
    public class GameSave : MonoBehaviour {
        [SerializeField] private GameObject autoSaveBanana;
        
        public void SaveGame(string saveUuid) {
            var date = DateTime.ParseExact(DateTime.Now.ToString("U"), "U", CultureInfo.CurrentCulture).ToString(CultureInfo.CurrentCulture);
            
            ObjectsReference.Instance.mapsManager.currentMap.RefreshItemsDataMap();
            
            SaveBananasInventory();
            SaveRawMaterialsInventory();
            SaveIngredientsInventory();
            
            SaveBananaSlot();
            SaveActiveItem();
            
            SaveBananaManVitals();
            SavePositionAndRotation();

            SaveTutorialState();
            
            ObjectsReference.Instance.saveData.Save(saveUuid, date);
        }

        private static void SaveBananasInventory() {
            foreach (var inventorySlot in ObjectsReference.Instance.bananasInventory.bananasInventory) {
                ObjectsReference.Instance.gameData.bananaManSavedData.bananaInventory[inventorySlot.Key.ToString()] = inventorySlot.Value;
            }
        }

        private static void SaveRawMaterialsInventory() {
            foreach (var inventorySlot in ObjectsReference.Instance.rawMaterialsInventory.rawMaterialsInventory) {
                ObjectsReference.Instance.gameData.bananaManSavedData.rawMaterialsInventory[inventorySlot.Key.ToString()] = inventorySlot.Value;
            }
        }

        private static void SaveIngredientsInventory() {
            foreach (var inventorySlot in ObjectsReference.Instance.ingredientsInventory.ingredientsInventory) {
                ObjectsReference.Instance.gameData.bananaManSavedData.ingredientsInventory[inventorySlot.Key.ToString()] = inventorySlot.Value;
            }
        }
        
        private static void SaveBananaSlot() {
            if (ObjectsReference.Instance.quickSlotsManager.bananaSlotItemScriptableObject != null)
               ObjectsReference.Instance.gameData.bananaManSavedData.bananaSlot = ObjectsReference.Instance.quickSlotsManager.bananaSlotItemScriptableObject.bananaType.ToString();
        }

        private static void SaveBananaManVitals() {
            ObjectsReference.Instance.gameData.bananaManSavedData.health = ObjectsReference.Instance.bananaMan.health; 
            ObjectsReference.Instance.gameData.bananaManSavedData.resistance = ObjectsReference.Instance.bananaMan.resistance; 
        }

        private static void SavePositionAndRotation() {
            var bananaManTransform = ObjectsReference.Instance.bananaMan.transform;
            ObjectsReference.Instance.gameData.lastPositionOnMap = bananaManTransform.position;
            ObjectsReference.Instance.gameData.lastRotationOnMap = bananaManTransform.rotation.eulerAngles;
            
            ObjectsReference.Instance.gameData.bananaManSavedData.xWorldPosition = ObjectsReference.Instance.gameData.lastPositionOnMap.x;
            ObjectsReference.Instance.gameData.bananaManSavedData.yWorldPosition = ObjectsReference.Instance.gameData.lastPositionOnMap.y;
            ObjectsReference.Instance.gameData.bananaManSavedData.zworldPosition = ObjectsReference.Instance.gameData.lastPositionOnMap.z;

            ObjectsReference.Instance.gameData.bananaManSavedData.xWorldRotation = ObjectsReference.Instance.gameData.lastRotationOnMap.x;
            ObjectsReference.Instance.gameData.bananaManSavedData.yWorldRotation = ObjectsReference.Instance.gameData.lastRotationOnMap.y;
            ObjectsReference.Instance.gameData.bananaManSavedData.zWorldRotation = ObjectsReference.Instance.gameData.lastRotationOnMap.z;
        }

        private void SaveTutorialState() {
            ObjectsReference.Instance.gameData.bananaManSavedData.hasFinishedTutorial =
                ObjectsReference.Instance.bananaMan.tutorialFinished;
        }

        private static void SaveActiveItem() {
            ObjectsReference.Instance.gameData.bananaManSavedData.activeBanana =
                ObjectsReference.Instance.bananaMan.activeItem.bananaType;
        }
        
        public void StartAutoSave() {
            if (ObjectsReference.Instance.gameSettings.saveDelayMinute > 0) InvokeRepeating(nameof(AutoSave), ObjectsReference.Instance.gameSettings.saveDelayMinute, ObjectsReference.Instance.gameSettings.saveDelayMinute);
        }
        
        public void ResetAutoSave() {
            CancelInvoke();
            if (ObjectsReference.Instance.gameSettings.saveDelayMinute > 0) InvokeRepeating(nameof(AutoSave), ObjectsReference.Instance.gameSettings.saveDelayMinute, ObjectsReference.Instance.gameSettings.saveDelayMinute);
        }

        public void CancelAutoSave() {
            CancelInvoke();
        }
        
        public void AutoSave() {
            autoSaveBanana.SetActive(true);
            SaveGame(ObjectsReference.Instance.gameData.currentSaveUuid);
            Invoke(nameof(HideAutoSaveBanana), 5);
        }

        private void HideAutoSaveBanana() {
            autoSaveBanana.SetActive(false);
        }
    }
}
