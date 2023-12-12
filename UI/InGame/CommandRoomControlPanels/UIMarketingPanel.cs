using TMPro;
using UnityEngine;

namespace UI.InGame.CommandRoomControlPanels {
    public class UIMarketingPanel : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI visitorsNumberText;

        [SerializeField] private GameObject visitorsExpected;
        [SerializeField] private GameObject nextWaveInCountdown; 
        [SerializeField] private GameObject launchAdCampaign;

        [SerializeField] private TextMeshProUGUI countdownTimerText;
        
        public void SetVisitorsExpected(string visitorsNumber) {
            launchAdCampaign.SetActive(false);
            nextWaveInCountdown.SetActive(false);
            visitorsExpected.SetActive(true);
            
            visitorsNumberText.text = visitorsNumber;
        }
        
        // TODO : on last chimp welcomed (pirate or nice visitor), authorize new ad campaign
        public void SetNewCampaignAvailable() {
            visitorsExpected.SetActive(false);
            nextWaveInCountdown.SetActive(false);
            launchAdCampaign.SetActive(true);
        }

        public void SetNextWaveCountdown() {
            visitorsExpected.SetActive(false);
            launchAdCampaign.SetActive(false);
            nextWaveInCountdown.SetActive(true);
        }

        public void SetCountdownValue(int value) {
            countdownTimerText.text = value.ToString();
        }
    }
}
