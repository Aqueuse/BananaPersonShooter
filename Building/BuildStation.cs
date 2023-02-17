using System.Collections.Generic;
using Data;
using Enums;
using Game;
using Input;
using UI;
using UI.InGame;
using UI.InGame.BuildStation;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Localization.Settings;

namespace Building {
    public class BuildStation : MonoSingleton<BuildStation> {
        [SerializeField] private CanvasGroup miniChimpPlateformBuilderCanvasGroup;

        [SerializeField] private GenericDictionary<string, GameObject> plateforms;
        [SerializeField] private GameObject leftslideContent;
        
        private Inventory _inventory;
        private Dictionary<ItemThrowableType, int> leftsideContentDictionnary;

        // 2 ingots = 1 plateform

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
        
        public void ShowBuildStationInterface() {
            UIManager.Instance.Set_active(miniChimpPlateformBuilderCanvasGroup, true);
            UIBuildStationInventoryLeft.Instance.RefreshInventoySlots();
            
            InputManager.Instance.uiSchemaContext = UISchemaSwitchType.BUILDSTATION;
            GameManager.Instance.PauseGame(true);
            EventSystem.current.SetSelectedGameObject(miniChimpPlateformBuilderCanvasGroup.gameObject);
        }

        public void HideBuildStationInterface() {
            UIManager.Instance.Set_active(miniChimpPlateformBuilderCanvasGroup, false);
            GameManager.Instance.PauseGame(false);
        }

        // search wich items are selected in the left panel
        private void CheckLeftSideSelectedItems() {
            leftsideContentDictionnary = new Dictionary<ItemThrowableType, int>();
            
            foreach (RectTransform child in leftslideContent.GetComponent<RectTransform>()) {
                if (child.gameObject.activeInHierarchy) {
                    var uiLeftSlot = child.GetComponent<UIBuildStationInventorySlotLeft>();
                    if (uiLeftSlot.toggle.isOn) {
                        leftsideContentDictionnary.Add(uiLeftSlot.itemThrowableType, Inventory.Instance.bananaManInventory[uiLeftSlot.itemThrowableType]);
                    }
                }
            }
        }

        public void GiveToPlayer(UIBuildStationInventorySlotRight uStationInventorySlot) {
            Inventory.Instance.AddQuantity(uStationInventorySlot.itemThrowableType, ItemThrowableCategory.PLATEFORM, uStationInventorySlot.quantity);
            
            var message = "+ "+uStationInventorySlot.quantity+" " +LocalizationSettings.Instance.GetStringDatabase().GetLocalizedString("platform");
            UIQueuedMessages.Instance.AddMessage(message);
            
            foreach (var ingredient in ScriptableObjectManager.Instance.GetPlateformCost()) {
                Inventory.Instance.RemoveQuantity( ingredient.Key, ingredient.Value*uStationInventorySlot.quantity);
            }
            
            UIBuildStationInventoryLeft.Instance.RefreshInventoySlots();
            ShowPossiblesConversions();
        }

        private void UnSelectAll() {
            foreach (var plateform in plateforms) {
                plateform.Value.SetActive(false);
            }
        }
    }
}
