using System.Collections.Generic;
using InGame.CommandRoomPanelControls;
using UI.InGame.CommandRoomControlPanels;
using UnityEngine;

namespace InGame.MiniGames.MarketingCampaignMiniGame {
    public class MarketingCampaignManager : MonoBehaviour {
        private System.Random systemRandom;
        private bool isAdCampaignAvailable;
        
        [SerializeField] private UIMarketingPanel uiMarketingPanel;

        public List<CharacterType> spaceships;
        
        private void Start() {
            systemRandom = new System.Random();
            spaceships = new List<CharacterType>();
        }

        private void SetCampaignAvailable() {
            uiMarketingPanel.SetNewCampaignAvailable();
        }

        public void StartCampaign() {
            CommandRoomControlPanelsManager.Instance.ShowPanel(CommandRoomPanelType.MARKETING);
            uiMarketingPanel.SetCampaignOnProgress();
            SpawnSpaceships();
        }
    
        private void SpawnSpaceships() {
            var piratesSpaceshipNumber = Random.Range(2, 12);
            var visitorsSpaceshipNumber = Random.Range(2, 12);
            var merchantsSpaceshipNumber = Random.Range(2, 12);

            spaceships = ShuffleSpaceships(piratesSpaceshipNumber, visitorsSpaceshipNumber, merchantsSpaceshipNumber);

            ObjectsReference.Instance.spaceTrafficControlMiniGameManager.AddNewSpaceship("RNX-282");
        }
        
        private List<CharacterType> ShuffleSpaceships(int pirateSpaceshipsQuantity, int visitorsSpaceshipsQuantity, int merchantsSpaceshipsQuantity) {
            spaceships.Clear();

            for (int i = 0; i < pirateSpaceshipsQuantity; i++) {
                spaceships.Add(CharacterType.PIRATE);
            }

            for (int i = 0; i < visitorsSpaceshipsQuantity; i++) {
                spaceships.Add(CharacterType.VISITOR);
            }

            for (int i = 0; i < merchantsSpaceshipsQuantity; i++) {
                spaceships.Add(CharacterType.MERCHIMP);
            }

            int listCount = spaceships.Count;

            while (listCount > 1) {  
                listCount--;
                int nextRandomIndex = systemRandom.Next(listCount + 1);
                (spaceships[nextRandomIndex], spaceships[listCount]) = (spaceships[listCount], spaceships[nextRandomIndex]);
            }

            return spaceships;
        }

        public void RemoveGuest() {
            if (spaceships.Count == 0) {
                SetCampaignAvailable();
            }

            else {
                spaceships.RemoveAt(0);
            }
        }
    }
}