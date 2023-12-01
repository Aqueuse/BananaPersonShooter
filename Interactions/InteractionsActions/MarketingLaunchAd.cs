using System.Collections.Generic;
using System.Linq;
using Game.BananaCannonMiniGame;
using Game.CommandRoomPanelControls;
using UnityEngine;

namespace Interactions.InteractionsActions {
    public class MarketingLaunchAd : Interact {
        public override void Activate(GameObject interactedGameObject) {
            CommandRoomControlPanelsManager.Instance.ShowPanel(CommandRoomPanelType.MARKETING);

            var piratesSpaceshipNumber = Random.Range(2, 12);
            var visitorsSpaceshipNumber = Random.Range(2, 12);

            var spaceships = ShuffleSpaceships(piratesSpaceshipNumber, visitorsSpaceshipNumber);

            foreach (var spaceship in spaceships) {
                Debug.Log(spaceship);
            }
            
            BananaCannonMiniGameManager.Instance.AddNewWave(spaceships);

            CommandRoomControlPanelsManager.Instance.uIMarketingPanel.SetVisitorsExpected(BananaCannonMiniGameManager.Instance._spaceshipsQuantity.ToString());
        }

        private List<CharacterType> ShuffleSpaceships(int pirateSpaceshipsQuantity, int visitorsSpaceshipsQuantity) {
            var spaceships = new List<CharacterType>();

            for (int i = 0; i < pirateSpaceshipsQuantity; i++) {
                spaceships.Add(CharacterType.PIRATE);
            }

            for (int i = 0; i < visitorsSpaceshipsQuantity; i++) {
                spaceships.Add(CharacterType.VISITOR);
            }
            
            var Random = new System.Random();

            int listCount = spaceships.Count;

            while (listCount > 1) {  
                listCount--;

                int nextRandomIndex = Random.Next(listCount + 1);

                (spaceships[nextRandomIndex], spaceships[listCount]) = (spaceships[listCount], spaceships[nextRandomIndex]);
            }

            return spaceships;
        }
    }
}
