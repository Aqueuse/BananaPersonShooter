using System.Collections.Generic;
using Data;
using Enums;
using UI.InGame;
using UI.InGame.PlateformBuilder;
using UnityEngine;
using UnityEngine.Localization.Settings;

namespace Building {
    public class Convertor : MonoSingleton<Convertor> {
        [SerializeField] private GameObject leftslideContent;

        [SerializeField] private GenericDictionary<string, GameObject> plateforms;
        
        private Inventory _inventory;
        private Dictionary<ItemThrowableType, int> leftsideContentDictionnary;

        // 2 debris + 2 banana = 1 plateform

        public void ShowPossiblesConversions() {
            CheckLeftSideSelectedItems();
            UnSelectAll();

            if (leftsideContentDictionnary.ContainsKey(ItemThrowableType.INGOT)) {
                if (leftsideContentDictionnary[ItemThrowableType.INGOT] >= 2) {
                    plateforms["plateform1"].SetActive(true);
                }
                if (leftsideContentDictionnary[ItemThrowableType.INGOT] >= 10) {
                    plateforms["plateform5"].SetActive(true);
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
            Inventory.Instance.AddQuantity(uInventorySlot.itemThrowableType, ItemThrowableCategory.PLATEFORM, uInventorySlot.quantity);
            
            var message = "+ "+uInventorySlot.quantity+" " +LocalizationSettings.Instance.GetStringDatabase().GetLocalizedString("platform");
            UIQueuedMessages.Instance.AddMessage(message);
            
            foreach (var ingredient in ScriptableObjectManager.Instance.GetPlateformCost()) {
                Inventory.Instance.RemoveQuantity( ingredient.Key, ingredient.Value*uInventorySlot.quantity);
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
