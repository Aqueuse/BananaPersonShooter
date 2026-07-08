using UnityEngine;

namespace UI.InGame.GestionView {
    public class UiGestion : MonoBehaviour {
        public void SwitchViews() {
            if (ObjectsReference.Instance.gestionViewMode.isActiveAndEnabled) {
                ObjectsReference.Instance.gestionViewMode.CancelGhost();
                ObjectsReference.Instance.gestionViewMode.viewModeContextType = ViewModeContextType.SCAN;
                ObjectsReference.Instance.gestionViewMode.enabled = false;

                ObjectsReference.Instance.uInventoriesManager.GetCurrentUIHelper().ShowDefaultHelper();

                ObjectsReference.Instance.uiManager.SetActive(UICanvasGroupType.HUD_GESTION, false);
                ObjectsReference.Instance.uiManager.SetActive(UICanvasGroupType.HUD_BANANAMAN, true);

                ObjectsReference.Instance.bananaMan.SetToPlayable();

                ObjectsReference.Instance.camerasManager.SwitchToBananaManView();
            }

            else {
                ObjectsReference.Instance.uiManager.SetActive(UICanvasGroupType.HUD_BANANAMAN, false);
                ObjectsReference.Instance.uiManager.SetActive(UICanvasGroupType.HUD_GESTION, true);
            
                ObjectsReference.Instance.bananaMan.SetToNotPlayable();
            
                ObjectsReference.Instance.gestionViewMode.enabled = true;
                ObjectsReference.Instance.inputManager.SwitchContext(InputContext.GESTION_VIEW);

                ObjectsReference.Instance.camerasManager.SwitchToGestionView();
            }
        }
    }
}