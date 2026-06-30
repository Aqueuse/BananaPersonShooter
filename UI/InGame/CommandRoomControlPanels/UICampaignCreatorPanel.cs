using InGame.MiniGames.Communications;
using UnityEngine;

namespace UI.InGame.CommandRoomControlPanels {
    public class UICampaignCreatorPanel : MonoBehaviour {
        [SerializeField] private GameObject launchCampaignDownButton;

        private const float campaignWorldsBoxShrinkedWidth = 104.04f;
        private const float campaignWorldsBoxExpandedWidth = 146.6995f;
        
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
    }
}