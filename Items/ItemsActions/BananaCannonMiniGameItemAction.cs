using Game.BananaCannonMiniGame;
using UnityEngine;

namespace Items.ItemsActions {
    public class BananaCannonMiniGameItemAction : MonoBehaviour {
        public static void Activate() {
            if (ObjectsReference.Instance.bananaMan.tutorialFinished) BananaCannonMiniGameManager.Instance.SwitchToMiniGame();
        }
    }
}
