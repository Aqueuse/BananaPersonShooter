using System;
using System.Globalization;
using UnityEngine;

namespace Save {
    public class GameSave : MonoBehaviour {
        [SerializeField] private GameObject autoSaveBanana;
        
        public void SaveGame(string saveUuid) {
            Debug.Log("save");
            
            var date = DateTime.ParseExact(DateTime.Now.ToString("U"), "U", CultureInfo.CurrentCulture).ToString(CultureInfo.CurrentCulture);
            
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
                ObjectsReference.Instance.gameData.bananaManSaved.bananaInventory[inventorySlot.Key.ToString()] = inventorySlot.Value;
            }
        }

        private static void SaveRawMaterialsInventory() {
            foreach (var inventorySlot in ObjectsReference.Instance.rawMaterialsInventory.rawMaterialsInventory) {
                ObjectsReference.Instance.gameData.bananaManSaved.rawMaterialsInventory[inventorySlot.Key.ToString()] = inventorySlot.Value;
            }
        }

        private static void SaveIngredientsInventory() {
            foreach (var inventorySlot in ObjectsReference.Instance.ingredientsInventory.ingredientsInventory) {
                ObjectsReference.Instance.gameData.bananaManSaved.ingredientsInventory[inventorySlot.Key.ToString()] = inventorySlot.Value;
            }
        }
        
        private static void SaveBananaSlot() {
            if (ObjectsReference.Instance.quickSlotsManager.bananaSlotItemScriptableObject != null)
               ObjectsReference.Instance.gameData.bananaManSaved.bananaSlot = ObjectsReference.Instance.quickSlotsManager.bananaSlotItemScriptableObject.bananaType.ToString();
        }

        private static void SaveBananaManVitals() {
            ObjectsReference.Instance.gameData.bananaManSaved.health = ObjectsReference.Instance.bananaMan.health; 
            ObjectsReference.Instance.gameData.bananaManSaved.resistance = ObjectsReference.Instance.bananaMan.resistance; 
        }

        private static void SavePositionAndRotation() {
            var bananaManTransform = ObjectsReference.Instance.bananaMan.transform;
            ObjectsReference.Instance.gameData.lastPositionOnMap = bananaManTransform.position;
            ObjectsReference.Instance.gameData.lastRotationOnMap = bananaManTransform.rotation.eulerAngles;
            
            ObjectsReference.Instance.gameData.bananaManSaved.xWorldPosition = ObjectsReference.Instance.gameData.lastPositionOnMap.x;
            ObjectsReference.Instance.gameData.bananaManSaved.yWorldPosition = ObjectsReference.Instance.gameData.lastPositionOnMap.y;
            ObjectsReference.Instance.gameData.bananaManSaved.zworldPosition = ObjectsReference.Instance.gameData.lastPositionOnMap.z;

            ObjectsReference.Instance.gameData.bananaManSaved.xWorldRotation = ObjectsReference.Instance.gameData.lastRotationOnMap.x;
            ObjectsReference.Instance.gameData.bananaManSaved.yWorldRotation = ObjectsReference.Instance.gameData.lastRotationOnMap.y;
            ObjectsReference.Instance.gameData.bananaManSaved.zWorldRotation = ObjectsReference.Instance.gameData.lastRotationOnMap.z;
        }

        private void SaveTutorialState() {
            ObjectsReference.Instance.gameData.bananaManSaved.hasFinishedTutorial =
                ObjectsReference.Instance.bananaMan.tutorialFinished;
        }

        private static void SaveActiveItem() {
            ObjectsReference.Instance.gameData.bananaManSaved.activeBanana =
                ObjectsReference.Instance.bananaMan.activeItem.bananaType;
        }
        
        public void StartAutoSave() {
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
