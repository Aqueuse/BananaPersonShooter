using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace InGame.MiniGames.MarketingCampaignMiniGame {
    public class AdCampaign : MonoBehaviour {
        public int touristsNumber;
        public int piratesNumber;
        public int merchimpsNumber;

        [SerializeField] private TextMeshProUGUI touristsNumberText;
        [SerializeField] private TextMeshProUGUI piratesNumberText;
        [SerializeField] private TextMeshProUGUI merchimpsNumberText;
        
        public List<AdWordBox> wordBoxes;

        public void TryAddWordToCampaign(AdWordBox adWordBox) {
            if (wordBoxes.Count < 6) {
                wordBoxes.Add(adWordBox);
                adWordBox.isInCurrentCampaign = true;
                ObjectsReference.Instance.uiMarketingPanel.MoveAdWordBoxToCurrentCampaign(adWordBox);
                
                RefreshChimpsNumber();
            }

            else {
                // Blink ad Campaign
                ObjectsReference.Instance.audioManager.PlayEffect(SoundEffectType.NO_BANANA, 0);
            }
        }
        
        public void RemoveAdWordBox(AdWordBox adWordBox) {
            wordBoxes.Remove(adWordBox);
            ObjectsReference.Instance.uiMarketingPanel.MoveAdWordBoxToList(adWordBox);
            
            RefreshChimpsNumber();
        }
        
        private void RefreshChimpsNumber() {
            touristsNumber = 0;
            piratesNumber  = 0;
            merchimpsNumber  = 0;
            
            foreach (var adWordBox in wordBoxes) {
                // later check if it is a combo box and keep it for the end of calculation
                touristsNumber += adWordBox.touristsWeight;
                piratesNumber += adWordBox.piratesWeight;
                merchimpsNumber += adWordBox.merchantsWeight;
            }

            if (touristsNumber < 0) touristsNumber = 0;
            if (piratesNumber < 0) piratesNumber = 0;
            if (merchimpsNumber < 0) merchimpsNumber = 0;
            
            touristsNumberText.text = touristsNumber.ToString();
            piratesNumberText.text = piratesNumber.ToString();
            merchimpsNumberText.text = merchimpsNumber.ToString();
        }
    }
}
