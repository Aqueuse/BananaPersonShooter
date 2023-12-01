using Game.BananaCannonMiniGame;
using Game.CommandRoomPanelControls;
using UnityEngine;

namespace Interactions.InteractionsActions {
    public class BananaCannonMiniGameInteraction : Interact {
        public override void Activate(GameObject interactedGameObject) {
            if (ObjectsReference.Instance.bananaMan.tutorialFinished) {
                CommandRoomControlPanelsManager.Instance.ShowPanel(CommandRoomPanelType.BANANA_CANNON);
                BananaCannonMiniGameManager.Instance.PlayMiniGame();
            }
        }
    }
}
