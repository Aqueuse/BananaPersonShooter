using System.Collections.Generic;
using Game.BananaCannonMiniGame;
using Game.CommandRoomPanelControls;
using UI.InGame.CommandRoomControlPanels;
using UnityEngine;

namespace Game.MarketingCampaignMiniGame {
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

            spaceships = ShuffleSpaceships(piratesSpaceshipNumber, visitorsSpaceshipNumber);

            BananaCannonMiniGameManager.Instance.AddNewWave(spaceships);
        }
        
        private List<CharacterType> ShuffleSpaceships(int pirateSpaceshipsQuantity, int visitorsSpaceshipsQuantity) {
            spaceships.Clear();

            for (int i = 0; i < pirateSpaceshipsQuantity; i++) {
                spaceships.Add(CharacterType.PIRATE);
            }

            for (int i = 0; i < visitorsSpaceshipsQuantity; i++) {
                spaceships.Add(CharacterType.VISITOR);
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