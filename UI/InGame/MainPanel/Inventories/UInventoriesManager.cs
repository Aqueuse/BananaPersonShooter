﻿using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace UI.InGame.MainPanel.Inventories {
    public class UInventoriesManager : MonoBehaviour {
        [SerializeField] private GenericDictionary<DroppedType, Image> buttonImageByInventoryCategory;
        
        [SerializeField] private GenericDictionary<DroppedType, CanvasGroup> canvasGroupByInventoryCategory;

        public GenericDictionary<DroppedType, GameObject> lastSelectedItemByInventoryCategory;

        [SerializeField] private TextMeshProUGUI bitkongQuantityText;
        
        public DroppedType lastFocusedInventory = DroppedType.BANANA;
        public BuildableTabType lastFocusedBlueprintInventory = BuildableTabType.FUN;

        [SerializeField] private UIHelper gamepadUIHelper;
        [SerializeField] private UIHelper keyboardUIHelper;
        
        public void SetLastSelectedItem(DroppedType droppedType, GameObject lastSelectedItem) {
            lastFocusedInventory = droppedType;
            lastSelectedItemByInventoryCategory[droppedType] = lastSelectedItem;
        }

        public void OpenInventories() {
            SwitchToInventoryTab(lastFocusedInventory);
        }

        private void FocusInventory() {
            if (lastSelectedItemByInventoryCategory[lastFocusedInventory] != null) {
                EventSystem.current.SetSelectedGameObject(lastSelectedItemByInventoryCategory[lastFocusedInventory]);
                FocusFirstSlotInInventory(lastFocusedInventory);
            }
        }
        
        private void FocusFirstSlotInInventory(DroppedType droppedType) {
            switch (droppedType) {
                case DroppedType.BANANA:
                    ObjectsReference.Instance.bananaManUiBananasInventory.SelectFirstSlot();
                    break;
                case DroppedType.INGREDIENTS:
                    ObjectsReference.Instance.bananaManUiIngredientsInventory.SelectFirstSlot();
                    break;
                case DroppedType.RAW_MATERIAL:
                    ObjectsReference.Instance.bananaManUIRawMaterialsInventory.SelectFirstSlot();
                    break;
                case DroppedType.MANUFACTURED_ITEMS:
                    ObjectsReference.Instance.bananaManUiManufacturedItemsInventory.SelectFirstSlot();
                    break;
                case DroppedType.BLUEPRINT:
                    ObjectsReference.Instance.bananaManUiBlueprintsInventory.SelectFirstSlot(lastFocusedBlueprintInventory);
                    break;
                case DroppedType.FOOD:
                    ObjectsReference.Instance.bananaManUiFoodInventory.SelectFirstSlot();
                    break;
            }
        }
        
        public void SwitchToBananasInventory() { SwitchToInventoryTab(DroppedType.BANANA); }
        public void SwitchToRawMaterialsInventory() { SwitchToInventoryTab(DroppedType.RAW_MATERIAL); }
        public void SwitchToIngredientsInventory() { SwitchToInventoryTab(DroppedType.INGREDIENTS); }
        public void SwitchToManufacturedItemsInventory() { SwitchToInventoryTab(DroppedType.MANUFACTURED_ITEMS); }
        public void SwitchToFoodInventory() { SwitchToInventoryTab(DroppedType.FOOD); }
        public void SwitchToBuildablesInventory() { SwitchToInventoryTab(DroppedType.BLUEPRINT); }

        public void SwitchToInventoryTab(DroppedType droppedType) {
            lastFocusedInventory = droppedType;

            foreach (var (category, canvasGroup) in canvasGroupByInventoryCategory) {
                SetActive(canvasGroup, category == droppedType);
            }

            FocusInventory();
            ObjectsReference.Instance.uiToolTipOnMouseHover.Hide();

            switch (droppedType) {
                case DroppedType.BANANA:
                    ObjectsReference.Instance.bananaManUiBananasInventory.RefreshUInventory();
                    break;
                case DroppedType.INGREDIENTS:
                    ObjectsReference.Instance.bananaManUiIngredientsInventory.RefreshUInventory();
                    break;
                case DroppedType.RAW_MATERIAL:
                    ObjectsReference.Instance.bananaManUIRawMaterialsInventory.RefreshUInventory();
                    break;
                case DroppedType.MANUFACTURED_ITEMS:
                    ObjectsReference.Instance.bananaManUiManufacturedItemsInventory.RefreshUInventory();
                    break;
                case DroppedType.FOOD:
                    ObjectsReference.Instance.bananaManUiFoodInventory.RefreshUInventory();
                    break;
                case DroppedType.BLUEPRINT:
                    ObjectsReference.Instance.bananaManUiBlueprintsInventory.RefreshUInventory(lastFocusedBlueprintInventory);
                    break;
            }
        }

        public void SwitchToLastFocusedInventory() {
            SwitchToInventoryTab(lastFocusedInventory);
        }

        public UIHelper GetCurrentUIHelper() {
            if (Gamepad.current == null) return gamepadUIHelper;
            return keyboardUIHelper;
        }

        public void ShowCurrentUIHelper() {
            HideUIHelpers();
            GetCurrentUIHelper().ShowHelper();
        }

        public void HideUIHelpers() {
            keyboardUIHelper.HideHelper();
            gamepadUIHelper.HideHelper();
        }
        

        public void SetBitKongQuantity(int bitkongQuantity) {
            bitkongQuantityText.text = bitkongQuantity + " BTK";
        }

        public void ActivateBuildablesInventory() {
            buttonImageByInventoryCategory[DroppedType.BLUEPRINT].gameObject.SetActive(true);
        }
        
        public void SetActive(CanvasGroup canvasGroup, bool visible) {
            canvasGroup.alpha = visible ? 1 : 0;
            canvasGroup.interactable = visible;
            canvasGroup.blocksRaycasts = visible;
        }
    }
}