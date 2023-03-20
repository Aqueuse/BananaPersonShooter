using Building.Plateforms;
using Data;
using Enums;
using Game;
using Input;
using TMPro;
using UI;
using UI.InGame;
using UI.InGame.BuildStation;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Localization.Settings;

namespace Building {
    public class BuildStation : MonoSingleton<BuildStation> {
        [SerializeField] private CanvasGroup miniChimpPlateformBuilderCanvasGroup;
        [SerializeField] private UICanvasItemsStatic buildStationUiCanvasItemsStatic;
        
        [SerializeField] private TextMeshProUGUI costQuantityText;
        [SerializeField] private TextMeshProUGUI ingredientText;

        [SerializeField] private GenericDictionary<string, GameObject> craftablesUIitems;
        [SerializeField] private GenericDictionary<ItemThrowableType, GameObject> grabbableItemsByType;
        
        private Animator _buildStationAnimator;
        private AudioSource _audioBuildstationSource;
        
        private ItemThrowableType _activeItemType;
        private int _quantityToPrint;
        private ItemThrowableType _rawMaterial;
        
        private int _totalCost;
        
        private GameObject _printedItemGameObject;
        private static readonly int PrintTrigger = Animator.StringToHash("PRINT");
        
        private void Start() {
            _buildStationAnimator = GetComponent<Animator>();
            _audioBuildstationSource = GetComponentInChildren<AudioSource>();
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
            _activeItemType = uiStationInventorySlot.itemThrowableType;
            _rawMaterial = ScriptableObjectManager.Instance.GetCraftIngredient(_activeItemType);

            ingredientText.text = LocalizationSettings.Instance.GetStringDatabase().GetLocalizedString(_rawMaterial.ToString().ToLower());
            _totalCost = ScriptableObjectManager.Instance.GetCraftCost(_activeItemType, uiStationInventorySlot.quantity);
            costQuantityText.text = "("+_totalCost+")";
            
            if (Inventory.Instance.bananaManInventory[_rawMaterial] >= _quantityToPrint+ScriptableObjectManager.Instance.GetCraftCost(_activeItemType, uiStationInventorySlot.quantity)) {
                _quantityToPrint = uiStationInventorySlot.quantity;
            }
        }

        public void Print() {
            if (_quantityToPrint > 0) {
                Inventory.Instance.RemoveQuantity(_rawMaterial, _totalCost);
                _buildStationAnimator.SetTrigger(PrintTrigger);
                _audioBuildstationSource.Play();

                HideBuildStationInterface();
                buildStationUiCanvasItemsStatic.gameObject.layer = LayerMask.NameToLayer("Default");
                buildStationUiCanvasItemsStatic.HideUI();
            }
        }
        
        public void AddToStack() {
            if (_printedItemGameObject == null) {
                _printedItemGameObject = Instantiate(grabbableItemsByType[_activeItemType]);
            }

            _printedItemGameObject.GetComponent<GrabbableItem>().AddQuantity(1);
            _quantityToPrint--;
            
            if (_quantityToPrint > 0) {
                _buildStationAnimator.SetTrigger(PrintTrigger);
            }
            else {
                _audioBuildstationSource.Stop();
            }
        }

        public void RemovePlatform() {
            Destroy(_printedItemGameObject);
            buildStationUiCanvasItemsStatic.gameObject.layer = LayerMask.NameToLayer("Items");
        }

        private void UnSelectAll() {
            foreach (var plateform in craftablesUIitems) {
                plateform.Value.SetActive(false);
            }
        }
    }
}
