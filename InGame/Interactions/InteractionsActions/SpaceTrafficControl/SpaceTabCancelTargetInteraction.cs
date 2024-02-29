using UnityEngine;

namespace InGame.Interactions.InteractionsActions.SpaceTrafficControl {
    public class SpaceTabCancelTargetInteraction : Interaction {
        public override void Activate(GameObject interactedGameObject) {
            ObjectsReference.Instance.spaceTrafficControlMiniGameManager.StopCannonControl();
        }
    }
}
