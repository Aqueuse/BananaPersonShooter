using InGame.MiniGames.MarketingCampaignMiniGame;
using UnityEngine;

namespace UI.InGame.CommandRoomControlPanels {
    public class UIMarketingPanel : MonoBehaviour {
        private const float campaignWorldsBoxShrinkedWidth = 104.04f;
        private const float campaignWorldsBoxExpandedWidth = 146.6995f;

        [SerializeField] private RectTransform currentCampaignRectTransform;
        
        [SerializeField] private GameObject launchCampaignDownButton;
        [SerializeField] private GameObject worldsBoxScrollList;
        
        [SerializeField] private Transform currentCampaignContainer;
        [SerializeField] private Transform adWorldsListContainer;

        private readonly Vector3 currentCampaignContainerBoxSize = new (0.576944f, 0.576944f, 0.576944f);
        private readonly Vector3 AdWorldsListContainerBoxSize = Vector3.one;
        
        public void MoveAdWordBoxToCurrentCampaign(AdWordBox adWordBox) {
            adWordBox.transform.SetParent(currentCampaignContainer);
            adWordBox.GetComponent<RectTransform>().localScale = currentCampaignContainerBoxSize;
        }

        public void MoveAdWordBoxToList(AdWordBox adWordBox) {
            adWordBox.transform.SetParent(adWorldsListContainer);
            adWordBox.GetComponent<RectTransform>().localScale = AdWorldsListContainerBoxSize;
        }
        
        public void SwitchToCampaignCreator() {
            worldsBoxScrollList.SetActive(true);
            launchCampaignDownButton.SetActive(true);
            
            currentCampaignRectTransform.localPosition = new Vector3(-0.4469f, 0.238f);
            currentCampaignRectTransform.sizeDelta =
                new Vector2(campaignWorldsBoxShrinkedWidth, currentCampaignRectTransform.sizeDelta.y);
        }
        
        public void SwitchToCurrentCampaign() {
            worldsBoxScrollList.SetActive(false);
            launchCampaignDownButton.SetActive(false);
            
            currentCampaignRectTransform.localPosition = new Vector3(-0f, 0.238f);
            currentCampaignRectTransform.sizeDelta =
                new Vector2(campaignWorldsBoxExpandedWidth, currentCampaignRectTransform.sizeDelta.y);
        }
    }
}
