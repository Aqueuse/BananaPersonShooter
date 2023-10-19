using System;
using System.Globalization;
using Enums;
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
            SaveBlueprintsInventory();
            
            SaveSlots();
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

        private static void SaveBlueprintsInventory() {
            foreach (var inventorySlot in ObjectsReference.Instance.blueprintsInventory.blueprintsInventory) {
                ObjectsReference.Instance.gameData.bananaManSavedData.blueprintsInventory[inventorySlot.Key.ToString()] = inventorySlot.Value;
            }
        }

        private static void SaveSlots() {
            ObjectsReference.Instance.gameData.bananaManSavedData.slots = new() {
                "EMPTY,EMPTY",
                "EMPTY,EMPTY",
                "EMPTY,EMPTY",
                "EMPTY,EMPTY" 
            };

            for (var i = 0; i < ObjectsReference.Instance.uiQuickSlotsManager.uiQuickSlotsScripts.Count; i++) {
                var uiSlotScript = ObjectsReference.Instance.uiQuickSlotsManager.uiQuickSlotsScripts[i];
                
                if (uiSlotScript.slotItemScriptableObject == null) continue;
                
                var itemCategory = uiSlotScript.slotItemScriptableObject.itemCategory;

                switch (itemCategory) {
                    case ItemCategory.BUILDABLE:
                        ObjectsReference.Instance.gameData.bananaManSavedData.slots[i] = itemCategory+","+uiSlotScript.slotItemScriptableObject.buildableType;
                        break;
                    case ItemCategory.BANANA:
                        ObjectsReference.Instance.gameData.bananaManSavedData.slots[i] = itemCategory+","+uiSlotScript.slotItemScriptableObject.bananaType;
                        break;
                    case ItemCategory.RAW_MATERIAL:
                        ObjectsReference.Instance.gameData.bananaManSavedData.slots[i] = itemCategory+","+uiSlotScript.slotItemScriptableObject.rawMaterialType;
                        break;
                    case ItemCategory.INGREDIENT:
                        ObjectsReference.Instance.gameData.bananaManSavedData.slots[i] = itemCategory+","+uiSlotScript.slotItemScriptableObject.ingredientsType;
                        break;
                }
            }
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
            ObjectsReference.Instance.gameData.bananaManSavedData.activeBanana = ObjectsReference.Instance.bananaMan.activeBananaType;
            ObjectsReference.Instance.gameData.bananaManSavedData.activeItemCategory = ObjectsReference.Instance.bananaMan.activeItemCategory;
            ObjectsReference.Instance.gameData.bananaManSavedData.activeBuildableType = ObjectsReference.Instance.bananaMan.activeBuildableType;
        }
        
        public void StartAutoSave() {
            if (ObjectsReference.Instance.gameSettings.saveDelay > 0) InvokeRepeating(nameof(AutoSave), ObjectsReference.Instance.gameSettings.saveDelay, ObjectsReference.Instance.gameSettings.saveDelay);
        }
        
        public void ResetAutoSave() {
            CancelInvoke();
            if (ObjectsReference.Instance.gameSettings.saveDelay > 0) InvokeRepeating(nameof(AutoSave), ObjectsReference.Instance.gameSettings.saveDelay, ObjectsReference.Instance.gameSettings.saveDelay);
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
