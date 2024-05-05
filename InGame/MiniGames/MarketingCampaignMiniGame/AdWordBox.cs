using UnityEngine;

namespace InGame.MiniGames.MarketingCampaignMiniGame {
    public class AdWordBox : MonoBehaviour {
        public int touristsWeight;
        public int piratesWeight;
        public int merchantsWeight;

        public bool isInCurrentCampaign;

        public void Activate() {
            if (isInCurrentCampaign) {
                ObjectsReference.Instance.adMarketingCampaignManager.adCampaign.RemoveAdWordBox(this);
                isInCurrentCampaign = false;
            }
            else {
                ObjectsReference.Instance.adMarketingCampaignManager.adCampaign.TryAddWordToCampaign(this);
            }
        }
    }
}
