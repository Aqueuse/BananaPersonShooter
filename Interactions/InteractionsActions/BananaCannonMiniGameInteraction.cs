using Game.BananaCannonMiniGame;
using Game.CommandRoomPanelControls;
using UnityEngine;

namespace Interactions.InteractionsActions {
    public class BananaCannonMiniGameInteraction : Interaction {
        public override void Activate(GameObject interactedGameObject) {
            if (ObjectsReference.Instance.bananaMan.tutorialFinished) {
                CommandRoomControlPanelsManager.Instance.ShowPanel(CommandRoomPanelType.BANANA_CANNON);
                ObjectsReference.Instance.gameManager.gameContext = GameContext.IN_BANANA_CANNON_MINI_GAME;
                BananaCannonMiniGameManager.Instance.PlayMiniGame();
            }
        }
    }
}
