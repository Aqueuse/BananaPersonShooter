using Game.BananaCannonMiniGame;
using UnityEngine;

namespace Interactions.InteractionsActions {
    public class BananaCannonMiniGameInteraction : MonoBehaviour {
        public static void Activate() {
            if (ObjectsReference.Instance.bananaMan.tutorialFinished) BananaCannonMiniGameManager.Instance.SwitchToMiniGame();
        }
    }
}
