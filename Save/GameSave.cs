using System;
using System.Linq;
using Building;
using Data;
using Enums;
using Player;
using UI.InGame;
using UI.InGame.Inventory;
using UI.Menus;
using UnityEngine;

namespace Save {
    public class GameSave : MonoSingleton<GameSave> {
        [SerializeField] private GameObject[] debrisPrefab;

        private Vector3[] debrisPosition;
        private Quaternion[] debrisRotation;
        private int[] debrisPrefabIndex;

        public Transform initialSpawnTransform;

        public string lastMap = "map01";
        public Vector3 lastPositionOnMap;
        
        private GameObject debrisContainer;

        public GenericDictionary<String, MapDataScriptableObject> mapDatasBySceneNames;


        public void SaveGameData() {
            SaveInventory();
            SaveSlots();
            SaveBananaManVitals();
            SavePositionAndLastMap();
            SaveActiveItem();
            SaveMonkeysSatiety();
            SaveDebrisPositionAndRotation();
            
            // update the text in option with the actual date and hour
            UIOptionsMenu.Instance.SetActualDateAndHour(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        }

        public void LoadGameData() {
            LoadInventory();
            LoadSlots();
            LoadBananaManVitals();
            LoadPositionAndLastMap();
            LoadActiveItem();
            LoadMonkeysSatiety();
            LoadMapData.Instance.Load();
        }

        public void ResetGameData() {
            ResetInventory();
            ResetSlots();
            ResetBananaManVitals();
            ResetPositionAndLastMap();
            ResetActiveItem();
            ResetMonkeysSasiety();
            
            UIOptionsMenu.Instance.EmptyDateAndHour();

            BananaMan.Instance.transform.position = initialSpawnTransform.position;
        
            SaveGameData();
            UIOptionsMenu.Instance.HideConfirmationMessage();
            
            GameManager.Instance.ReturnHome();
            
            // lock maps
            // reinit mini chimps quests
            // reinit spaceship state
            // reinit assets positions on maps
        }

        private void SaveInventory() {
            foreach (var inventorySlot in Inventory.Instance.bananaManInventory) {
                PlayerPrefs.SetInt(inventorySlot.Key.ToString(), inventorySlot.Value);
            }
        }

        private void LoadInventory() {
            foreach (var bananaSlot in Inventory.Instance.bananaManInventory.ToList()) {
                Inventory.Instance.bananaManInventory[bananaSlot.Key] = PlayerPrefs.GetInt(bananaSlot.Key.ToString(), 0);
            }
        }

        private void ResetInventory() {
            foreach (var bananaSlot in Inventory.Instance.bananaManInventory.ToList()) {
                Inventory.Instance.bananaManInventory[bananaSlot.Key] = 0;
            }
        }


        private void SaveSlots() {
            foreach (var slot in UISlotsManager.Instance.slotsMappingToInventory) {
                PlayerPrefs.SetInt("inventorySlot"+slot.Key, slot.Value);
            }
        }

        private void LoadSlots() {
            UInventory.Instance.ActivateAllInventory();  // activate temporally all the inventory to find the index of slots

            foreach (var slot in UISlotsManager.Instance.slotsMappingToInventory.ToList()) {
                var itemType = UInventory.Instance.GetItemThrowableTypeByIndex(PlayerPrefs.GetInt("inventorySlot"+slot.Key));
                var itemCategory = UInventory.Instance.GetItemThrowableCategoryByIndex(PlayerPrefs.GetInt("inventorySlot"+slot.Key));

                if (Inventory.Instance.bananaManInventory[itemType] > 0) {
                    UISlotsManager.Instance.slotsMappingToInventory[slot.Key] = PlayerPrefs.GetInt("inventorySlot"+slot.Key);
                    UISlotsManager.Instance.uiSlotsScripts[slot.Key].SetSlot(itemType, itemCategory);
                    UISlotsManager.Instance.uiSlotsScripts[slot.Key].SetSprite(UInventory.Instance.GetItemSprite(itemType));
                }
            }

            UInventory.Instance.ActivateAllInventory();  // activate temporally all the inventory to find the index of slots
        }

        private void ResetSlots() {
            foreach (var slot in UISlotsManager.Instance.slotsMappingToInventory) {
                PlayerPrefs.SetInt("inventorySlot"+slot.Key, 0);
            }

            foreach (UISlot uiSlot in UISlotsManager.Instance.uiSlotsScripts) {
                uiSlot.EmptySlot();
            }
        }


        private void SaveBananaManVitals() {
            PlayerPrefs.SetFloat("health", BananaMan.Instance.health);
            PlayerPrefs.SetFloat("resistance", BananaMan.Instance.resistance);
        }

        private void LoadBananaManVitals() {
            BananaMan.Instance.health = PlayerPrefs.GetFloat("health", 100);
            BananaMan.Instance.resistance = PlayerPrefs.GetFloat("resistance", 100);

            UIVitals.Instance.Set_Health(BananaMan.Instance.health);
            UIVitals.Instance.Set_Resistance(BananaMan.Instance.resistance);
        }

        private void ResetBananaManVitals() {
            BananaMan.Instance.health = 100;
            BananaMan.Instance.resistance = 100;

            UIVitals.Instance.Set_Health(BananaMan.Instance.health);
            UIVitals.Instance.Set_Resistance(BananaMan.Instance.resistance);
        }


        private void SavePositionAndLastMap() {
            PlayerPrefs.SetString("Last Map", lastMap);

            Vector3 playerPosition = BananaMan.Instance.gameObject.transform.position; 

            PlayerPrefs.SetFloat("PlayerXPosition", playerPosition.x);
            PlayerPrefs.SetFloat("PlayerYPosition", playerPosition.y);
            PlayerPrefs.SetFloat("PlayerZPosition", playerPosition.z);
        }

        private void LoadPositionAndLastMap() {
            var initialSpawnPosition = initialSpawnTransform.position;
        
            lastPositionOnMap = new Vector3(
                PlayerPrefs.GetFloat("PlayerXPosition", initialSpawnPosition.x),
                PlayerPrefs.GetFloat("PlayerYPosition", initialSpawnPosition.y),
                PlayerPrefs.GetFloat("PlayerZPosition", initialSpawnPosition.z)
            );

            lastMap = PlayerPrefs.GetString("Last Map", "Map01");
        }

        private void ResetPositionAndLastMap() {
            var initialSpawnPosition = initialSpawnTransform.position;

            lastPositionOnMap = initialSpawnPosition;
            lastMap =  "map01";
        }


        private void SaveActiveItem() {
            UInventory.Instance.ActivateAllInventory();
            PlayerPrefs.SetInt("activeItem", UInventory.Instance.GetSlotIndex(BananaMan.Instance.activeItemThrowableType));
        }

        private void LoadActiveItem() {
            var activeItemType = UInventory.Instance.GetItemThrowableTypeByIndex(PlayerPrefs.GetInt("activeItem"));
            var activeItemCategory = UInventory.Instance.GetItemThrowableCategoryByIndex(PlayerPrefs.GetInt("activeItem"));

            if (activeItemCategory == ItemThrowableCategory.BANANA) {
                BananaMan.Instance.activeItem = ScriptableObjectManager.Instance.GetBananaScriptableObject(activeItemType);
            }

            BananaMan.Instance.activeItemThrowableType = activeItemType;
            BananaMan.Instance.activeItemThrowableCategory = activeItemCategory;
        }

        private void ResetActiveItem() {
            BananaMan.Instance.activeItemThrowableType = ItemThrowableType.EMPTY;
            BananaMan.Instance.activeItemThrowableCategory = ItemThrowableCategory.EMPTY;
        
            PlayerPrefs.SetInt("activeItem", UInventory.Instance.GetSlotIndex(ItemThrowableType.EMPTY));
        }


        private void SaveMonkeysSatiety() {
            foreach (var mapData in mapDatasBySceneNames) {
                PlayerPrefs.SetFloat(mapData.Value.MonkeyType.ToString().ToUpper(), mapData.Value.monkeySasiety);
            }
        }

        private void LoadMonkeysSatiety() {
            foreach (var mapData in mapDatasBySceneNames) {
                mapData.Value.monkeySasiety = PlayerPrefs.GetFloat(mapData.Value.MonkeyType.ToString().ToUpper());
            }
        }

        private void ResetMonkeysSasiety() {
            foreach (var mapData in mapDatasBySceneNames) {
                mapData.Value.monkeySasiety = 50;
                PlayerPrefs.SetFloat(mapData.Value.MonkeyType.ToString().ToUpper(), mapData.Value.monkeySasiety);
            }
        }
        
        /////////////////// DEBRIS ///////////////////////
        
        private void SaveDebrisPositionAndRotation() {
            var debrisTransforms = GameObject.FindGameObjectWithTag("debrisContainer").GetComponentsInChildren<Transform>().ToList();
            debrisTransforms.RemoveAt(0);
            
            debrisPrefabIndex = new int[debrisTransforms.Count];
            debrisPosition = new Vector3[debrisTransforms.Count];
            debrisRotation = new Quaternion[debrisTransforms.Count];
            
            for (var i=0; i<debrisTransforms.Count; i++) {
                debrisPrefabIndex[i] = debrisTransforms[i].GetComponent<Debris>().prefabIndex;
                debrisPosition[i] = debrisTransforms[i].position;
                debrisRotation[i] = debrisTransforms[i].rotation;
            }

            SaveMapData.Instance.Save(debrisPosition, debrisRotation, debrisPrefabIndex);
        }

        public void RespawnDebrisOnMap(string sceneName) {
            debrisContainer = GameObject.FindGameObjectWithTag("debrisContainer");

            Destroy(debrisContainer);
            
            debrisContainer = new GameObject("debris");
            debrisContainer.transform.parent = null;
            debrisContainer.tag = "debrisContainer";

            var mapData = mapDatasBySceneNames[sceneName];
            
            for (var i=0; i<mapDatasBySceneNames[sceneName].debrisIndex.Length; i++) {
                var debris = Instantiate(debrisPrefab[mapData.debrisIndex[i]], debrisContainer.transform, true);
                debris.transform.position = mapData.debrisPosition[i];
                debris.transform.rotation = mapData.debrisRotation[i];
            }
        }
    }
}
