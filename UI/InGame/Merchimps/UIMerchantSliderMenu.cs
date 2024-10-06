using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.InGame.Merchimps {
    public class UIMerchantSliderMenu : MonoBehaviour {
        public Slider quantitySlider;
        [SerializeField] private TextMeshProUGUI maximumQuantityText;
        [SerializeField] private TextMeshProUGUI actualQuantityText;
        
        [SerializeField] private TextMeshProUGUI costText;
        [SerializeField] private TextMeshProUGUI actualMonneyText;
        
        [SerializeField] private Button buyButton;
        [SerializeField] private TextMeshProUGUI buyButtonText;

        [SerializeField] private TradeContext tradeContext;
        
        public void SetQuantitySliderMaxValue(int maxValue) {
            quantitySlider.maxValue = maxValue;
            maximumQuantityText.text = maxValue.ToString();

            if (maxValue == 1) {
                actualQuantityText.text = "";
            }
        }
        
        public void SetCostValue(float sliderValue) {
            if (ObjectsReference.Instance.chimpManager.merchimpsManager.activeItemScriptableObject == null) return;
            
            if (quantitySlider.maxValue > 1) actualQuantityText.text = ""+ (int)sliderValue;
            
            if (tradeContext == TradeContext.BUY) {
                var bitkongValue = ObjectsReference.Instance.chimpManager.merchimpsManager.activeItemScriptableObject.bitKongValue;
                var bananaManMonneyQuantity = ObjectsReference.Instance.bananaMan.bananaManData.bitKongQuantity; 
                
                if (bananaManMonneyQuantity < bitkongValue * (int)sliderValue) {
                    buyButtonText.text = "can't buy";
                    buyButtonText.color = Color.red;
                    costText.color = Color.red;
                    buyButton.interactable = false;
                }
                else {
                    buyButtonText.text = "buy";
                    buyButtonText.color = Color.black;
                    costText.color = Color.black;
                    buyButton.interactable = true;
                }

                costText.text = "cost : " + bitkongValue * (int)sliderValue + " BTK";
                actualMonneyText.text = "you have " + bananaManMonneyQuantity + " BTK";
            }

            if (tradeContext == TradeContext.SELL) {
                var bitkongValue = ObjectsReference.Instance.chimpManager.merchimpsManager.activeItemScriptableObject.bitKongValue;
                var merchantMonneyQuantity = ObjectsReference.Instance.trade.monkeyMenData.bitKongQuantity; 

                if (merchantMonneyQuantity < bitkongValue * (int)sliderValue) {
                    buyButtonText.text = "can't sell";
                    buyButtonText.color = Color.red;
                    costText.color = Color.red;
                    buyButton.interactable = false;
                }
                else {
                    buyButtonText.text = "sell";
                    buyButtonText.color = Color.black;
                    costText.color = Color.black;
                    buyButton.interactable = true;
                }

                costText.text = "bring : " + bitkongValue * (int)sliderValue + " BTK";
                actualMonneyText.text = "merchant have " + merchantMonneyQuantity + " BTK";
            }
        }
    }
}
