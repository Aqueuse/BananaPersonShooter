using Data;
using Enums;
using Game;
using Input;
using Input.UIActions;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Localization.Settings;

namespace UI.InGame.BuildStation {
    public class UIBuildStation : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI costQuantityText;
        [SerializeField] private TextMeshProUGUI ingredientText;

        [SerializeField] private GenericDictionary<BuildableType, GameObject> craftablesUIitems;
        
        public void ShowBuildStationInterface() {
            UIManager.Instance.Set_active(UICanvasGroupType.BUILDSTATION, true);
            
            InputManager.Instance.uiSchemaContext = UISchemaSwitchType.BUILDSTATION;
            GameManager.Instance.PauseGame(true);
            
            ShowPossiblesConversions();
            
            EventSystem.current.SetSelectedGameObject(UIManager.Instance.canvasGroupsByUICanvasType[UICanvasGroupType.BUILDSTATION].gameObject);
        }


        public void HideBuildStationInterface() {
            UIManager.Instance.Set_active(UICanvasGroupType.BUILDSTATION, false);
            costQuantityText.text = "0";
            
            GameManager.Instance.PauseGame(false);
        }
    
        private void ShowPossiblesConversions() {
            UnSelectAll();

            if (Game.Inventory.Instance.bananaManInventory[ItemThrowableType.INGOT] >= 2) {
                craftablesUIitems[BuildableType.PLATEFORM].SetActive(true);
            }
            if (Game.Inventory.Instance.bananaManInventory[ItemThrowableType.INGOT] >= 10) {
                craftablesUIitems[BuildableType.PLATEFORM5].SetActive(true);
            }
        }
    
        public void SetActivePrint(UIBuildStationInventorySlot uiStationInventorySlot) {
            UIBuildStationActions.Instance.activeBuildStation.activeItemType = uiStationInventorySlot.itemThrowableType;
            UIBuildStationActions.Instance.activeBuildStation.rawMaterial = ScriptableObjectManager.Instance.GetCraftIngredient(UIBuildStationActions.Instance.activeBuildStation.activeItemType);

            ingredientText.text = LocalizationSettings.Instance.GetStringDatabase().GetLocalizedString(UIBuildStationActions.Instance.activeBuildStation.rawMaterial.ToString().ToLower());
            UIBuildStationActions.Instance.activeBuildStation.totalCost = ScriptableObjectManager.Instance.GetCraftCost(UIBuildStationActions.Instance.activeBuildStation.activeItemType, uiStationInventorySlot.quantity);
            costQuantityText.text = "("+UIBuildStationActions.Instance.activeBuildStation.totalCost+")";
            
            if (Game.Inventory.Instance.bananaManInventory[UIBuildStationActions.Instance.activeBuildStation.rawMaterial] >= UIBuildStationActions.Instance.activeBuildStation.quantityToPrint+ScriptableObjectManager.Instance.GetCraftCost(UIBuildStationActions.Instance.activeBuildStation.activeItemType, uiStationInventorySlot.quantity)) {
                UIBuildStationActions.Instance.activeBuildStation.quantityToPrint = uiStationInventorySlot.quantity;
            }
        }
    
        private void UnSelectAll() {
            foreach (var plateform in craftablesUIitems) {
                plateform.Value.SetActive(false);
            }
        }
    }
}
