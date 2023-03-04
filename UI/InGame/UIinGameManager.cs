using UnityEngine;

namespace UI.InGame {
    public class UIinGameManager : MonoSingleton<UIinGameManager> {
        [SerializeField] private  UICanvasItemsStatic[] uiCanvasLookAtPlayer;
        
        public void HideAllUIsinGame() {
            foreach (var uiCanvas in uiCanvasLookAtPlayer) {
                uiCanvas.HideUI();
            }
        }
    }
}
