using InGame.Items.ItemsProperties.Characters;
using InGame.Monkeys.Merchimps;
using TMPro;
using UI.InGame.Inventory;
using UnityEngine;
using UnityEngine.UI;

namespace UI.InGame.Merchimps {
    public class UIMerchant : MonoBehaviour {
        public UIMerchantSliderMenu buyUiMerchantSliderMenu;
        public UIMerchantSliderMenu sellUiMerchantSliderMenu;

        [SerializeField] private CanvasGroup sellInventoryCanvasGroup;
        [SerializeField] private Image sellButtonImage;
        [SerializeField] private TextMeshProUGUI sellText;
        
        [SerializeField] private CanvasGroup buyInventoryCanvasGroup;
        [SerializeField] private Image buyButtonImage;
        [SerializeField] private TextMeshProUGUI buyText;

        public UIBananasInventory merchantSellUiBananasInventory;
        public UIIngredientsInventory merchantSellUiIngredientsInventory;
        public UIManufacturedItemsInventory merchantSellUiManufacturedItemsInventory;
    
        public UIIngredientsInventory merchantBuyUiIngredientsInventory;
        public UIManufacturedItemsInventory merchantBuyUiManufacturedItemsInventory;
        public UIRawMaterialsInventory merchantBuyUiRawMaterialsInventory;
        
        [SerializeField] private TextMeshProUGUI merchantBitKongQuantityTextMeshProUGUI;
        [SerializeField] private TextMeshProUGUI bananaManBitKongQuantityTextMeshProUGUI;
        
        [SerializeField] private Color activatedColor;

        private MerchimpsManager merchimpsManager;

        private void Start() {
            merchimpsManager = ObjectsReference.Instance.chimpManager.merchimpsManager;
        }

        public void Switch_to_Sell_inventory() {
            sellInventoryCanvasGroup.alpha = 1;
            sellInventoryCanvasGroup.interactable = true;
            sellInventoryCanvasGroup.blocksRaycasts = true;
            
            buyInventoryCanvasGroup.alpha = 0;
            buyInventoryCanvasGroup.interactable = false;
            buyInventoryCanvasGroup.blocksRaycasts = false;
            
            sellButtonImage.color = activatedColor;
            buyButtonImage.color = Color.black;
            
            sellText.color = Color.black;
            buyText.color = activatedColor;
            
            SwitchToSellBananasInventory();
        }
        
        public void Switch_to_Buy_inventory() {
            sellInventoryCanvasGroup.alpha = 0;
            sellInventoryCanvasGroup.interactable = false;
            sellInventoryCanvasGroup.blocksRaycasts = false;

            buyInventoryCanvasGroup.alpha = 1;
            buyInventoryCanvasGroup.interactable = true;
            buyInventoryCanvasGroup.blocksRaycasts = true;

            sellButtonImage.color = Color.black;
            buyButtonImage.color = activatedColor;

            sellText.color = activatedColor;
            buyText.color = Color.black;
            
            SwitchToBuyIngredientsInventory();
        }

        // onclick show ui with options to buy (slider 1 to max)
        public void ShowBuyOptions() {
            var slotItemScriptableObject = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<UInventorySlot>()
                .itemScriptableObject;

            merchimpsManager.activeItemScriptableObject = slotItemScriptableObject;

            buyUiMerchantSliderMenu.gameObject.SetActive(true);
            sellUiMerchantSliderMenu.gameObject.SetActive(false);

            buyUiMerchantSliderMenu.SetCostValue(1);
            buyUiMerchantSliderMenu.SetQuantitySliderMaxValue(merchimpsManager.GetMerchantItemQuantity(slotItemScriptableObject));
        }
        
        // onclick show ui with options to sell (slider 1 to max)
        public void ShowSellOptions() {
            var slotItemScriptableObject = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<UInventorySlot>()
                .itemScriptableObject;
            
            merchimpsManager.activeItemScriptableObject = slotItemScriptableObject;

            sellUiMerchantSliderMenu.gameObject.SetActive(true);
            buyUiMerchantSliderMenu.gameObject.SetActive(false);
            
            sellUiMerchantSliderMenu.SetCostValue(1);
            sellUiMerchantSliderMenu.SetQuantitySliderMaxValue(merchimpsManager.GetBananaManItemQuantity(slotItemScriptableObject));
        }

        public void HideOptions() {
            sellUiMerchantSliderMenu.gameObject.SetActive(false);
            buyUiMerchantSliderMenu.gameObject.SetActive(false);
        }

        public bool IsInOptionsMenu() {
            return buyUiMerchantSliderMenu.isActiveAndEnabled || sellUiMerchantSliderMenu.isActiveAndEnabled;
        }

        public void InitializeInventories(InventoryScriptableObject inventoryScriptableObject) {
            ObjectsReference.Instance.trade.merchantPropertiesScriptableObject = inventoryScriptableObject;
            merchantBuyUiIngredientsInventory.inventoryScriptableObject = inventoryScriptableObject;
            merchantBuyUiRawMaterialsInventory.inventoryScriptableObject = inventoryScriptableObject;
            merchantBuyUiManufacturedItemsInventory.inventoryScriptableObject = inventoryScriptableObject;
        }
        
        public void RefreshMerchantInventories() {
            merchantSellUiBananasInventory.RefreshUInventory();
            merchantSellUiIngredientsInventory.RefreshUInventory();
            merchantSellUiManufacturedItemsInventory.RefreshUInventory();
            
            merchantBuyUiIngredientsInventory.RefreshUInventory();
            merchantBuyUiRawMaterialsInventory.RefreshUInventory();
            merchantBuyUiManufacturedItemsInventory.RefreshUInventory();

        }

        public void SwitchToSellBananasInventory() {
            merchantSellUiBananasInventory.Activate();
            merchantSellUiIngredientsInventory.Desactivate();
            merchantSellUiManufacturedItemsInventory.Desactivate();
        }
        
        public void SwitchToSellIngredientsInventory() {
            merchantSellUiBananasInventory.Desactivate();
            merchantSellUiIngredientsInventory.Activate();
            merchantSellUiManufacturedItemsInventory.Desactivate();
        }
        
        public void SwitchToSellManufacturedInventory() {
            merchantSellUiBananasInventory.Desactivate();
            merchantSellUiIngredientsInventory.Desactivate();
            merchantSellUiManufacturedItemsInventory.Activate();
        }

        public void SwitchToBuyIngredientsInventory() {
            merchantBuyUiIngredientsInventory.Activate();
            merchantBuyUiManufacturedItemsInventory.Desactivate();
            merchantBuyUiRawMaterialsInventory.Desactivate();
        }
        
        public void SwitchToBuyManufacturedItemsInventory() {
            merchantBuyUiIngredientsInventory.Desactivate();
            merchantBuyUiManufacturedItemsInventory.Activate();
            merchantBuyUiRawMaterialsInventory.Desactivate();
        }

        public void SwitchToBuyRawMaterialsInventory() {
            merchantBuyUiIngredientsInventory.Desactivate();
            merchantBuyUiManufacturedItemsInventory.Desactivate();
            merchantBuyUiRawMaterialsInventory.Activate();
        }
        
        public void ValidateBuy() {
            ObjectsReference.Instance.trade.Buy(merchimpsManager.activeItemScriptableObject);
            buyUiMerchantSliderMenu.gameObject.SetActive(false);
            sellUiMerchantSliderMenu.gameObject.SetActive(false);
        }

        public void ValidateSell() {
            ObjectsReference.Instance.trade.Sell(merchimpsManager.activeItemScriptableObject);
            buyUiMerchantSliderMenu.gameObject.SetActive(false);
            sellUiMerchantSliderMenu.gameObject.SetActive(false);
        }

        public void RefreshBitkongQuantities() {
            merchantBitKongQuantityTextMeshProUGUI.text = merchimpsManager.activeMerchimp.merchantPropertiesScriptableObject.bitKongQuantity + " BTK";
            bananaManBitKongQuantityTextMeshProUGUI.text = ObjectsReference.Instance.bananaMan.inventories.bitKongQuantity + " BTK";
        }
    }
}
