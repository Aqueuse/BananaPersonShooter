using System;
using System.Globalization;
using UnityEngine;

namespace Save {
    public class GameSave : MonoBehaviour {
        private SaveData _saveData;

        private void Start() {
            _saveData = ObjectsReference.Instance.saveData;
        }

        public void SaveGameData(string saveUuid) {
            var date = DateTime.ParseExact(DateTime.Now.ToString("U"), "U", CultureInfo.CurrentCulture).ToString(CultureInfo.CurrentCulture);

            if (ObjectsReference.Instance.gameManager.gameContext == GameContext.IN_GAME) {
                ObjectsReference.Instance.mapsManager.currentMap.RefreshAspirablesDataMap();
            }

            SaveInventory();
            SaveBlueprints();
            SaveSlots();
            SaveBananaManVitals();
            SavePositionAndRotation();
            SaveActiveItem();
            SaveMonkeysSatiety();

            SaveAspirablesPositionRotationPrefabIndexByUuid(saveUuid);
            
            ObjectsReference.Instance.saveData.Save(saveUuid, date);
        }

        private void SaveInventory() {
            foreach (var inventorySlot in ObjectsReference.Instance.inventory.bananaManInventory) {
                ObjectsReference.Instance.gameData.bananaManSavedData.inventory[inventorySlot.Key.ToString()] = inventorySlot.Value;
            }
        }

        private void SaveBlueprints() {
            var activeBlueprintsSlots = ObjectsReference.Instance.uiBlueprints.GetActivatedBlueprints();

            foreach (var blueprintsSlot in activeBlueprintsSlots) {
                if (!ObjectsReference.Instance.gameData.bananaManSavedData.blueprints.Contains(blueprintsSlot.buildableType
                        .ToString())) {
                    ObjectsReference.Instance.gameData.bananaManSavedData.blueprints.Add(blueprintsSlot.buildableType.ToString());
                }
            }
        }

        private void SaveSlots() {
            ObjectsReference.Instance.gameData.bananaManSavedData.slots = new() {
                "EMPTY,EMPTY",
                "EMPTY,EMPTY",
                "EMPTY,EMPTY",
                "EMPTY,EMPTY" 
            };

            for (int i = 0; i < ObjectsReference.Instance.uiSlotsManager.uiSlotsScripts.Count; i++) {
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

        private void SaveBananaManVitals() {
            ObjectsReference.Instance.gameData.bananaManSavedData.health = ObjectsReference.Instance.bananaMan.health; 
            ObjectsReference.Instance.gameData.bananaManSavedData.resistance = ObjectsReference.Instance.bananaMan.resistance; 
        }

        private void SavePositionAndRotation() {
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

        private void SaveActiveItem() {
            ObjectsReference.Instance.gameData.bananaManSavedData.activeItem = ObjectsReference.Instance.bananaMan.activeItemType;
            ObjectsReference.Instance.gameData.bananaManSavedData.activeItemCategory = ObjectsReference.Instance.bananaMan.activeItemCategory;
            ObjectsReference.Instance.gameData.bananaManSavedData.activeBuildableType = ObjectsReference.Instance.bananaMan.activeBuildableType;
        }

        private void SaveMonkeysSatiety() {
            foreach (var map in ObjectsReference.Instance.mapsManager.mapBySceneName) {
                // add other monkeys and other maps
                ObjectsReference.Instance.gameData.mapSavedDatasByMapName[map.Key].monkeySasiety = map.Value.monkeySasiety;
            }
        }

        private void SaveAspirablesPositionRotationPrefabIndexByUuid(string saveUuid) {
            foreach (var map in ObjectsReference.Instance.mapsManager.mapBySceneName) {
                if (map.Value.aspirablesCategories.Count != 0) {
                    var mapToSave = map.Value;

                    _saveData.SaveDataCBOR(
                        mapName: mapToSave.mapName,
                        aspirablesPositions: mapToSave.aspirablesPositions,
                        aspirablesRotations:mapToSave.aspirablesRotations,
                        saveUuid: saveUuid,
                        debrisPrefabsIndex: mapToSave.aspirablesPrefabsIndex,
                        aspirablesCategories:mapToSave.aspirablesCategories,
                        buildableTypes: mapToSave.aspirablesBuildableTypes,
                        itemTypes:mapToSave.aspirablesItemTypes
                        );
                }
            }
        }
    }
}
