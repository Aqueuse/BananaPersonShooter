using Building;
using UnityEngine;

namespace UI.InGame {
    public class UIinGameManager : MonoSingleton<UIinGameManager> {
        [SerializeField] private  UICanvasItemsStatic[] uiCanvasLookAtPlayer;
        
        public void HideAllUIsinGame() {
            foreach (var uiCanvas in uiCanvasLookAtPlayer) {
                uiCanvas.HideUI();
            }
        }

        public void SetHiddablesVisibility() {
            foreach (var uiCanvasItemsHiddable in MapItems.Instance.debrisContainer.GetComponentsInChildren<UICanvasItemsHiddable>()) {
                uiCanvasItemsHiddable.SetVisibility();
            }
            
            foreach (var uiCanvasItemsHiddable in MapItems.Instance.bananaTreesContainer.GetComponentsInChildren<UICanvasItemsHiddable>()) {
                uiCanvasItemsHiddable.SetVisibility();
            }
        }
    }
}
