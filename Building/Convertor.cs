using System.Collections.Generic;
using Data;
using Enums;
using UI.InGame;
using UI.InGame.PlateformBuilder;
using UnityEngine;

namespace Building {
    public class Convertor : MonoSingleton<Convertor> {
        [SerializeField] private GameObject leftslideContent;

        [SerializeField] private GenericDictionary<string, GameObject> plateforms;
        
        private Inventory _inventory;
        private Dictionary<ItemThrowableType, int> leftsideContentDictionnary;

        // 2 rockets + 2 banana = 1 plateform

        public void ShowPossiblesConversions() {
            CheckLeftSideSelectedItems();
            UnSelectAll();

            if (leftsideContentDictionnary.ContainsKey(ItemThrowableType.ROCKET) &&
                leftsideContentDictionnary.ContainsKey(ItemThrowableType.CAVENDISH)) {
                if (leftsideContentDictionnary[ItemThrowableType.ROCKET] >= 2 && leftsideContentDictionnary[ItemThrowableType.CAVENDISH] >= 2) {
                    plateforms["plateformCavendish1"].SetActive(true);
                }
                if (leftsideContentDictionnary[ItemThrowableType.ROCKET] >= 10 && leftsideContentDictionnary[ItemThrowableType.CAVENDISH] >= 10) {
                    plateforms["plateformCavendish5"].SetActive(true);
                }
            }
        }

        // search wich items are selected in the left panel
        private void CheckLeftSideSelectedItems() {
            leftsideContentDictionnary = new Dictionary<ItemThrowableType, int>();
            
            foreach (RectTransform child in leftslideContent.GetComponent<RectTransform>()) {
                if (child.gameObject.activeInHierarchy) {
                    var uiLeftSlot = child.GetComponent<UIBuildInventorySlotLeft>();
                    if (uiLeftSlot.toggle.isOn) {
                        leftsideContentDictionnary.Add(uiLeftSlot.itemThrowableType, Inventory.Instance.bananaManInventory[uiLeftSlot.itemThrowableType]);
                    }
                }
            }
        }

        public void GiveToPlayer(UIBuildInventorySlotRight uInventorySlot) {
            Inventory.Instance.bananaManInventory[uInventorySlot.itemThrowableType] += uInventorySlot.quantity;
            
            var message = "+ "+uInventorySlot.quantity+" " +ScriptableObjectManager.Instance.GetPlateformName(uInventorySlot.itemThrowableType);
            UIQueuedMessages.Instance.AddMessage(message);
            
            foreach (var ingredient in ScriptableObjectManager.Instance.GetPlateformCost(uInventorySlot.itemThrowableType)) {
                Inventory.Instance.bananaManInventory[ingredient.Key] -= ingredient.Value*uInventorySlot.quantity;
                if (Inventory.Instance.bananaManInventory[ingredient.Key] < 0) {
                    Inventory.Instance.bananaManInventory[ingredient.Key] = 0;
                }
            }
            
            UIBuildInventoryLeft.Instance.RefreshInventoySlots();
            ShowPossiblesConversions();
        }

        private void UnSelectAll() {
            foreach (var plateform in plateforms) {
                plateform.Value.SetActive(false);
            }
        }
    }
}
