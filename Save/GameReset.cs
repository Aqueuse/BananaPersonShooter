using System;
using System.Globalization;
using System.IO;
using System.Linq;
using Building;
using Data;
using Enums;
using Player;
using Save.Templates;
using UI.InGame;
using UI.InGame.Inventory;
using UI.InGame.QuickSlots;
using UI.Save;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Save {
    public class GameReset : MonoSingleton<GameReset> {
        [SerializeField] private GameObject[] debrisPrefab;

        private Vector3[] debrisPosition;
        private Quaternion[] debrisRotation;
        private int[] debrisPrefabIndex;

        public Transform initialSpawnTransform;

        public string lastMap = "COMMANDROOM";
        public Vector3 lastPositionOnMap;

        private GameObject debrisContainer;

        public GenericDictionary<String, MapDataScriptableObject> mapDatasBySceneNames;

        private BananaManSavedData bananaManSavedData;
        private MAP01SavedData map01SavedData;

        public void ResetGameData(string saveUuid) {
            ResetInventory();
            ResetSlots();
            ResetBananaManVitals();
            ResetPositionAndLastMap();
            ResetActiveItem();
            ResetMonkeysSasiety();

            BananaMan.Instance.transform.position = initialSpawnTransform.position;

            var date = DateTime.ParseExact(DateTime.Now.ToString("U"), "U", CultureInfo.CurrentCulture).ToString(CultureInfo.CurrentCulture);
            GameSave.Instance.SaveGameData(saveUuid, date);

            GameManager.Instance.ReturnHome();

            // lock maps
            // reinit mini chimps quests
            // reinit spaceship state
            // reinit assets positions on maps
        }

        private void ResetInventory() {
            foreach (var bananaSlot in Inventory.Instance.bananaManInventory.ToList()) {
                Inventory.Instance.bananaManInventory[bananaSlot.Key] = 0;
            }
        }

        private void ResetSlots() {
            foreach (var slot in UISlotsManager.Instance.slotsMappingToInventory) {
                PlayerPrefs.SetInt("inventorySlot" + slot.Key, 0);
            }

            foreach (UISlot uiSlot in UISlotsManager.Instance.uiSlotsScripts) {
                uiSlot.EmptySlot();
            }
        }

        private void ResetBananaManVitals() {
            BananaMan.Instance.health = 100;
            BananaMan.Instance.resistance = 100;

            UIVitals.Instance.Set_Health(BananaMan.Instance.health);
            UIVitals.Instance.Set_Resistance(BananaMan.Instance.resistance);
        }

        private void ResetPositionAndLastMap() {
            var initialSpawnPosition = initialSpawnTransform.position;

            lastPositionOnMap = initialSpawnPosition;
            lastMap = "COMMANDROOM";
        }

        private void ResetActiveItem() {
            BananaMan.Instance.activeItemThrowableType = ItemThrowableType.EMPTY;
            BananaMan.Instance.activeItemThrowableCategory = ItemThrowableCategory.EMPTY;

            PlayerPrefs.SetInt("activeItem", UInventory.Instance.GetSlotIndex(ItemThrowableType.EMPTY));
        }

        private void ResetMonkeysSasiety() {
            foreach (var mapData in mapDatasBySceneNames) {
                mapData.Value.monkeySasiety = 50;
                PlayerPrefs.SetFloat(mapData.Value.MonkeyType.ToString().ToUpper(), mapData.Value.monkeySasiety);
            }
        }

        /////////////////// DEBRIS ///////////////////////
    }
}
        
