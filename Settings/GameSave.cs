using System.Linq;
using Data;
using Enums;
using Player;
using UI.InGame;
using UI.InGame.Inventory;
using UI.Menus;
using UnityEngine;

namespace Settings {
    public class GameSave : MonoSingleton<GameSave> {
        public Transform initialSpawnTransform;

        public string lastMap = "Map01";
        public Vector3 lastPositionOnMap;

        public void LoadPlayerGameState() {
            // position
            var initialSpawnPosition = initialSpawnTransform.position;
        
            lastPositionOnMap = new Vector3(
                PlayerPrefs.GetFloat("PlayerXPosition", initialSpawnPosition.x),
                PlayerPrefs.GetFloat("PlayerYPosition", initialSpawnPosition.y),
                PlayerPrefs.GetFloat("PlayerZPosition", initialSpawnPosition.z)
            );

            lastMap = PlayerPrefs.GetString("Last Map", "Map01");
            
            // Has Mover ?
            BananaMan.Instance.hasMover = PlayerPrefs.GetString("HasMover").Equals("True");
            
            // inventory
            foreach (var bananaSlot in Inventory.Instance.bananaManInventory.ToList()) {
                Inventory.Instance.bananaManInventory[bananaSlot.Key] = PlayerPrefs.GetInt(bananaSlot.Key.ToString(), 0);
            }

            // slots
            UInventory.Instance.ActivateAllInventory();  // activate temporally all the inventory to find the index of slots

            foreach (var slot in UISlotsManager.Instance.slotsMappingToInventory.ToList()) {
                var itemType = UInventory.Instance.GetItemThrowableTypeByIndex(PlayerPrefs.GetInt("inventorySlot"+slot.Key));
                var itemCategory = UInventory.Instance.GetItemThrowableCategoryByIndex(PlayerPrefs.GetInt("inventorySlot"+slot.Key));
            
                UISlotsManager.Instance.slotsMappingToInventory[slot.Key] = PlayerPrefs.GetInt("inventorySlot"+slot.Key);
                UISlotsManager.Instance.uiSlotsScripts[slot.Key].SetSlot(itemType, itemCategory);
                UISlotsManager.Instance.uiSlotsScripts[slot.Key].SetSprite(UInventory.Instance.GetItemSprite(itemType));
            }
        
            // active item type, category, banana
            var activeItemType = UInventory.Instance.GetItemThrowableTypeByIndex(PlayerPrefs.GetInt("activeItem"));
            var activeItemCategory = UInventory.Instance.GetItemThrowableCategoryByIndex(PlayerPrefs.GetInt("activeItem"));

            if (activeItemCategory == ItemThrowableCategory.BANANA) {
                BananaMan.Instance.activeItem = ScriptableObjectManager.Instance.GetBananaScriptableObject(activeItemType);
            }

            BananaMan.Instance.activeItemThrowableType = activeItemType;
            BananaMan.Instance.activeItemThrowableCategory = activeItemCategory;

            // health and resistance
            BananaMan.Instance.health = PlayerPrefs.GetFloat("health", 100);
            BananaMan.Instance.resistance = PlayerPrefs.GetFloat("resistance", 100);

            // refects values in UI
            UIVitals.Instance.Set_Health(BananaMan.Instance.health);
            UIVitals.Instance.Set_Resistance(BananaMan.Instance.resistance);
        }

        public void SavePlayerGameState() {
            // position and map
            PlayerPrefs.SetString("Last Map", lastMap);

            Vector3 playerPosition = BananaMan.Instance.gameObject.transform.position; 

            PlayerPrefs.SetFloat("PlayerXPosition", playerPosition.x);
            PlayerPrefs.SetFloat("PlayerYPosition", playerPosition.y);
            PlayerPrefs.SetFloat("PlayerZPosition", playerPosition.z);

            // bananas in inventory
            foreach (var bananaSlot in Inventory.Instance.bananaManInventory) {
                PlayerPrefs.SetInt(bananaSlot.Key.ToString(), bananaSlot.Value);
            }
        
            //has mover ?
            PlayerPrefs.SetString("HasMover", BananaMan.Instance.hasMover.ToString());
            
            // slots state
            foreach (var slot in UISlotsManager.Instance.slotsMappingToInventory) {
                PlayerPrefs.SetInt("inventorySlot"+slot.Key, slot.Value);
            }
        
            // active item
            UInventory.Instance.ActivateAllInventory();
            PlayerPrefs.SetInt("activeItem", UInventory.Instance.GetSlotIndex(BananaMan.Instance.activeItemThrowableType));

            // health and resistance
            PlayerPrefs.SetFloat("health", BananaMan.Instance.health);
            PlayerPrefs.SetFloat("resistance", BananaMan.Instance.resistance);
        
        
            // update the text in option with the actual date and hour
            UIOptionsMenu.Instance.SetActualDateAndHour(System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        }

        public void ResetPlayerState() {
            UIOptionsMenu.Instance.HideConfirmationMessage();
        
            // position
            var initialSpawnPosition = initialSpawnTransform.position;

            lastPositionOnMap = initialSpawnPosition;
            lastMap =  "Map01";
        
            // inventory
            foreach (var bananaSlot in Inventory.Instance.bananaManInventory.ToList()) {
                Inventory.Instance.bananaManInventory[bananaSlot.Key] = 0;
            }
        
            BananaMan.Instance.hasMover = false;
            PlayerPrefs.SetString("HasMover", "False");
        
            // active itemType and Category
            BananaMan.Instance.activeItemThrowableType = ItemThrowableType.ROCKET;
            BananaMan.Instance.activeItemThrowableCategory = ItemThrowableCategory.ROCKET;
        
            // active item
            PlayerPrefs.SetInt("activeItem", UInventory.Instance.GetSlotIndex(ItemThrowableType.ROCKET));

            // slots
            foreach (var slot in UISlotsManager.Instance.slotsMappingToInventory) {
                PlayerPrefs.SetInt("inventorySlot"+slot.Key, 0);
            }
            foreach (var instanceUISlotsScript in UISlotsManager.Instance.uiSlotsScripts) {
                instanceUISlotsScript.SetSlot(ItemThrowableType.ROCKET, ItemThrowableCategory.ROCKET);
            }

            // health and resistance
            BananaMan.Instance.health = 100;
            BananaMan.Instance.resistance = 100;

            UIVitals.Instance.Set_Health(BananaMan.Instance.health);
            UIVitals.Instance.Set_Resistance(BananaMan.Instance.resistance);
        
            UIOptionsMenu.Instance.EmptyDateAndHour();

            BananaMan.Instance.transform.position = initialSpawnPosition;
        
            SavePlayerGameState();
        
            GameManager.Instance.ReturnHome();

            // lock maps
            // reinit mini chimps quests
            // reinit spaceship state
            // reinit central workstation state
            // reinit assets positions on maps
        }

    }
}
