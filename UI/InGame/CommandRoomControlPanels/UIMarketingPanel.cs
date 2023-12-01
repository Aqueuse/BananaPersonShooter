using TMPro;
using UnityEngine;

namespace UI.InGame.CommandRoomControlPanels {
    public class UIMarketingPanel : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI visitorsNumberText;

        [SerializeField] private GameObject visitorsExpected;
        [SerializeField] private GameObject launchAdCampaign;
        
        public void SetVisitorsExpected(string visitorsNumber) {
            launchAdCampaign.SetActive(false);
            
            visitorsExpected.SetActive(true);
            visitorsNumberText.text = visitorsNumber;
        }
        
        // TODO : on last chimp welcomed (pirate or nice visitor), authorize new ad campaign
        public void SetNewCampaignAvailable() {
            visitorsExpected.SetActive(false);
            
            launchAdCampaign.SetActive(true);
        }
    }
}
