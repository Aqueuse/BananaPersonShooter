using System.Collections.Generic;
using Game.BananaCannonMiniGame;
using Game.CommandRoomPanelControls;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Interactions.InteractionsActions {
    public class MarketingLaunchAd : Interact {
        private System.Random systemRandom;

        private int timeBeforeWave;

        private void Start() {
            systemRandom = new System.Random();
        }

        public override void Activate(GameObject interactedGameObject) {
            CommandRoomControlPanelsManager.Instance.ShowPanel(CommandRoomPanelType.MARKETING);

            timeBeforeWave = 10;

            CommandRoomControlPanelsManager.Instance.uIMarketingPanel.SetNextWaveCountdown();
            CommandRoomControlPanelsManager.Instance.uIMarketingPanel.SetCountdownValue(timeBeforeWave);
            
            InvokeRepeating(nameof(DecrementeTime), 1, 1);
        }

        private void DecrementeTime() {
            timeBeforeWave -= 1;
            CommandRoomControlPanelsManager.Instance.uIMarketingPanel.SetCountdownValue(timeBeforeWave);
            
            ObjectsReference.Instance.audioManager.PlayEffect(SoundEffectType.COUNTDOWN, 0);

            if (timeBeforeWave < 1) {
                var piratesSpaceshipNumber = Random.Range(2, 12);
                var visitorsSpaceshipNumber = Random.Range(2, 12);

                var spaceships = ShuffleSpaceships(piratesSpaceshipNumber, visitorsSpaceshipNumber);

                BananaCannonMiniGameManager.Instance.AddNewWave(spaceships);

                CancelInvoke(nameof(DecrementeTime));
                CommandRoomControlPanelsManager.Instance.uIMarketingPanel.SetNewCampaignAvailable();
            }
        }
        
        

        private List<CharacterType> ShuffleSpaceships(int pirateSpaceshipsQuantity, int visitorsSpaceshipsQuantity) {
            var spaceships = new List<CharacterType>();

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
    }
}
