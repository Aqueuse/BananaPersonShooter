using System.Linq;
using TMPro;
using Tweaks;
using UI.InGame.CommandRoomControlPanels;
using UnityEngine;
using UnityEngine.UI;

namespace InGame.MiniGames.Communications {
    public class CommunicationsManager : MonoBehaviour {
        [SerializeField] private CanvasGroup adCampaignCreationToolCanvasGroup;
        [SerializeField] private CanvasGroup communicationsPanelCanvasGroup;
        [SerializeField] private CanvasGroup adCampaignDownButtonsCanvasGroup;

        [SerializeField] private Image communicationButtonImage;
        [SerializeField] private TextMeshProUGUI communicationButtonQuantity;

        public Color hangarAvailableColor;
        public Color hangarUnavailableColor;

        private GameObject spaceshipMessage;
        
        public AdCampaign adCampaign;
        
        private bool isAdCampaignAvailable;
        
        public void RefreshCommunicationQuantityButton() {
            var communicationsOpenNumber =
                ObjectsReference.Instance.uiCommunicationPanel.gameObject.GetComponentsInChildren<UICommunicationButton>().Length;
            
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

        public void LaunchAdCampaign() {
            ObjectsReference.Instance.spaceshipsSpawner.SpawnSpaceshipsWithAdCampaign();
            HideAdCampaignControlButtons();
        }
        
        public void ShowAdCampaignControlButtons() {
            UITweaks.SetCanvasGroupActif(adCampaignDownButtonsCanvasGroup, true);
        }
        
        public void HideAdCampaignControlButtons() {
            UITweaks.SetCanvasGroupActif(adCampaignDownButtonsCanvasGroup, false);
        }
    }
}
