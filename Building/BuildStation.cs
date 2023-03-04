using Building.Plateforms;
using Data;
using Enums;
using Game;
using Input;
using TMPro;
using UI;
using UI.InGame.BuildStation;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Building {
    public class BuildStation : MonoSingleton<BuildStation> {
        [SerializeField] private CanvasGroup miniChimpPlateformBuilderCanvasGroup;
        [SerializeField] private TextMeshProUGUI costQuantityText;

        [SerializeField] private GenericDictionary<string, GameObject> craftablesUIitems;
        [SerializeField] private GenericDictionary<ItemThrowableType, GameObject> grabbableItemsByType;

        private Animator buildStationAnimator;
        
        private ItemThrowableType activeItemType;
        private int quantityToPrint;
        private ItemThrowableType rawMaterial;
        
        private int costByUnit;
        
        private GameObject printedItemGameObject;
        private static readonly int PrintTrigger = Animator.StringToHash("PRINT");

        private void Start() {
            buildStationAnimator = GetComponent<Animator>();
        }

        public void ShowBuildStationInterface() {
            UIManager.Instance.Set_active(miniChimpPlateformBuilderCanvasGroup, true);
            
            InputManager.Instance.uiSchemaContext = UISchemaSwitchType.BUILDSTATION;
            GameManager.Instance.PauseGame(true);
            
            ShowPossiblesConversions();
            
            EventSystem.current.SetSelectedGameObject(miniChimpPlateformBuilderCanvasGroup.gameObject);
        }

        public void HideBuildStationInterface() {
            UIManager.Instance.Set_active(miniChimpPlateformBuilderCanvasGroup, false);
            costQuantityText.text = "0";
            
            GameManager.Instance.PauseGame(false);
        }
        
        private void ShowPossiblesConversions() {
            UnSelectAll();

            if (Inventory.Instance.bananaManInventory[ItemThrowableType.INGOT] >= 2) {
                craftablesUIitems["platform1"].SetActive(true);
            }
            if (Inventory.Instance.bananaManInventory[ItemThrowableType.INGOT] >= 10) {
                    craftablesUIitems["platform5"].SetActive(true);
            }
        }
        
        public void SetActivePrint(UIBuildStationInventorySlot uiStationInventorySlot) {
            activeItemType = uiStationInventorySlot.itemThrowableType;
            quantityToPrint += uiStationInventorySlot.quantity;

            rawMaterial = ScriptableObjectManager.Instance.GetCraftIngredient(activeItemType);
            costByUnit = ScriptableObjectManager.Instance.GetCraftCost(activeItemType, 1);
            
            costQuantityText.text = ScriptableObjectManager.Instance.GetCraftCost(activeItemType, quantityToPrint).ToString();
        }

        public void Print() {
            if (quantityToPrint > 0) {
                Inventory.Instance.RemoveQuantity(rawMaterial, costByUnit*quantityToPrint);
                buildStationAnimator.SetTrigger(PrintTrigger);
            
                HideBuildStationInterface();
            }
        }
        
        public void AddToStack() {
            if (printedItemGameObject == null) {
                printedItemGameObject = Instantiate(grabbableItemsByType[activeItemType]);
            }

            printedItemGameObject.GetComponent<GrabbableItem>().AddQuantity(1);
            quantityToPrint--;
            
            if (quantityToPrint > 0) {
                buildStationAnimator.SetTrigger(PrintTrigger);
            }
        }

        public void RemovePlatform() {
            Destroy(printedItemGameObject);
        }

        private void UnSelectAll() {
            foreach (var plateform in craftablesUIitems) {
                plateform.Value.SetActive(false);
            }
        }
    }
}
