using System.Linq;
using Building;
using Player;
using UI.InGame.Inventory;
using UI.InGame.QuickSlots;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Save {
    public class GameSave : MonoSingleton<GameSave> {
        public void SaveGameData(string saveUuid, string date) {
            SaveInventory();
            SaveSlots();
            SaveBananaManVitals();
            SavePosition();
            SaveActiveItem();
            SaveMonkeysSatiety();

            var sceneName = SceneManager.GetActiveScene().name.ToUpper();
            if (GameManager.Instance.isInGame && GameData.Instance.mapDatasBySceneNames[sceneName].hasDebris) {
                SaveDebrisPositionAndRotationByUuid(saveUuid);
            }

            SaveData.Instance.Save(saveUuid, date);
        }

        private void SaveInventory() {
            foreach (var inventorySlot in Inventory.Instance.bananaManInventory) {
                GameData.Instance.bananaManSavedData.inventory[inventorySlot.Key.ToString()] = inventorySlot.Value;
            }
        }

        private void SaveSlots() {
            foreach (var slot in UISlotsManager.Instance.slotsMappingToInventory) {
                GameData.Instance.bananaManSavedData.slots["inventorySlot" + slot.Key] = slot.Value;
            }
        }

        private void SaveBananaManVitals() {
            GameData.Instance.bananaManSavedData.health = BananaMan.Instance.health; 
            GameData.Instance.bananaManSavedData.resistance = BananaMan.Instance.resistance; 
        }

        private void SavePosition() {
            GameData.Instance.lastPositionOnMap = BananaMan.Instance.transform.position;
            GameData.Instance.bananaManSavedData.xWorldPosition = GameData.Instance.lastPositionOnMap.x;
            GameData.Instance.bananaManSavedData.yWorldPosition = GameData.Instance.lastPositionOnMap.y;
            GameData.Instance.bananaManSavedData.zworldPosition = GameData.Instance.lastPositionOnMap.z;
        }

        private void SaveActiveItem() {
            UInventory.Instance.ActivateAllInventory();

            GameData.Instance.bananaManSavedData.active_item =
                UInventory.Instance.GetSlotIndex(BananaMan.Instance.activeItemThrowableType); 
        }

        private void SaveMonkeysSatiety() {
            foreach (var mapData in GameData.Instance.mapDatasBySceneNames) {
                // TODO : add other monkeys and other maps
                GameData.Instance.map01SavedData.monkey_sasiety = mapData.Value.monkeySasiety;
            }
        }

        /////////////////// DEBRIS ///////////////////////

        private void SaveDebrisPositionAndRotationByUuid(string saveUuid) {
            var debrisTransforms = GameObject.FindGameObjectWithTag("debrisContainer")
                .GetComponentsInChildren<Transform>().ToList();
            debrisTransforms.RemoveAt(0);

            GameData.Instance.debrisPrefabIndex = new int[debrisTransforms.Count];
            GameData.Instance.debrisPosition = new Vector3[debrisTransforms.Count];
            GameData.Instance.debrisRotation = new Quaternion[debrisTransforms.Count];

            for (var i = 0; i < debrisTransforms.Count; i++) {
                GameData.Instance.debrisPrefabIndex[i] = debrisTransforms[i].GetComponent<Debris>().prefabIndex;
                GameData.Instance.debrisPosition[i] = debrisTransforms[i].position;
                GameData.Instance.debrisRotation[i] = debrisTransforms[i].rotation;
            }

            SaveData.Instance.SaveMapDataByUuid(
                GameData.Instance.debrisPosition, 
                GameData.Instance.debrisRotation, 
                GameData.Instance.debrisPrefabIndex,
                saveUuid
            );
        }
    }
}
