using UnityEngine;

namespace UI.InGame.CommandRoomControlPanels {
    public class UIMarketingPanel : MonoBehaviour {
        [SerializeField] private GameObject adCampaignInProgress; 
        [SerializeField] private GameObject adCampaignAvailable;
        
        public void SetNewCampaignAvailable() {
            adCampaignInProgress.SetActive(false);
            adCampaignAvailable.SetActive(true);
        }
        
        public void SetCampaignOnProgress() {
            adCampaignInProgress.SetActive(true);
            adCampaignAvailable.SetActive(false);
        }
    }
}
