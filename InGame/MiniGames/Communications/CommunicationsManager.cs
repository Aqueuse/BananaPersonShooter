using System.Linq;
using InGame.Items.ItemsBehaviours;
using TMPro;
using Tweaks;
using UnityEngine;
using UnityEngine.UI;

namespace InGame.MiniGames.Communications {
    public class CommunicationsManager : MonoBehaviour {
        [SerializeField] private CanvasGroup adCampaignCreationToolCanvasGroup;
        [SerializeField] private CanvasGroup communicationsPanelCanvasGroup;
        
        [SerializeField] private Image communicationButtonImage;
        [SerializeField] private TextMeshProUGUI communicationButtonQuantity;

        public Color hangarAvailableColor;
        public Color hangarUnavailableColor;

        private GameObject spaceshipMessage;
        
        public AdCampaign adCampaign;
        
        private bool isAdCampaignAvailable;
        
        public void RefreshCommunicationQuantityButton() {
            var communicationsOpenNumber = ObjectsReference.Instance.spaceTrafficControlManager.spaceshipBehavioursByGuid.Count(spaceshipBehaviour => spaceshipBehaviour.Value.spaceshipData.travelState == TravelState.FREE_FLIGHT);

            communicationButtonImage.color = communicationsOpenNumber > 0 ? hangarUnavailableColor : hangarAvailableColor;

            communicationButtonQuantity.text = "(" + communicationsOpenNumber + ")";
        }
        public void SwitchToAdCampaignTab() {
            UITweaks.SetCanvasGroupActif(adCampaignCreationToolCanvasGroup, true);
            UITweaks.SetCanvasGroupActif(communicationsPanelCanvasGroup, false);
        }
            
        public void SwitchToCommunicationsTab() {
            UITweaks.SetCanvasGroupActif(adCampaignCreationToolCanvasGroup, false);
            UITweaks.SetCanvasGroupActif(communicationsPanelCanvasGroup, true);
        }
    }
}
