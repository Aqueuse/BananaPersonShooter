using UI.InGame.CommandRoomControlPanels;
using UnityEngine;

namespace InGame.MiniGames.MarketingCampaign {
    public class AdMarketingCampaignManager : MonoBehaviour {
        public AdCampaign adCampaign;
        
        private bool isAdCampaignAvailable;

        [SerializeField] private UICommunicationPanel uiCommunicationPanel;
    }
}