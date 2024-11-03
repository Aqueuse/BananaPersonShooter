using UI.InGame.CommandRoomControlPanels;
using UnityEngine;

namespace InGame.MiniGames.MarketingCampaignMiniGame {
    public class AdMarketingCampaignManager : MonoBehaviour {
        public AdCampaign adCampaign;
        
        private bool isAdCampaignAvailable;

        [SerializeField] private UIMarketingPanel uiMarketingPanel;
    }
}