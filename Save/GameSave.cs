using System;
using System.Globalization;
using UnityEngine;

namespace Save {
    public class GameSave : MonoBehaviour {
        [SerializeField] private GameObject autoSaveBanana;
        
        public void SaveGame(string saveUuid) {
            var date = DateTime.ParseExact(DateTime.Now.ToString("U"), "U", CultureInfo.CurrentCulture).ToString(CultureInfo.CurrentCulture);
            
            ObjectsReference.Instance.mapsManager.currentMap.RefreshAspirablesItemsDataMap();
            SaveInventory();

            SaveBlueprints();
            SaveSlots();
            SaveActiveItem();

            SaveMonkeysSatiety();

            SaveBananaManVitals();
            SavePositionAndRotation();
            
            ObjectsReference.Instance.saveData.Save(saveUuid, date);
        }

        private static void SaveInventory() {
            foreach (var inventorySlot in ObjectsReference.Instance.inventory.bananaManInventory) {
                ObjectsReference.Instance.gameData.bananaManSavedData.inventory[inventorySlot.Key.ToString()] = inventorySlot.Value;
            }
        }

        private static void SaveBlueprints() {
            var activeBlueprintsSlots = ObjectsReference.Instance.uiBlueprints.GetActivatedBlueprints();

            foreach (var blueprintsSlot in activeBlueprintsSlots) {
                if (!ObjectsReference.Instance.gameData.bananaManSavedData.blueprints.Contains(blueprintsSlot.buildableType
                        .ToString())) {
                    ObjectsReference.Instance.gameData.bananaManSavedData.blueprints.Add(blueprintsSlot.buildableType.ToString());
                }
            }
        }

        private static void SaveSlots() {
            ObjectsReference.Instance.gameData.bananaManSavedData.slots = new() {
                "EMPTY,EMPTY",
                "EMPTY,EMPTY",
                "EMPTY,EMPTY",
                "EMPTY,EMPTY" 
            };

            for (var i = 0; i < ObjectsReference.Instance.uiSlotsManager.uiSlotsScripts.Count; i++) {
                var uiSlotScript = ObjectsReference.Instance.uiSlotsManager.uiSlotsScripts[i];
                
                var itemCategory = uiSlotScript.itemCategory;
                var itemType = uiSlotScript.itemType;
                var buildableType = uiSlotScript.buildableType;

                if (itemCategory == ItemCategory.BUILDABLE) {
                    ObjectsReference.Instance.gameData.bananaManSavedData.slots[i] = itemCategory+","+buildableType;
                }
                else {
                    ObjectsReference.Instance.gameData.bananaManSavedData.slots[i] = itemCategory+","+itemType;
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

        private static void SaveActiveItem() {
            ObjectsReference.Instance.gameData.bananaManSavedData.activeItem = ObjectsReference.Instance.bananaMan.activeItemType;
            ObjectsReference.Instance.gameData.bananaManSavedData.activeItemCategory = ObjectsReference.Instance.bananaMan.activeItemCategory;
            ObjectsReference.Instance.gameData.bananaManSavedData.activeBuildableType = ObjectsReference.Instance.bananaMan.activeBuildableType;
        }

        private static void SaveMonkeysSatiety() {
            foreach (var map in ObjectsReference.Instance.mapsManager.mapBySceneName) {
                // add other monkeys and other maps
                ObjectsReference.Instance.gameData.mapSavedDatasByMapName[map.Key].monkeySasiety = map.Value.monkeySasiety;
            }
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
