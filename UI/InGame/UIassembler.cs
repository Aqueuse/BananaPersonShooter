using TMPro;
using UnityEngine;

namespace UI.InGame {
    public enum AssemblerMode {
        IDLE,
        BANANA_GUN
    }

    public class UIassembler : MonoBehaviour {
        [SerializeField] private CanvasGroup basicTextCanvasGroup;
        [SerializeField] private CanvasGroup bananaGunReparationCanvasGroup;
        [SerializeField] private TextMeshProUGUI bananaGunPiecesQuantityText;

        public AssemblerMode assemblerMode = AssemblerMode.IDLE;
        
        public void SwitchToBananaGunReparationMode() {
            basicTextCanvasGroup.alpha = 0;
            bananaGunReparationCanvasGroup.alpha = 1;
        }

        public void SwitchToIdleMode() {
            basicTextCanvasGroup.alpha = 1;
            bananaGunReparationCanvasGroup.alpha = 0;
        }

        public void SetBananaGunPiecesQuantity(int newQuantity) {
            bananaGunPiecesQuantityText.text = newQuantity + "/8";
        }
    }
}
