using UnityEngine;

namespace InGame.Interactions.InteractionsActions {
    public class GuichetInteraction : Interaction {
        public override void Activate(GameObject interactedGameObject) {
            ObjectsReference.Instance.uiManager.canvasGroupsByUICanvasType[UICanvasGroupType.CROSSHAIRS].alpha = 0;
            ObjectsReference.Instance.inputManager.SwitchContext(InputContext.UI);

            ObjectsReference.Instance.uiTouristReception.RefreshUIWaintingList();
            ObjectsReference.Instance.uiManager.SetActive(UICanvasGroupType.VISITOR_WAITING_LIST, true);
        }
    }
}
