using UnityEngine;

namespace Interactions.InteractionsActions {
    public class SasInteraction : Interaction {
        public override void Activate(GameObject interactedGameObject) {
            ObjectsReference.Instance.uiManager.canvasGroupsByUICanvasType[UICanvasGroupType.CROSSHAIRS].alpha = 0;
            ObjectsReference.Instance.inputManager.SwitchContext(InputContext.VISITOR_WAITING_LIST_MINI_GAME);
            ObjectsReference.Instance.gameManager.gameContext = GameContext.IN_VISITOR_WAITING_LIST_MINI_GAME;

            ObjectsReference.Instance.uiVisitorReception.RefreshUIWaintingList();
            ObjectsReference.Instance.uiManager.SetActive(UICanvasGroupType.VISITOR_WAITING_LIST, true);
        }
    }
}
